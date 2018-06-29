using System;

namespace PostageApp.Abstractions
{
    public class MessageTransmissionsDataTransmission
    {
        public string Status { get; set; }
        public string ResultCode { get; set; }
        public string ResultMessage { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? FailedAt { get; set; }
        public DateTime? OpenedAt { get; set; }
    }
}
