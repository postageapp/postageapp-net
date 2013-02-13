namespace TWG.PostageApp.Common
{
    /// <summary>
    /// Represents Postage server response statuses.
    /// </summary>
    public enum ResponseStatus
    {
        /// <summary>
        /// Unknown status.
        /// </summary>
        Unknown,

        /// <summary>
        /// Completed.
        /// </summary>
        Ok,

        /// <summary>
        /// Bad request.
        /// </summary>
        BadRequest,

        /// <summary>
        /// Resource not found.
        /// </summary>
        NotFound,

        /// <summary>
        /// JSON parse error.
        /// </summary>
        InvalidJson,

        /// <summary>
        /// Unauthorized.
        /// </summary>
        Unauthorized,

        /// <summary>
        /// Call error.
        /// </summary>
        CallError,

        /// <summary>
        /// Precondition failed.
        /// </summary>
        PreconditionFailed
    }
}