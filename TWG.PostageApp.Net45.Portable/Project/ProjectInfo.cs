using System.Collections.Generic;
using TWG.PostageApp.Common;

namespace TWG.PostageApp.Project
{
    /// <summary>
    /// Represents project info.
    /// </summary>
    public class ProjectInfo
    {
        /// <summary>
        /// Gets or sets project name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets project URL.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets transmissions information.
        /// </summary>
        public TransmissionsStatistic Transmissions { get; set; }

        /// <summary>
        /// Gets or sets users.
        /// </summary>
        public Dictionary<string, string> Users { get; set; }
    }
}
