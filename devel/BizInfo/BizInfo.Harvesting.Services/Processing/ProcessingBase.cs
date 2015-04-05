using System;
using System.IO;
using System.Text;
using BizInfo.App.Services.Logging;
using BizInfo.Harvesting.Interfaces.Processing;
using BizInfo.Harvesting.Services.Core;
using BizInfo.Model.Interfaces;
using HtmlAgilityPack;

namespace BizInfo.Harvesting.Services.Processing
{
    public class ProcessingBase
    {
        protected ProcessingBase(Encoding encoding, UrlLoadOptions loadOptions)
        {
            this.Encoding = encoding;
            this.LoadOptions = loadOptions;
        }

        public Encoding Encoding
        {
            get; protected set;
        }

        public UrlLoadOptions LoadOptions
        {
            get;
            protected set;
        }

        public HtmlDocument LoadHtmlDocument(string url, IUrlDownloader loader)
        {
            DateTime loadTime;
            return LoadHtmlDocument(url, loader, Encoding, LoadOptions, out loadTime);
        }

        public HtmlDocument LoadHtmlDocument(string url, IUrlDownloader loader, out DateTime loadTime)
        {
            return LoadHtmlDocument(url, loader, Encoding, LoadOptions, out loadTime);
        }

        public HtmlDocument LoadHtmlDocument(string url, IUrlDownloader loader, UrlLoadOptions loadOptions, out DateTime loadTime)
        {
            return LoadHtmlDocument(url, loader, Encoding, loadOptions, out loadTime);
        }

        protected static HtmlDocument LoadHtmlDocument(string url, IUrlDownloader loader, Encoding encoding, UrlLoadOptions loadOptions, out DateTime loadTime)
        {
            using (loader.LogOperation(string.Format("Loading {0} [{1}, {2}]", url, encoding, loadOptions)))
            {
                var request = new UrlDownloadRequest(url) {CanLoadFromCache = (loadOptions & UrlLoadOptions.LoadFromCacheIfPossible) != UrlLoadOptions.None, CanStoreToCache = (loadOptions & UrlLoadOptions.StoreToCacheWhenLoaded) != UrlLoadOptions.None};
                loader.Load(request);
                var htmlDocument = new HtmlDocument();
                htmlDocument.Load(new MemoryStream(request.Content), encoding);
                loadTime = request.LoadTime;
                return htmlDocument;
            }
        }

        public static string ToAbsoluteUrl(string relativeUrl, string baseUrl)
        {
            var uri = new Uri(relativeUrl, UriKind.RelativeOrAbsolute);
            if (uri.IsAbsoluteUri) return relativeUrl;
            var absUri = new Uri(new Uri(baseUrl), uri);
            return absUri.ToString();
        }

    }
}