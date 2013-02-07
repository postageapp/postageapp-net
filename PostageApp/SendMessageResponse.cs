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
            var o = JObject.Parse(json);

            if (o["response"] != null)
            {
                if (o["response"]["uid"] != null)
                    Uid = o["response"]["uid"].ToString();

                if (o["response"]["message"] != null)
                    ErrorMessage = o["response"]["message"].ToString();

                if (o["response"]["status"] != null)
                    switch (o["response"]["status"].ToString())
                    {
                        case "ok":
                            Status = SendMessageResponseStatus.Ok;
                            break;
                        case "bad_request":
                            Status = SendMessageResponseStatus.BadRequest;
                            break;
                        case "not_found":
                            Status = SendMessageResponseStatus.NotFound;
                            break;
                        case "invalid_json":
                            Status = SendMessageResponseStatus.InvalidJson;
                            break;
                        case "unauthorized":
                            Status = SendMessageResponseStatus.Unauthorized;
                            break;
                        case "call_error":
                            Status = SendMessageResponseStatus.CallError;
                            break;
                        case "precondition_failed":
                            Status = SendMessageResponseStatus.PreconditionFailed;
                            break;
                        default:
                            Status = SendMessageResponseStatus.Unknown;
                            break;
                    }
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
