namespace PostageApp.Abstractions
{
    public class MessageDeliveryStatusResponseDataDeliveryStatusItem
    {
        public string UniqueId { get; set; }
        public string Recipient { get; set; }
        public MessageDeliveryStatus Status { get; set; }
    }
}
