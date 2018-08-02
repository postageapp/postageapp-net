using System;

namespace PostageApp.Abstractions
{
    public class GetSuppressionListResponseDataItem
    {
        public string Status { get; set; }
        public string SuppressionState { get; set; }
        public DateTime? BlockedUntil { get; set; }
        public string Message { get; set; }
    }
}
