using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using PostageApp.Abstractions;
using PostageApp.Http.Internal;

namespace PostageApp.Http
{
    public class HttpPostageAppClient : IPostageAppClient
    {
        private const string V1_GET_ACCOUNT_INFO_URL = "/v.1.0/get_account_info.json";
        private const string V1_GET_MESSAGE_RECEIPT_URL = "/v.1.0/get_message_receipt.json";
        private const string V1_GET_MESSAGE_TRANSMISSIONS_URL = "/v.1.0/get_message_transmissions.json";
        private const string V1_GET_MESSAGES_URL = "/v.1.0/get_messages.json";
        private const string V1_SEND_MESSAGE_URL = "/v.1.0/send_message.json";
        private const string V1_GET_PROJECT_INFO_URL = "/v.1.0/get_project_info.json";
        private const string V1_GET_METRICS_URL = "/v.1.0/get_metrics.json";
        private const string V1_GET_SUPPRESSION_LIST_URL = "/v.1.0/get_suppression_list.json";
        private const string V1_GET_MESSAGE_DELIVERY_STATUS_URL = "/v.1.0/message_delivery_status.json";
        private const string V1_GET_MESSAGES_HISTORY_URL = "/v.1.0/messages_history.json";
        private const string V1_GET_MESSAGES_HISTORY_DETAILED_URL = "/v.1.0/messages_history_detailed.json";

        private readonly HttpPostageAppClientOptions _options;
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public HttpPostageAppClient(IOptions<HttpPostageAppClientOptions> options, HttpClient httpClient)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy(false, false)
            };

