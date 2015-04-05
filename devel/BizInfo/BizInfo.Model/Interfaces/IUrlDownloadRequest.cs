using System;

namespace BizInfo.Model.Interfaces
{
    public interface IUrlDownloadRequest
    {
        /// <summary>
        /// Can be content loaded from cache?
        /// </summary>
        bool CanLoadFromCache { get; }

        /// <summary>
        /// Can be content loaded directly (from web)?
        /// </summary>
        bool CanLoadDirectly { get; }

        /// <summary>
        /// Can be result of download stored to cache?
        /// </summary>
        bool CanStoreToCache { get; }

        /// <summary>
        /// Max age of cache content. If <c>null</c> no limit for cached content age is used.
        /// </summary>
        TimeSpan? MaxAgeOfCachedContent { get; }

        /// <summary>
        /// Url which should be loaded
        /// </summary>
        string Url { get; }

        /// <summary>
        /// Acceptor of the downloaded content
        /// </summary>
        IContentAcceptor ContentAcceptor { get; }

        /// <summary>
        /// Was content loaded from cache?
        /// </summary>
        bool IsContentLoadedFromCache { get; set; }

        /// <summary>
        /// Was content stored to cache?
        /// </summary>
        bool IsContentStoredToCache { get; set; }
    }
}