using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TWG.PostageApp.Common;
using TWG.PostageApp.Converters;
using TWG.PostageApp.Message;

namespace TWG.PostageApp.Net45.Portable.Tests
{
    /// <summary>
    /// Represents response converter test.
    /// </summary>
    [TestClass]
    public class PostageResponseConverterTest
    {
        /// <summary>
        /// JSON response serializer settings.
        /// </summary>
        private readonly JsonSerializerSettings _responseSerializerSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostageResponseConverterTest"/> class.
        /// </summary>
        public PostageResponseConverterTest()
        {
            _responseSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            _responseSerializerSettings.Converters.Add(new PostageResponseConverter());
            _responseSerializerSettings.Converters.Add(new MessageResponseConverter());
        }

        /// <summary>
        /// Test parses message id.
        /// </summary>
        [TestMethod]
        public void TestParsesMessageId()
        {
            var messageResponseContainer = JsonConvert.DeserializeObject<ResponseContainer<MessageResponse>>("{ \"data\": { \"message\": { \"id\": \"1234567890\" } } }", _responseSerializerSettings);
            Assert.AreEqual((uint)1234567890, messageResponseContainer.Data.Id);
        }

        /// <summary>
        /// Test parses statuses.
        /// </summary>
        [TestMethod]
        public void TestParsesStatuses()
        {
            var messageResponseContainer = JsonConvert.DeserializeObject<ResponseContainer<MessageResponse>>("{ \"response\": { \"status\": \"ok\" } }", _responseSerializerSettings);
            Assert.AreEqual(ResponseStatus.Ok, messageResponseContainer.Response.Status);
            messageResponseContainer = JsonConvert.DeserializeObject<ResponseContainer<MessageResponse>>("{ \"response\": { \"status\": \"bad_request\" } }", _responseSerializerSettings);
            Assert.AreEqual(ResponseStatus.BadRequest, messageResponseContainer.Response.Status);
            messageResponseContainer = JsonConvert.DeserializeObject<ResponseContainer<MessageResponse>>("{ \"response\": { \"status\": \"not_found\" } }", _responseSerializerSettings);
            Assert.AreEqual(ResponseStatus.NotFound, messageResponseContainer.Response.Status);
            messageResponseContainer = JsonConvert.DeserializeObject<ResponseContainer<MessageResponse>>("{ \"response\": { \"status\": \"invalid_json\" } }", _responseSerializerSettings);
            Assert.AreEqual(ResponseStatus.InvalidJson, messageResponseContainer.Response.Status);
            messageResponseContainer = JsonConvert.DeserializeObject<ResponseContainer<MessageResponse>>("{ \"response\": { \"status\": \"unauthorized\" } }", _responseSerializerSettings);
            Assert.AreEqual(ResponseStatus.Unauthorized, messageResponseContainer.Response.Status);
            messageResponseContainer = JsonConvert.DeserializeObject<ResponseContainer<MessageResponse>>("{ \"response\": { \"status\": \"call_error\" } }", _responseSerializerSettings);
            Assert.AreEqual(ResponseStatus.CallError, messageResponseContainer.Response.Status);
            messageResponseContainer = JsonConvert.DeserializeObject<ResponseContainer<MessageResponse>>("{ \"response\": { \"status\": \"precondition_failed\" } }", _responseSerializerSettings);
            Assert.AreEqual(ResponseStatus.PreconditionFailed, messageResponseContainer.Response.Status);
            messageResponseContainer = JsonConvert.DeserializeObject<ResponseContainer<MessageResponse>>("{ \"response\": { \"status\": \"something unexpected\" } }", _responseSerializerSettings);
            Assert.AreEqual(ResponseStatus.Unknown, messageResponseContainer.Response.Status);
        }

        /// <summary>
        /// Test parses UID.
        /// </summary>
        [TestMethod]
        public void TestParsesUid()
        {
            var messageResponseContainer = JsonConvert.DeserializeObject<ResponseContainer<MessageResponse>>("{ \"response\": { \"uid\": \"27cf6ede7501a32d54d22abe17e3c154d2cae7f3\" } }", _responseSerializerSettings);
            Assert.AreEqual("27cf6ede7501a32d54d22abe17e3c154d2cae7f3", messageResponseContainer.Response.Uid);
        }

        /// <summary>
        /// Test parses error message.
        /// </summary>
        [TestMethod]
        public void TestParsesErrorMessage()
        {
            var messageResponseContainer = JsonConvert.DeserializeObject<ResponseContainer<MessageResponse>>("{ \"response\": { \"message\": \"Something went wrong!\" } }", _responseSerializerSettings);
            Assert.AreEqual("Something went wrong!", messageResponseContainer.Response.ErrorMessage);
        }
    }
}
