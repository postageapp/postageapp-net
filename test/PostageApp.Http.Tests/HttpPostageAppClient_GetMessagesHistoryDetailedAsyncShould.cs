using System;
using System.Threading.Tasks;

using PostageApp.Abstractions;

using Xunit;

namespace PostageApp.Http.Tests
{
    public class HttpPostageAppClient_GetMessagesHistoryDetailedAsyncShould
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
            var result = await client.GetMessagesHistoryDetailedAsync(new DateTime(2017, 03, 29), new DateTime(2017, 03, 30));

            // Assert
            Assert.True(result.Succeeded);
            Assert.Null(result.Error);

            Assert.Equal("ok", result.ResponseMeta.Status);
            Assert.Null(result.ResponseMeta.Uid);
            Assert.Null(result.ResponseMeta.Message);

            Assert.Equal(2, result.Data.MessagesHistory.Length);

            Assert.Equal("36598cc85e020a01384ea7653872c8e87536869c@mailer.postageapp.com", result.Data.MessagesHistory[0].UniqueId);
            Assert.Equal(23490345, result.Data.MessagesHistory[0].MessageId);
            Assert.Equal("36598cc85e020a01384ea7653872c8e87536869c", result.Data.MessagesHistory[0].Uid);
            Assert.Equal("recipient@example.com", result.Data.MessagesHistory[0].Recipient);
            Assert.Equal("completed", result.Data.MessagesHistory[0].Status);
            Assert.Equal(new DateTime(2017, 03, 30, 21, 24, 41), result.Data.MessagesHistory[0].CreatedAt);

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
            var result = await client.GetMessagesHistoryDetailedAsync();

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal(GetMessagesHistoryErrorCode.Unauthorized, result.Error.Value);

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
            var result = await client.GetMessagesHistoryDetailedAsync();

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal(GetMessagesHistoryErrorCode.Locked, result.Error.Value);

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
                () => client.GetMessagesHistoryDetailedAsync());

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
                () => client.GetMessagesHistoryDetailedAsync());

            // Assert
            Assert.Equal("The posted content is not valid JSON.", exception.Message);

            builder.MockHttp.VerifyNoOutstandingRequest();
        }
    }
}