            _jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.None
            };

            _jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter { CamelCaseText = true });
        }

        public async Task<GetAccountInfoResult> GetAccountInfoAsync(string apiKey = null, CancellationToken cancellationToken = default)
        {
            (HttpResponseMessage httpResponseMessage, PostageAppResponsePayload<GetAccountInfoResponseData> payload)
                = await SendHttpAsync<PostageAppResponsePayload<GetAccountInfoResponseData>>(
                    HttpMethod.Post,
                    new Uri(new Uri(_options.BaseUri), V1_GET_ACCOUNT_INFO_URL),
                    new { ApiKey = apiKey ?? _options.ApiKey },
                    cancellationToken);

            switch (httpResponseMessage.StatusCode)
            {
                case HttpStatusCode.NotAcceptable:
                    throw new InvalidOperationException(payload.Response.Message);
                case HttpStatusCode.Conflict:
                    throw new InvalidOperationException(payload.Response.Message);
                case HttpStatusCode.Unauthorized:
                    return GetAccountInfoResult.Unauthorized(payload.Response);
                case (HttpStatusCode)423:
                    return GetAccountInfoResult.Locked(payload.Response);
            }

            return GetAccountInfoResult.Success(payload.Response, payload.Data);
        }

        public async Task<GetMessageDeliveryStatusResult> GetMessageDeliveryStatusAsync(string uid, string apiKey = null, CancellationToken cancellationToken = default)
        {
            if (uid == null)
            {
                throw new ArgumentNullException(nameof(uid));
            }

            (HttpResponseMessage httpResponseMessage, PostageAppResponsePayload<MessageDeliveryStatusResponseData> payload)
                = await SendHttpAsync<PostageAppResponsePayload<MessageDeliveryStatusResponseData>>(
                HttpMethod.Post,
                new Uri(new Uri(_options.BaseUri), V1_GET_MESSAGE_DELIVERY_STATUS_URL),
                new { ApiKey = apiKey ?? _options.ApiKey, Uid = uid },
                cancellationToken);

            switch (httpResponseMessage.StatusCode)
            {
                case HttpStatusCode.NotAcceptable:
                    throw new InvalidOperationException(payload.Response.Message);
                case HttpStatusCode.Conflict:
                    throw new InvalidOperationException(payload.Response.Message);
                case HttpStatusCode.Unauthorized:
                    return GetMessageDeliveryStatusResult.Unauthorized(payload.Response);
                case HttpStatusCode.NotFound:
                    return GetMessageDeliveryStatusResult.NotFound(payload.Response);
                case (HttpStatusCode)423:
                    return GetMessageDeliveryStatusResult.Locked(payload.Response);
            }

            return GetMessageDeliveryStatusResult.Success(payload.Response, payload.Data);
        }

        public async Task<GetMessageReceiptResult> GetMessageReceiptAsync(string uid, string apiKey = null, CancellationToken cancellationToken = default)
        {
            if (uid == null)
            {
                throw new ArgumentNullException(nameof(uid));
            }

            (HttpResponseMessage httpResponseMessage, PostageAppResponsePayload<MessageRecieptResponseData> payload)
                = await SendHttpAsync<PostageAppResponsePayload<MessageRecieptResponseData>>(
                HttpMethod.Post,
                new Uri(new Uri(_options.BaseUri), V1_GET_MESSAGE_RECEIPT_URL),
                new { ApiKey = apiKey ?? _options.ApiKey, Uid = uid },
                cancellationToken);

            switch (httpResponseMessage.StatusCode)
            {
                case HttpStatusCode.NotAcceptable:
                    throw new InvalidOperationException(payload.Response.Message);
                case HttpStatusCode.Conflict:
                    throw new InvalidOperationException(payload.Response.Message);
                case HttpStatusCode.Unauthorized:
                    return GetMessageReceiptResult.Unauthorized(payload.Response);
                case HttpStatusCode.NotFound:
                    return GetMessageReceiptResult.NotFound(payload.Response);
                case (HttpStatusCode)423:
                    return GetMessageReceiptResult.Locked(payload.Response);
            }

            return GetMessageReceiptResult.Success(payload.Response, payload.Data);
        }

        public async Task<GetMessagesResult> GetMessagesAsync(int page = 1, string apiKey = null, CancellationToken cancellationToken = default)
        {
            (HttpResponseMessage httpResponseMessage, PostageAppResponsePayload<MessageResponseData> payload)
                = await SendHttpAsync<PostageAppResponsePayload<MessageResponseData>>(
                HttpMethod.Post,
                new Uri(new Uri(_options.BaseUri), V1_GET_MESSAGES_URL),
                new { ApiKey = apiKey ?? _options.ApiKey, Arguments = new { page } },
                cancellationToken);

            switch (httpResponseMessage.StatusCode)
            {
                case HttpStatusCode.NotAcceptable:
                    throw new InvalidOperationException(payload.Response.Message);
                case HttpStatusCode.Conflict:
                    throw new InvalidOperationException(payload.Response.Message);
                case HttpStatusCode.Unauthorized:
                    return GetMessagesResult.Unauthorized(payload.Response);
                case (HttpStatusCode)423:
                    return GetMessagesResult.Locked(payload.Response);
            }

            return GetMessagesResult.Success(payload.Response, payload.Data);
        }

        public async Task<GetMessagesHistoryResult> GetMessagesHistoryAsync(string apiKey = null, CancellationToken cancellationToken = default)
        {
            (HttpResponseMessage httpResponseMessage, PostageAppResponsePayload<GetMessagesHistoryResponseData> payload)
                = await SendHttpAsync<PostageAppResponsePayload<GetMessagesHistoryResponseData>>(
                HttpMethod.Post,
                new Uri(new Uri(_options.BaseUri), V1_GET_MESSAGES_HISTORY_URL),
                new { ApiKey = apiKey ?? _options.ApiKey },
                cancellationToken);

            switch (httpResponseMessage.StatusCode)
            {
                case HttpStatusCode.NotAcceptable:
                    throw new InvalidOperationException(payload.Response.Message);
                case HttpStatusCode.Conflict:
                    throw new InvalidOperationException(payload.Response.Message);
                case HttpStatusCode.Unauthorized:
                    return GetMessagesHistoryResult.Unauthorized(payload.Response);
                case (HttpStatusCode)423:
                    return GetMessagesHistoryResult.Locked(payload.Response);
            }

            return GetMessagesHistoryResult.Success(payload.Response, payload.Data);
        }

        public async Task<GetMessagesHistoryDetailedResult> GetMessagesHistoryDetailedAsync(
            DateTime? from = null,
            DateTime? to = null,
            string apiKey = null,
            CancellationToken cancellationToken = default)
        {
            (HttpResponseMessage httpResponseMessage, PostageAppResponsePayload<GetMessagesHistoryDetailedResponseData> payload)
                = await SendHttpAsync<PostageAppResponsePayload<GetMessagesHistoryDetailedResponseData>>(
                HttpMethod.Post,
                new Uri(new Uri(_options.BaseUri), V1_GET_MESSAGES_HISTORY_DETAILED_URL),
                new { ApiKey = apiKey ?? _options.ApiKey, From = from?.ToString("yyyy-MM-dd"), To = to?.ToString("yyyy-MM-dd") },
                cancellationToken);

            switch (httpResponseMessage.StatusCode)
            {
                case HttpStatusCode.NotAcceptable:
                    throw new InvalidOperationException(payload.Response.Message);
                case HttpStatusCode.Conflict:
                    throw new InvalidOperationException(payload.Response.Message);
                case HttpStatusCode.Unauthorized:
                    return GetMessagesHistoryDetailedResult.Unauthorized(payload.Response);
                case (HttpStatusCode)423:
                    return GetMessagesHistoryDetailedResult.Locked(payload.Response);
            }

            return GetMessagesHistoryDetailedResult.Success(payload.Response, payload.Data);
        }

        public async Task<GetMessageTransmissionsResult> GetMessageTransmissionsAsync(string uid, string apiKey = null, CancellationToken cancellationToken = default)
        {
            if (uid == null)
            {
                throw new ArgumentNullException(nameof(uid));
            }

            (HttpResponseMessage httpResponseMessage, PostageAppResponsePayload<MessageTransmissionsData> payload)
                = await SendHttpAsync<PostageAppResponsePayload<MessageTransmissionsData>>(
                HttpMethod.Post,
                new Uri(new Uri(_options.BaseUri), V1_GET_MESSAGE_TRANSMISSIONS_URL),
                new { ApiKey = apiKey ?? _options.ApiKey, Uid = uid },
                cancellationToken);

            switch (httpResponseMessage.StatusCode)
            {
                case HttpStatusCode.NotAcceptable:
                    throw new InvalidOperationException(payload.Response.Message);
                case HttpStatusCode.Conflict:
                    throw new InvalidOperationException(payload.Response.Message);
                case HttpStatusCode.Unauthorized:
                    return GetMessageTransmissionsResult.Unauthorized(payload.Response);
                case HttpStatusCode.NotFound:
                    return GetMessageTransmissionsResult.NotFound(payload.Response);
                case (HttpStatusCode)423:
                    return GetMessageTransmissionsResult.Locked(payload.Response);
            }

            return GetMessageTransmissionsResult.Success(payload.Response, payload.Data);
        }

        public async Task<GetMetricsResult> GetMetricsAsync(string apiKey = null, CancellationToken cancellationToken = default)
        {
            (HttpResponseMessage httpResponseMessage, PostageAppResponsePayload<GetMetricsResponseData> payload)
                = await SendHttpAsync<PostageAppResponsePayload<GetMetricsResponseData>>(
                    HttpMethod.Post,
                    new Uri(new Uri(_options.BaseUri), V1_GET_METRICS_URL),
                    new { ApiKey = apiKey ?? _options.ApiKey },
                    cancellationToken);

            switch (httpResponseMessage.StatusCode)
            {
                case HttpStatusCode.NotAcceptable:
                    throw new InvalidOperationException(payload.Response.Message);
                case HttpStatusCode.Conflict:
                    throw new InvalidOperationException(payload.Response.Message);
                case HttpStatusCode.Unauthorized:
                    return GetMetricsResult.Unauthorized(payload.Response);
                case (HttpStatusCode)423:
                    return GetMetricsResult.Locked(payload.Response);
            }

            return GetMetricsResult.Success(payload.Response, payload.Data);
        }

        public async Task<GetProjectInfoResult> GetProjectInfoAsync(string apiKey = null, CancellationToken cancellationToken = default)
        {
            (HttpResponseMessage httpResponseMessage, PostageAppResponsePayload<GetProjectInfoResponseData> payload)
                = await SendHttpAsync<PostageAppResponsePayload<GetProjectInfoResponseData>>(
                    HttpMethod.Post,
                    new Uri(new Uri(_options.BaseUri), V1_GET_PROJECT_INFO_URL),
                    new { ApiKey = apiKey ?? _options.ApiKey },
                    cancellationToken);

            switch (httpResponseMessage.StatusCode)
            {
                case HttpStatusCode.NotAcceptable:
                    throw new InvalidOperationException(payload.Response.Message);
                case HttpStatusCode.Conflict:
                    throw new InvalidOperationException(payload.Response.Message);
                case HttpStatusCode.Unauthorized:
                    return GetProjectInfoResult.Unauthorized(payload.Response);
                case (HttpStatusCode)423:
                    return GetProjectInfoResult.Locked(payload.Response);
            }

            return GetProjectInfoResult.Success(payload.Response, payload.Data);
        }

        public async Task<GetSuppressionListResult> GetSuppressionListAsync(string apiKey = null, CancellationToken cancellationToken = default)
        {
            (HttpResponseMessage httpResponseMessage, PostageAppResponsePayload<GetSuppressionListResponseData> payload)
                = await SendHttpAsync<PostageAppResponsePayload<GetSuppressionListResponseData>>(
                    HttpMethod.Post,
                    new Uri(new Uri(_options.BaseUri), V1_GET_SUPPRESSION_LIST_URL),
                    new { ApiKey = apiKey ?? _options.ApiKey },
                    cancellationToken);

            switch (httpResponseMessage.StatusCode)
            {
                case HttpStatusCode.NotAcceptable:
                    throw new InvalidOperationException(payload.Response.Message);
                case HttpStatusCode.Conflict:
                    throw new InvalidOperationException(payload.Response.Message);
                case HttpStatusCode.Unauthorized:
                    return GetSuppressionListResult.Unauthorized(payload.Response);
                case (HttpStatusCode)423:
                    return GetSuppressionListResult.Locked(payload.Response);
            }

            return GetSuppressionListResult.Success(payload.Response, payload.Data);
        }

        public async Task<SendMessageResult> SendMessageAsync(Message message, string uid = null, string apiKey = null, CancellationToken cancellationToken = default)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var recipients = message.Recipients?
                .Select(x => new KeyValuePair<string, Dictionary<string, string>>(x.Recipient, x.Variables))
                .ToDictionary(x => x.Key, x => x.Value);

            var headers = message.Headers.ToList();

            if (message.From != null)
            {
                headers.Add(new KeyValuePair<string, string>("from", message.From));
            }

            if (message.Subject != null)
            {
                headers.Add(new KeyValuePair<string, string>("subject", message.Subject));
            }

            if (message.ReplyTo != null)
            {
                headers.Add(new KeyValuePair<string, string>("reply-to", message.ReplyTo));
            }

            var attachments = message.Attachments
                .Select(x => new KeyValuePair<string, SendMessageRequestAttachment>(x.Key, new SendMessageRequestAttachment
                {
                    ContentType = x.Value.ContentType,
                    Content = Convert.ToBase64String(x.Value.Content)
                }))
                .ToDictionary(x => x.Key, x => x.Value);

            var requestModel = new SendMessageRequest
            {
                ApiKey = apiKey ?? _options.ApiKey,
                Uid = uid,
                Arguments = new SendMessageRequestArgs
                {
                    Template = message.Template,
                    RecipientOverride = message.RecipientOverride,
                    Recipients = recipients,
                    Content = message.Content,
                    Headers = headers.ToDictionary(x => x.Key, x => x.Value),
                    Variables = message.Variables,
                    Attachments = attachments
                }
            };

            (HttpResponseMessage httpResponseMessage, PostageAppResponsePayload<MessageRecieptResponseData> payload)
                = await SendHttpAsync<PostageAppResponsePayload<MessageRecieptResponseData>>(
                HttpMethod.Post,
                new Uri(new Uri(_options.BaseUri), V1_SEND_MESSAGE_URL),
                requestModel,
                cancellationToken);

            switch (httpResponseMessage.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return SendMessageResult.BadRequest(payload.Response);
                case HttpStatusCode.PreconditionFailed:
                    return SendMessageResult.PreconditionFailed(payload.Response);
                case HttpStatusCode.NotAcceptable:
                    return SendMessageResult.InvalidUTF8(payload.Response);
                case HttpStatusCode.Conflict:
                    throw new InvalidOperationException(payload.Response.Message);
                case HttpStatusCode.Unauthorized:
                    return SendMessageResult.Unauthorized(payload.Response);
                case (HttpStatusCode)423:
                    return SendMessageResult.Locked(payload.Response);
            }

            return SendMessageResult.Success(payload.Response, payload.Data);
        }

        private async Task<(HttpResponseMessage httpResponseMessage, TResult payload)>
            SendHttpAsync<TResult>(HttpMethod method, Uri uri, object item, CancellationToken cancellationToken = default)
        {
            if (method != HttpMethod.Post)
            {
                throw new ArgumentException("Value must be post.", nameof(method));
            }

            var body = JsonConvert.SerializeObject(item, _jsonSerializerSettings);

            // a new StringContent must be created for each retry
            // as it is disposed after each call
            var requestMessage = new HttpRequestMessage(method, uri)
            {
                Content = new StringContent(body, System.Text.Encoding.UTF8, "application/json")
            };

            HttpResponseMessage httpResponseMessage = await _httpClient.SendAsync(requestMessage, cancellationToken);

            string responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
            var payload = JsonConvert.DeserializeObject<TResult>(responseContent, _jsonSerializerSettings);

            return (httpResponseMessage, payload);
        }
    }
}
