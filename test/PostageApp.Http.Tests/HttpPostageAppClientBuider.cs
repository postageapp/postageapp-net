
using System;
using System.IO;
using System.Net;
using System.Net.Http;

using Microsoft.Extensions.Options;

using Moq;

using RichardSzalay.MockHttp;

namespace PostageApp.Http.Tests
{
    public class HttpPostageAppClientBuider
    {
        private string _baseUri;
        private string _apiKey = "";

        public Mock<IOptions<HttpPostageAppClientOptions>> MockOptions { get; private set; }
        public MockHttpMessageHandler MockHttp { get; private set; }

        public HttpPostageAppClientBuider()
        {
            MockOptions = new Mock<IOptions<HttpPostageAppClientOptions>>();
            MockHttp = new MockHttpMessageHandler();
        }

        public HttpPostageAppClient Build()
        {
            MockOptions.Setup(x => x.Value)
                .Returns(new HttpPostageAppClientOptions
                {
                    BaseUri = _baseUri,
                    ApiKey = _apiKey
                });

            SetupGetAccountInfoMockHttp();
            SetupGetMessageDeliveryStatusMockHttp();
            SetupGetMessageRecieptMockHttp();
            SetupGetMessagesHistoryMockHttp();
            SetupGetMessagesHistoryDetailedMockHttp();
            SetupGetMessageTransmissionsMockHttp();
            SetupGetMetricsMockHttp();
            SetupGetProjectInfoMockHttp();
            SetupGetSuppressionListMockHttp();

            return new HttpPostageAppClient(
                MockOptions.Object,
                MockHttp.ToHttpClient());
        }

        public HttpPostageAppClientBuider WithBaseUri(string baseUri)
        {
            _baseUri = baseUri;

            return this;
        }

        public HttpPostageAppClientBuider WithApiKey(string apiKey)
        {
            _apiKey = apiKey;

            return this;
        }

        private void SetupGetAccountInfoMockHttp()
        {
            var uri = $"{_baseUri}/v.1.0/get_account_info.json";

            SetupGeneralCallError(uri, (t) => BuildContent(t));
            SetupGeneralConflict(uri, (t) => BuildContent(t));
            SetupGeneralLocked(uri, (t) => BuildContent(t));
            SetupGeneralUnauthorized(uri, (t) => BuildContent(t));

            MockHttp.When(HttpMethod.Post, uri)
                .WithContent(BuildContent("successfull_api_key"))
                .Respond("application/json", ReadFixtureFile("GetAccountInfoResponse"));
        }

        private void SetupGetProjectInfoMockHttp()
        {
            var uri = $"{_baseUri}/v.1.0/get_project_info.json";

            SetupGeneralCallError(uri, (t) => BuildContent(t));
            SetupGeneralConflict(uri, (t) => BuildContent(t));
            SetupGeneralLocked(uri, (t) => BuildContent(t));
            SetupGeneralUnauthorized(uri, (t) => BuildContent(t));

            MockHttp.When(HttpMethod.Post, uri)
                .WithContent(BuildContent("successfull_api_key"))
                .Respond("application/json", ReadFixtureFile("GetProjectInfoResponse"));
        }

        private void SetupGetSuppressionListMockHttp()
        {
            var uri = $"{_baseUri}/v.1.0/get_suppression_list.json";

            SetupGeneralCallError(uri, (t) => BuildContent(t));
            SetupGeneralConflict(uri, (t) => BuildContent(t));
            SetupGeneralLocked(uri, (t) => BuildContent(t));
            SetupGeneralUnauthorized(uri, (t) => BuildContent(t));

            MockHttp.When(HttpMethod.Post, uri)
                .WithContent(BuildContent("successfull_api_key"))
                .Respond("application/json", ReadFixtureFile("GetSuppressionListResponse"));
        }

        private void SetupGetMessagesHistoryMockHttp()
        {
            var uri = $"{_baseUri}/v.1.0/messages_history.json";

            SetupGeneralCallError(uri, (t) => BuildContent(t));
            SetupGeneralConflict(uri, (t) => BuildContent(t));
            SetupGeneralLocked(uri, (t) => BuildContent(t));
            SetupGeneralUnauthorized(uri, (t) => BuildContent(t));

            MockHttp.When(HttpMethod.Post, uri)
                .WithContent(BuildContent("successfull_api_key"))
                .Respond("application/json", ReadFixtureFile("GetMessagesHistoryResponse"));
        }

        private void SetupGetMessagesHistoryDetailedMockHttp()
        {
            var uri = $"{_baseUri}/v.1.0/messages_history_detailed.json";

            SetupGeneralCallError(uri, (t) => BuildContentRange(t));
            SetupGeneralConflict(uri, (t) => BuildContentRange(t));
            SetupGeneralLocked(uri, (t) => BuildContentRange(t));
            SetupGeneralUnauthorized(uri, (t) => BuildContentRange(t));

            MockHttp.When(HttpMethod.Post, uri)
                .WithContent(BuildContentRange("successfull_api_key", "2017-03-29", "2017-03-30"))
                .Respond("application/json", ReadFixtureFile("GetMessagesHistoryDetailedResponse"));
        }

        private void SetupGetMetricsMockHttp()
        {
            var uri = $"{_baseUri}/v.1.0/get_metrics.json";

            SetupGeneralCallError(uri, (t) => BuildContent(t));
            SetupGeneralConflict(uri, (t) => BuildContent(t));
            SetupGeneralLocked(uri, (t) => BuildContent(t));
            SetupGeneralUnauthorized(uri, (t) => BuildContent(t));

            MockHttp.When(HttpMethod.Post, uri)
                .WithContent(BuildContent("successfull_api_key"))
                .Respond("application/json", ReadFixtureFile("GetMetricsResponse"));
        }

