using Newtonsoft.Json;

namespace TWG.PostageApp.Metrics
{
    /// <summary>
    /// Represents metric statistics.
    /// </summary>
    public struct MetricStatistic
    {
        /// <summary>
        /// Gets or sets current percent.
        /// </summary>
        [JsonProperty(PropertyName = "current_percent")]
        public float CurrentPercent { get; set; }

        /// <summary>
        /// Gets or sets previous percent.
        /// </summary>
        [JsonProperty(PropertyName = "previous_percent")]
        public float PreviousPercent { get; set; }

        /// <summary>
        /// Gets or sets percent difference.
        /// </summary>
        [JsonProperty(PropertyName = "diff_percent")]
        public float DiffPercent { get; set; }

        /// <summary>
        /// Gets or sets current value.
        /// </summary>
        [JsonProperty(PropertyName = "current_value")]
        public int CurrentValue { get; set; }

        /// <summary>
        /// Gets or sets previous percent.
        /// </summary>
        [JsonProperty(PropertyName = "previous_value")]
        public int PreviousValue { get; set; }
    }
}