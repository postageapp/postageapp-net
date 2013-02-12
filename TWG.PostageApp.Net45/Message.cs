using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TWG.PostageApp
{
    /// <summary>
    /// Represents message. 
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        public Message()
        {
            Attachments = new List<Attachment>();
            Recipients = new List<Recipient>();
            Headers = new Dictionary<string, string>();
            Variables = new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets or sets message UID.
        /// </summary>
        [DataMember(Name = "uid")]
        public string Uid { get; set; }

        /// <summary>
        /// Gets or sets template.
        /// </summary>
        [DataMember(Name = "template")]
        public string Template { get; set; }

        /// <summary>
        /// Gets attachments.
        /// </summary>
        [DataMember(Name = "attachments")]
        public IList<Attachment> Attachments { get; private set; }

        /// <summary>
        /// Gets recipient's emails.
        /// </summary>
        [DataMember(Name = "recipiens")]
        public IList<Recipient> Recipients { get; private set; }

        /// <summary>
        /// Gets or sets recipient's email.
        /// </summary>
        public Recipient Recipient
        {
            get
            {
                return Recipients[0];
            }

            set
            {
                Recipients.Insert(0, value);
            }
        }

        /// <summary>
        /// Gets variables.
        /// </summary>
        [DataMember(Name = "variables")]
        public IDictionary<string, string> Variables { get; private set; }

        /// <summary>
        /// Gets message headers.
        /// </summary>
        [DataMember(Name = "headers")]
        public IDictionary<string, string> Headers { get; private set; }

        /// <summary>
        /// Gets or sets recipient override email.
        /// </summary>
        [DataMember(Name = "recipient_override")]
        public string RecipientOverride { get; set; }

        /// <summary>
        /// Gets or sets message subject.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets 
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// Gets or sets reply.
        /// </summary>
        public string ReplyTo { get; set; }

        /// <summary>
        /// Gets or sets message plain text.
        /// </summary>
        [DataMember(Name = "text/plain")]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets message html text.
        /// </summary>
        [DataMember(Name = "text/html")]
        public string Html { get; set; }
    }
}
