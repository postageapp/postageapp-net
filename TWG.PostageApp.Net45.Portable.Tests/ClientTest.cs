using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TWG.PostageApp.Common;
using TWG.PostageApp.Message;
using TWG.PostageApp.Transmissions;

namespace TWG.PostageApp.Net45.Portable.Tests
{
    /// <summary>
    /// Represents Postage App tests.
    /// </summary>
    [TestClass]
    public class ClientTest
    {
        #region Common

        /// <summary>
        /// Gets recipient override email.
        /// </summary>
        private static string RecipientOverride
        {
            get { return ConfigurationManager.AppSettings["testRecipient"]; }
        }

        /// <summary>
        /// Gets api key.
        /// </summary>
        private static string ApiKey
        {
            get { return ConfigurationManager.AppSettings["apiKey"]; }
        }

        #endregion

        #region Send Message

        /// <summary>
        /// The test send message invalid domain async.
        /// </summary>
        /// <returns>
        /// <see cref="Task"/> of operation.
        /// </returns>
        [TestMethod]
        public async Task TestSendMessageInvalidDomainAsync()
        {
            var client = new Client(ApiKey) { BaseUri = "http://0.0.0.0/" };

            var threwException = false;
            try
            {
                var message = new Message.Message
                {
                    Uid = Guid.NewGuid().ToString(),
                    Recipient = new Recipient("test@null.postageapp.com"),
                    RecipientOverride = RecipientOverride,
                    Text = "This is my text content"
                };

                await client.SendMessageAsync(message);
            }
            catch (PostageResponseException<MessageResponse> exception)
            {
                threwException = true;
                Assert.IsNull(exception.ResponseContainer);
            }

            Assert.IsTrue(threwException);
        }

        /// <summary>
        /// Test bad message sending async.
        /// </summary>
        /// <returns> Task. </returns>
        [TestMethod]
        public async Task TestSendMessageBadRequestAsync()
        {
            var client = new Client(ApiKey);

            var threwException = false;
            try
            {
                await client.SendMessageAsync(new Message.Message());
            }
            catch (PostageResponseException<MessageResponse> exception)
            {
                threwException = true;
                Assert.AreEqual(ResponseStatus.BadRequest, exception.ResponseContainer.Response.Status);
            }

            Assert.IsTrue(threwException);
        }

        /// <summary>
        /// Test message sending async.
        /// </summary>
        /// <returns> Task. </returns>
        [TestMethod]
        public async Task TestSendMessageSuccessAsync()
        {
            var client = new Client(ApiKey);

            var message = new Message.Message
            {
                Uid = Guid.NewGuid().ToString(),
                Recipient = new Recipient("test@null.postageapp.com"),
                RecipientOverride = RecipientOverride,
                Text = "This is my text content"
            };

            var responseContainer = await client.SendMessageAsync(message);
            Assert.AreEqual(ResponseStatus.Ok, responseContainer.Response.Status);
            Assert.IsNotNull(responseContainer.Response.Uid);
            Assert.IsTrue(responseContainer.Response.Uid.Length > 0);
            Assert.IsNotNull(responseContainer.Data.Id);
            Assert.IsTrue(responseContainer.Data.Id > 0);
        }

        /// <summary>
        /// Test send message unauthorized async.
        /// </summary>
        /// <returns>
        /// <see cref="Task"/> of operation.
        /// </returns>
        [TestMethod]
        public async Task TestSendMessageUnauthorizedAsync()
        {
            var client = new Client("abc123ThisIsNotValid");

            var threwException = false;
            try
            {
                await client.SendMessageAsync(new Message.Message());
            }
            catch (PostageResponseException<MessageResponse> exception)
            {
                threwException = true;
                Assert.AreEqual(ResponseStatus.Unauthorized, exception.ResponseContainer.Response.Status);
            }

            Assert.IsTrue(threwException);
        }

        /// <summary>
        /// Test send message precondition failed async.
        /// </summary>
        /// <returns>
        /// <see cref="Task"/> of operation.
        /// </returns>
        [TestMethod]
        public async Task TestSendMessagePreconditionFailedAsync()
        {
            var client = new Client(ApiKey);

            var threwException = false;
            try
            {
                await client.SendMessageAsync(new Message.Message { Template = "some-unknown-template-xxxxxxxxxxxxxx" });
            }
            catch (PostageResponseException<MessageResponse> exception)
            {
                threwException = true;
                Assert.AreEqual(ResponseStatus.PreconditionFailed, exception.ResponseContainer.Response.Status);
            }

            Assert.IsTrue(threwException);
        }

