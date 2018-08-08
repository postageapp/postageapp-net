using System;
using System.Threading.Tasks;

using PostageApp.Abstractions;

using Xunit;

namespace PostageApp.Http.Tests
{
    public class HttpPostageAppClient_GetMessagesAsyncShould
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
            var result = await client.GetMessagesAsync(2);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Null(result.Error);

            Assert.Equal("ok", result.ResponseMeta.Status);
            Assert.Null(result.ResponseMeta.Uid);
            Assert.Null(result.ResponseMeta.Message);

            Assert.Equal(2, result.Data.Count);

            Assert.Equal("5", result.Data["key1"].ProjectId);
            Assert.Equal("new_message_notification", result.Data["key1"].Template);
            Assert.Equal(2, result.Data["key1"].TransmissionsTotal);
            Assert.Equal(0, result.Data["key1"].TransmissionsFailed);
            Assert.Equal(2, result.Data["key1"].TransmissionsCompleted);
            Assert.Equal(new DateTime(2018, 05, 30, 15, 01, 21, DateTimeKind.Utc), result.Data["key1"].CreatedAt.ToUniversalTime());
            Assert.Equal(new DateTime(2018, 06, 29, 15, 01, 21, DateTimeKind.Utc), result.Data["key1"].WillPurgeAt?.ToUniversalTime());

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
            var result = await client.GetMessagesAsync();

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal(GetMessagesErrorCode.Unauthorized, result.Error.Value);

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
            var result = await client.GetMessagesAsync();

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal(GetMessagesErrorCode.Locked, result.Error.Value);

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
                () => client.GetMessagesAsync());

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
                () => client.GetMessagesAsync());

            // Assert
            Assert.Equal("The posted content is not valid JSON.", exception.Message);

            builder.MockHttp.VerifyNoOutstandingRequest();
        }
    }
}
