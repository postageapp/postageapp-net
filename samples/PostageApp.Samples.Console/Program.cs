using System.Threading.Tasks;

using Microsoft.Extensions.Options;

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

            // 8d98e24814123a964eb9a57b87cab53a1fa95565
            // 367abbe965360923a196cdd56baf1412957f3553
            var result = await client.GetMessageDeliveryStatusAsync("8d98e24814123a964eb9a57b87cab53a1fa95565");
        }
    }
}
