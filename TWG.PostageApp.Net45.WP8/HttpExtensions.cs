using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace TWG.PostageApp
{
    /// <summary>
    /// Represents async/await <see cref="System.Net.HttpWebRequest"/> extensions.
    /// </summary>
    public static class HttpExtensions
    {
        /// <summary>
        /// Gets request stream async.
        /// </summary>
        /// <param name="request"> Request. </param>
        /// <returns> Task of request stream. </returns>
        public static Task<Stream> GetRequestStreamAsync(this HttpWebRequest request)
        {
            var taskComplete = new TaskCompletionSource<Stream>();
            request.BeginGetRequestStream(
                asyncResult =>
                {
                    try
                    {
                        Stream requestStream = request.EndGetRequestStream(asyncResult);
                        taskComplete.TrySetResult(requestStream);
                    }
                    catch (WebException exception)
                    {
                        taskComplete.SetException(exception);
                    }
                },
                request);
            return taskComplete.Task;
        }

        /// <summary>
        /// Gets response async.
        /// </summary>
        /// <param name="request"> Request. </param>
        /// <returns> Task of responce. </returns>
        public static Task<HttpWebResponse> GetResponseAsync(this HttpWebRequest request)
        {
            var taskComplete = new TaskCompletionSource<HttpWebResponse>();
            request.BeginGetResponse(
                asyncResponse =>
                {
                    try
                    {
                        var responseRequest = (HttpWebRequest)asyncResponse.AsyncState;
                        var someResponse = (HttpWebResponse)responseRequest.EndGetResponse(asyncResponse);
                        taskComplete.TrySetResult(someResponse);
                    }
                    catch (WebException exception)
                    {
                        var failedResponse = (HttpWebResponse)exception.Response;
                        taskComplete.TrySetResult(failedResponse);
                    }
                },
                request);
            return taskComplete.Task;
        }
    }
}
