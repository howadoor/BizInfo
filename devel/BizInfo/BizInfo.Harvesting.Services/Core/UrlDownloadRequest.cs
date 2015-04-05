using System;
using BizInfo.Model.Interfaces;

namespace BizInfo.Harvesting.Services.Core
{
    /// <summary>
    /// Represents request of URL download. Simple implementation of <see cref="IUrlDownloadRequest"/>.
    /// </summary>
    public class UrlDownloadRequest : IUrlDownloadRequest, IContentAcceptor
    {
        public UrlDownloadRequest(string url)
        {
            Url = url;
            CanLoadDirectly = true;
        }

        /// <summary>
        /// Time of content load
        /// </summary>
        public DateTime LoadTime { get; set; }

        /// <summary>
        /// Downloaded content
        /// </summary>
        public byte[] Content { get; set; }

        /// <summary>
        /// Content length
        /// </summary>
        public long ContentLength { get; set; }

        /// <summary>
        /// Was content loaded from cache?
        /// </summary>
        public bool IsContentLoadedFromCache { get; set; }

        /// <summary>
        /// Was content stored to cache?
        /// </summary>
        public bool IsContentStoredToCache { get; set; }

        #region IContentAcceptor Members

        /// <summary>
        /// Accept content as an array of bytes
        /// </summary>
        /// <param name="content"></param>
        /// <param name="loadTime"></param>
        public void Accept(byte[] content, DateTime loadTime)
        {
            LoadTime = loadTime;
            Content = content;
            ContentLength = content.Length;
        }

        /// <summary>
        /// Accept content as an array of bytes with length
        /// </summary>
        /// <param name="content"></param>
        /// <param name="contentLength"></param>
        /// <param name="loadTime"></param>
        public void Accept(byte[] content, long contentLength, DateTime loadTime)
        {
            LoadTime = loadTime;
            Content = content;
            ContentLength = contentLength;
        }

        #endregion

        #region IUrlDownloadRequest Members

        /// <summary>
        /// Can be content loaded from cache?
        /// </summary>
        public bool CanLoadFromCache { get; set; }

        /// <summary>
        /// Can be content loaded directly (from web)?
        /// </summary>
        public bool CanLoadDirectly { get; set; }

        /// <summary>
        /// Can be result of download stored to cache?
        /// </summary>
        public bool CanStoreToCache { get; set; }

        /// <summary>
        /// Max age of cache content. If <c>null</c> no limit for cached content age is used.
        /// </summary>
        public TimeSpan? MaxAgeOfCachedContent { get; set; }

        /// <summary>
        /// Url which should be loaded
        /// </summary>
        public string Url { get; private set; }

        public IContentAcceptor ContentAcceptor
        {
            get { return this; }
        }

        #endregion

        public override string ToString()
        {
            return string.Format("Url {0}{1}{2}{3}{4} Loaded {5} Length {6}{7}{8}", Url,
                                 CanLoadFromCache ? " FromCache" : string.Empty,
                                 CanLoadDirectly ? " Directly" : string.Empty,
                                 CanStoreToCache ? " ToCache" : string.Empty,
                                 MaxAgeOfCachedContent.HasValue ? string.Format(" MaxAge {0}", MaxAgeOfCachedContent.Value) : string.Empty,
                                 LoadTime,
                                 ContentLength,
                                 IsContentLoadedFromCache ? " FromCache" : string.Empty,
                                 IsContentLoadedFromCache ? " ToCache" : string.Empty);
        }
    }
}