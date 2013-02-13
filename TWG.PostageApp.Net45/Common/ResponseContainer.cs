namespace TWG.PostageApp.Common
{
    /// <summary>
    /// Represents server response container.
    /// </summary>
    /// <typeparam name="T"> Data type. </typeparam>
    public class ResponseContainer<T>
    {
        /// <summary>
        /// Gets or sets server response status.
        /// </summary>
        public PostageResponse Response { get; set; }

        /// <summary>
        /// Gets or sets response data.
        /// </summary>
        public T Data { get; set; }
    }
}