using System.Collections.Generic;
using TWG.PostageApp.Common;

namespace TWG.PostageApp
{
    /// <summary>
    /// Represents account info.
    /// </summary>
    public class AccountInfo
    {
        /// <summary>
        /// Gets or sets account name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets account URL.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets transmissions information.
        /// </summary>
        public TransmissionsStatistic Transmissions { get; set; }

        /// <summary>
        /// Gets or sets account users.
        /// </summary>
        public Dictionary<string, string> Users { get; set; }
    }
}
