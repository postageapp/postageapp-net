using System;
using System.IO;
using System.Net;
using System.Text;

namespace PostageApp
{
    public class Client
    {     
        private const string BaseUri = "https://api.postageapp.com/v.1.0/";

        private string ApiKey { get; set; }

        public Client(string apiKey)
        {
            ApiKey = apiKey;
        }

        public SendMessageResponse SendMessage(SendMessageRequest sendMessageRequest)
        {
            const string url = BaseUri + "send_message.json";
            var postData = sendMessageRequest.ToJson(ApiKey);
            var json = JsonPost(url, postData);
            return new SendMessageResponse(json);
        }

        private static string JsonPost(string url, string postData)
        {
            var byteArray = Encoding.UTF8.GetBytes(postData);

            var request = WebRequest.Create(url);
            request.Method = WebRequestMethods.Http.Post;
            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;

            var dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            var response = request.GetResponse();
            dataStream = response.GetResponseStream();
            if (dataStream == null)
            {
                throw new Exception("Unexpected null response stream");
            }

            var reader = new StreamReader(dataStream);
            var responseFromServer = reader.ReadToEnd();

            reader.Close();
            dataStream.Close();
            response.Close();

            return responseFromServer;
        }
    }
}