        /// <summary>
        /// Test send message UID round trip async.
        /// </summary>
        /// <returns>
        /// <see cref="Task"/> of operation.
        /// </returns>
        [TestMethod]
        public async Task TestSendMessageUidRoundTripAsync()
        {
            var uid = Guid.NewGuid().ToString();

            var client = new Client(ApiKey);

            var responseContainer = await client.SendMessageAsync(new Message.Message
            {
                Uid = uid,
                Subject = "Specific Uid: " + uid,
                Text = "This email should have a specific uid",
                Recipient = new Recipient("test@null.postageapp.com"),
                RecipientOverride = RecipientOverride
            });

            Assert.AreEqual(ResponseStatus.Ok, responseContainer.Response.Status);
            Assert.AreEqual(uid, responseContainer.Response.Uid);
        }

        /// <summary>
        /// Test send message with headers async.
        /// </summary>
        /// <returns>
        /// <see cref="Task"/> of operation.
        /// </returns>
        [TestMethod]
        public async Task TestSendMessageWithHeadersAsync()
        {
            var client = new Client(ApiKey);
            var message = new Message.Message
                {
                    Uid = Guid.NewGuid().ToString(),
                    Recipient = new Recipient("test@null.postageapp.com"),
                    RecipientOverride = RecipientOverride,
                    Text = "This email should have a custom from name and subject line."
                };

            message.Headers.Add("From", "test.robot@null.postageapp.com");
            message.Headers.Add("Subject", "This is a custom subject line");

            var responseContainer = await client.SendMessageAsync(message);

            Assert.AreEqual(ResponseStatus.Ok, responseContainer.Response.Status);
        }

        /// <summary>
        /// Test send message with html content async.
        /// </summary>
        /// <returns>
        /// <see cref="Task"/> of operation.
        /// </returns>
        [TestMethod]
        public async Task TestSendMessageWithHtmlContentAsync()
        {
            var client = new Client(ApiKey);

            var responseContainer = await client.SendMessageAsync(new Message.Message
                {
                    Uid = Guid.NewGuid().ToString(),
                    Subject = "Html body",
                    Recipient = new Recipient("test@null.postageapp.com"),
                    RecipientOverride = RecipientOverride,
                    Text = "This email should have some html content.",
                    Html = "<h1>Title</h1><p>This is an <em>html email</em></p></h1>"
                });

            Assert.AreEqual(ResponseStatus.Ok, responseContainer.Response.Status);
        }

        /// <summary>
        /// Test send message with attachment async.
        /// </summary>
        /// <returns>
        /// <see cref="Task"/> of operation.
        /// </returns>
        [TestMethod]
        public async Task TestSendMessageWithAttachmentAsync()
        {
            var client = new Client(ApiKey);

            var fileContent = "file contents!\n\n";

            var contentBytes = Encoding.UTF8.GetBytes(fileContent);
            var stream = new MemoryStream(contentBytes);

            var message = new Message.Message
            {
                Uid = Guid.NewGuid().ToString(),
                Subject = "Has attachment",
                Recipient = new Recipient("test@null.postageapp.com"),
                RecipientOverride = RecipientOverride,
                Text = "This email should have an attachment",
            };

            message.Attachments.Add(new Attachment(stream, "readme.txt", "text/plain"));

            var responseContainer = await client.SendMessageAsync(message);

            Assert.AreEqual(ResponseStatus.Ok, responseContainer.Response.Status);
        }

        #endregion

        #region Get Messages

        /// <summary>
        /// Test send message unauthorized async.
        /// </summary>
        /// <returns>
        /// <see cref="Task"/> of operation.
        /// </returns>
        [TestMethod]
        public async Task TestGetMessagesUnauthorizedAsync()
        {
            var client = new Client("abc123ThisIsNotValid");

            var threwException = false;
            try
            {
                await client.GetMessagesAsync();
            }
            catch (PostageResponseException<Dictionary<string, MessageInfo>> exception)
            {
                threwException = true;
                Assert.AreEqual(ResponseStatus.Unauthorized, exception.ResponseContainer.Response.Status);
            }

            Assert.IsTrue(threwException);
        }

        #endregion

        #region Get Project Info

        /// <summary>
        /// Test get project success.
        /// </summary>
        /// <returns>
        /// <see cref="Task"/> of operation.
        /// </returns>
        [TestMethod]
        public async Task TestGetProjectInfoSuccessAsync()
        {
            var client = new Client(ApiKey);

            var responseContainer = await client.GetProjectInfoAsync();

            Assert.AreEqual(ResponseStatus.Ok, responseContainer.Response.Status);
            Assert.IsNotNull(responseContainer.Data);
            Assert.IsNotNull(responseContainer.Data.Url);
        }

