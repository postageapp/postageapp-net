using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TWG.PostageApp.Converters;
using TWG.PostageApp.Message;

namespace TWG.PostageApp.Net45.Portable.Tests
{
    /// <summary>
    /// Represents message converter test.
    /// </summary>
    [TestClass]
    public class MessageConverterTest
    {
        /// <summary>
        /// Test project api key.
        /// </summary>
        private const string API_KEY = "abc123";

        /// <summary>
        /// JSON request serializer settings.
        /// </summary>
        private readonly JsonSerializerSettings _requestSerializerSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageConverterTest"/> class.
        /// </summary>
        public MessageConverterTest()
        {
            _requestSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            _requestSerializerSettings.Converters.Add(new MessageConverter(API_KEY));
        }

        /// <summary>
        /// Test JSON serialization includes api key.
        /// </summary>
        [TestMethod]
        public void TestJsonSerializationIncludesApiKey()
        {
            var message = new Message.Message();

            var messageJson = JsonConvert.SerializeObject(message, _requestSerializerSettings);
            Assert.IsTrue(messageJson.Contains(string.Format("\"api_key\":\"{0}\"", API_KEY)));
        }

        /// <summary>
        /// Test JSON serialization includes text.
        /// </summary>
        [TestMethod]
        public void TestJsonSerializationIncludesText()
        {
            var text = "my content";
            var message = new Message.Message { Text = text };

            var messageJson = JsonConvert.SerializeObject(message, _requestSerializerSettings);

            Assert.IsTrue(messageJson.Contains(string.Format("\"arguments\":{{\"content\":{{\"text/plain\":\"{0}\"}}}}", text)));
        }

        /// <summary>
        /// Test JSON serialization includes text.
        /// </summary>
        [TestMethod]
        public void TestJsonSerializationIncludesHtml()
        {
            var text = "<h1>my content</h1>";
            var message = new Message.Message { Html = text };

            var messageJson = JsonConvert.SerializeObject(message, _requestSerializerSettings);

            Assert.IsTrue(messageJson.Contains(string.Format("\"arguments\":{{\"content\":{{\"text/html\":\"{0}\"}}}}", text)));
        }

        /// <summary>
        /// Test JSON serialization includes recipient.
        /// </summary>
        [TestMethod]
        public void TestJsonSerializationIncludesRecipient()
        {
            var recipient = "test@null.postageapp.com";
            var message = new Message.Message { Recipient = new Recipient(recipient) };

            var messageJson = JsonConvert.SerializeObject(message, _requestSerializerSettings);

            Assert.IsTrue(messageJson.Contains(string.Format("{{\"recipients\":{{\"{0}\":{{}}}}}}", recipient)));
        }

        /// <summary>
        /// Test JSON serialization includes recipients.
        /// </summary>
        [TestMethod]
        public void TestJsonSerializationIncludesRecipients()
        {
            var email1 = "test@null.postageapp.com";
            var recipient1 = new Recipient(email1);
            recipient1.Variables.Add("actor", "Steve Gutenberg");

            var email2 = "test2@null.postageapp.com";
            var recipient2 = new Recipient(email2);
            recipient2.Variables.Add("actor", "Steve Martin");

            var message = new Message.Message();
            message.Recipients.Add(recipient1);
            message.Recipients.Add(recipient2);
            var messageJson = JsonConvert.SerializeObject(message, _requestSerializerSettings);
            Assert.IsTrue(messageJson.Contains("\"arguments\":" +
                                               "{\"recipients\":{\"test@null.postageapp.com\":{\"actor\":\"Steve Gutenberg\"}," +
                                               "\"test2@null.postageapp.com\":{\"actor\":\"Steve Martin\"}}}"));
        }

