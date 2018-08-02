
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
            SetupGetMessageTransmissionsMockHttp();
            SetupGetMetricsMockHttp();

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

            SetupGeneralCallError(uri);
            SetupGeneralConflict(uri);
            SetupGeneralLocked(uri);
            SetupGeneralUnauthorized(uri);

            MockHttp.When(HttpMethod.Post, uri)
                .WithContent(BuildContent("successfull_api_key", null))
                .Respond("application/json", ReadFixtureFile("GetAccountInfoResponse"));
        }

        private void SetupGetMetricsMockHttp()
        {
            var uri = $"{_baseUri}/v.1.0/get_metrics.json";

            SetupGeneralCallError(uri);
            SetupGeneralConflict(uri);
            SetupGeneralLocked(uri);
            SetupGeneralUnauthorized(uri);

            MockHttp.When(HttpMethod.Post, uri)
                .WithContent(BuildContent("successfull_api_key", null))
                .Respond("application/json", ReadFixtureFile("GetMetricsResponse"));
        }

        private void SetupGetMessageDeliveryStatusMockHttp()
        {
            var uri = $"{_baseUri}/v.1.0/message_delivery_status.json";

            SetupGeneralCallError(uri, "some_uid");
            SetupGeneralConflict(uri, "some_uid");
            SetupGeneralLocked(uri, "some_uid");
            SetupGeneralUnauthorized(uri, "some_uid");
            SetupGeneralNotFound(uri, "another_uid");

            MockHttp.When(HttpMethod.Post, uri)
                .WithContent(BuildContent("successfull_api_key", "some_uid"))
                .Respond("application/json", ReadFixtureFile("GetMessageDeliveryStatusResponse"));
        }

        private void SetupGetMessageRecieptMockHttp()
        {
            var uri = $"{_baseUri}/v.1.0/get_message_receipt.json";

            SetupGeneralCallError(uri, "some_uid");
            SetupGeneralConflict(uri, "some_uid");
            SetupGeneralLocked(uri, "some_uid");
            SetupGeneralUnauthorized(uri, "some_uid");
            SetupGeneralNotFound(uri, "another_uid");

            MockHttp.When(HttpMethod.Post, uri)
                .WithContent(BuildContent("successfull_api_key", "some_uid"))
                .Respond("application/json", ReadFixtureFile("GetMessageRecieptResponse"));
        }

        private void SetupGetMessageTransmissionsMockHttp()
        {
            var uri = $"{_baseUri}/v.1.0/get_message_transmissions.json";

            SetupGeneralCallError(uri, "some_uid");
            SetupGeneralConflict(uri, "some_uid");
            SetupGeneralLocked(uri, "some_uid");
            SetupGeneralUnauthorized(uri, "some_uid");
            SetupGeneralNotFound(uri, "another_uid");

            MockHttp.When(HttpMethod.Post, uri)
                .WithContent(BuildContent("successfull_api_key", "some_uid"))
                .Respond("application/json", ReadFixtureFile("GetMessageTransmissionsResponse"));
        }

        private string BuildContent(string apiKey, string uid)
        {
            if (uid == null)
            {
                return "{\"api_key\":\"" + apiKey + "\"}";
            }

            return "{\"api_key\":\"" + apiKey + "\",\"uid\":\"" + uid + "\"}";
        }

        private string ReadFixtureFile(string fileName)
        {
            return File.ReadAllText($"./fixtures/{fileName}.json");
        }

        private void SetupGeneralCallError(string uri, string uid = null)
        {
            MockHttp.When(HttpMethod.Post, uri)
                .WithContent(BuildContent("call_error_api_key", uid))
                .Respond(HttpStatusCode.Conflict, "application/json", "{\"response\":{\"status\":\"call_error\",\"uid\":null,\"message\":\"The posted content is not valid JSON.\"}}");
        }

        private void SetupGeneralConflict(string uri, string uid = null)
        {
            MockHttp.When(HttpMethod.Post, uri)
                .WithContent(BuildContent("conflict_api_key", uid))
                .Respond(HttpStatusCode.Conflict, "application/json", "{\"response\":{\"status\":\"conflict\",\"uid\":null,\"message\":\"Invalid API version.\"}}");
        }

        private void SetupGeneralLocked(string uri, string uid = null)
        {
            MockHttp.When(HttpMethod.Post, uri)
                .WithContent(BuildContent("locked_api_key", uid))
                .Respond((HttpStatusCode)423, "application/json", "{\"response\":{\"status\":\"locked\",\"uid\":null,\"message\":\"The specified API key is not valid.\"}}");
        }

        private void SetupGeneralUnauthorized(string uri, string uid = null)
        {
            MockHttp.When(HttpMethod.Post, uri)
                .WithContent(BuildContent("none_api_key", uid))
                .Respond(HttpStatusCode.Unauthorized, "application/json", "{\"response\":{\"status\":\"unauthorized\",\"uid\":null,\"message\":\"Invalid or inactive project API key used.\"}}");
        }

        private void SetupGeneralNotFound(string uri, string uid)
        {
            MockHttp.When(HttpMethod.Post, uri)
                .WithContent(BuildContent("not_found_api_key", uid))
                .Respond(HttpStatusCode.NotFound, "application/json", "{\"response\":{\"status\":\"not_found\",\"uid\":null,\"message\":\"Message with UID MESSAGE_UID was not found.\"}}");
        }
    }
}
