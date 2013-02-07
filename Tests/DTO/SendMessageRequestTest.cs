using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using PostageApp.DTO;

namespace Tests.DTO
{
    [TestClass]
    public class SendMessageRequestTest
    {
        private const string ApiKey = "abc123";

        [TestMethod]
        public void TestJsonSerializationIncludesApiKey()
        {
            var r = new SendMessageRequest();

            var json = r.ToJson(ApiKey);
            var o = JObject.Parse(json);

            Assert.AreEqual(ApiKey, o["api_key"]);
        }

        [TestMethod]
        public void TestJsonSerializationIncludesContent()
        {
            var r = new SendMessageRequest()
                {
                    Content = new Content()
                    {
                        Text = "my content",
                        Html = "<h1>my content</h1>"
                    }
                };

            var json = r.ToJson(ApiKey);
            var o = JObject.Parse(json);

            Assert.IsNotNull(o["content"]);
            Assert.AreEqual(o["content"]["text/plain"], "my content");
            Assert.AreEqual(o["content"]["text/html"], "<h1>my content</h1>");
        }

        [TestMethod]
        public void TestJsonSerializationIncludesUid()
        {
            const string uid = "27cf6ede7501a32d54d22abe17e3c154d2cae7f3";
            var r = new SendMessageRequest() {Uid = uid};

            var json = r.ToJson(ApiKey);
            var o = JObject.Parse(json);

            Assert.AreEqual(uid, o["uid"]);
        }

        [TestMethod]
        public void TestJsonSerializationIncludesRecipients()
        {

            var recipients = new List<Recipient>();

            recipients.Add(new Recipient("test@null.postageapp.com")
                {
                    Variables = new Dictionary<string, string>()
                        {
                            {"actor", "Steve Gutenberg"}
                        }
                });

            recipients.Add(new Recipient("test2@null.postageapp.com")
            {
                Variables = new Dictionary<string, string>()
                        {
                            {"actor", "Steve Martin"}
                        }
            });


            var r = new SendMessageRequest()
                {
                    Recipients = recipients
                };

            var json = r.ToJson(ApiKey);
            var o = JObject.Parse(json);

            Assert.AreEqual("Steve Gutenberg", o["arguments"]["recipients"]["test@null.postageapp.com"]["actor"]);
            Assert.AreEqual("Steve Martin", o["arguments"]["recipients"]["test2@null.postageapp.com"]["actor"]);
        }

    }
}