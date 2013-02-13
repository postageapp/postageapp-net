using System;
using System.Collections.Generic;

namespace TWG.PostageApp.Message
{
    /// <summary>
    /// Represents message recipient.
    /// </summary>
    public class Recipient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Recipient"/> class.
        /// </summary>
        /// <param name="email"> Recipient email. </param>
        public Recipient(string email)
        {
            if (email == null)
            {
                throw new ArgumentNullException("email");
            }

            Email = email;
            Variables = new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets or sets recipient email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets recipient variables.
        /// </summary>
        public IDictionary<string, string> Variables { get; private set; }
    }
}