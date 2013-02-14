using System.Runtime.InteropServices;

namespace TWG.PostageApp.Common
{
    /// <summary>
    /// Represents postageApp server response.
    /// </summary>
    public class PostageResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PostageResponse"/> class.
        /// </summary>
        /// <param name="uid"> Response UID. </param>
        /// <param name="status">Response status. </param>
        /// <param name="message"> Response message. </param>
        public PostageResponse(string uid, ResponseStatus status, [Optional] string message)
        {
            Uid = uid;
            Status = status;
            ErrorMessage = message;
        }

        /// <summary>
        /// Gets response UID.
        /// </summary>
        public string Uid { get; private set; }

        /// <summary>
        /// Gets response status.
        /// </summary>
        public ResponseStatus Status { get; private set; }

        /// <summary>
        /// Gets error message.
        /// </summary>
        public string ErrorMessage { get; private set; }
    }
}
