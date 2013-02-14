using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using TWG.PostageApp.Converters;
using TWG.PostageApp.Project;

namespace TWG.PostageApp.Net45.Portable.Tests
{
    /// <summary>
    /// Represents project info converter test.
    /// </summary>
    [TestClass]
    public class ProjectInfoConverterTest
    {
        /// <summary>
        /// JSON response serializer settings.
        /// </summary>
        private readonly JsonSerializerSettings _responseSerializerSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectInfoConverterTest"/> class.
        /// </summary>
        public ProjectInfoConverterTest()
        {
            _responseSerializerSettings = new JsonSerializerSettings();

            _responseSerializerSettings.Converters.Add(new ProjectInfoConverter());
        }

        /// <summary>
        /// Test parses.
        /// </summary>
        [TestMethod]
        public void TestParser()
        {
            var jsonString = "{\"project\":{\"name\":\"Test\",\"url\":\"https://api.postageapp.com/projects/1212\",\"transmissions\":{\"today\":27,\"this_month\":5353,\"overall\":15551},\"users\":{\"postage+tester@twg.ca\":\"Test User\",\"cloudy@mailinator.com\":\"\\u2601\"}}}";
            var projectInfo = JsonConvert.DeserializeObject<ProjectInfo>(jsonString, _responseSerializerSettings);

            Assert.AreEqual("Test", projectInfo.Name);
            Assert.AreEqual("https://api.postageapp.com/projects/1212", projectInfo.Url);
            Assert.AreEqual(15551, projectInfo.Transmissions.OverallCount);
            Assert.AreEqual(5353, projectInfo.Transmissions.ThisMonthCount);
            Assert.AreEqual(27, projectInfo.Transmissions.TodayCount);
            Assert.AreEqual(2, projectInfo.Users.Count);

            var user = projectInfo.Users.First();

            Assert.AreEqual("postage+tester@twg.ca", user.Key);
            Assert.AreEqual("Test User", user.Value);
        }
    }
}
