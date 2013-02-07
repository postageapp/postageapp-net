using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using PostageApp;
using Tests.Extensions;

namespace Tests
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
        public void TestJsonSerializationIncludesUid()
        {
            const string uid = "27cf6ede7501a32d54d22abe17e3c154d2cae7f3";
            var r = new SendMessageRequest() {Uid = uid};

            var json = r.ToJson(ApiKey);
            var o = JObject.Parse(json);

            Assert.AreEqual(uid, o["uid"]);
        }

        [TestMethod]
        public void TestJsonSerializationIncludesText()
        {
            var r = new SendMessageRequest() { Text = "my content" };

            var json = r.ToJson(ApiKey);
            var o = JObject.Parse(json);

            Assert.IsNotNull(o["arguments"]["content"]);
            Assert.AreEqual(o["arguments"]["content"]["text/plain"], "my content");
        }

        [TestMethod]
        public void TestJsonSerializationIncludesHtml()
        {
            var r = new SendMessageRequest() { Html = "<h1>my content</h1>" };

            var json = r.ToJson(ApiKey);
            var o = JObject.Parse(json);

            Assert.IsNotNull(o["arguments"]["content"]);
            Assert.AreEqual(o["arguments"]["content"]["text/html"], "<h1>my content</h1>");
        }

        [TestMethod]
        public void TestJsonSerializationIncludesRecipient()
        {
            var r = new SendMessageRequest() { Recipient = "test@null.postageapp.com" };

            var json = r.ToJson(ApiKey);
            var o = JObject.Parse(json);

            Assert.IsNotNull(o["arguments"]["recipients"]["test@null.postageapp.com"]);
        }

        [TestMethod]
        public void TestJsonSerializationIncludesRecipients()
        {
            var recipients = new List<Recipient>
                {
                    new Recipient("test@null.postageapp.com")
                        {
                            Variables = new Dictionary<string, string>()
                                {
                                    {"actor", "Steve Gutenberg"}
                                }
                        },
                    new Recipient("test2@null.postageapp.com")
                        {
                            Variables = new Dictionary<string, string>()
                                {
                                    {"actor", "Steve Martin"}
                                }
                        }
                };

            var r = new SendMessageRequest() { Recipients = recipients };

            var json = r.ToJson(ApiKey);
            var o = JObject.Parse(json);

            Assert.AreEqual("Steve Gutenberg", o["arguments"]["recipients"]["test@null.postageapp.com"]["actor"]);
            Assert.AreEqual("Steve Martin", o["arguments"]["recipients"]["test2@null.postageapp.com"]["actor"]);
        }

        [TestMethod]
        public void TestJsonSerializationIncludesRecipientOverride()
        {
            var r = new SendMessageRequest() { RecipientOverride = "test@null.postageapp.com" };

            var json = r.ToJson(ApiKey);
            var o = JObject.Parse(json);

            Assert.AreEqual(ApiKey, o["arguments"]["recipient_override"]);
        }


        [TestMethod]
        public void TestJsonSerializationIncludesTemplate()
        {
            const string slug = "some-template-slug";
            var r = new SendMessageRequest() { Template = slug };

            var json = r.ToJson(ApiKey);
            var o = JObject.Parse(json);

            Assert.AreEqual(slug, o["arguments"]["template"]);
        }

        [TestMethod]
        public void TestJsonSerializationIncludesVariables()
        {
            var r = new SendMessageRequest() { 
                Variables = new Dictionary<string, string>()
                {
                    { "movie", "Pee Wee's Big Adventure" },
                    { "actor", "Meryl Streep" }
                }
            };

            var json = r.ToJson(ApiKey);
            var o = JObject.Parse(json);

            Assert.AreEqual("Meryl Streep", o["arguments"]["variables"]["actor"]);
            Assert.AreEqual("Pee Wee's Big Adventure", o["arguments"]["variables"]["movie"]);
        }

        [TestMethod]
        public void TestJsonSerializationIncludesFromHeader()
        {
            var r = new SendMessageRequest() { From = "test@null.postageapp.com" };

            var json = r.ToJson(ApiKey);
            var o = JObject.Parse(json);

            Assert.IsNotNull(o["arguments"]["headers"]);
            Assert.AreEqual("test@null.postageapp.com", o["arguments"]["headers"]["From"]);
        }

        [TestMethod]
        public void TestJsonSerializationIncludesSubjectHeader()
        {
            var r = new SendMessageRequest() { Subject = "Hello world" };

            var json = r.ToJson(ApiKey);
            var o = JObject.Parse(json);

            Assert.IsNotNull(o["arguments"]["headers"]);
            Assert.AreEqual("Hello world", o["arguments"]["headers"]["Subject"]);
        }

        [TestMethod]
        public void TestJsonSerializationIncludesReplyToHeader()
        {
            var r = new SendMessageRequest() { ReplyTo = "test@null.postageapp.com" };

            var json = r.ToJson(ApiKey);
            var o = JObject.Parse(json);

            Assert.IsNotNull(o["arguments"]["headers"]);
            Assert.AreEqual("test@null.postageapp.com", o["arguments"]["headers"]["Reply-To"]);
        }

        [TestMethod]
        public void TestJsonSerializationIncludesHeaders()
        {
            var r = new SendMessageRequest()
            {
                Headers = new Dictionary<string, string>()
                {
                    { "Subject", "Hello friend!"},
                    { "X-Accept-Language", "en-us, en" }
                }
            };

            var json = r.ToJson(ApiKey);
            var o = JObject.Parse(json);

            Assert.AreEqual("Hello friend!", o["arguments"]["headers"]["Subject"]);
            Assert.AreEqual("en-us, en", o["arguments"]["headers"]["X-Accept-Language"]);
        }

        [TestMethod]
        public void TestJsonSerializationIncludesAttachments()
        {
            const string fileContents = "this is my file content";

            var r = new SendMessageRequest
                {
                    Attachments = new List<Attachment>()
                        {
                            new Attachment(fileContents.ToStream(), "notes.txt", "text/plain")
                        }
                };

            var json = r.ToJson(ApiKey);
            var o = JObject.Parse(json);

            Assert.AreEqual("text/plain", o["arguments"]["attachments"]["notes.txt"]["content_type"]);
            Assert.AreEqual("dGhpcyBpcyBteSBmaWxlIGNvbnRlbnQ=", o["arguments"]["attachments"]["notes.txt"]["content"]);
        }
    }
}