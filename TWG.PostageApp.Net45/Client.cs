using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TWG.PostageApp.Converters;

namespace TWG.PostageApp
{
    /// <summary>
    /// Represents PostageApp client.
    /// </summary>
    public class Client
    {
        /// <summary>
        /// Default api url.
        /// </summary>
        private const string DEFAULT_URL = "https://api.postageapp.com/v.1.0/";

        /// <summary>
        /// JSON request serializer settings.
        /// </summary>
        private readonly JsonSerializerSettings _requestSerializerSettings;

        /// <summary>
        /// JSON response serializer settings.
        /// </summary>
        private readonly JsonSerializerSettings _responseSerializerSettings;

        /// <summary>
        /// Api JSON encoding.
        /// </summary>
        private readonly Encoding _encoding;

        /// <summary>
        /// Base url backing filed.
        /// </summary>
        private string _baseUri;

        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class.
        /// </summary>
        /// <param name="apiKey"> Project api key.</param>
        public Client(string apiKey)
        {
            ApiKey = apiKey;

            _encoding = Encoding.UTF8;
            _requestSerializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
            _requestSerializerSettings.Converters.Add(new MessageConverter(apiKey));

            _responseSerializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
            _responseSerializerSettings.Converters.Add(new PostageResponseConverter());
            _responseSerializerSettings.Converters.Add(new MessageResponseConverter());
        }

        /// <summary>
        /// Gets or sets base api url.
        /// </summary>
        public string BaseUri
        {
            get
            {
                return _baseUri ?? DEFAULT_URL;
            }

            set
            {
                _baseUri = value;
            }
        }

        /// <summary>
        /// Gets project api key.
        /// </summary>
        internal string ApiKey { get; private set; }

        /// <summary>
        /// Sends message async.
        /// </summary>
        /// <param name="message"> Message. </param>
        /// <returns> Task of response. </returns>
        public async Task<ResponseContainer<MessageResponse>> SendMessageAsync(Message message)
        {
            var client = new HttpClient { BaseAddress = new Uri(BaseUri) };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var messageJson = await JsonConvert.SerializeObjectAsync(message, Formatting.None, _requestSerializerSettings);
            HttpContent content = new StringContent(messageJson, _encoding, "application/json");

            HttpResponseMessage response;
            string stringResponse;
            try
            {
                response = await client.PostAsync("send_message.json", content);
            }
            catch (HttpRequestException requestException)
            {
                throw new PostageResponseException<MessageResponse>(requestException);
            }

            if (response.StatusCode == HttpStatusCode.OK)
            {
                stringResponse = await response.Content.ReadAsStringAsync();
            }
            else
            {
                if (response.Content.Headers.ContentType.MediaType.StartsWith("application/json"))
                {
                    stringResponse = await response.Content.ReadAsStringAsync();
                    if (stringResponse != null)
                    {
                        var responseC = await JsonConvert.DeserializeObjectAsync<ResponseContainer<MessageResponse>>(stringResponse, _responseSerializerSettings);
                        throw new PostageResponseException<MessageResponse>(responseC, null);
                    }
                }

                throw new PostageResponseException<MessageResponse>();
            }

            var responseContainer = await JsonConvert.DeserializeObjectAsync<ResponseContainer<MessageResponse>>(stringResponse, _responseSerializerSettings);
            return responseContainer;
        }

        /// <summary>
        /// Sends message.
        /// </summary>
        /// <param name="message"> Message. </param>
        /// <exception cref="PostageResponseException{T}"> Response exception. </exception>
        /// <returns> Response. </returns>
        public ResponseContainer<MessageResponse> SendMessage(Message message)
        {
            string stringResponse;
            using (var client = new WebClient { BaseAddress = BaseUri, Encoding = _encoding })
            {
                var messageJson = JsonConvert.SerializeObject(message, Formatting.None, _requestSerializerSettings);

                client.Headers[HttpRequestHeader.ContentType] = "application/json";

                try
                {
                    stringResponse = client.UploadString("send_message.json", messageJson);
                }
                catch (WebException exception)
                {
                    var webResponse = exception.Response;
                    if (webResponse != null)
                    {
                        if (webResponse.ContentType.StartsWith("application/json"))
                        {
                            var responseStream = exception.Response.GetResponseStream();

                            if (responseStream == null)
                            {
                                throw new PostageResponseException<MessageResponse>(exception);
                            }

                            using (var reader = new StreamReader(responseStream))
                            {
                                var responseBody = reader.ReadToEnd();
                                var responseC =
                                    JsonConvert.DeserializeObject<ResponseContainer<MessageResponse>>(responseBody, _responseSerializerSettings);
                                throw new PostageResponseException<MessageResponse>(responseC, exception);
                            }
                        }
                    }

                    throw new PostageResponseException<MessageResponse>(exception);
                }
            }

            var responseContainer = JsonConvert.DeserializeObject<ResponseContainer<MessageResponse>>(stringResponse, _responseSerializerSettings);
            return responseContainer;
        }
    }
}
