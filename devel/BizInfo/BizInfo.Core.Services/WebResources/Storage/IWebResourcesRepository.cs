using System.Collections.Generic;

namespace BizInfo.Core.Services.WebResources.Storage
{
    public interface IWebResourcesRepository
    {
        /// <summary>
        /// Enumerates through stored resources identifiers
        /// </summary>
        IEnumerable<long> StoredResourceIds { get; }

        /// <summary>
        /// Returns resource by <see cref="resourceId"/>
        /// </summary>
        /// <param name="resourceId">Identifier of the resource</param>
        /// <returns>Instance of <see cref="IStoredResource"/>. Throws an exception if no resource with identifier <see cref="resourceId"/> exists in the repository.</returns>
        IStoredResource this[long resourceId] { get; }

        /// <summary>
        /// Tries to get instance of <see cref="IStoredResource"/> identified by <see cref="resourceId"/>
        /// </summary>
        /// <param name="resourceId">Identifier of the resource</param>
        /// <param name="resource">Resource found</param>
        /// <returns><c>true</c> if resource found, <c>false</c> otherwise</returns>
        bool TryGetResource(long resourceId, out IStoredResource resource);

        /// <summary>
        /// Checks if resource identified by <see cref="resourceId"/> exists
        /// </summary>
        /// <param name="resourceId">Identifier of the resource</param>
        /// <returns><c>true</c> if resource exists, <c>false</c> otherwise</returns>
        bool ResourceExists(long resourceId);

        /// <summary>
        /// Creates new resource in the repository. Throws an exception if resource identified by <see cref="resourceId"/> already exists in the repository.
        /// </summary>
        /// <param name="resourceId">Identifier of the newly created resource</param>
        /// <param name="resourceUrl">URL of the newly created resource</param>
        /// <param name="properties">Optional resource properties</param>
        /// <returns>Newly created instance of <see cref="IStoredResource"/></returns>
        IStoredResource CreateResource(long resourceId, string resourceUrl, ResourceProperties properties = null);

        /// <summary>
        /// Removes resource identified by <see cref="resourceId"/> from the repository
        /// </summary>
        /// <param name="resourceId">Identifier of the resource</param>
        void Remove(long resourceId);

        /// <summary>
        /// Destroys whole repository with all content and all related stuff
        /// </summary>
        void DestroyRepository();
    }
}