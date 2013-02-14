using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TWG.PostageApp.Common;
using TWG.PostageApp.Converters;
using TWG.PostageApp.Message;
using TWG.PostageApp.Project;
using TWG.PostageApp.Transmissions;

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
            _responseSerializerSettings.Converters.Add(new ProjectInfoConverter());
            _responseSerializerSettings.Converters.Add(new AccountInfoConverter());
            _responseSerializerSettings.Converters.Add(new MetricsConverter());
            _responseSerializerSettings.Converters.Add(new MessageTransmissionsConverter());
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
        /// <exception cref="PostageResponseException{T}"> When http exception or PostageApp server error occurred. </exception>
        /// <returns> Task of response. </returns>
        public async Task<ResponseContainer<MessageResponse>> SendMessageAsync(Message.Message message)
        {
            return await PostAsync<MessageResponse>("send_message.json", message);
        }

        /// <summary>
        /// Gets message's info from server async.
        /// </summary>
        /// <exception cref="PostageResponseException{T}"> When http exception or PostageApp server error occurred. </exception>
        /// <returns> Task of response of message's info. </returns>
        public async Task<ResponseContainer<Dictionary<string, MessageInfo>>> GetMessagesAsync()
        {
            return await PostAsync<Dictionary<string, MessageInfo>>("get_messages.json", new { api_key = ApiKey });
        }

        /// <summary>
        /// Gets current project info from server async.
        /// </summary>
        /// <exception cref="PostageResponseException{T}"> When http exception or PostageApp server error occurred. </exception>
        /// <returns> Task of response of project's info. </returns>
        public async Task<ResponseContainer<ProjectInfo>> GetProjectInfoAsync()
        {
            return await PostAsync<ProjectInfo>("get_project_info.json", new { api_key = ApiKey });
        }

        /// <summary>
        /// Gets current project info from server async.
        /// </summary>
        /// <exception cref="PostageResponseException{T}"> When http exception or PostageApp server error occurred. </exception>
        /// <returns> Task of response of project's info. </returns>
        public async Task<ResponseContainer<AccountInfo>> GetAccountInfoAsync()
        {
            return await PostAsync<AccountInfo>("get_account_info.json", new { api_key = ApiKey });
        }

        /// <summary>
        /// Gets current project info from server async.
        /// </summary>
        /// <param name="uid"> Message UID. </param>
        /// <exception cref="PostageResponseException{T}"> When http exception or PostageApp server error occurred. </exception>
        /// <returns> Task of response of project's info. </returns>
        public async Task<ResponseContainer<MessageResponse>> GetMessageReceiptAsync(string uid)
        {
            return await PostAsync<MessageResponse>("get_message_receipt.json", new { api_key = ApiKey, uid });
        }

        /// <summary>
        /// Gets project metrics from server async.
        /// </summary>
        /// <exception cref="PostageResponseException{T}"> When http exception or PostageApp server error occurred. </exception>
        /// <returns> Task of response of project's metrics. </returns>
        public async Task<ResponseContainer<Metrics.Metrics>> GetMetricsAsync()
        {
            return await PostAsync<Metrics.Metrics>("get_metrics.json", new { api_key = ApiKey });
        }

        /// <summary>
        /// Gets  message transmissions from server async.
        /// </summary>
        /// <param name="uid"> Message UID. </param>
        /// <exception cref="PostageResponseException{T}"> When http exception or PostageApp server error occurred. </exception>
        /// <returns> Task of response of message transmissions. </returns>
        public async Task<ResponseContainer<MessageTransmissions>> GetMessageTransmissionsAsync(string uid)
        {
            return await PostAsync<MessageTransmissions>("get_message_transmissions.json", new { api_key = ApiKey, uid });
        }

        /// <summary>
        /// Base Postage App method async.
        /// </summary>
        /// <typeparam name="T"> Response object type. </typeparam>
        /// <param name="path"> Resource address. </param>
        /// <param name="data"> Requesting data. </param>
        /// <exception cref="PostageResponseException{T}"> When http exception or PostageApp server error occurred. </exception>
        /// <returns> Task of response container of type <see cref="T"/>. </returns>
        private async Task<ResponseContainer<T>> PostAsync<T>(string path, object data)
        {
            var dataJson = JsonConvert.SerializeObject(data, Formatting.None, _requestSerializerSettings);
            string stringResponse;
            HttpWebResponse response;
            try
            {
                response = await PostAsync(path, dataJson);
            }
            catch (WebException requestException)
            {
                throw new PostageResponseException<T>(requestException);
            }

            if (response.StatusCode == HttpStatusCode.OK)
            {
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    stringResponse = sr.ReadToEnd();
                } 
            }
            else
            {
                if (response.ContentType.StartsWith("application/json"))
                {
                    using (var sr = new StreamReader(response.GetResponseStream()))
                    {
                        stringResponse = sr.ReadToEnd();
                    }

                    if (stringResponse != null)
                    {
                        var responseC = JsonConvert.DeserializeObject<ResponseContainer<T>>(stringResponse, _responseSerializerSettings);
                        throw new PostageResponseException<T>(responseC, null);
                    }
                }

                throw new PostageResponseException<MessageResponse>();
            }

            var responseContainer = JsonConvert.DeserializeObject<ResponseContainer<T>>(stringResponse, _responseSerializerSettings);
            return responseContainer;
        }

        /// <summary>
        /// Base http POST method async.
        /// </summary>
        /// <param name="path"> Resource address. </param>
        /// <param name="data"> Requesting data string. </param>
        /// <returns> Task of <see cref="HttpWebResponse"/>. </returns>
        private async Task<HttpWebResponse> PostAsync(string path, string data)
        {
            var request = (HttpWebRequest)WebRequest.Create(BaseUri + path);
            request.Method = "POST";
            request.ContentType = "application/json";

            var requestBytes = _encoding.GetBytes(data);

            using (var requestStream = await request.GetRequestStreamAsync())
            {
                await requestStream.WriteAsync(requestBytes, 0, requestBytes.Length);
            }

           return await request.GetResponseAsync();
        }
    }
}
