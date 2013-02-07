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

            var request = new SendMessageRequest();
            client.SendMessage(request);

            Assert.IsTrue(true);
        }
    }
}
