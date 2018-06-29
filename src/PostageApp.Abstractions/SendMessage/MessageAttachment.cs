namespace PostageApp.Abstractions
{
    public class MessageAttachment
    {
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets BASE64_ENCODED_CONTENT
        /// </summary>
        public string Content { get; set; }
    }
}
