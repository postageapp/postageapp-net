using System.Collections.Generic;

namespace PostageApp.Abstractions
{
    public class Message
    {
        public string From { get; set; }

        public string Subject { get; set; }

        public string Template { get; set; }

        public PostageAppRequestRecipient[] Recipients { get; set; }

        public Dictionary<string, string> Variables { get; set; }
    }
}
