using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace BizInfo.Core.Services.WebResources.Storage
{
    /// <summary>
    /// Implementation of <see cref="IWebResourcesRepository"/> which stores web resources in the ZIP files.
    /// </summary>
    public class WebResourcesRepository : IWebResourcesRepository
    {
        /// <summary>
        /// Creates new repository in the <see cref="repositoryFolder"/>
        /// </summary>
        /// <param name="repositoryFolder"></param>
        public WebResourcesRepository(string repositoryFolder)
        {
            RepositoryFolder = repositoryFolder;
        }

        /// <summary>
        /// Root folder of the repository
        /// </summary>
        public string RepositoryFolder { get; private set; }

        #region IWebResourcesRepository Members

        public IEnumerable<long> StoredResourceIds
        {
            get
            {
                foreach (var file in Directory.GetFiles(RepositoryFolder, "*.zip", SearchOption.AllDirectories))
                {
                    var filename = Path.GetFileNameWithoutExtension(file);
                    if (Regex.IsMatch(filename, @"^\d+$"))
                    {
                        var resourceId = long.Parse(filename);
                        yield return resourceId;
                    }
                }
            }
        }

        public IStoredResource this[long resourceId]
        {
            get
            {
                IStoredResource resource;
                if (!TryGetResource(resourceId, out resource)) throw new ArgumentException("No resource with this identifier exists in the repository", "resourceId");
                return resource;
            }
        }

        public bool TryGetResource(long resourceId, out IStoredResource resource)
        {
            var filename = GetStoredResourcePathName(resourceId);
            if (!File.Exists(filename))
            {
                resource = null;
                return false;
            }
            resource = new StoredResource(resourceId, filename);
            return true;
        }

        public bool ResourceExists(long resourceId)
        {
            return File.Exists(GetStoredResourcePathName(resourceId));
        }

        public IStoredResource CreateResource(long resourceId, string resourceUrl, ResourceProperties properties = null)
        {
            var filename = GetStoredResourcePathName(resourceId, true);
            if (File.Exists(filename)) throw new ArgumentException("Resource with this identifier already exists", "resourceUrl");
            return new StoredResource(resourceId, filename, resourceUrl, properties);
        }

        public void Remove(long resourceId)
        {
            throw new NotImplementedException();
        }

        public void DestroyRepository()
        {
            Directory.Delete(RepositoryFolder, true);
        }

        #endregion

        /// <summary>
        /// Creates a full path of the resource file based on <see cref="resourceId"/>
        /// </summary>
        /// <param name="resourceId">Identifier of the resource</param>
        /// <param name="create">If <c>true</c> resource path is created</param>
        /// <returns>Path of the resource file</returns>
        protected string GetStoredResourcePath(long resourceId, bool create = false)
        {
            var mega = resourceId/1000000;
            var kilo = resourceId/1000;
            var megaFolder = Path.Combine(RepositoryFolder, string.Format("{0}-{1}", mega*10000000, (mega + 1)*10000000 - 1));
            if (create) Directory.CreateDirectory(megaFolder);
            var kiloFolder = Path.Combine(megaFolder, string.Format("{0}-{1}", kilo*1000, (kilo + 1)*1000 - 1));
            if (create) Directory.CreateDirectory(kiloFolder);
            return kiloFolder;
        }

        /// <summary>
        /// Returns a filename for the resource with <see cref="resourceId"/>
        /// </summary>
        /// <param name="resourceId">Identifier of the resource</param>
        /// <returns>Filename of the resource file</returns>
        protected string GetStoredResourceFilename(long resourceId)
        {
            return string.Format("{0}.zip", resourceId);
        }

        /// <summary>
        /// Creates a full pathname of the resource file based on <see cref="resourceId"/>
        /// </summary>
        /// <param name="resourceId">Identifier of the resource</param>
        /// <param name="create">If <c>true</c> resource path is created</param>
        /// <returns>Pathname of the resource file</returns>
        protected string GetStoredResourcePathName(long resourceId, bool create = false)
        {
            return Path.Combine(GetStoredResourcePath(resourceId, create), GetStoredResourceFilename(resourceId));
        }
    }
}