        /// <summary>
        /// Test JSON serialization includes recipient override.
        /// </summary>
        [TestMethod]
        public void TestJsonSerializationIncludesRecipientOverride()
        {
            var recipientOverride = "test@null.postageapp.com";
            var message = new Message.Message { RecipientOverride = recipientOverride };

            var messageJson = JsonConvert.SerializeObject(message, _requestSerializerSettings);

            Assert.IsTrue(messageJson.Contains(string.Format(":{{\"recipient_override\":\"{0}\"}}", recipientOverride)));
        }

        /// <summary>
        /// Test JSON serialization includes template.
        /// </summary>
        [TestMethod]
        public void TestJsonSerializationIncludesTemplate()
        {
            var slug = "some-template-slug";
            var message = new Message.Message { Template = slug };

            var messageJson = JsonConvert.SerializeObject(message, _requestSerializerSettings);

            Assert.IsTrue(messageJson.Contains(string.Format(":{{\"template\":\"{0}\"}}", slug)));
        }

        /// <summary>
        /// Test JSON serialization includes variables.
        /// </summary>
        [TestMethod]
        public void TestJsonSerializationIncludesVariables()
        {
            var message = new Message.Message();
            message.Variables.Add("movie", "Pee Wee's Big Adventure");
            message.Variables.Add("actor", "Meryl Streep");

            var messageJson = JsonConvert.SerializeObject(message, _requestSerializerSettings);
            Assert.IsTrue(
                messageJson.Contains(
                    ":{\"variables\":{\"movie\":\"Pee Wee's Big Adventure\",\"actor\":\"Meryl Streep\"}}"));
        }

        /// <summary>
        /// Test JSON serialization includes subject header.
        /// </summary>
        [TestMethod]
        public void TestJsonSerializationIncludesSubjectHeader()
        {
            var text = "my content";
            var message = new Message.Message { Subject = text };

            var messageJson = JsonConvert.SerializeObject(message, _requestSerializerSettings);

            Assert.IsTrue(messageJson.Contains(string.Format("\"arguments\":{{\"headers\":{{\"subject\":\"{0}\"}}}}", text)));
        }

        /// <summary>
        /// Test JSON serialization includes reply to header.
        /// </summary>
        [TestMethod]
        public void TestJsonSerializationIncludesReplyToHeader()
        {
            var replyTo = "test@null.postageapp.com";
            var message = new Message.Message { ReplyTo = replyTo };

            var messageJson = JsonConvert.SerializeObject(message, _requestSerializerSettings);

            Assert.IsTrue(messageJson.Contains(string.Format("\"arguments\":{{\"headers\":{{\"reply-to\":\"{0}\"}}}}", replyTo)));
        }

        /// <summary>
        /// Test JSON serialization includes headers.
        /// </summary>
        [TestMethod]
        public void TestJsonSerializationIncludesHeaders()
        {
            var message = new Message.Message();
            message.Headers.Add("Subject", "Hello friend!");
            message.Headers.Add("X-Accept-Language", "en-us, en");

            var messageJson = JsonConvert.SerializeObject(message, _requestSerializerSettings);

            Assert.IsTrue(messageJson.Contains("{\"headers\":{\"Subject\":\"Hello friend!\",\"X-Accept-Language\":\"en-us, en\"}}"));
        }

        /// <summary>
        /// Test JSON serialization includes attachments.
        /// </summary>
        [TestMethod]
        public void TestJsonSerializationIncludesAttachments()
        {
            var fileContent = "this is my file content";
            
            var contentBytes = Encoding.UTF8.GetBytes(fileContent);
            var stream = new MemoryStream(contentBytes);
            var message = new Message.Message();
            message.Attachments.Add(new Attachment(stream, "notes.txt", "text/plain"));

            var messageJson = JsonConvert.SerializeObject(message, _requestSerializerSettings);
            Assert.IsTrue(
                messageJson.Contains(
                    "{\"attachments\":{\"notes.txt\":{\"content_type\":\"text/plain\",\"content\":\"dGhpcyBpcyBteSBmaWxlIGNvbnRlbnQ=\"}}}"));
        }
    }
}
