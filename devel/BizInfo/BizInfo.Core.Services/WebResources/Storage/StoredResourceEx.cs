using System.Linq;

namespace BizInfo.Core.Services.WebResources.Storage
{
    /// <summary>
    /// Collects extension methods of <see cref="IStoredResource"/> interface
    /// </summary>
    public static class StoredResourceEx
    {
        /// <summary>
        /// Looks for newest resource version in <see cref="resource"/>
        /// </summary>
        /// <param name="resource">resource where newest version will be searched</param>
        /// <returns>Newest version or <c>null</c> when <see cref="resource"/> contains no versions at all</returns>
        public static IStoredResourceVersion GetNewestVersion(this IStoredResource resource)
        {
            IStoredResourceVersion newestVersion = null;
            foreach (var version in resource.Versions)
            {
                if (newestVersion == null || newestVersion.CreationTime < version.CreationTime) newestVersion = version;
            }
            return newestVersion;
        }
    }
}