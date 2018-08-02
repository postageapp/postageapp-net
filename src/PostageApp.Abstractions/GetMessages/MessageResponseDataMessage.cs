using System;

namespace PostageApp.Abstractions.GetMessages
{
    public class MessageResponseDataMessage
    {
        public string ProjectId { get; set; }
        public string Template { get; set; }
        public long TransmissionsTotal { get; set; }
        public long TransmissionsFailed { get; set; }
        public long TransmissionsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? WillPurgeAt { get; set; }
    }
}
