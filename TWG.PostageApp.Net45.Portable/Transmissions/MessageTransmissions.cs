using System.Collections.Generic;

namespace TWG.PostageApp.Transmissions
{
    /// <summary>
    /// Represents message transmissions.
    /// </summary>
    public class MessageTransmissions : Dictionary<string, MessageTransmission>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageTransmissions"/> class.
        /// </summary>
        public MessageTransmissions()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageTransmissions"/> class.
        /// </summary>
        /// <param name="dictionary"> Transmission dictionary. </param>
        public MessageTransmissions(IDictionary<string, MessageTransmission> dictionary)
            : base(dictionary)
        {        
        }

        /// <summary>
        /// Gets or sets message id.
        /// </summary>
        public uint Id { get; set; }
    }
}