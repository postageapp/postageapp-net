namespace PostageApp.Abstractions
{
    public class GetMetricsResult
    {
        public static GetMetricsResult Success(PostageAppResponseMeta responseMeta, GetMetricsResponseData data)
        {
            return new GetMetricsResult
            {
                Succeeded = true,
                ResponseMeta = responseMeta,
                Data = data
            };
        }

        public static GetMetricsResult Unauthorized(PostageAppResponseMeta responseMeta)
        {
            return new GetMetricsResult
            {
                Error = GetMetricsErrorCode.Unauthorized,
                ResponseMeta = responseMeta
            };
        }

        public static GetMetricsResult Locked(PostageAppResponseMeta responseMeta)
        {
            return new GetMetricsResult
            {
                Error = GetMetricsErrorCode.Locked,
                ResponseMeta = responseMeta
            };
        }

        private GetMetricsResult()
        { }

        public bool Succeeded { get; private set; }

        public GetMetricsErrorCode? Error { get; private set; }

        public PostageAppResponseMeta ResponseMeta { get; private set; }

        public GetMetricsResponseData Data { get; private set; }
    }

    public enum GetMetricsErrorCode
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
