namespace PostageApp.Abstractions
{
    public class GetMessageDeliveryStatusResult
    {
        public static GetMessageDeliveryStatusResult Success(PostageAppResponseMeta responseMeta, MessageDeliveryStatusResponseData data)
        {
            return new GetMessageDeliveryStatusResult
            {
                Succeeded = true,
                ResponseMeta = responseMeta,
                Data = data
            };
        }

        public static GetMessageDeliveryStatusResult Unauthorized(PostageAppResponseMeta responseMeta)
        {
            return new GetMessageDeliveryStatusResult
            {
                Error = GetMessageDeliveryStatusErrorCode.Unauthorized,
                ResponseMeta = responseMeta
            };
        }

        public static GetMessageDeliveryStatusResult Locked(PostageAppResponseMeta responseMeta)
        {
            return new GetMessageDeliveryStatusResult
            {
                Error = GetMessageDeliveryStatusErrorCode.Locked,
                ResponseMeta = responseMeta
            };
        }

        public static GetMessageDeliveryStatusResult NotFound(PostageAppResponseMeta responseMeta)
        {
            return new GetMessageDeliveryStatusResult
            {
                Error = GetMessageDeliveryStatusErrorCode.NotFound,
                ResponseMeta = responseMeta
            };
        }

        private GetMessageDeliveryStatusResult()
        { }

        public bool Succeeded { get; private set; }

        public GetMessageDeliveryStatusErrorCode? Error { get; private set; }

        public PostageAppResponseMeta ResponseMeta { get; private set; }

        public MessageDeliveryStatusResponseData Data { get; private set; }
    }

    public enum GetMessageDeliveryStatusErrorCode
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
