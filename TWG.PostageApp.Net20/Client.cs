﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
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
        /// Sends message.
        /// </summary>
        /// <param name="message"> Message. </param>
        /// <exception cref="PostageResponseException{T}"> When http exception or PostageApp server error occurred. </exception>
        /// <returns> Response. </returns>
        public ResponseContainer<MessageResponse> SendMessage(Message.Message message)
        {
            return Post<MessageResponse>("send_message.json", message);
        }

        /// <summary>
        /// Gets message's info from server.
        /// </summary>
        /// <exception cref="PostageResponseException{T}"> When http exception or PostageApp server error occurred. </exception>
        /// <returns> Response of message's info. </returns>
        public ResponseContainer<Dictionary<string, MessageInfo>> GetMessages()
        {
            return Post<Dictionary<string, MessageInfo>>("get_messages.json", new { api_key = ApiKey });
        }

        /// <summary>
        /// Gets current project info from server.
        /// </summary>
        /// <exception cref="PostageResponseException{T}"> When http exception or PostageApp server error occurred. </exception>
        /// <returns> Response of project's info. </returns>
        public ResponseContainer<ProjectInfo> GetProjectInfo()
        {
            return Post<ProjectInfo>("get_project_info.json", new { api_key = ApiKey });
        }

        /// <summary>
        /// Gets current project info from server.
        /// </summary>
        /// <exception cref="PostageResponseException{T}"> When http exception or PostageApp server error occurred. </exception>
        /// <returns> Response of project's info. </returns>
        public ResponseContainer<AccountInfo> GetAccountInfo()
        {
            return Post<AccountInfo>("get_account_info.json", new { api_key = ApiKey });
        }

        /// <summary>
        /// Gets current project info from server.
        /// </summary>
        /// <param name="uid"> Message UID. </param>
        /// <exception cref="PostageResponseException{T}"> When http exception or PostageApp server error occurred. </exception>
        /// <returns> Response of project's info. </returns>
        public ResponseContainer<MessageResponse> GetMessageReceipt(string uid)
        {
            return Post<MessageResponse>("get_message_receipt.json", new { api_key = ApiKey, uid });
        }

        /// <summary>
        /// Gets project metrics from server.
        /// </summary>
        /// <exception cref="PostageResponseException{T}"> When http exception or PostageApp server error occurred. </exception>
        /// <returns> Response of project's metrics. </returns>
        public ResponseContainer<Metrics.Metrics> GetMetrics()
        {
            return Post<Metrics.Metrics>("get_metrics.json", new { api_key = ApiKey });
        }

        /// <summary>
        /// Gets message transmissions from server.
        /// </summary>
        /// <param name="uid"> Message UID. </param>
        /// <exception cref="PostageResponseException{T}"> When http exception or PostageApp server error occurred. </exception>
        /// <returns> Response of message transmissions. </returns>
        public ResponseContainer<MessageTransmissions> GetMessageTransmissions(string uid)
        {
            return Post<MessageTransmissions>("get_message_transmissions.json", new { api_key = ApiKey, uid });
        }

        /// <summary>
        /// Base Postage App method.
        /// </summary>
        /// <typeparam name="T"> Response object type. </typeparam>
        /// <param name="path"> Resource address. </param>
        /// <param name="data"> Requesting data. </param>
        /// <exception cref="PostageResponseException{T}"> When http exception or PostageApp server error occurred. </exception>
        /// <returns> Response container of type <see cref="T"/>. </returns>
        private ResponseContainer<T> Post<T>(string path, object data)
        {
            string stringResponse;
            using (var client = new WebClient { BaseAddress = BaseUri, Encoding = _encoding })
            {
                var objectJson = JsonConvert.SerializeObject(data, Formatting.None, _requestSerializerSettings);

                client.Headers[HttpRequestHeader.ContentType] = "application/json";

                try
                {
                    stringResponse = client.UploadString(path, objectJson);
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
                                throw new PostageResponseException<T>(exception);
                            }

                            using (var reader = new StreamReader(responseStream))
                            {
                                var responseBody = reader.ReadToEnd();
                                var responseC =
                                    JsonConvert.DeserializeObject<ResponseContainer<T>>(responseBody, _responseSerializerSettings);
                                throw new PostageResponseException<T>(responseC, exception);
                            }
                        }
                    }

                    throw new PostageResponseException<T>(exception);
                }
            }

            var responseContainer = JsonConvert.DeserializeObject<ResponseContainer<T>>(stringResponse, _responseSerializerSettings);
            return responseContainer;
        }
    }
}
