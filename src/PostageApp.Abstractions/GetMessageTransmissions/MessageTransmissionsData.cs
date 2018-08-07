using System.Collections.Generic;

namespace PostageApp.Abstractions
{
    public class MessageTransmissionsData
    {
        public MessageTransmissionsDataMessage Message { get; set; }

        public Dictionary<string, MessageTransmissionsDataTransmission> Transmissions { get; set; }
    }
}
