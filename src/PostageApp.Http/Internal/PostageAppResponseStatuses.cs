namespace PostageApp.Abstractions
{
    internal static class PostageAppResponseStatuses
    {
        public const string Ok = "ok";
        public const string BadRequest = "bad_request";
        public const string NotFound = "not_found";
        public const string InvalidJson = "invalid_json";
        public const string Unauthorized = "unauthorized";
        public const string CallError = "call_error";
        public const string PreconditionFailed = "precondition_failed";
    }
}
