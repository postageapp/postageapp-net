using Newtonsoft.Json;

namespace TWG.PostageApp.Metrics
{
    /// <summary>
    /// Represents metric.
    /// </summary>
    public class Metric
    {
        /// <summary>
        /// Gets or sets delivered statistics.
        /// </summary>
        [JsonProperty(PropertyName = "delivered")]
        public MetricStatistic Delivered { get; set; }

        /// <summary>
        /// Gets or sets opened statistics.
        /// </summary>
        [JsonProperty(PropertyName = "opened")]
        public MetricStatistic Opened { get; set; }

        /// <summary>
        /// Gets or sets failed statistics.
        /// </summary>
        [JsonProperty(PropertyName = "failed")]
        public MetricStatistic Failed { get; set; }

        /// <summary>
        /// Gets or sets rejected statistics.
        /// </summary>
        [JsonProperty(PropertyName = "rejected")]
        public MetricStatistic Rejected { get; set; }

        /// <summary>
        /// Gets or sets created statistics.
        /// </summary>
        [JsonProperty(PropertyName = "created")]
        public MetricStatistic Created { get; set; }

        /// <summary>
        /// Gets or sets queued statistics.
        /// </summary>
        [JsonProperty(PropertyName = "queued")]
        public MetricStatistic Queued { get; set; }

        /// <summary>
        /// Gets or sets clicked statistics.
        /// </summary>
        [JsonProperty(PropertyName = "clicked")]
        public MetricStatistic Clicked { get; set; }

        /// <summary>
        /// Gets or sets spammed statistics.
        /// </summary>
        [JsonProperty(PropertyName = "spammed")]
        public MetricStatistic Spammed { get; set; }
    }
}
