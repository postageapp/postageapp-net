namespace PostageApp.Abstractions
{
    public class GetMessagesResult
    {
        public static GetMessagesResult Success(PostageAppResponseMeta responseMeta, MessageResponseData data)
        {
            return new GetMessagesResult
            {
                Succeeded = true,
                ResponseMeta = responseMeta,
                Data = data
            };
        }

        public static GetMessagesResult Unauthorized(PostageAppResponseMeta responseMeta)
        {
            return new GetMessagesResult
            {
                Error = GetMessagesErrorCode.Unauthorized,
                ResponseMeta = responseMeta
            };
        }

        public static GetMessagesResult Locked(PostageAppResponseMeta responseMeta)
        {
            return new GetMessagesResult
            {
                Error = GetMessagesErrorCode.Locked,
                ResponseMeta = responseMeta
            };
        }

        private GetMessagesResult()
        { }

        public bool Succeeded { get; private set; }

        public GetMessagesErrorCode? Error { get; private set; }

        public PostageAppResponseMeta ResponseMeta { get; private set; }

        public MessageResponseData Data { get; private set; }
    }

    public enum GetMessagesErrorCode
    {
        /// <summary>
        /// Invalid or inactive account API key used.
        /// </summary>
        Unauthorized,

        /// <summary>
        /// The specified API key is not valid.
        /// </summary>
        Locked
    }
}
