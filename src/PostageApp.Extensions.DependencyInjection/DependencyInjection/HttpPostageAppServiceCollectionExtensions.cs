using System;

using Microsoft.Extensions.DependencyInjection;

using PostageApp.Abstractions;
using PostageApp.Http;

namespace PostageApp.Extensions.DependencyInjection.DependencyInjection
{
    public static class HttpPostageAppServiceCollectionExtensions
    {
        public static IHttpClientBuilder AddPostageAppClient(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            return services.AddHttpClient<IPostageAppClient, HttpPostageAppClient>();
        }
    }
}
