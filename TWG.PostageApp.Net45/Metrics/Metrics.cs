using Newtonsoft.Json;

namespace TWG.PostageApp.Metrics
{
    /// <summary>
    /// Represents project metrics.
    /// </summary>
    public class Metrics
    {
        /// <summary>
        /// Gets or sets metric for last hour.
        /// </summary>
        [JsonProperty(PropertyName = "hour")]
        public Metric Hour { get; set; }

        /// <summary>
        /// Gets or sets metric for last day.
        /// </summary>
        [JsonProperty(PropertyName = "date")]
        public Metric Date { get; set; }

        /// <summary>
        /// Gets or sets metric for last week.
        /// </summary>
        [JsonProperty(PropertyName = "week")]
        public Metric Week { get; set; }

        /// <summary>
        /// Gets or sets metric for last month.
        /// </summary>
        [JsonProperty(PropertyName = "month")]
        public Metric Month { get; set; }
    }
}