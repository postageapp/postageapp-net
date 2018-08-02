using System;

namespace PostageApp.Abstractions
{
    public class MessageTransmissionsDataTransmission
    {
        // TODO enum
        public string Status { get; set; }
        public string ResultCode { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? FailedAt { get; set; }
        public DateTime? OpenedAt { get; set; }
        public DateTime? ClickedAt { get; set; }
    }
}
