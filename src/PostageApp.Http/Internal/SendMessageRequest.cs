namespace PostageApp.Http.Internal
{
    internal class SendMessageRequest
    {
        public string ApiKey { get; set; }

        public string Uid { get; set; }

        public SendMessageRequestArgs Arguments { get; set; }
    }
}
