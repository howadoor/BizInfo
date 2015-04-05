using System;
using System.Collections.Generic;

namespace BizInfo.Core.Services.WebResources.Storage
{
    /// <summary>
    /// Represents declaration of the content of <see cref="StoredResource"/>
    /// </summary>
    public class StoredResourceManifest
    {
        /// <summary>
        /// Url of the stored resource
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Properties information. Its content is defined by client. Simply some string to store something.
        /// </summary>
        public ResourceProperties Properties { get; set; }

        /// <summary>
        /// List of versions
        /// </summary>
        public List<ResourceVersionManifest> Versions { get; set; }

        /// <summary>
        /// Creates new instance and initializes it
        /// </summary>
        public StoredResourceManifest()
        {
            Versions = new List<ResourceVersionManifest>();
        }
    }

    /// <summary>
    /// Declaration of one version of <see cref="StoredResource"/>
    /// </summary>
    public class ResourceVersionManifest
    {
        /// <summary>
        /// Filename of the version
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// Creation time of the version
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Properties information. Its content is defined by client. Simply some string to store something.
        /// </summary>
        public ResourceProperties Properties { get; set; }
    }
}