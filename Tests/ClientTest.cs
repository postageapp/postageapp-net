using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PostageApp;
using Tests.Extensions;

namespace Tests
{
    [TestClass]
    public class ClientTest
    {
        private static string RecipientOverride
        {
            get { return ConfigurationManager.AppSettings["testRecipient"]; }
        }
        
        private static string ApiKey
        {
            get { return ConfigurationManager.AppSettings["apiKey"]; }
        }

        [TestMethod]
        public void TestSendMessageInvalidDomain()
        {
            var client = new Client(ApiKey)
                {
                    BaseUri = "http://0.0.0.0/"
                };

            var threwException = false;
            try
            {
                client.SendMessage(new SendMessageRequest());
            }
            catch (SendMessageException e)
            {
                threwException = true;
                Assert.AreEqual(502, e.StatusCode);
                Assert.AreEqual(SendMessageResponseStatus.Unknown, e.SendMessageResponse.Status);
            }

            Assert.IsTrue(threwException);
        }

        [TestMethod]
        public void TestSendMessageBadRequest()
        {
            var client = new Client(ApiKey);

            var threwException = false;
            try
            {
                client.SendMessage(new SendMessageRequest());
            }
            catch (SendMessageException e)
            {
                threwException = true;
                Assert.AreEqual(400, e.StatusCode);
                Assert.AreEqual(SendMessageResponseStatus.BadRequest, e.SendMessageResponse.Status);
            }

            Assert.IsTrue(threwException);
        }

        [TestMethod]
        public void TestSendMessageUnauthorized()
        {
            var client = new Client("abc123ThisIsNotValid");

            var threwException = false;
            try
            {
                client.SendMessage(new SendMessageRequest());
            }
            catch (SendMessageException e)
            {
                threwException = true;
                Assert.AreEqual(401, e.StatusCode);
                Assert.AreEqual(SendMessageResponseStatus.Unauthorized, e.SendMessageResponse.Status);
            }

            Assert.IsTrue(threwException);
        }

        [TestMethod]
        public void TestSendMessagePreconditionFailed()
        {
            var client = new Client(ApiKey);

            var threwException = false;
            try
            {
                client.SendMessage(new SendMessageRequest()
                    {
                        Template = "some-unknown-template-xxxxxxxxxxxxxx"
                    });
            }
            catch (SendMessageException e)
            {
                threwException = true;
                Assert.AreEqual(412, e.StatusCode);
                Assert.AreEqual(SendMessageResponseStatus.PreconditionFailed, e.SendMessageResponse.Status);
            }

            Assert.IsTrue(threwException);
        }

        [TestMethod]
        public void TestSendMessageSuccess()
        {
            var client = new Client(ApiKey);

            var response = client.SendMessage(new SendMessageRequest()
                {
                    Uid = Guid.NewGuid().ToString(),
                    Recipient = "test@null.postageapp.com",
                    RecipientOverride = RecipientOverride,
                    Text = "This is my text content"
                });

            Assert.AreEqual(SendMessageResponseStatus.Ok, response.Status);
            Assert.IsNotNull(response.Uid);
            Assert.IsTrue(response.Uid.Length > 0);
            Assert.IsNotNull(response.MessageId);
            Assert.IsTrue(response.MessageId > 0);
        }

        [TestMethod]
        public void TestSendMessageUidRoundTrip()
        {
            var uid = Guid.NewGuid().ToString();

            var client = new Client(ApiKey);

            var response = client.SendMessage(new SendMessageRequest()
            {
                Uid = uid,
                Subject = "Specific Uid: " + uid,
                Text = "This email should have a specific uid",
                Recipient = "test@null.postageapp.com",
                RecipientOverride = RecipientOverride
            });

            Assert.AreEqual(SendMessageResponseStatus.Ok, response.Status);
            Assert.AreEqual(uid, response.Uid); 
        }       

        [TestMethod]
        public void TestSendMessageWithHeaders()
        {
            var client = new Client(ApiKey);

            var headers = new Dictionary<string, string>()
                {
                    {"From", "test.robot@null.postageapp.com"},
                    {"Subject", "This is a custom subject line"}
                };

            var response = client.SendMessage(new SendMessageRequest()
            {
                Uid = Guid.NewGuid().ToString(),
                Recipient = "test@null.postageapp.com",
                RecipientOverride = RecipientOverride,
                Headers = headers,
                Text = "This email should have a custom from name and subject line."
            });

            Assert.AreEqual(SendMessageResponseStatus.Ok, response.Status);
        }

        [TestMethod]
        public void TestSendMessageWithHtmlContent()
        {
            var client = new Client(ApiKey);

            var response = client.SendMessage(new SendMessageRequest()
            {
                Uid = Guid.NewGuid().ToString(),
                Subject = "Html body",
                Recipient = "test@null.postageapp.com",
                RecipientOverride = RecipientOverride,
                Text = "This email should have some html content.",
                Html = "<h1>Title</h1><p>This is an <em>html email</em></p></h1>"
            });

            Assert.AreEqual(SendMessageResponseStatus.Ok, response.Status);
        }

        [TestMethod]
        public void TestSendMessageWithAttachment()
        {
            var client = new Client(ApiKey);

            var response = client.SendMessage(new SendMessageRequest()
            {
                Uid = Guid.NewGuid().ToString(),
                Subject = "Has attachment",
                Recipient = "test@null.postageapp.com",
                RecipientOverride = RecipientOverride,
                Text = "This email should have an attachment",
                Attachments = new Attachment[]
                    {
                        new Attachment("file contents!\n\n".ToStream(), "readme.txt", "text/plain")
                    }
            });

            Assert.AreEqual(SendMessageResponseStatus.Ok, response.Status);
        }    
    }
}
