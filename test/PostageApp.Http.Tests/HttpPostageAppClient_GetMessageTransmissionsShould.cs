using System;
using System.Threading.Tasks;

using PostageApp.Abstractions;

using Xunit;

namespace PostageApp.Http.Tests
{
    public class HttpPostageAppClient_GetMessageTransmissionsShould
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
            var result = await client.GetMessageTransmissionsAsync("some_uid");

            // Assert
            Assert.True(result.Succeeded);
            Assert.Null(result.Error);

            Assert.Equal("ok", result.ResponseMeta.Status);
            Assert.Equal("some_uid", result.ResponseMeta.Uid);
            Assert.Null(result.ResponseMeta.Message);

            Assert.NotNull(result.Data.Message);
            Assert.Equal(425416050, result.Data.Message.Id);

            Assert.Equal(2, result.Data.Transmissions.Count);

            Assert.Equal("completed", result.Data.Transmissions["test@example.org"].Status);
            Assert.Equal(new DateTime(2018, 05, 30, 15, 01, 22), result.Data.Transmissions["test@example.org"].CreatedAt);
            Assert.Null(result.Data.Transmissions["test@example.org"].FailedAt);
            Assert.Equal(new DateTime(2018, 05, 30, 15, 07, 16), result.Data.Transmissions["test@example.org"].OpenedAt);
            Assert.Null(result.Data.Transmissions["test@example.org"].ClickedAt);
            Assert.Null(result.Data.Transmissions["test@example.org"].ResultCode);
            Assert.Null(result.Data.Transmissions["test@example.org"].ErrorMessage);

            Assert.Equal("failed", result.Data.Transmissions["bad@example.org"].Status);
            Assert.Equal(new DateTime(2012, 07, 04, 18, 58, 57), result.Data.Transmissions["bad@example.org"].CreatedAt);
            Assert.Equal(new DateTime(2012, 07, 04, 18, 58, 59), result.Data.Transmissions["bad@example.org"].FailedAt);
            Assert.Null(result.Data.Transmissions["bad@example.org"].OpenedAt);
            Assert.Null(result.Data.Transmissions["bad@example.org"].ClickedAt);
            Assert.Equal("SMTP_554", result.Data.Transmissions["bad@example.org"].ResultCode);
            Assert.Equal("User does not exist", result.Data.Transmissions["bad@example.org"].ErrorMessage);

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
            var result = await client.GetMessageTransmissionsAsync("another_uid");

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal(GetMessageTransmissionsErrorCode.NotFound, result.Error.Value);

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
            var result = await client.GetMessageTransmissionsAsync("some_uid");

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal(GetMessageTransmissionsErrorCode.Unauthorized, result.Error.Value);

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
            var result = await client.GetMessageTransmissionsAsync("some_uid");

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal(GetMessageTransmissionsErrorCode.Locked, result.Error.Value);

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
                () => client.GetMessageTransmissionsAsync("some_uid"));

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
                () => client.GetMessageTransmissionsAsync("some_uid"));

            // Assert
            Assert.Equal("The posted content is not valid JSON.", exception.Message);

            builder.MockHttp.VerifyNoOutstandingRequest();
        }
    }
}
