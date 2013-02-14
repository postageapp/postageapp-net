using System;
using Newtonsoft.Json;

namespace TWG.PostageApp.Message
{
    /// <summary>
    /// Represents message info.
    /// </summary>
    public class MessageInfo
    {
        /// <summary>
        /// Gets or sets project id.
        /// </summary>
        [JsonProperty(PropertyName = "project_id")]
        public int ProjectId { get; set; }

        /// <summary>
        /// Gets or sets template.
        /// </summary>]
        [JsonProperty(PropertyName = "template")]
        public string Template { get; set; }

        /// <summary>
        /// Gets or sets total transmissions count.
        /// </summary>
        [JsonProperty(PropertyName = "transmissions_total")]
        public int TotalTransmissionsCount { get; set; }

        /// <summary>
        /// Gets or sets failed transmissions count.
        /// </summary>
        [JsonProperty(PropertyName = "transmissions_failed")]
        public int FailedTransmissionsCount { get; set; }

        /// <summary>
        /// Gets or sets completed transmissions count.
        /// </summary>
        [JsonProperty(PropertyName = "transmissions_completed")]
        public int CompletedTransmissionsCount { get; set; }

        /// <summary>
        /// Gets or sets creation date.
        /// </summary>
        [JsonProperty(PropertyName = "created_at")]
        public DateTime CreateAt { get; set; }

        /// <summary>
        /// Gets or sets purge date.
        /// </summary>
        [JsonProperty(PropertyName = "will_purge_at")]
        public DateTime WillPurgeAt { get; set; }
    }
}