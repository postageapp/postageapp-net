using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PostageApp.Abstractions;
using Xunit;

namespace PostageApp.Http.Tests
{
    public class HttpPostageAppClient_SendMessageAsyncShould
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
            var result = await client.GetAccountInfoAsync();

            // Assert
            Assert.True(result.Succeeded);
            Assert.Null(result.Error);

            Assert.Equal("ok", result.ResponseMeta.Status);
            Assert.Null(result.ResponseMeta.Uid);
            Assert.Null(result.ResponseMeta.Message);

            Assert.Equal("AccountName", result.Data.Account.Name);
            Assert.Equal("https://account-name.postageapp.com/", result.Data.Account.Url);

            Assert.Equal(84, result.Data.Account.Transmissions.Today);
            Assert.Equal(3750, result.Data.Account.Transmissions.ThisMonth);
            Assert.Equal(291517, result.Data.Account.Transmissions.Overall);

            Assert.Equal(new Dictionary<string, string> {
                { "email1@email.com", "User 1" },
                { "email2@email.com", "User2" }
            }, result.Data.Account.Users);

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
            var result = await client.GetAccountInfoAsync();

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal(GetAccountInfoErrorCode.Unauthorized, result.Error.Value);

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
            var result = await client.GetAccountInfoAsync();

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal(GetAccountInfoErrorCode.Locked, result.Error.Value);

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
                () => client.GetAccountInfoAsync());

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
                () => client.GetAccountInfoAsync());

            // Assert
            Assert.Equal("The posted content is not valid JSON.", exception.Message);

            builder.MockHttp.VerifyNoOutstandingRequest();
        }
    }
}
