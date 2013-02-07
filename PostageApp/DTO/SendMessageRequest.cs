﻿using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PostageApp.DTO
{
    public class Recipient
    {
        public string Email { get; set; }
        public IDictionary<string, string> Variables { get; set; }

        public Recipient(string email)
        {
            Email = email;
            Variables = new Dictionary<string, string>();
        }
    }

    public class Attachment
    {
        public string Filename { get; set; }
        public string ContentType { get; set; }
        public string Content { get; set; }
    }

    public class Content
    {
        public string Text { get; set; }
        public string Html { get; set; }
    }

    public class SendMessageRequest
    {
        public string Uid { get; set; }
        public Content Content { get; set; }
        public string Template { get; set; }
        public IList<Attachment> Attachments { get; set; }
        public IList<Recipient> Recipients { get; set; }
        
        public IDictionary<string, string> Variables { get; set; }
        public IDictionary<string, string> Headers { get; set; }
        public string RecipientOverride { get; set; }

        public SendMessageRequest()
        {
            Content = new Content();
            Attachments = new List<Attachment>();
            Recipients = new List<Recipient>();
            Variables = new Dictionary<string, string>();
            Headers = new Dictionary<string, string>();
        }

        public string ToJson(string apiKey)
        {
            var root = new JObject {{"api_key", new JValue(apiKey)}};

            if (Uid != null)
            {
                root.Add(new JProperty("uid", Uid));
            }

            if (Content.Html != null || Content.Text != null)
            {
                var content = new JObject();
                root.Add(new JProperty("content", content));

                if (Content.Text != null) content.Add("text/plain", new JValue(Content.Text));
                if (Content.Html != null) content.Add("text/html", new JValue(Content.Html));
            }

            var arguments = new JObject();
            root.Add(new JProperty("arguments", arguments));

            if (Recipients.Count > 0)
            {
                var recipients = new JObject();
                arguments.Add(new JProperty("recipients", recipients));

                foreach (var r in Recipients)
                {
                    var variables = new JObject();
                    recipients.Add(new JProperty(r.Email, variables));

                    foreach (var k in r.Variables.Keys)
                        variables.Add(new JProperty(k, r.Variables[k]));
                }
            }

            return root.ToString(Formatting.Indented);
        }
    }
}
