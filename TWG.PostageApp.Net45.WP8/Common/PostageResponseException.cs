using System;

namespace TWG.PostageApp.Common
{
    /// <summary>
    /// Represents postage response exception.
    /// </summary>
    /// <typeparam name="T">
    /// Data response type.
    /// </typeparam>
    public class PostageResponseException<T> : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PostageResponseException{T}"/> class.
        /// </summary>
        /// <param name="response">
        /// The response.
        /// </param>
        /// <param name="innerException">
        /// The inner exception.
        /// </param>
        public PostageResponseException(ResponseContainer<T> response, Exception innerException)
            : base(response.Response.ErrorMessage, innerException)
        {
            if (response == null)
            {
                throw new ArgumentNullException("response");
            }

            ResponseContainer = response;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PostageResponseException{T}"/> class.
        /// </summary>
        /// <param name="innerException">
        /// The inner exception.
        /// </param>
        public PostageResponseException(Exception innerException)
            : base(string.Empty, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PostageResponseException{T}"/> class.
        /// </summary>
        public PostageResponseException()
        {
        }

/*
        /// <summary>
        /// Initializes a new instance of the <see cref="PostageResponseException"/> class.
        /// </summary>
        /// <param name="exception"> Web exception. </param>
        public PostageResponseException(WebException exception)
        {
            WebException = exception;

            if (exception.Response != null)
            {
                StatusCode = (int)((HttpWebResponse)exception.Response).StatusCode;
            }
        }*/

        /// <summary>
        /// Gets response container.
        /// </summary>
        public ResponseContainer<T> ResponseContainer { get; private set; }
    }
}