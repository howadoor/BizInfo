using System;
using System.Data.Objects;
using System.IO;
using System.Linq;
using BizInfo.Harvesting.Interfaces.Processing;
using BizInfo.Model.Entities;
using BizInfo.Model.Interfaces;

namespace BizInfo.Harvesting.Services.Core
{
    internal class UrlBlobsCache
    {
        BlobsStorage blobsStorage = CreateBlobsStorage();

        /// <summary>
        /// TODO Extremely stupid solution of configuration
        /// </summary>
        /// <returns></returns>
        private static BlobsStorage CreateBlobsStorage()
        {
            var path = @"h:\BizInfo\Blobs";
            if (!Directory.Exists(path)) path = @"c:\projects\Autocentrum Hrušovany\BizInfo\Blobs";
            return new BlobsStorage(path);
        }

        public UrlBlobsCache(BizInfoModelContainer container)
        {
            Container = new BizInfoModelContainer(container.Connection.ConnectionString);
        }

        public BizInfoModelContainer Container { get; private set; }

        public bool TryGetUrl(string url, out DownloadedBlob blob, TimeSpan? maxAge = null)
        {
            lock (this)
            {
                if (!new Uri(url).IsAbsoluteUri) throw new ArgumentException("Only absolute URLs are allowed", "url");
                blob = CachedBlobs(url, maxAge).FirstOrDefault();
                return blob != null;
            }
        }

        private IOrderedQueryable<DownloadedBlob> CachedBlobs(string url, TimeSpan? maxAge = null)
        {
            var minDate = maxAge.HasValue ? DateTime.Now - maxAge.Value : new DateTime(1900, 1, 1);
            return from candidateBlob in Container.DownloadedBlobSet
                   where candidateBlob.SourceUrl == url && candidateBlob.DownloadDate >= minDate
                   orderby candidateBlob.DownloadDate
                   select candidateBlob;
        }

        public bool TryGetUrl(IUrlDownloadRequest request)
        {
            DownloadedBlob blob;
            if (!TryGetUrl(request.Url, out blob, request.MaxAgeOfCachedContent)) return false;
            request.IsContentLoadedFromCache = true;
            request.ContentAcceptor.Accept(LoadContent(blob), blob.DownloadDate);
            return true;
        }

        private byte[] LoadContent(DownloadedBlob blob)
        {
            byte[] content;
            int length;
            return blobsStorage.TryRead((int) blob.Id, blob.Extension, out content, out length) ? content : null;
        }

        /// <summary>
        /// Stores content belonging to <see cref="url"/> into the cache. When such content already exists, it is replaced.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <param name="contentLength"></param>
        /// <param name="loadTime"></param>
        /// <param name="extension"></param>
        public void AddUrl(string url, byte[] content, long contentLength, DateTime loadTime, string extension)
        {
            if (!new Uri(url).IsAbsoluteUri) throw new ArgumentException("Only absolute URLs are allowed", "url");
            lock (this)
            {
                var blob = Container.DownloadedBlobSet.Where(candidateBlob => candidateBlob.SourceUrl == url).OrderByDescending(candidateBlob => candidateBlob.DownloadDate).FirstOrDefault();
                if (blob == null)
                {
                    blob = Container.DownloadedBlobSet.CreateObject();
                    Container.DownloadedBlobSet.AddObject(blob);
                }
                blob.SourceUrl = url;
                blob.DownloadDate = loadTime;
                blob.Extension = extension;
                Container.SaveChanges();
                SaveContent(blob, content, contentLength);
            }
        }

        private void SaveContent(DownloadedBlob blob, byte[] content, long contentLength)
        {
            blobsStorage.Store((int) blob.Id, blob.Extension, content, (int) contentLength);
        }

        public bool IsInCache(string url)
        {
            if (!new Uri(url).IsAbsoluteUri) throw new ArgumentException("Only absolute URLs are allowed", "url");
            return CachedBlobs(url).FirstOrDefault() != null;
        }

        private IQueryable<WebSource> WebSources(string url)
        {
            return from candidateWebSource in Container.WebSourceSet
                   where candidateWebSource.Url == url 
                   select candidateWebSource;
        }
        
        public bool AddAsScouted(string url)
        {
            lock (this)
            {
                var count = WebSources(url).Count();
                if (count > 1) throw new InvalidOperationException("Found two WebSource objects in the database with the same URL");
                if (count > 0) return false;
                var webSource = Container.WebSourceSet.CreateObject();
                Container.WebSourceSet.AddObject(webSource);
                webSource.Url = url;
                webSource.Scouted = DateTime.Now;
                Container.SaveChanges();
            }
            return true;
        }

        public void Remove(string url)
        {
            lock (this)
            {
                var blobs = CachedBlobs(url).ToArray();
                foreach (var blob in blobs)
                {
                    Container.DownloadedBlobSet.DeleteObject(blob);
                    blobsStorage.TryRemove((int) blob.Id, blob.Extension);
                }
            }
        }
    }
}