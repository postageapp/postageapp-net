using System.Collections.Generic;

namespace PostageApp.Abstractions
{
    public class MessageRecipient
    {
        public MessageRecipient(string recipient)
        {
            Recipient = recipient;
        }

        public MessageRecipient(string recipient, Dictionary<string, string> variables)
            : this(recipient)
        {
            Variables = variables;
        }

        public string Recipient { get; }
        public Dictionary<string, string> Variables { get; }
    }
}
