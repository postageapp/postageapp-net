using System;

namespace PostageApp.Abstractions
{
    public class GetMessagesHistoryResponseDataItem
    {
        public long Id { get; set; }

        public string Uid { get; set; }

        public string Subject { get; set; }

        public DateTime CreatedAt { get; set; }

        public int Count { get; set; }

        public int QueuedCount { get; set; }

        public int FailedCount { get; set; }

        public int CompletedCount { get; set; }
    }
}
