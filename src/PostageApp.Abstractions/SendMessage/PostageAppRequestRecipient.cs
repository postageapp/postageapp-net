using System.Collections.Generic;

namespace PostageApp.Abstractions
{
    public class PostageAppRequestRecipient
    {
        public PostageAppRequestRecipient(string recipient)
        {
        }

        public PostageAppRequestRecipient(string recipient, Dictionary<string, string> variables)
            : this(recipient)
        {
        }
    }
}
