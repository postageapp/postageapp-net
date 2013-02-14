using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using TWG.PostageApp.Converters;
using TWG.PostageApp.Transmissions;

namespace TWG.PostageApp.Net45.Portable.Tests
{
    /// <summary>
    /// Represents message transmissions converter test.
    /// </summary>
    [TestClass]
    public class MessageTransmissionsConverterTest
    {
        /// <summary>
        /// JSON response serializer settings.
        /// </summary>
        private readonly JsonSerializerSettings _responseSerializerSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageTransmissionsConverterTest"/> class.
        /// </summary>
        public MessageTransmissionsConverterTest()
        {
            _responseSerializerSettings = new JsonSerializerSettings();

            _responseSerializerSettings.Converters.Add(new MessageTransmissionsConverter());
        }

        /// <summary>
        /// Test parses.
        /// </summary>
        [TestMethod]
        public void TestParser()
        {
            var jsonString = "{\"message\":{\"id\":30284902},\"transmissions\":" +
                             "{\"test@null.postageapp.com\":{\"status\":\"completed\",\"created_at\":\"2013-02-07 06:26:31\",\"failed_at\":null,\"opened_at\":null,\"result_code\":\"SMTP_250\",\"error_message\":\"Accepted\"}," +
                             "\"watson@dcw.ca\":{\"status\":\"completed\",\"created_at\":\"2013-02-07 06:26:31\",\"failed_at\":null,\"opened_at\":null,\"result_code\":\"SMTP_250\",\"error_message\":\"2.0.0 OK 1360218398 z5si12528528ani.189 - gsmtp\"}}}";
            var messageTransmissions = JsonConvert.DeserializeObject<MessageTransmissions>(jsonString, _responseSerializerSettings);
            Assert.AreEqual(uint.Parse("30284902"), messageTransmissions.Id);
            Assert.AreEqual(2, messageTransmissions.Count);

            var transmission = messageTransmissions.First();

            Assert.AreEqual("test@null.postageapp.com", transmission.Key);
            Assert.AreEqual("completed", transmission.Value.Status);
            Assert.AreEqual(DateTime.Parse("2013-02-07 06:26:31"), transmission.Value.CreatedAt);
            Assert.AreEqual(null, transmission.Value.FailedAt);
            Assert.AreEqual(null, transmission.Value.OpenedAt);
            Assert.AreEqual("SMTP_250", transmission.Value.ResultCode);
            Assert.AreEqual("Accepted", transmission.Value.ResultMessage);
        }
    }
}