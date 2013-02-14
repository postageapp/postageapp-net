using System;
using Newtonsoft.Json;

namespace TWG.PostageApp.Transmissions
{
    /// <summary>
    /// Represents
    /// </summary>
    public class MessageTransmission
    {
        /// <summary>
        /// Gets or sets transmission status.
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets date when the transmission was created.
        /// </summary>
        [JsonProperty(PropertyName = "created_at")]
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets date when the transmission was failed.
        /// </summary>
        [JsonProperty(PropertyName = "failed_at")]
        public DateTime? FailedAt { get; set; }

        /// <summary>
        /// Gets or sets date when the transmission was opened.
        /// </summary>
        [JsonProperty(PropertyName = "opened_at")]
        public DateTime? OpenedAt { get; set; }

        /// <summary>
        /// Gets or sets transmission postal result code.
        /// </summary>
        [JsonProperty(PropertyName = "result_code")]
        public string ResultCode { get; set; }

        /// <summary>
        /// Gets or sets transmission result message.
        /// </summary>
        [JsonProperty(PropertyName = "error_message")]
        public string ResultMessage { get; set; }
    }
}
