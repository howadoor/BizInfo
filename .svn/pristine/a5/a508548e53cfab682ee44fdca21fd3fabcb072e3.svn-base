using System;
using System.Collections.Generic;
using System.IO;

namespace BizInfo.Core.Services.WebResources.Storage
{
    /// <summary>
    /// Represents one stored resource
    /// </summary>
    public interface IStoredResource
    {
        /// <summary>
        /// Url of the resource
        /// </summary>
        string Url { get; }

        /// <summary>
        /// Properties informations extending informations from URL
        /// </summary>
        ResourceProperties Properties { get; }

        /// <summary>
        /// Returns all versions of web source stored in the instance of <see cref="IStoredResource"/>
        /// </summary>
        IEnumerable<IStoredResourceVersion> Versions { get; }

        /// <summary>
        /// Identifier of the resource in repository
        /// </summary>
        long Id { get; }

        /// <summary>
        /// Creates new version of resource
        /// </summary>
        /// <param name="creationTime">Creation time of the version</param>
        /// <param name="data">Data</param>
        /// <param name="properties">Optional properties of the version</param>
        /// <returns>New instance of <see cref="IStoredResourceVersion"/></returns>
        IStoredResourceVersion AddVersion(DateTime creationTime, Stream data, ResourceProperties properties = null);
    }

    /// <summary>
    /// Properties of <see cref="IStoredResource"/> and <see cref="IStoredResourceVersion"/>
    /// </summary>
    public class ResourceProperties : Dictionary<object, object>
    {
    }

    /// <summary>
    /// Version of stored web resource (<see cref="IStoredResource"/>). Each resource can have many versions, usually downloaded data from the same URL in different times.
    /// </summary>
    public interface IStoredResourceVersion
    {
        /// <summary>
        /// Properties informations
        /// </summary>
        ResourceProperties Properties { get; }

        /// <summary>
        /// Time of creation (downloading) data of this version
        /// </summary>
        DateTime CreationTime { get; }

        /// <summary>
        /// Returns data (content) of this version of resource
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="length"></param>
        void GetContent(out byte[] bytes, out long length);
    }
}