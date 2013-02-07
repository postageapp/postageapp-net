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

        private void ParseJsonResponse(JToken o)
        {
            if (o == null) return;
            if (o["uid"] != null)
                Uid = o["uid"].ToString();

            if (o["message"] != null)
                ErrorMessage = o["message"].ToString();

            if (o["status"] != null)
                Status = ParseStatus(o["status"].ToString());
        }

        private void ParseJsonData(JToken o)
        {
            if (o == null) return;
            if (o["message"] == null) return;

            uint messageId;
            if (o["message"]["id"] != null)
                if (UInt32.TryParse(o["message"]["id"].ToString(), out messageId))
                    MessageId = messageId;            
        }

        private void ParseJson(string json)
        {
            var o = JObject.Parse(json);
            ParseJsonResponse(o["response"]);
            ParseJsonData(o["data"]);
        }
    }
}
