using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PostageApp.Abstractions;
using Xunit;

namespace PostageApp.Http.Tests
{
    public class HttpPostageAppClient_SendMessageAsyncShould
    {
        [Fact]
        public async Task ReturnSuccededGiven2Recipients()
        {
            // Arrange
            var builder = new HttpPostageAppClientBuider()
                .WithBaseUri("http://test")
                .WithApiKey("successfull_api_key");

            var client = builder.Build();

            var message = new Message
            {
                Recipients = new List<MessageRecipient>
                {
                    new MessageRecipient("e1@example.com"),
                    new MessageRecipient("e2@example.com")
                }
            };

            // Act
            var result = await client.SendMessageAsync(message);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Null(result.Error);

            Assert.Equal("ok", result.ResponseMeta.Status);
            Assert.Equal("27cf6ede7501a32d54d22abe17e3c154d2cae7f3", result.ResponseMeta.Uid);
            Assert.Null(result.ResponseMeta.Message);

            Assert.Equal(1234567890, result.Data.Message.Id);
            Assert.Equal("http://YOUR_ACCOUNT.postageapp.com/projects/YOUR_PROJECT_ID/messages/MESSAGE_ID", result.Data.Message.Url);
            Assert.Equal("ignored", result.Data.Message.Duplicate);

            builder.MockHttp.VerifyNoOutstandingRequest();
        }

        [Fact]
        public async Task ReturnSuccededGivenFrom()
        {
            // Arrange
            var builder = new HttpPostageAppClientBuider()
                .WithBaseUri("http://test")
                .WithApiKey("successfull_api_key");

            var client = builder.Build();

            var message = new Message
            {
                From = "e1@example.com"
            };

            // Act
            var result = await client.SendMessageAsync(message);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Null(result.Error);

            Assert.Equal("ok", result.ResponseMeta.Status);
            Assert.Equal("27cf6ede7501a32d54d22abe17e3c154d2cae7f3", result.ResponseMeta.Uid);
            Assert.Null(result.ResponseMeta.Message);

            Assert.Equal(1234567890, result.Data.Message.Id);

            builder.MockHttp.VerifyNoOutstandingRequest();
        }

        [Fact]
        public async Task ReturnSuccededGivenSubject()
        {
            // Arrange
            var builder = new HttpPostageAppClientBuider()
                .WithBaseUri("http://test")
                .WithApiKey("successfull_api_key");

            var client = builder.Build();

            var message = new Message
            {
                Subject = "Subject"
            };

            // Act
            var result = await client.SendMessageAsync(message);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Null(result.Error);

            Assert.Equal("ok", result.ResponseMeta.Status);
            Assert.Equal("27cf6ede7501a32d54d22abe17e3c154d2cae7f3", result.ResponseMeta.Uid);
            Assert.Null(result.ResponseMeta.Message);

            Assert.Equal(1234567890, result.Data.Message.Id);

            builder.MockHttp.VerifyNoOutstandingRequest();
        }

        [Fact]
        public async Task ReturnSuccededGivenReplyTo()
        {
            // Arrange
            var builder = new HttpPostageAppClientBuider()
                .WithBaseUri("http://test")
                .WithApiKey("successfull_api_key");

            var client = builder.Build();

            var message = new Message
            {
                ReplyTo = "e1@example.com"
            };

            // Act
            var result = await client.SendMessageAsync(message);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Null(result.Error);

            Assert.Equal("ok", result.ResponseMeta.Status);
            Assert.Equal("27cf6ede7501a32d54d22abe17e3c154d2cae7f3", result.ResponseMeta.Uid);
            Assert.Null(result.ResponseMeta.Message);

            Assert.Equal(1234567890, result.Data.Message.Id);

            builder.MockHttp.VerifyNoOutstandingRequest();
        }