        #endregion

        #region Get Account Info

        /// <summary>
        /// Test get project success.
        /// </summary>
        /// <returns>
        /// <see cref="Task"/> of operation.
        /// </returns>
        [TestMethod]
        public async Task TestGetAccountInfoSuccessAsync()
        {
            var client = new Client(ApiKey);

            var responseContainer = await client.GetAccountInfoAsync();

            Assert.AreEqual(ResponseStatus.Ok, responseContainer.Response.Status);
            Assert.IsNotNull(responseContainer.Data);
            Assert.IsNotNull(responseContainer.Data.Url);
        }

        #endregion

        #region Get Message Receipt

        /// <summary>
        /// Test get message receipt success async.
        /// </summary>
        /// <returns>
        /// <see cref="Task"/> of operation.
        /// </returns>
        [TestMethod]
        public async Task TestGetMessageReceiptSuccessAsync()
        {
            var client = new Client(ApiKey);

            var message = new Message.Message
            {
                Uid = Guid.NewGuid().ToString(),
                Recipient = new Recipient("test@null.postageapp.com"),
                RecipientOverride = RecipientOverride,
                Text = "This is my text content"
            };

            var sendResponseContainer = await client.SendMessageAsync(message);

            var responseContainer = await client.GetMessageReceiptAsync(sendResponseContainer.Response.Uid);

            Assert.AreEqual(ResponseStatus.Ok, responseContainer.Response.Status);
            Assert.AreEqual(sendResponseContainer.Data.Id, responseContainer.Data.Id);
        }

        /// <summary>
        /// Test not existing message async.
        /// </summary>
        /// <returns> Task. </returns>
        [TestMethod]
        public async Task TestGetMessageReceiptNotFoundAsync()
        {
            var client = new Client(ApiKey);

            var threwException = false;
            try
            {
               await client.GetMessageReceiptAsync("strange UID");
            }
            catch (PostageResponseException<MessageResponse> exception)
            {
                threwException = true;
                Assert.AreEqual(ResponseStatus.NotFound, exception.ResponseContainer.Response.Status);
            }

            Assert.IsTrue(threwException);
        }

        #endregion

        #region Get Metrics

        /// <summary>
        /// Test get message receipt success.
        /// </summary>
        /// <returns>
        /// <see cref="Task"/> of operation.
        /// </returns>
        [TestMethod]
        public async Task TestGetMetricsSuccessAsync()
        {
            var client = new Client(ApiKey);

            var responseContainer = await client.GetMetricsAsync();

            Assert.AreEqual(ResponseStatus.Ok, responseContainer.Response.Status);
            Assert.IsNotNull(responseContainer.Data);
            Assert.IsNotNull(responseContainer.Data.Date);
            Assert.IsNotNull(responseContainer.Data.Hour);
            Assert.IsNotNull(responseContainer.Data.Month);
            Assert.IsNotNull(responseContainer.Data.Week);
        }

        #endregion

        #region Get Message Transmissions

        /// <summary>
        /// Test get message transmissions success async.
        /// </summary>
        /// <returns>
        /// <see cref="Task"/> of operation.
        /// </returns>
        [TestMethod]
        public async Task TestGetMessageTransmissionsSuccessAsync()
        {
            var client = new Client(ApiKey);

            var messagesContainer = await client.GetMessagesAsync();

            var firstMessage = messagesContainer.Data.FirstOrDefault(pair => pair.Value.TotalTransmissionsCount > 1);

            var responseContainer = await client.GetMessageTransmissionsAsync(firstMessage.Key);

            Assert.AreEqual(ResponseStatus.Ok, responseContainer.Response.Status);
            Assert.IsNotNull(responseContainer.Data);
        }

        /// <summary>
        /// Test not existing message transmissions async.
        /// </summary>
        /// <returns>
        /// <see cref="Task"/> of operation.
        /// </returns>
        [TestMethod]
        public async Task TestGetMessageTransmissionsNotFoundAsync()
        {
            var client = new Client(ApiKey);

            var threwException = false;
            try
            {
               await client.GetMessageTransmissionsAsync("strange UID");
            }
            catch (PostageResponseException<MessageTransmissions> exception)
            {
                threwException = true;
                Assert.AreEqual(ResponseStatus.NotFound, exception.ResponseContainer.Response.Status);
            }

            Assert.IsTrue(threwException);
        }

        #endregion
    }
}
