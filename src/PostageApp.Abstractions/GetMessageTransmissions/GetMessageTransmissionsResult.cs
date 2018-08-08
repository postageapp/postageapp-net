namespace PostageApp.Abstractions
{
    public class GetMessageTransmissionsResult
    {
        public static GetMessageTransmissionsResult Success(PostageAppResponseMeta responseMeta, MessageTransmissionsData data)
        {
            return new GetMessageTransmissionsResult
            {
                Succeeded = true,
                ResponseMeta = responseMeta,
                Data = data
            };
        }

        public static GetMessageTransmissionsResult Unauthorized(PostageAppResponseMeta responseMeta)
        {
            return new GetMessageTransmissionsResult
            {
                Error = GetMessageTransmissionsErrorCode.Unauthorized,
                ResponseMeta = responseMeta
            };
        }

        public static GetMessageTransmissionsResult Locked(PostageAppResponseMeta responseMeta)
        {
            return new GetMessageTransmissionsResult
            {
                Error = GetMessageTransmissionsErrorCode.Locked,
                ResponseMeta = responseMeta
            };
        }

        public static GetMessageTransmissionsResult NotFound(PostageAppResponseMeta responseMeta)
        {
            return new GetMessageTransmissionsResult
            {
                Error = GetMessageTransmissionsErrorCode.NotFound,
                ResponseMeta = responseMeta
            };
        }

        private GetMessageTransmissionsResult()
        { }

        public bool Succeeded { get; private set; }

        public GetMessageTransmissionsErrorCode? Error { get; private set; }

        public PostageAppResponseMeta ResponseMeta { get; private set; }

        public MessageTransmissionsData Data { get; private set; }
    }

    public enum GetMessageTransmissionsErrorCode
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
