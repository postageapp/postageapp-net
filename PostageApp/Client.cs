using System.Net;
using System.Text;

namespace PostageApp
{
    public class Client
    {
        private const string DefaultUri = "https://api.postageapp.com/v.1.0/";

        private string ApiKey { get; set; }

        private string _baseUri;
        public string BaseUri
        {
            get { return _baseUri ?? DefaultUri; }
            set { _baseUri = value; }
        }

        public Client(string apiKey)
        {
            ApiKey = apiKey;
        }

        public SendMessageResponse SendMessage(SendMessageRequest sendMessageRequest)
        {
            string url = BaseUri + "send_message.json";
            var postData = sendMessageRequest.ToJson(ApiKey);
            var request = JsonPostRequest(url, postData);

            try
            {
                return new SendMessageResponse(request.GetResponse());
            }
            catch (WebException e)
            {
                throw new SendMessageException(e);
            }
        }

        private static WebRequest JsonPostRequest(string url, string postData)
        {
            var byteArray = Encoding.UTF8.GetBytes(postData);

            var request = WebRequest.Create(url);
            request.Method = WebRequestMethods.Http.Post;
            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;

            var dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            return request;
        }
    }
}
