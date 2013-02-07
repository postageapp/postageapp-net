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
        private Recipient TestRecipient
        {
            get
            {
                var email = ConfigurationManager.AppSettings["testRecipient"];
                return email == null ? null : new Recipient(email);
            }
        }
        
        [TestMethod]
        public void TestSendMessage()
        {
            var client = new Client(ConfigurationManager.AppSettings["apiKey"]);

            var recipients = new List<Recipient>() {new Recipient("test@null.postageapp.com")};
            if (TestRecipient != null) recipients.Add(TestRecipient);

            var response = client.SendMessage(new SendMessageRequest()
                {
                    Recipients = recipients,
                    Content = new Content() { Text = "This is my text content" }
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

            var client = new Client(ConfigurationManager.AppSettings["apiKey"]);

            var recipients = new List<Recipient>() { new Recipient("test@null.postageapp.com") };
            if (TestRecipient != null) recipients.Add(TestRecipient);

            var response = client.SendMessage(new SendMessageRequest()
            {
                Uid = uid,
                Headers = new Dictionary<string, string>() { { "Subject", "Specific Uid: " + uid } },
                Content = new Content() { Text = "This email should have a specific uid" },
                Recipients = recipients,
            });

            Assert.AreEqual(SendMessageResponseStatus.Ok, response.Status);
            Assert.AreEqual(uid, response.Uid); 
        }       

        [TestMethod]
        public void TestSendMessageWithHeaders()
        {
            var client = new Client(ConfigurationManager.AppSettings["apiKey"]);

            var recipients = new List<Recipient>() { new Recipient("test@null.postageapp.com") };
            if (TestRecipient != null) recipients.Add(TestRecipient);

            var headers = new Dictionary<string, string>()
                {
                    {"From", "test.robot@null.postageapp.com"},
                    {"Subject", "This is a custom subject line"}
                };

            var response = client.SendMessage(new SendMessageRequest()
            {
                Recipients = recipients,
                Headers = headers,
                Content = new Content() { Text = "This email should have a custom from name and subject line." }
            });

            Assert.AreEqual(SendMessageResponseStatus.Ok, response.Status);
        }

        [TestMethod]
        public void TestSendMessageWithHtmlContent()
        {
            var client = new Client(ConfigurationManager.AppSettings["apiKey"]);

            var recipients = new List<Recipient>() { new Recipient("test@null.postageapp.com") };
            if (TestRecipient != null) recipients.Add(TestRecipient);

            var response = client.SendMessage(new SendMessageRequest()
            {
                Headers = new Dictionary<string, string>() { { "Subject", "Html body"} },
                Recipients = recipients,
                Content = new Content()
                    {
                        Text = "This email should have ",
                        Html = "<h1>Title</h1><p>This is an <em>html email</em></p></h1>"
                    }
            });

            Assert.AreEqual(SendMessageResponseStatus.Ok, response.Status);
        }       
    
    }
}
