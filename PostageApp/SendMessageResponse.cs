using System;
using Newtonsoft.Json.Linq;

namespace PostageApp
{
    public enum SendMessageResponseStatus
    {
        Ok,
        BadRequest,
        NotFound,
        InvalidJson,
        Unauthorized,
        CallError,
        PreconditionFailed,
        Unknown
    };

    public class SendMessageResponse
    {
        public string Uid { get; set; }
        public uint MessageId { get; set; }
        public SendMessageResponseStatus Status { get; set; }
        public string ErrorMessage { get; set; }

        public SendMessageResponse(string json)
        {
            ParseJson(json);
        }

        private SendMessageResponseStatus ParseStatus(string status)
        {
            switch (status)
            {
                case "ok":
                    return SendMessageResponseStatus.Ok;
                case "bad_request":
                    return SendMessageResponseStatus.BadRequest;
                case "not_found":
                    return SendMessageResponseStatus.NotFound;
                case "invalid_json":
                    return SendMessageResponseStatus.InvalidJson;
                case "unauthorized":
                    return SendMessageResponseStatus.Unauthorized;
                case "call_error":
                    return SendMessageResponseStatus.CallError;
                case "precondition_failed":
                    return SendMessageResponseStatus.PreconditionFailed;
                default:
                    return SendMessageResponseStatus.Unknown;
            }
        }

        private void ParseJson(string json)
        {
            var o = JObject.Parse(json);

            if (o["response"] != null)
            {
                if (o["response"]["uid"] != null)
                    Uid = o["response"]["uid"].ToString();

                if (o["response"]["message"] != null)
                    ErrorMessage = o["response"]["message"].ToString();

                if (o["response"]["status"] != null)
                    Status = ParseStatus(o["response"]["status"].ToString());
            }

            if (o["data"] != null)
            {
                if (o["data"]["message"] != null)
                {
                    uint messageId;
                    if (o["data"]["message"]["id"] != null)
                        if (UInt32.TryParse(o["data"]["message"]["id"].ToString(), out messageId))
                            MessageId = messageId;
                }
            }
        }
    }
}
