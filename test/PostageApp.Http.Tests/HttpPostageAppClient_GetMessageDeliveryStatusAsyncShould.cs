using System;
using System.Threading.Tasks;

using PostageApp.Abstractions;

using Xunit;

namespace PostageApp.Http.Tests
{
    public class HttpPostageAppClient_GetMessageDeliveryStatusAsyncShould
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
            var result = await client.GetMessageDeliveryStatusAsync("some_uid");

            // Assert
            Assert.True(result.Succeeded);
            Assert.Null(result.Error);

            Assert.Equal("ok", result.ResponseMeta.Status);
            Assert.Equal("some_uid", result.ResponseMeta.Uid);
            Assert.Null(result.ResponseMeta.Message);

            Assert.Equal(2, result.Data.DeliveryStatus.Length);

            Assert.Equal("123@x.mailer.postageapp.com", result.Data.DeliveryStatus[0].UniqueId);
            Assert.Equal("completed@y.com", result.Data.DeliveryStatus[0].Recipient);
            Assert.Equal(MessageDeliveryStatus.Completed, result.Data.DeliveryStatus[0].Status);

            Assert.Equal("456@x.mailer.postageapp.com", result.Data.DeliveryStatus[1].UniqueId);
            Assert.Equal("queued@y.com", result.Data.DeliveryStatus[1].Recipient);
            Assert.Equal(MessageDeliveryStatus.Queued, result.Data.DeliveryStatus[1].Status);

            builder.MockHttp.VerifyNoOutstandingRequest();
        }

        [Fact]
        public async Task ReturnNotFoundGivenUnknownUid()
        {
            // Arrange
            var builder = new HttpPostageAppClientBuider()
                .WithBaseUri("http://test")
                .WithApiKey("not_found_api_key");

            var client = builder.Build();

            // Act
            var result = await client.GetMessageDeliveryStatusAsync("another_uid");

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal(GetMessageDeliveryStatusErrorCode.NotFound, result.Error.Value);

            Assert.Equal("not_found", result.ResponseMeta.Status);
            Assert.Null(result.ResponseMeta.Uid);
            Assert.Equal("Message with UID MESSAGE_UID was not found.", result.ResponseMeta.Message);

            Assert.Null(result.Data);

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
            var result = await client.GetMessageDeliveryStatusAsync("some_uid");

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal(GetMessageDeliveryStatusErrorCode.Unauthorized, result.Error.Value);

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
            var result = await client.GetMessageDeliveryStatusAsync("some_uid");

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal(GetMessageDeliveryStatusErrorCode.Locked, result.Error.Value);

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
                () => client.GetMessageDeliveryStatusAsync("some_uid"));

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
                () => client.GetMessageDeliveryStatusAsync("some_uid"));

            // Assert
            Assert.Equal("The posted content is not valid JSON.", exception.Message);

            builder.MockHttp.VerifyNoOutstandingRequest();
        }
    }
}
