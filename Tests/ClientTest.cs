using System.Collections.Generic;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PostageApp;

namespace Tests
{
    [TestClass]
    public class ClientTest
    {
        [TestMethod]
        public void TestSendMessage()
        {
            var client = new Client(ConfigurationManager.AppSettings["apiKey"]);
            
            var response = client.SendMessage(new SendMessageRequest()
                {
                    Recipients = new List<Recipient>() { new Recipient(@"test@null.postageapp.com") },
                    Content = new Content() { Text = "This is my text content" }
                });

            Assert.AreEqual(SendMessageResponseStatus.Ok, response.Status);
            Assert.IsNotNull(response.Uid);
            Assert.IsTrue(response.Uid.Length > 0);
            Assert.IsNotNull(response.MessageId);
            Assert.IsTrue(response.MessageId > 0);
        }    
    }
}
