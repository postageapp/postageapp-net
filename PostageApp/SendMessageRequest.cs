using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PostageApp
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
                root.Add(new JProperty("uid", Uid));

            var arguments = new JObject();
            root.Add(new JProperty("arguments", arguments));

            if (Content.Html != null || Content.Text != null)
            {
                var content = new JObject();
                arguments.Add(new JProperty("content", content));

                if (Content.Text != null) content.Add("text/plain", new JValue(Content.Text));
                if (Content.Html != null) content.Add("text/html", new JValue(Content.Html));
            }

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

            if (Template != null)
                arguments.Add(new JProperty("template", Template));

            if (Variables.Count > 0)
            {                
                var variables = new JObject();
                arguments.Add(new JProperty("variables", variables));

                foreach (var k in Variables.Keys)
                    variables.Add(new JProperty(k, Variables[k]));                
            }

            if (Headers.Count > 0)
            {
                var headers = new JObject();
                arguments.Add(new JProperty("headers", headers));

                foreach (var k in Headers.Keys)
                    headers.Add(new JProperty(k, Headers[k]));
            }

            if (Attachments.Count > 0)
            {
                var attachments = new JObject();
                arguments.Add(new JProperty("attachments", attachments));

                foreach (var a in Attachments)
                {
                    var attachment = new JObject();
                    attachments.Add(new JProperty(a.Filename, attachment));
                    attachment.Add(new JProperty("content_type", a.ContentType));
                    attachment.Add(new JProperty("content", a.Content));
                }
            }

            return root.ToString(Formatting.None);
        }
    }
}
