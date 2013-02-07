using Microsoft.VisualStudio.TestTools.UnitTesting;
using PostageApp;


namespace Tests
{
    [TestClass]
    public class SendMessageResponseTest
    {
        [TestMethod]
        public void TestParsesMessageId()
        {
            var response = new SendMessageResponse(@"{ ""data"": { ""message"": { ""id"": ""1234567890"" } } }");
            Assert.AreEqual((uint)1234567890, response.MessageId);               
        }

        [TestMethod]
        public void TestParsesStatuses()
        {
            Assert.AreEqual(SendMessageResponseStatus.Ok, new SendMessageResponse(@"{ ""response"": { ""status"": ""ok"" } }").Status);
            Assert.AreEqual(SendMessageResponseStatus.BadRequest, new SendMessageResponse(@"{ ""response"": { ""status"": ""bad_request"" } }").Status);
            Assert.AreEqual(SendMessageResponseStatus.NotFound, new SendMessageResponse(@"{ ""response"": { ""status"": ""not_found"" } }").Status);
            Assert.AreEqual(SendMessageResponseStatus.InvalidJson, new SendMessageResponse(@"{ ""response"": { ""status"": ""invalid_json"" } }").Status);
            Assert.AreEqual(SendMessageResponseStatus.Unauthorized, new SendMessageResponse(@"{ ""response"": { ""status"": ""unauthorized"" } }").Status);
            Assert.AreEqual(SendMessageResponseStatus.CallError, new SendMessageResponse(@"{ ""response"": { ""status"": ""call_error"" } }").Status);
            Assert.AreEqual(SendMessageResponseStatus.PreconditionFailed, new SendMessageResponse(@"{ ""response"": { ""status"": ""precondition_failed"" } }").Status);
            Assert.AreEqual(SendMessageResponseStatus.Unknown, new SendMessageResponse(@"{ ""response"": { ""status"": ""something unexpected"" } }").Status);
        }

        [TestMethod]
        public void TestParsesUid()
        {
            var response = new SendMessageResponse(@"{ ""response"": { ""uid"": ""27cf6ede7501a32d54d22abe17e3c154d2cae7f3"" } }");
            Assert.AreEqual("27cf6ede7501a32d54d22abe17e3c154d2cae7f3", response.Uid);
        }

        [TestMethod]
        public void TestParsesErrorMessage()
        {
            var response = new SendMessageResponse(@"{ ""response"": { ""message"": ""Something went wrong!"" } }");
            Assert.AreEqual("Something went wrong!", response.ErrorMessage);
        }
    }
}
