using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PostageApp;

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
        public void TestSendMessage()
        {
            var client = new Client(ApiKey);

            var response = client.SendMessage(new SendMessageRequest()
                {
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
                Subject = "Has attachment",
                Recipient = "test@null.postageapp.com",
                RecipientOverride = RecipientOverride,
                Text = "This email should have an attachment"
            });

            Assert.AreEqual(SendMessageResponseStatus.Ok, response.Status);
        }    
    }
}
