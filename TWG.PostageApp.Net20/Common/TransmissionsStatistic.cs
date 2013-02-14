using Newtonsoft.Json;

namespace TWG.PostageApp.Common
{
    /// <summary>
    /// Represents transmissions information.
    /// </summary>
    public struct TransmissionsStatistic
    {
        /// <summary>
        /// Gets or sets transmission count for today.
        /// </summary>
        [JsonProperty(PropertyName = "today")]
        public int TodayCount { get; set; }

        /// <summary>
        /// Gets or sets transmission count for this month.
        /// </summary>
        [JsonProperty(PropertyName = "this_month")]
        public int ThisMonthCount { get; set; }

        /// <summary>
        /// Gets or sets transmission count for all time.
        /// </summary>
        [JsonProperty(PropertyName = "overall")]
        public int OverallCount { get; set; }
    }
}