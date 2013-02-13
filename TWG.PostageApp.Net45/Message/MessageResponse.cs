namespace TWG.PostageApp.Message
{
    /// <summary>
    /// Represents message response.
    /// </summary>
    public class MessageResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageResponse"/> class.
        /// </summary>
        /// <param name="id"> Message id. </param>
        /// <param name="url"> Message url path. </param>
        public MessageResponse(uint id, string url)
        {
            Id = id;
            Url = url;
        }

        /// <summary>
        /// Gets message id.
        /// </summary>
        public uint Id { get; private set; }

        /// <summary>
        /// Gets message url.
        /// </summary>
        public string Url { get; private set; }
    }
}