        [Fact]
        public async Task ReturnSuccededGivenUid()
        {
            // Arrange
            var builder = new HttpPostageAppClientBuider()
                .WithBaseUri("http://test")
                .WithApiKey("successfull_api_key");

            var client = builder.Build();

            var message = new Message();

            // Act
            var result = await client.SendMessageAsync(message, "27cf6ede7501a32d54d22abe17e3c154d2cae7f3");

            // Assert
            Assert.True(result.Succeeded);
            Assert.Null(result.Error);

            Assert.Equal("ok", result.ResponseMeta.Status);
            Assert.Equal("27cf6ede7501a32d54d22abe17e3c154d2cae7f3", result.ResponseMeta.Uid);
            Assert.Null(result.ResponseMeta.Message);

            Assert.Equal(1234567890, result.Data.Message.Id);

            builder.MockHttp.VerifyNoOutstandingRequest();
        }

        [Fact]
        public async Task ReturnSuccededGivenAttachment()
        {
            // Arrange
            var builder = new HttpPostageAppClientBuider()
                .WithBaseUri("http://test")
                .WithApiKey("successfull_api_key");

            var client = builder.Build();

            var content = Encoding.UTF8.GetBytes("Hello World!");

            var message = new Message
            {
                Attachments = new Dictionary<string, MessageAttachment>
                {
                    { "file.txt", new MessageAttachment(content, "text") }
                }
            };

            // Act
            var result = await client.SendMessageAsync(message);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Null(result.Error);

            Assert.Equal("ok", result.ResponseMeta.Status);
            Assert.Equal("27cf6ede7501a32d54d22abe17e3c154d2cae7f3", result.ResponseMeta.Uid);
            Assert.Null(result.ResponseMeta.Message);

            Assert.Equal(1234567890, result.Data.Message.Id);

            builder.MockHttp.VerifyNoOutstandingRequest();
        }

        [Fact]
        public async Task ReturnSuccededGivenCustomHeader()
        {
            // Arrange
            var builder = new HttpPostageAppClientBuider()
                .WithBaseUri("http://test")
                .WithApiKey("successfull_api_key");

            var client = builder.Build();
            var message = new Message
            {
                Headers = new Dictionary<string, string>
                {
                    { "custom_header", "value" }
                }
            };

            // Act
            var result = await client.SendMessageAsync(message);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Null(result.Error);

            Assert.Equal("ok", result.ResponseMeta.Status);
            Assert.Equal("27cf6ede7501a32d54d22abe17e3c154d2cae7f3", result.ResponseMeta.Uid);
            Assert.Null(result.ResponseMeta.Message);

            Assert.Equal(1234567890, result.Data.Message.Id);

            builder.MockHttp.VerifyNoOutstandingRequest();
        }

        [Fact]
        public async Task ReturnSuccededGivenTemplate()
        {
            // Arrange
            var builder = new HttpPostageAppClientBuider()
                .WithBaseUri("http://test")
                .WithApiKey("successfull_api_key");

            var client = builder.Build();
            var message = new Message
            {
                Template = "template1"
            };

            // Act
            var result = await client.SendMessageAsync(message);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Null(result.Error);

            Assert.Equal("ok", result.ResponseMeta.Status);
            Assert.Equal("27cf6ede7501a32d54d22abe17e3c154d2cae7f3", result.ResponseMeta.Uid);
            Assert.Null(result.ResponseMeta.Message);

            Assert.Equal(1234567890, result.Data.Message.Id);

            builder.MockHttp.VerifyNoOutstandingRequest();
        }