        private void SetupGetMessageDeliveryStatusMockHttp()
        {
            var uri = $"{_baseUri}/v.1.0/message_delivery_status.json";

            SetupGeneralCallError(uri, (t) => BuildContentUid(t, "some_uid"));
            SetupGeneralConflict(uri, (t) => BuildContentUid(t, "some_uid"));
            SetupGeneralLocked(uri, (t) => BuildContentUid(t, "some_uid"));
            SetupGeneralUnauthorized(uri, (t) => BuildContentUid(t, "some_uid"));
            SetupGeneralNotFound(uri, (t) => BuildContentUid(t, "another_uid"));

            MockHttp.When(HttpMethod.Post, uri)
                .WithContent(BuildContentUid("successfull_api_key", "some_uid"))
                .Respond("application/json", ReadFixtureFile("GetMessageDeliveryStatusResponse"));
        }

        private void SetupGetMessageRecieptMockHttp()
        {
            var uri = $"{_baseUri}/v.1.0/get_message_receipt.json";

            SetupGeneralCallError(uri, (t) => BuildContentUid(t, "some_uid"));
            SetupGeneralConflict(uri, (t) => BuildContentUid(t, "some_uid"));
            SetupGeneralLocked(uri, (t) => BuildContentUid(t, "some_uid"));
            SetupGeneralUnauthorized(uri, (t) => BuildContentUid(t, "some_uid"));
            SetupGeneralNotFound(uri, (t) => BuildContentUid(t, "another_uid"));

            MockHttp.When(HttpMethod.Post, uri)
                .WithContent(BuildContentUid("successfull_api_key", "some_uid"))
                .Respond("application/json", ReadFixtureFile("GetMessageRecieptResponse"));
        }

        private void SetupGetMessageTransmissionsMockHttp()
        {
            var uri = $"{_baseUri}/v.1.0/get_message_transmissions.json";

            SetupGeneralCallError(uri, (t) => BuildContentUid(t, "some_uid"));
            SetupGeneralConflict(uri, (t) => BuildContentUid(t, "some_uid"));
            SetupGeneralLocked(uri, (t) => BuildContentUid(t, "some_uid"));
            SetupGeneralUnauthorized(uri, (t) => BuildContentUid(t, "some_uid"));
            SetupGeneralNotFound(uri, (t) => BuildContentUid(t, "another_uid"));

            MockHttp.When(HttpMethod.Post, uri)
                .WithContent(BuildContentUid("successfull_api_key", "some_uid"))
                .Respond("application/json", ReadFixtureFile("GetMessageTransmissionsResponse"));
        }

        private string BuildContentUid(string apiKey, string uid)
        {
            return "{\"api_key\":\"" + apiKey + "\",\"uid\":\"" + uid + "\"}";
        }

        private string BuildContentRange(string apiKey, string from = null, string to = null)
        {
            if (from == null)
            {
                from = "null";
            }
            else
            {
                from = "\"" + from + "\"";
            }

            if (to == null)
            {
                to = "null";
            }
            else
            {
                to = "\"" + to + "\"";
            }

            return "{\"api_key\":\"" + apiKey + "\",\"from\":" + from + ",\"to\":" + to + "}";
        }

        private string BuildContent(string apiKey)
        {
            return "{\"api_key\":\"" + apiKey + "\"}";
        }

        private string ReadFixtureFile(string fileName)
        {
            return File.ReadAllText($"./fixtures/{fileName}.json");
        }

        private void SetupGeneralCallError(string uri, Func<string, string> contentBuilder)
        {
            MockHttp.When(HttpMethod.Post, uri)
                .WithContent(contentBuilder("call_error_api_key"))
                .Respond(HttpStatusCode.Conflict, "application/json", "{\"response\":{\"status\":\"call_error\",\"uid\":null,\"message\":\"The posted content is not valid JSON.\"}}");
        }

        private void SetupGeneralConflict(string uri, Func<string, string> contentBuilder)
        {
            MockHttp.When(HttpMethod.Post, uri)
                .WithContent(contentBuilder("conflict_api_key"))
                .Respond(HttpStatusCode.Conflict, "application/json", "{\"response\":{\"status\":\"conflict\",\"uid\":null,\"message\":\"Invalid API version.\"}}");
        }

        private void SetupGeneralLocked(string uri, Func<string, string> contentBuilder)
        {
            MockHttp.When(HttpMethod.Post, uri)
                .WithContent(contentBuilder("locked_api_key"))
                .Respond((HttpStatusCode)423, "application/json", "{\"response\":{\"status\":\"locked\",\"uid\":null,\"message\":\"The specified API key is not valid.\"}}");
        }

        private void SetupGeneralUnauthorized(string uri, Func<string, string> contentBuilder)
        {
            MockHttp.When(HttpMethod.Post, uri)
                .WithContent(contentBuilder("none_api_key"))
                .Respond(HttpStatusCode.Unauthorized, "application/json", "{\"response\":{\"status\":\"unauthorized\",\"uid\":null,\"message\":\"Invalid or inactive project API key used.\"}}");
        }

        private void SetupGeneralNotFound(string uri, Func<string, string> contentBuilder)
        {
            MockHttp.When(HttpMethod.Post, uri)
                .WithContent(contentBuilder("not_found_api_key"))
                .Respond(HttpStatusCode.NotFound, "application/json", "{\"response\":{\"status\":\"not_found\",\"uid\":null,\"message\":\"Message with UID MESSAGE_UID was not found.\"}}");
        }
    }
}
