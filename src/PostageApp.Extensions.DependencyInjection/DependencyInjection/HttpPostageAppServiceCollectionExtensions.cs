using System;

using PostageApp.Abstractions;
using PostageApp.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HttpPostageAppServiceCollectionExtensions
    {
        public static IHttpClientBuilder AddPostageAppClient(this IServiceCollection services, Action<HttpPostageAppClientOptions> configureAction = null)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configureAction != null)
            {
                services.Configure(configureAction);
            }

            return services.AddHttpClient<IPostageAppClient, HttpPostageAppClient>();
        }
    }
}
