using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using TWG.PostageApp.Converters;

namespace TWG.PostageApp.Net45.Portable.Tests
{
    /// <summary>
    /// Represents account info converter test.
    /// </summary>
    [TestClass]
    public class AccountInfoConverterTest
    {
        /// <summary>
        /// JSON response serializer settings.
        /// </summary>
        private readonly JsonSerializerSettings _responseSerializerSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountInfoConverterTest"/> class.
        /// </summary>
        public AccountInfoConverterTest()
        {
            _responseSerializerSettings = new JsonSerializerSettings();

            _responseSerializerSettings.Converters.Add(new AccountInfoConverter());
        }

        /// <summary>
        /// Test parses.
        /// </summary>
        [TestMethod]
        public void TestParser()
        {
            var jsonString = "{\"account\":{\"name\":\"test\",\"url\":\"https://test.postageapp.com/\",\"transmissions\":{\"today\":27,\"this_month\":5356,\"overall\":15773},\"users\":{\"postage+tester@twg.ca\":\"Test User\",\"cloudy@mailinator.com\":\"\\u2601\"}}}";
            var accountInfo = JsonConvert.DeserializeObject<AccountInfo>(jsonString, _responseSerializerSettings);

            Assert.AreEqual("test", accountInfo.Name);
            Assert.AreEqual("https://test.postageapp.com/", accountInfo.Url);
            Assert.AreEqual(15773, accountInfo.Transmissions.OverallCount);
            Assert.AreEqual(5356, accountInfo.Transmissions.ThisMonthCount);
            Assert.AreEqual(27, accountInfo.Transmissions.TodayCount);
            Assert.AreEqual(2, accountInfo.Users.Count);

            var user = accountInfo.Users.First();

            Assert.AreEqual("postage+tester@twg.ca", user.Key);
            Assert.AreEqual("Test User", user.Value);
        }
    }
}
