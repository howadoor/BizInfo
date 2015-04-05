using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using BizInfo.App.Services.Logging;
using BizInfo.App.Services.Tools;
using BizInfo.Harvesting.Interfaces.Processing;
using BizInfo.Model.Entities;
using BizInfo.Model.Interfaces;

namespace BizInfo.Harvesting.Services.Core
{
    /// <summary>
    /// Metody pro stahování dat z internetu. data jsou lokalizována pomocí URL.
    /// </summary>
    public class UrlDownloader : IUrlDownloader
    {
        private static readonly Dictionary<string, ServerTimes> serverTimesStore = new Dictionary<string, ServerTimes>();
        private static readonly List<string> proxies = ProxyTools.GetFreshProxies();
        private readonly UrlBlobsCache cache;
        private readonly Random random = new Random(DateTime.Now.Millisecond);

        public UrlDownloader(BizInfoModelContainer container)
        {
            cache = new UrlBlobsCache(container);
        }

        #region IUrlDownloader Members

        public void Load(IUrlDownloadRequest request)
        {
            Load(request, @"html");
            this.LogInfo(string.Format("Download {0}", request));
        }

        #endregion

        /// <summary>
        /// Stáhne stránku určenou argumentem <see cref="url"/>
        /// </summary>
        /// <param name="url">Adresa stránky ke stažení</param>
        public byte[] Load(string url, out long length, out DateTime loadTime)
        {
            // Open a connection
            byte[] pageContent = LoadPageContent(url, out length);
            loadTime = DateTime.Now;
            return pageContent;
        }

        private byte[] LoadPageContent(string url, out long length)
        {
            var serverTimes = GetServerTimes(url);
            //lock (serverTimes)
            {
                for (int i = 0; i < 10; i++)
                {
                    try
                    {
                        if (i > 0) Console.WriteLine("Retry {0} for {1}, minimum interval {2}", i, url, serverTimes.MinimumInterval.TotalMilliseconds);
                        var wait = (serverTimes.LastLoad + serverTimes.MinimumInterval) - DateTime.Now;
                        //if (wait.TotalMilliseconds > 1000) Console.WriteLine("Sleep {0}", wait);
                        //if (wait.TotalMilliseconds > 0) Thread.Sleep(wait);
                        serverTimes.LastLoad = DateTime.Now;
                        long _len = 0;
                        byte[] content = null;
                        content = DoLoadPageContent(url, out _len);
                        /*
                        var task = Task.Factory.StartNew(() => content = DoLoadPageContent(url, out _len)).ContinueWith((previous) => Console.WriteLine("?"), TaskContinuationOptions.OnlyOnFaulted);
                        if (!task.Wait(20000))
                        {
                            throw new InvalidOperationException("Timeout exceeded");
                        }*/
                        length = _len;
                        if (length == 0 || content == null || content.Length == 0) throw new InvalidOperationException("No data received");
                        serverTimes.MinimumInterval = TimeSpan.FromMilliseconds(serverTimes.MinimumInterval.TotalMilliseconds/2);
                        serverTimes.LastLoad = DateTime.Now;
                        Console.WriteLine("Loading succeeded ({0} bytes)", length);
                        return content;
                    }
                    catch (WebException webException)
                    {
                        if (webException.Response != null)
                        {
                            var statusCode = ((HttpWebResponse) webException.Response).StatusCode;
                            Console.WriteLine(statusCode);
                            if (statusCode == HttpStatusCode.NotFound) return GetDataFromResponse(webException.Response, out length);
                            // if (statusCode == HttpStatusCode.NotFound) break;
                        }
                        else
                        {
                            Console.WriteLine("No response, exception {0}", webException);
                        }
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine("Loading raised an exception {0}", exception.Message);
                    }
                    serverTimes.MinimumInterval = TimeSpan.FromMilliseconds(Math.Min(Math.Max(serverTimes.MinimumInterval.TotalMilliseconds, 50)*2, 30000));
                }
            }
            throw new InvalidOperationException("Cannot load page content. Maximum of retries exceeded.");
        }

        private ServerTimes GetServerTimes(string url)
        {
            lock (this)
            {
                ServerTimes serverTimes;
                var key = UrlTools.GetServerOnly(url);
                if (serverTimesStore.TryGetValue(key, out serverTimes)) return serverTimes;
                return serverTimesStore[key] = new ServerTimes();
            }
        }

        private byte[] DoLoadPageContent(string url, out long length)
        {
            var webRequestObject = (HttpWebRequest) WebRequest.Create(url);
            webRequestObject.Timeout = 10000;
            if (url.Contains("www.annonce.cz"))
            {
                var proxyAddress = string.Format("http://{0}", proxies[random.Next()%proxies.Count]);
                webRequestObject.Proxy = new WebProxy(proxyAddress);
                Console.WriteLine("Using proxy {0}", proxyAddress);
            }

            /*
            // You can also specify additional header values like 
            // the user agent or the referer:
            WebRequestObject.UserAgent = ".NET Framework/2.0";
            WebRequestObject.Referer = "http://www.example.com/";
            */

            using (var response = webRequestObject.GetResponse())
            {
                return GetDataFromResponse(response, out length);
            }
        }

        private byte[] GetDataFromResponse(WebResponse response, out long length)
        {
            // Request response:
            byte[] pageContent;
            // Open data stream:
            using (var webStream = response.GetResponseStream())
            {
                // Create reader object:
                using (var memStream = new MemoryStream())
                {
                    webStream.CopyTo(memStream);
                    // Read the entire stream content:
                    pageContent = memStream.GetBuffer();
                    length = memStream.Length;
                    // Cleanup
                    memStream.Close();
                }
                webStream.Close();
            }
            return pageContent;
        }

        public void Load(IUrlDownloadRequest request, string extension)
        {
            if (request.CanLoadFromCache && cache.TryGetUrl(request)) return;
            if (!request.CanLoadDirectly) return;
            long contentLength;
            DateTime loadTime;
            var content = Load(request.Url, out contentLength, out loadTime);
            request.ContentAcceptor.Accept(content, contentLength, loadTime);
            if (request.CanStoreToCache)
            {
                cache.AddUrl(request.Url, content, contentLength, loadTime, extension);
                request.IsContentStoredToCache = true;
            }
        }

        public bool IsInCache(string offerUrl)
        {
            return cache.IsInCache(offerUrl);
        }

        public bool AddAsScouted(string url)
        {
            return cache.AddAsScouted(url);
        }

        public void DeleteFromCache(string url)
        {
            cache.Remove(url);
        }

        #region Nested type: ServerTimes

        private class ServerTimes
        {
            public DateTime LastLoad = DateTime.Now;
            public TimeSpan MinimumInterval = new TimeSpan(0, 0, 1);
        }

        #endregion
    }
}