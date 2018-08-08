namespace PostageApp.Abstractions
{
    public class GetMessageReceiptResult
    {
        public static GetMessageReceiptResult Success(PostageAppResponseMeta responseMeta, MessageRecieptResponseData data)
        {
            return new GetMessageReceiptResult
            {
                Succeeded = true,
                ResponseMeta = responseMeta,
                Data = data
            };
        }

        public static GetMessageReceiptResult Unauthorized(PostageAppResponseMeta responseMeta)
        {
            return new GetMessageReceiptResult
            {
                Error = GetMessageReceiptErrorCode.Unauthorized,
                ResponseMeta = responseMeta
            };
        }

        public static GetMessageReceiptResult Locked(PostageAppResponseMeta responseMeta)
        {
            return new GetMessageReceiptResult
            {
                Error = GetMessageReceiptErrorCode.Locked,
                ResponseMeta = responseMeta
            };
        }

        public static GetMessageReceiptResult NotFound(PostageAppResponseMeta responseMeta)
        {
            return new GetMessageReceiptResult
            {
                Error = GetMessageReceiptErrorCode.NotFound,
                ResponseMeta = responseMeta
            };
        }

        private GetMessageReceiptResult()
        { }

        public bool Succeeded { get; private set; }

        public GetMessageReceiptErrorCode? Error { get; private set; }

        public PostageAppResponseMeta ResponseMeta { get; private set; }

        public MessageRecieptResponseData Data { get; private set; }
    }

    public enum GetMessageReceiptErrorCode
    {
        /// <summary>
        /// Invalid or inactive account API key used.
        /// </summary>
        Unauthorized,

        /// <summary>
        /// The specified API key is not valid.
        /// </summary>
        Locked,

        /// <summary>
        /// Uid is not found.
        /// </summary>
        NotFound
    }
}
