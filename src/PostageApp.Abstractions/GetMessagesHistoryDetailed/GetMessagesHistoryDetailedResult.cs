namespace PostageApp.Abstractions
{
    public class GetMessagesHistoryDetailedResult
    {
        public static GetMessagesHistoryDetailedResult Success(PostageAppResponseMeta responseMeta, GetMessagesHistoryDetailedResponseData data)
        {
            return new GetMessagesHistoryDetailedResult
            {
                Succeeded = true,
                ResponseMeta = responseMeta,
                Data = data
            };
        }

        public static GetMessagesHistoryDetailedResult Unauthorized(PostageAppResponseMeta responseMeta)
        {
            return new GetMessagesHistoryDetailedResult
            {
                Error = GetMessagesHistoryErrorCode.Unauthorized,
                ResponseMeta = responseMeta
            };
        }

        public static GetMessagesHistoryDetailedResult Locked(PostageAppResponseMeta responseMeta)
        {
            return new GetMessagesHistoryDetailedResult
            {
                Error = GetMessagesHistoryErrorCode.Locked,
                ResponseMeta = responseMeta
            };
        }

        private GetMessagesHistoryDetailedResult()
        { }

        public bool Succeeded { get; private set; }

        public GetMessagesHistoryErrorCode? Error { get; private set; }

        public PostageAppResponseMeta ResponseMeta { get; private set; }

        public GetMessagesHistoryDetailedResponseData Data { get; private set; }
    }

    public enum GetMessagesHistoryDetailedErrorCode
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
