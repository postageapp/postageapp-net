using Microsoft.VisualStudio.TestTools.UnitTesting;
using PostageApp;
using PostageApp.DTO;

namespace Tests
{
    [TestClass]
    public class ClientTest
    {
        [TestMethod]
        public void TestSendMessage()
        {
            var client = new Client("abc123");

            var request = new SendMessageRequest();
            client.SendMessage(request);

            Assert.IsTrue(true);
        }
    }
}
