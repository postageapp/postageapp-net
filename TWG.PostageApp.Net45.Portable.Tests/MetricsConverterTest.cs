using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using TWG.PostageApp.Converters;

namespace TWG.PostageApp.Net45.Portable.Tests
{
    /// <summary>
    /// Represents metrics converter test.
    /// </summary>
    [TestClass]
    public class MetricsConverterTest
    {
        /// <summary>
        /// JSON response serializer settings.
        /// </summary>
        private readonly JsonSerializerSettings _responseSerializerSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="MetricsConverterTest"/> class.
        /// </summary>
        public MetricsConverterTest()
        {
            _responseSerializerSettings = new JsonSerializerSettings();

            _responseSerializerSettings.Converters.Add(new MetricsConverter());
        }

        /// <summary>
        /// Test parses.
        /// </summary>
        [TestMethod]
        public void TestParser()
        {
            var jsonString = "{\"metrics\":" +
                             "{\"hour\":{\"delivered\":" +
                                            "{\"current_percent\":100.0,\"previous_percent\":100.0,\"diff_percent\":0.0,\"current_value\":24,\"previous_value\":3}," +
                                        "\"opened\":" +
                                            "{\"current_percent\":0.0,\"previous_percent\":0.0,\"diff_percent\":0.0,\"current_value\":0,\"previous_value\":0},\"clicked\":{\"current_percent\":0.0,\"previous_percent\":0.0,\"diff_percent\":0.0,\"current_value\":0,\"previous_value\":0},\"failed\":{\"current_percent\":0.0,\"previous_percent\":0.0,\"diff_percent\":0.0,\"current_value\":0,\"previous_value\":0},\"rejected\":{\"current_percent\":0.0,\"previous_percent\":0.0,\"diff_percent\":0.0,\"current_value\":0,\"previous_value\":0},\"spammed\":{\"current_percent\":0.0,\"previous_percent\":0.0,\"diff_percent\":0.0,\"current_value\":0,\"previous_value\":0},\"created\":{\"current_percent\":0.0,\"previous_percent\":0.0,\"diff_percent\":0.0,\"current_value\":24,\"previous_value\":3},\"queued\":{\"current_percent\":0.0,\"previous_percent\":0.0,\"diff_percent\":0.0,\"current_value\":0,\"previous_value\":0}}," +
                             "\"date\":{\"delivered\":{\"current_percent\":100.0,\"previous_percent\":100.0,\"diff_percent\":0.0,\"current_value\":27,\"previous_value\":151},\"opened\":{\"current_percent\":0.0,\"previous_percent\":0.6622516556291391,\"diff_percent\":-0.6622516556291391,\"current_value\":0,\"previous_value\":1},\"clicked\":{\"current_percent\":0.0,\"previous_percent\":0.0,\"diff_percent\":0.0,\"current_value\":0,\"previous_value\":0},\"failed\":{\"current_percent\":0.0,\"previous_percent\":0.0,\"diff_percent\":0.0,\"current_value\":0,\"previous_value\":0},\"rejected\":{\"current_percent\":0.0,\"previous_percent\":0.0,\"diff_percent\":0.0,\"current_value\":0,\"previous_value\":0},\"spammed\":{\"current_percent\":0.0,\"previous_percent\":0.0,\"diff_percent\":0.0,\"current_value\":0,\"previous_value\":0},\"created\":{\"current_percent\":0.0,\"previous_percent\":0.0,\"diff_percent\":0.0,\"current_value\":27,\"previous_value\":151},\"queued\":{\"current_percent\":0.0,\"previous_percent\":0.0,\"diff_percent\":0.0,\"current_value\":0,\"previous_value\":0}},\"week\":{\"delivered\":{\"current_percent\":100.0,\"previous_percent\":16.878355375147308,\"diff_percent\":83.12164462485269,\"current_value\":194,\"previous_value\":5156},\"opened\":{\"current_percent\":0.5154639175257731,\"previous_percent\":0.006547073458164201,\"diff_percent\":0.508916844067609,\"current_value\":1,\"previous_value\":2},\"clicked\":{\"current_percent\":0.0,\"previous_percent\":0.0,\"diff_percent\":0.0,\"current_value\":0,\"previous_value\":0},\"failed\":{\"current_percent\":0.0,\"previous_percent\":0.0,\"diff_percent\":0.0,\"current_value\":0,\"previous_value\":0},\"rejected\":{\"current_percent\":0.0,\"previous_percent\":0.0098206101872463,\"diff_percent\":-0.0098206101872463,\"current_value\":0,\"previous_value\":3},\"spammed\":{\"current_percent\":0.0,\"previous_percent\":0.0,\"diff_percent\":0.0,\"current_value\":0,\"previous_value\":0},\"created\":{\"current_percent\":0.0,\"previous_percent\":0.0,\"diff_percent\":0.0,\"current_value\":194,\"previous_value\":30548},\"queued\":{\"current_percent\":0.0,\"previous_percent\":0.0,\"diff_percent\":0.0,\"current_value\":0,\"previous_value\":25389}},\"month\":{\"delivered\":{\"current_percent\":17.402901567887582,\"previous_percent\":99.99010194991587,\"diff_percent\":-82.58720038202829,\"current_value\":5350,\"previous_value\":10102},\"opened\":{\"current_percent\":0.00975863639320799,\"previous_percent\":0.009898050084133426,\"diff_percent\":-0.00013941369092543635,\"current_value\":3,\"previous_value\":1},\"clicked\":{\"current_percent\":0.0,\"previous_percent\":0.0,\"diff_percent\":0.0,\"current_value\":0,\"previous_value\":0},\"failed\":{\"current_percent\":0.0,\"previous_percent\":0.009898050084133426,\"diff_percent\":-0.009898050084133426,\"current_value\":0,\"previous_value\":1},\"rejected\":{\"current_percent\":0.00975863639320799,\"previous_percent\":0.0,\"diff_percent\":0.00975863639320799,\"current_value\":3,\"previous_value\":0},\"spammed\":{\"current_percent\":0.0,\"previous_percent\":0.0,\"diff_percent\":0.0,\"current_value\":0,\"previous_value\":0},\"created\":{\"current_percent\":0.0,\"previous_percent\":0.0,\"diff_percent\":0.0,\"current_value\":30742,\"previous_value\":10103},\"queued\":{\"current_percent\":0.0,\"previous_percent\":0.0,\"diff_percent\":0.0,\"current_value\":25389,\"previous_value\":0}}}}";
            var metrics = JsonConvert.DeserializeObject<Metrics.Metrics>(jsonString, _responseSerializerSettings);
            Assert.IsNotNull(metrics.Date);
            Assert.IsNotNull(metrics.Hour);
            Assert.IsNotNull(metrics.Month);
            Assert.IsNotNull(metrics.Week);

            Assert.IsNotNull(metrics.Date.Delivered);
            Assert.IsNotNull(metrics.Date.Clicked);
            Assert.IsNotNull(metrics.Date.Created);
            Assert.IsNotNull(metrics.Date.Failed);
            Assert.IsNotNull(metrics.Date.Opened);
            Assert.IsNotNull(metrics.Date.Queued);
            Assert.IsNotNull(metrics.Date.Rejected);
            Assert.IsNotNull(metrics.Date.Spammed);

            var delivered = metrics.Hour.Delivered;

            Assert.AreEqual(100.0, delivered.CurrentPercent);
            Assert.AreEqual(100.0, delivered.PreviousPercent);
            Assert.AreEqual(0.0, delivered.DiffPercent);
            Assert.AreEqual(24, delivered.CurrentValue);
            Assert.AreEqual(3, delivered.PreviousValue);

/*            Assert.AreEqual(2, metrics.Count);

            var transmission = metrics.First();

            Assert.AreEqual("test@null.postageapp.com", transmission.Key);
            Assert.AreEqual("completed", transmission.Value.Status);
            Assert.AreEqual(DateTime.Parse("2013-02-07 06:26:31"), transmission.Value.CreatedAt);
            Assert.AreEqual(null, transmission.Value.FailedAt);
            Assert.AreEqual(null, transmission.Value.OpenedAt);
            Assert.AreEqual("SMTP_250", transmission.Value.ResultCode);
            Assert.AreEqual("Accepted", transmission.Value.ResultMessage);*/
        }
    }
}