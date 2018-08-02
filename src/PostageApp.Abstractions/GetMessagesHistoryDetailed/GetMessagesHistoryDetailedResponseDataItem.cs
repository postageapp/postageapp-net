using System;

namespace PostageApp.Abstractions
{
    public class GetMessagesHistoryDetailedResponseDataItem
    {
        public string UniqueId { get; set; }

        public long MessageId { get; set; }

        public string Uid { get; set; }

        public string Recipient { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
