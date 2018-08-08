using System;
using System.Collections.Generic;

namespace PostageApp.Abstractions
{
    public class GetMessagesHistoryResult
    {
        public static GetMessagesHistoryResult Success(PostageAppResponseMeta responseMeta, GetMessagesHistoryResponseData data)
        {
            return new GetMessagesHistoryResult
            {
                Succeeded = true,
                ResponseMeta = responseMeta,
                Data = data
            };
        }

        public static GetMessagesHistoryResult Unauthorized(PostageAppResponseMeta responseMeta)
        {
            return new GetMessagesHistoryResult
            {
                Error = GetMessagesHistoryErrorCode.Unauthorized,
                ResponseMeta = responseMeta
            };
        }

        public static GetMessagesHistoryResult Locked(PostageAppResponseMeta responseMeta)
        {
            return new GetMessagesHistoryResult
            {
                Error = GetMessagesHistoryErrorCode.Locked,
                ResponseMeta = responseMeta
            };
        }

        private GetMessagesHistoryResult()
        { }

        public bool Succeeded { get; private set; }

        public GetMessagesHistoryErrorCode? Error { get; private set; }

        public PostageAppResponseMeta ResponseMeta { get; private set; }

        public GetMessagesHistoryResponseData Data { get; private set; }
    }

    public enum GetMessagesHistoryErrorCode
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
