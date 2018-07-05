namespace PostageApp.Abstractions
{
    public class MessageAttachment
    {
        public MessageAttachment(byte[] content, string contentType)
        {
            Content = content;
            ContentType = contentType;
        }

        public byte[] Content { get; }

        public string ContentType { get; }
    }
}
