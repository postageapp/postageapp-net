using System.Collections.Generic;

namespace PostageApp.Abstractions
{
    public class GetProjectInfoResponseDataAccount
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public GetAccountInfoResponseDataAccountTransmissions Transmissions { get; set; }
        public Dictionary<string, string> Users { get; set; }
    }
}
