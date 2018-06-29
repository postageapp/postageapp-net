using System.Threading.Tasks;

using Microsoft.Extensions.Options;
using PostageApp.Abstractions;
using PostageApp.Http;

namespace PostageApp.Samples.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var options = new HttpPostageAppClientOptions
            {
                BaseUri = "https://api.postageapp.com",
                ApiKey = ""
            };

            var client = new HttpPostageAppClient(Options.Create(options), new System.Net.Http.HttpClient());
        }
    }
}
