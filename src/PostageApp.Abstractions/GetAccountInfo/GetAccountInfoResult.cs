namespace PostageApp.Abstractions
{
    public class GetAccountInfoResult
    {
        public static GetAccountInfoResult Success(PostageAppResponseMeta responseMeta, GetAccountInfoResponseData data)
        {
            return new GetAccountInfoResult
            {
                Succeeded = true,
                ResponseMeta = responseMeta,
                Data = data
            };
        }

        public static GetAccountInfoResult Unauthorized(PostageAppResponseMeta responseMeta)
        {
            return new GetAccountInfoResult
            {
                Error = GetAccountInfoErrorCode.Unauthorized,
                ResponseMeta = responseMeta
            };
        }

        public static GetAccountInfoResult Locked(PostageAppResponseMeta responseMeta)
        {
            return new GetAccountInfoResult
            {
                Error = GetAccountInfoErrorCode.Locked,
                ResponseMeta = responseMeta
            };
        }

        private GetAccountInfoResult()
        { }

        public bool Succeeded { get; private set; }

        public GetAccountInfoErrorCode? Error { get; private set; }

        public PostageAppResponseMeta ResponseMeta { get; private set; }

        public GetAccountInfoResponseData Data { get; private set; }
    }

    public enum GetAccountInfoErrorCode
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
