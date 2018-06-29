namespace PostageApp.Abstractions
{
    public class SendMessageResult
    {
        public static SendMessageResult Success(PostageAppResponseMeta responseMeta, MessageRecieptResponseData data)
        {
            return new SendMessageResult
            {
                Succeeded = true,
                ResponseMeta = responseMeta,
                Data = data
            };
        }

        public static SendMessageResult Unauthorized(PostageAppResponseMeta responseMeta)
        {
            return new SendMessageResult
            {
                Error = SendMessageErrorCode.Unauthorized,
                ResponseMeta = responseMeta
            };
        }

        public static SendMessageResult Locked(PostageAppResponseMeta responseMeta)
        {
            return new SendMessageResult
            {
                Error = SendMessageErrorCode.Locked,
                ResponseMeta = responseMeta
            };
        }

        public static SendMessageResult BadRequest(PostageAppResponseMeta responseMeta)
        {
            return new SendMessageResult
            {
                Error = SendMessageErrorCode.Locked,
                ResponseMeta = responseMeta
            };
        }

        public static SendMessageResult PreconditionFailed(PostageAppResponseMeta responseMeta)
        {
            return new SendMessageResult
            {
                Error = SendMessageErrorCode.PreconditionFailed,
                ResponseMeta = responseMeta
            };
        }

        public static SendMessageResult InvalidUTF8(PostageAppResponseMeta responseMeta)
        {
            return new SendMessageResult
            {
                Error = SendMessageErrorCode.InvalidUTF8,
                ResponseMeta = responseMeta
            };
        }

        private SendMessageResult()
        { }

        public bool Succeeded { get; private set; }

        public SendMessageErrorCode? Error { get; private set; }

        public PostageAppResponseMeta ResponseMeta { get; private set; }

        public MessageRecieptResponseData Data { get; private set; }
    }

    public enum SendMessageErrorCode
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
        /// The API call is incomplete, typically missing fields that are necessary to complete processing.
        /// </summary>
        BadRequest,

        /// <summary>
        /// An account has been sending, either deliberately or inadvertently, emails with a high level of spam content.
        /// </summary>
        PreconditionFailed,

        /// <summary>
        /// An invalid UTF8 character has been detected.
        /// </summary>
        InvalidUTF8
    }
}
