using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TWG.PostageApp.Tests
{
    /// <summary>
    /// Represents Postage App tests.
    /// </summary>
    [TestClass]
    public class ClientTest
    {
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

        /// <summary>
        /// Test send message invalid domain.
        /// </summary>
        [TestMethod]
        public void TestSendMessageInvalidDomain()
        {
            var client = new Client(ApiKey) { BaseUri = "http://0.0.0.0/" };

            var threwException = false;
            try
            {
                var message = new Message
                {
                    Uid = Guid.NewGuid().ToString(),
                    Recipient = new Recipient("test@null.postageapp.com"),
                    RecipientOverride = RecipientOverride,
                    Text = "This is my text content"
                };

                client.SendMessage(message);
            }
            catch (PostageResponseException<MessageResponse> exception)
            {
                threwException = true;
                Assert.IsNull(exception.ResponseContainer);
            }

            Assert.IsTrue(threwException);
        }

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
                var message = new Message
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
        /// Test bad message sending.
        /// </summary>
        [TestMethod]
        public void TestSendMessageBadRequest()
        {
            var client = new Client(ApiKey);

            var threwException = false;
            try
            {
                client.SendMessage(new Message());
            }
            catch (PostageResponseException<MessageResponse> exception)
            {
                threwException = true;
                Assert.AreEqual(400, (int)((HttpWebResponse)((WebException)exception.InnerException).Response).StatusCode);
                Assert.AreEqual(ResponseStatus.BadRequest, exception.ResponseContainer.Response.Status);
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
                await client.SendMessageAsync(new Message());
            }
            catch (PostageResponseException<MessageResponse> exception)
            {
                threwException = true;
                Assert.AreEqual(ResponseStatus.BadRequest, exception.ResponseContainer.Response.Status);
            }

            Assert.IsTrue(threwException);
        }

        /// <summary>
        /// Test message sending.
        /// </summary>
        [TestMethod]
        public void TestSendMessageSuccess()
        {
            var client = new Client(ApiKey);

            var message = new Message
            {
                Uid = Guid.NewGuid().ToString(),
                Recipient = new Recipient("test@null.postageapp.com"),
                RecipientOverride = RecipientOverride,
                Text = "This is my text content"
            };

            var responseContainer = client.SendMessage(message);
            Assert.AreEqual(ResponseStatus.Ok, responseContainer.Response.Status);
            Assert.IsNotNull(responseContainer.Response.Uid);
            Assert.IsTrue(responseContainer.Response.Uid.Length > 0);
            Assert.IsNotNull(responseContainer.Data.Id);
            Assert.IsTrue(responseContainer.Data.Id > 0);
        }

        /// <summary>
        /// Test message sending async.
        /// </summary>
        /// <returns> Task. </returns>
        [TestMethod]
        public async Task TestSendMessageSuccessAsync()
        {
            var client = new Client(ApiKey);

            var message = new Message
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
        /// Test send message unauthorized.
        /// </summary>
        [TestMethod]
        public void TestSendMessageUnauthorized()
        {
            var client = new Client("abc123ThisIsNotValid");

            var threwException = false;
            try
            {
                client.SendMessage(new Message());
            }
            catch (PostageResponseException<MessageResponse> exception)
            {
                threwException = true;
                Assert.AreEqual(401, (int)((HttpWebResponse)((WebException)exception.InnerException).Response).StatusCode);
                Assert.AreEqual(ResponseStatus.Unauthorized, exception.ResponseContainer.Response.Status);
            }

            Assert.IsTrue(threwException);
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
                await client.SendMessageAsync(new Message());
            }
            catch (PostageResponseException<MessageResponse> exception)
            {
                threwException = true;
                Assert.AreEqual(ResponseStatus.Unauthorized, exception.ResponseContainer.Response.Status);
            }

            Assert.IsTrue(threwException);
        }

        /// <summary>
        /// Test send message precondition failed.
        /// </summary>
        [TestMethod]
        public void TestSendMessagePreconditionFailed()
        {
            var client = new Client(ApiKey);

            var threwException = false;
            try
            {
                client.SendMessage(new Message { Template = "some-unknown-template-xxxxxxxxxxxxxx" });
            }
            catch (PostageResponseException<MessageResponse> exception)
            {
                threwException = true;
                Assert.AreEqual(ResponseStatus.PreconditionFailed, exception.ResponseContainer.Response.Status);
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
               await client.SendMessageAsync(new Message { Template = "some-unknown-template-xxxxxxxxxxxxxx" });
            }
            catch (PostageResponseException<MessageResponse> exception)
            {
                threwException = true;
                Assert.AreEqual(ResponseStatus.PreconditionFailed, exception.ResponseContainer.Response.Status);
            }

            Assert.IsTrue(threwException);
        }

        /// <summary>
        /// Test send message UID round trip.
        /// </summary>
        [TestMethod]
        public void TestSendMessageUidRoundTrip()
        {
            var uid = Guid.NewGuid().ToString();

            var client = new Client(ApiKey);

            var responseContainer = client.SendMessage(new Message
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

            var responseContainer = await client.SendMessageAsync(new Message
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
        /// Test send message with headers.
        /// </summary>
        [TestMethod]
        public void TestSendMessageWithHeaders()
        {
            var client = new Client(ApiKey);
            var message = new Message
                {
                    Uid = Guid.NewGuid().ToString(),
                    Recipient = new Recipient("test@null.postageapp.com"),
                    RecipientOverride = RecipientOverride,
                    Text = "This email should have a custom from name and subject line."
                };

            message.Headers.Add("From", "test.robot@null.postageapp.com");
            message.Headers.Add("Subject", "This is a custom subject line");

            var responseContainer = client.SendMessage(message);

            Assert.AreEqual(ResponseStatus.Ok, responseContainer.Response.Status);
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
            var message = new Message
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
        /// Test send message with html content.
        /// </summary>
        [TestMethod]
        public void TestSendMessageWithHtmlContent()
        {
            var client = new Client(ApiKey);

            var responseContainer = client.SendMessage(new Message
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
        /// Test send message with html content async.
        /// </summary>
        /// <returns>
        /// <see cref="Task"/> of operation.
        /// </returns>
        [TestMethod]
        public async Task TestSendMessageWithHtmlContentAsync()
        {
            var client = new Client(ApiKey);

            var responseContainer = await client.SendMessageAsync(new Message
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
        /// Test send message with attachment.
        /// </summary>
        [TestMethod]
        public void TestSendMessageWithAttachment()
        {
            var client = new Client(ApiKey);

            var fileContent = "file contents!\n\n";

            var contentBytes = Encoding.UTF8.GetBytes(fileContent);
            var stream = new MemoryStream(contentBytes);

            var message = new Message
                {
                    Uid = Guid.NewGuid().ToString(),
                    Subject = "Has attachment",
                    Recipient = new Recipient("test@null.postageapp.com"),
                    RecipientOverride = RecipientOverride,
                    Text = "This email should have an attachment",
                };

            message.Attachments.Add(new Attachment(stream, "readme.txt", "text/plain"));

            var responseContainer = client.SendMessage(message);

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

            var message = new Message
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
    }
}