        [Fact]
        public async Task ReturnSuccededGivenContent()
        {
            // Arrange
            var builder = new HttpPostageAppClientBuider()
                .WithBaseUri("http://test")
                .WithApiKey("successfull_api_key");

            var client = builder.Build();
            var message = new Message
            {
                Content = new Dictionary<string, string>
                {
                    { "text/plain", "Text Content" },
                    { "text/html", "HTML Content" }
                }
            };

            // Act
            var result = await client.SendMessageAsync(message);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Null(result.Error);

            Assert.Equal("ok", result.ResponseMeta.Status);
            Assert.Equal("27cf6ede7501a32d54d22abe17e3c154d2cae7f3", result.ResponseMeta.Uid);
            Assert.Null(result.ResponseMeta.Message);

            Assert.Equal(1234567890, result.Data.Message.Id);

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
            var result = await client.SendMessageAsync(new Message());

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal(SendMessageErrorCode.Unauthorized, result.Error.Value);

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
            var result = await client.SendMessageAsync(new Message());

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal(SendMessageErrorCode.Locked, result.Error.Value);

            Assert.Equal("locked", result.ResponseMeta.Status);
            Assert.Null(result.ResponseMeta.Uid);
            Assert.Equal("The specified API key is not valid.", result.ResponseMeta.Message);

            Assert.Null(result.Data);

            builder.MockHttp.VerifyNoOutstandingRequest();
        }

        [Fact]
        public async Task ReturnBadRequestGivenBadRequestApiKey()
        {
            // Arrange
            var builder = new HttpPostageAppClientBuider()
                .WithBaseUri("http://test")
                .WithApiKey("bad_request_api_key");

            var client = builder.Build();

            // Act
            var result = await client.SendMessageAsync(new Message());

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal(SendMessageErrorCode.BadRequest, result.Error.Value);

            Assert.Equal("bad_request", result.ResponseMeta.Status);
            Assert.Equal("27cf6ede7501a32d54d22abe17e3c154d2cae7f3", result.ResponseMeta.Uid);
            Assert.Equal("Some error message. Like: you forgot the recipients!", result.ResponseMeta.Message);

            Assert.Null(result.Data);

            builder.MockHttp.VerifyNoOutstandingRequest();
        }

        [Fact]
        public async Task ReturnPreconditionFailedGivenPreconditionFailedApiKey()
        {
            // Arrange
            var builder = new HttpPostageAppClientBuider()
                .WithBaseUri("http://test")
                .WithApiKey("precondition_failed_key");

            var client = builder.Build();

            // Act
            var result = await client.SendMessageAsync(new Message());

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal(SendMessageErrorCode.PreconditionFailed, result.Error.Value);

            Assert.Equal("precondition_failed", result.ResponseMeta.Status);
            Assert.Null(result.ResponseMeta.Uid);
            Assert.Equal("The content of your email messages needs to be addressed. Please contact support for further information.", result.ResponseMeta.Message);

            Assert.Null(result.Data);

            builder.MockHttp.VerifyNoOutstandingRequest();
        }

        [Fact]
        public async Task ReturnInvalidUTF8GivenInvalidUTF8ApiKey()
        {
            // Arrange
            var builder = new HttpPostageAppClientBuider()
                .WithBaseUri("http://test")
                .WithApiKey("invalid_utf8_failed_key");

            var client = builder.Build();

            // Act
            var result = await client.SendMessageAsync(new Message());

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal(SendMessageErrorCode.InvalidUTF8, result.Error.Value);

            Assert.Equal("invalid_utf8", result.ResponseMeta.Status);
            Assert.Null(result.ResponseMeta.Uid);
            Assert.Equal("The posted content is not encoded in UTF8 or contains invalid characters. Error detected at byte 10.", result.ResponseMeta.Message);

            Assert.Null(result.Data);

            builder.MockHttp.VerifyNoOutstandingRequest();
        }

        [Fact]
        public async Task ThrowArgumentNullExceptionGivenNullMessage()
        {
            // Arrange
            var builder = new HttpPostageAppClientBuider()
                .WithBaseUri("http://test")
                .WithApiKey("conflict_api_key");

            var client = builder.Build();

            // Act
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => client.SendMessageAsync(null));

            // Assert
            Assert.Equal("message", exception.ParamName);
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
                () => client.SendMessageAsync(new Message()));

            // Assert
            Assert.Equal("Invalid API version.", exception.Message);

            builder.MockHttp.VerifyNoOutstandingRequest();
        }
    }
}
