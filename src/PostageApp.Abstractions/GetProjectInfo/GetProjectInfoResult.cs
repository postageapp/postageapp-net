namespace PostageApp.Abstractions
{
    public class GetProjectInfoResult
    {
        public static GetProjectInfoResult Success(PostageAppResponseMeta responseMeta, GetProjectInfoResponseData data)
        {
            return new GetProjectInfoResult
            {
                Succeeded = true,
                ResponseMeta = responseMeta,
                Data = data
            };
        }

        public static GetProjectInfoResult Unauthorized(PostageAppResponseMeta responseMeta)
        {
            return new GetProjectInfoResult
            {
                Error = GetProjectInfoErrorCode.Unauthorized,
                ResponseMeta = responseMeta
            };
        }

        public static GetProjectInfoResult Locked(PostageAppResponseMeta responseMeta)
        {
            return new GetProjectInfoResult
            {
                Error = GetProjectInfoErrorCode.Locked,
                ResponseMeta = responseMeta
            };
        }

        private GetProjectInfoResult()
        { }

        public bool Succeeded { get; private set; }

        public GetProjectInfoErrorCode? Error { get; private set; }

        public PostageAppResponseMeta ResponseMeta { get; private set; }

        public GetProjectInfoResponseData Data { get; private set; }
    }

    public enum GetProjectInfoErrorCode
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
