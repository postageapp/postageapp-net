using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TWG.PostageApp.Message;

namespace TWG.PostageApp.Converters
{
    /// <summary>
    /// Represents message converter.
    /// </summary>
    public class MessageConverter : JsonConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageConverter"/> class.
        /// </summary>
        /// <param name="apiKey"> Project api key. </param>
        public MessageConverter(string apiKey)
        {
            ApiKey = apiKey;
        }

        /// <summary>
        /// Gets or sets API key.
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param><param name="value">The value.</param><param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var message = (Message.Message)value;

            var root = new JObject { { "api_key", new JValue(ApiKey) } };

            if (message.Uid != null)
            {
                root.Add(new JProperty("uid", message.Uid));
            }

            var arguments = new JObject();
            root.Add(new JProperty("arguments", arguments));

            if (message.Html != null || message.Text != null)
            {
                var content = new JObject();
                arguments.Add(new JProperty("content", content));

                if (message.Text != null)
                {
                    content.Add("text/plain", new JValue(message.Text));
                }

                if (message.Html != null)
                {
                    content.Add("text/html", new JValue(message.Html));
                }
            }

            if (message.Recipients.Count != 0)
            {
                var recipients = new JObject();
                arguments.Add(new JProperty("recipients", recipients));

                foreach (var recipient in message.Recipients)
                {
                    var variables = new JObject();
                    recipients.Add(new JProperty(recipient.Email, variables));

                    foreach (var variable in recipient.Variables)
                    {
                        variables.Add(new JProperty(variable.Key, variable.Value));
                    }
                }
            }

            if (message.RecipientOverride != null)
            {
                arguments.Add(new JProperty("recipient_override", message.RecipientOverride));
            }

            if (message.Template != null)
            {
                arguments.Add(new JProperty("template", message.Template));
            }

            if (message.Variables.Count != 0)
            {
                var variables = new JObject();
                arguments.Add(new JProperty("variables", variables));

                foreach (var variable in message.Variables)
                {
                    variables.Add(new JProperty(variable.Key, variable.Value));
                }
            }

            if (message.Subject != null || message.From != null || message.ReplyTo != null || message.Headers.Count != 0)
            {
                var headers = new JObject();
                arguments.Add(new JProperty("headers", headers));

                if (message.Subject != null)
                {
                    headers.Add(new JProperty("subject", message.Subject));
                }

                if (message.From != null)
                {
                    headers.Add(new JProperty("From", message.From));
                }

                if (message.ReplyTo != null)
                {
                    headers.Add(new JProperty("reply-to", message.ReplyTo));
                }

                foreach (var header in message.Headers)
                {
                    headers.Add(new JProperty(header.Key, header.Value));
                }
            }

            if (message.Attachments.Count != 0)
            {
                var attachments = new JObject();
                arguments.Add(new JProperty("attachments", attachments));

                foreach (Attachment attachment in message.Attachments)
                {
                    var attachmentJObject = new JObject();
                    attachments.Add(new JProperty(attachment.FileName, attachmentJObject));
                    attachmentJObject.Add(new JProperty("content_type", attachment.ContentType));

                    var bytes = new byte[attachment.ContentStream.Length];
                    attachment.ContentStream.Read(bytes, 0, Convert.ToInt32(attachment.ContentStream.Length));
                    var content = Convert.ToBase64String(bytes);

                    attachmentJObject.Add(new JProperty("content", content));
                }
            }

            root.WriteTo(writer);
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader"/> to read from.</param><param name="objectType">Type of the object.</param><param name="existingValue">The existing value of object being read.</param><param name="serializer">The calling serializer.</param>
        /// <returns>
        /// The object value.
        /// </returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(Message.Message))
            {
                return true;
            }

            return false;
        }
    }
}