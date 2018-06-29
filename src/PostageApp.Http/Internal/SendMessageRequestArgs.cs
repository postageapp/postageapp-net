using System.Collections.Generic;

using PostageApp.Abstractions;

namespace PostageApp.Http.Internal
{
    internal class SendMessageRequestArgs
    {
        public string Template { get; set; }

        public string RecipientOverride { get; set; }

        public Dictionary<string, Dictionary<string, string>> Recipients { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public Dictionary<string, string> Content { get; set; }

        public Dictionary<string, MessageAttachment> Attachments { get; set; }

        public Dictionary<string, string> Variables { get; set; }
    }
}
