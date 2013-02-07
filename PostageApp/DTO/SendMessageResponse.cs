namespace PostageApp.DTO
{
    public enum SendMessageResponseStatus
    {
        Ok,
        BadRequest,
        Failed,
        NotFound,
        InvalidJson,
        Unauthorized,
        CallError,
        PreconditionFailed
    };

    public class SendMessageResponse
    {
        public string Uid { get; set; }
        public uint MessageId { get; set; }
        public SendMessageResponseStatus Status { get; set; }
        public string ErrorMessage { get; set; }

        public SendMessageResponse(string json)
        {
            // parse json
        }
    }
}
