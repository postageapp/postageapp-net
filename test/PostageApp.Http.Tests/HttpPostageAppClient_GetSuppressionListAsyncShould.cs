using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PostageApp.Abstractions;
using Xunit;

namespace PostageApp.Http.Tests
{
    public class HttpPostageAppClient_GetSuppressionListAsyncShould
    {
        [Fact]
        public async Task ReturnSuccededGivenExistingApiKey()
        {
            // Arrange
            var builder = new HttpPostageAppClientBuider()
                .WithBaseUri("http://test")
                .WithApiKey("successfull_api_key");

            var client = builder.Build();

            // Act
            var result = await client.GetSuppressionListAsync();

            // Assert
            Assert.True(result.Succeeded);
            Assert.Null(result.Error);

            Assert.Equal("ok", result.ResponseMeta.Status);
            Assert.Null(result.ResponseMeta.Uid);
            Assert.Null(result.ResponseMeta.Message);

            Assert.Equal(2, result.Data.Recipients.Count);

            Assert.Equal("rejected", result.Data.Recipients["email1@example.com"].Status);
            Assert.Equal("hard_bounce", result.Data.Recipients["email1@example.com"].SuppressionState);
            Assert.Equal(new DateTime(2014, 02, 09, 17, 32, 31), result.Data.Recipients["email1@example.com"].BlockedUntil);
            Assert.Equal("No Such User Here", result.Data.Recipients["email1@example.com"].Message);

            builder.MockHttp.VerifyNoOutstandingRequest();
        }

        [Fact]
        public async Task ReturnUnauthorizedGivenUnknownApiKey()
        {
            // Arrange
            var builder = new HttpPostageAppClientBuider()
                .WithBaseUri("http://test")
                .WithApiKey("none_api_key");

            var client = builder.Build();

            // Act
            var result = await client.GetSuppressionListAsync();

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal(GetSuppressionListErrorCode.Unauthorized, result.Error.Value);

            Assert.Equal("unauthorized", result.ResponseMeta.Status);
            Assert.Null(result.ResponseMeta.Uid);
            Assert.Equal("Invalid or inactive project API key used.", result.ResponseMeta.Message);

            Assert.Null(result.Data);

            builder.MockHttp.VerifyNoOutstandingRequest();
        }

        [Fact]
        public async Task ReturnLockedGivenLockedApiKey()
        {
            // Arrange
            var builder = new HttpPostageAppClientBuider()
                .WithBaseUri("http://test")
                .WithApiKey("locked_api_key");

            var client = builder.Build();

            // Act
            var result = await client.GetSuppressionListAsync();

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal(GetSuppressionListErrorCode.Locked, result.Error.Value);

            Assert.Equal("locked", result.ResponseMeta.Status);
            Assert.Null(result.ResponseMeta.Uid);
            Assert.Equal("The specified API key is not valid.", result.ResponseMeta.Message);

            Assert.Null(result.Data);

            builder.MockHttp.VerifyNoOutstandingRequest();
        }

        [Fact]
        public async Task ThrowInvalidOperationExceptionGivenConflictRequest()
        {
            // Arrange
            var builder = new HttpPostageAppClientBuider()
                .WithBaseUri("http://test")
                .WithApiKey("conflict_api_key");

            var client = builder.Build();

            // Act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => client.GetSuppressionListAsync());

            // Assert
            Assert.Equal("Invalid API version.", exception.Message);

            builder.MockHttp.VerifyNoOutstandingRequest();
        }

        [Fact]
        public async Task ThrowInvalidOperationExceptionGivenCallError()
        {
            // Arrange
            var builder = new HttpPostageAppClientBuider()
                .WithBaseUri("http://test")
                .WithApiKey("call_error_api_key");

            var client = builder.Build();

            // Act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => client.GetSuppressionListAsync());

            // Assert
            Assert.Equal("The posted content is not valid JSON.", exception.Message);

            builder.MockHttp.VerifyNoOutstandingRequest();
        }
    }
}
