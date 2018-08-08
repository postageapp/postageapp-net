namespace PostageApp.Abstractions
{
    public class GetSuppressionListResult
    {
        public static GetSuppressionListResult Success(PostageAppResponseMeta responseMeta, GetSuppressionListResponseData data)
        {
            return new GetSuppressionListResult
            {
                Succeeded = true,
                ResponseMeta = responseMeta,
                Data = data
            };
        }

        public static GetSuppressionListResult Unauthorized(PostageAppResponseMeta responseMeta)
        {
            return new GetSuppressionListResult
            {
                Error = GetSuppressionListErrorCode.Unauthorized,
                ResponseMeta = responseMeta
            };
        }

        public static GetSuppressionListResult Locked(PostageAppResponseMeta responseMeta)
        {
            return new GetSuppressionListResult
            {
                Error = GetSuppressionListErrorCode.Locked,
                ResponseMeta = responseMeta
            };
        }

        private GetSuppressionListResult()
        { }

        public bool Succeeded { get; private set; }

        public GetSuppressionListErrorCode? Error { get; private set; }

        public PostageAppResponseMeta ResponseMeta { get; private set; }

        public GetSuppressionListResponseData Data { get; private set; }
    }

    public enum GetSuppressionListErrorCode
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
