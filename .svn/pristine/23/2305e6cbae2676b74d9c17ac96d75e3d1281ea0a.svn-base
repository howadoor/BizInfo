using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace BizInfo.Harvesting.Services.Core
{
    /// <summary>
    /// Describes content of one compressed file in <see cref="BlobsStorage"/>. Should contain all informations for complete data recovery. It should be possible
    /// complete reconstruction of database only from stored blobs + extra info in manifests.
    /// </summary>
    public class BlobsStorageManifest
    {
        public BlobsForUrlCollection BlobsOfUrl = new BlobsForUrlCollection();

        public bool TryGetBlobsForUrl(string url, out IEnumerable<BlobFile> blobs)
        {
            lock (this)
            {
                if (!BlobsOfUrl.Contains(url))
                {
                    blobs = null;
                    return false;
                }
                blobs = BlobsOfUrl[url].BlobFiles;
                return true;
            }
        }

        public void AddBlobFile(string url, BlobFile blobFile)
        {
            lock (this)
            {
                if (!BlobsOfUrl.Contains(url)) BlobsOfUrl.Add(new BlobsForUrl(url));
                BlobsOfUrl[url].BlobFiles.Add(blobFile);
            }
        }
    }

    public class BlobsForUrlCollection : KeyedCollection<string, BlobsForUrl>
    {
        protected override string GetKeyForItem(BlobsForUrl item)
        {
            return item.Url;
        }
    }

    public class BlobsForUrl
    {
        /// <summary>
        /// Url of the original blob
        /// </summary>
        public string Url { get; set; }

        public List<BlobFile> BlobFiles { get; set; }

        public BlobsForUrl()
        {
            BlobFiles = new List<BlobFile>();
        }

        public BlobsForUrl(string url) : this()
        {
            Url = url;
        }
    }

    public class BlobFile
    {
        /// <summary>
        /// Name of the file in the blob cache
        /// </summary>
        public string BlobFilename;

        /// <summary>
        /// Time when blob was stored in the cache
        /// </summary>
        public DateTime StoredTime;

        
        /// <summary>
        /// Extra client's informations stored in the blobs cache within the file
        /// </summary>
        public Dictionary<string, object> ExtraInfo = new Dictionary<string, object>();
    }
}