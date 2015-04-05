using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using BizInfo.App.Services.Logging;
using BizInfo.App.Services.Tools;
using BizInfo.Harvesting.Services.Processing;
using HtmlAgilityPack;

namespace Crawler.Experiment
{
    public class Crawler
    {
        private Random random = new Random((int) DateTime.Now.Ticks);

        public int MaxUrlsForCrawling { get; set; }
        public int MaxCrawledUrls { get; set; }

        public List<string> UrlsForCrawling { get; private set; }
        public List<string> CrawledUrls { get; private set; }
        public HashSet<string> Harvest { get; private set; }
        
        public Crawler()
        {
            UrlsForCrawling = new List<string>();
            CrawledUrls = new List<string>();
            MaxUrlsForCrawling = 256*1024;
            MaxCrawledUrls = 256*1024;
            Harvest = new HashSet<string>();
        }

        public Crawler(params string [] url) : this()
        {
            AddUrlsForCrawling(url);
        }

        private void AddUrlsForCrawling(IEnumerable <string> urls)
        {
            lock (this)
            {
                foreach (var url in urls.Where(url => (UrlsForCrawling.Count < 1000 || !Harvest.Contains(UrlTools.GetServerOnly(url))) && IsUrlForCrawling(url) && !UrlsForCrawling.Contains(url) && !CrawledUrls.Contains(url)))
                {
                    if (UrlsForCrawling.Count >= MaxUrlsForCrawling) UrlsForCrawling.RemoveAt(random.Next(UrlsForCrawling.Count));
                    UrlsForCrawling.Add(url);
                }
            }
        }

        private bool IsUrlForCrawling(string url)
        {
            return new Uri(url).Host.EndsWith(".cz");
        }

        public void Crawl()
        {
            while (true)
            {
                var url = GetSomeUrlForCrawling();
                if (url == null) return;
                try
                {
                    var page = DownloadPage(url);
                    var urls = GetUrls(page, url);
                    AddUrlsForCrawling(urls, url);
                    Console.WriteLine("Loaded {0}", url);
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed {0}", url);
                }
            }
        }

        public void ParallelCrawl()
        {
            var tasks = new List<Task>();
            for (int i = 0; i < 256; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                                                    {
                                                        while(true)
                                                        {
                                                            Crawl();
                                                            Thread.Sleep(2000);
                                                        }
                                                    }

                              ));
            }
            Console.ReadLine();
        }

        private void AddUrlsForCrawling(IEnumerable<string> urls, string url)
        {
            var originalHost = UrlTools.GetServerOnly(url);
            AddUrlsForCrawling(urls.Where(candidateUrl => UrlTools.GetServerOnly(candidateUrl) != originalHost));
        }

        private IEnumerable<string> GetUrls(HtmlDocument page, string pageUrl)
        {
            return page.DocumentNode.Descendants("a").Where(anchor => anchor.Attributes.Contains("href")).Select(anchor => ProcessingBase.ToAbsoluteUrl(anchor.Attributes["href"].Value.Trim(), pageUrl)).ToArray();
        }

        private HtmlDocument DownloadPage(string url)
        {
            var webRequestObject = (HttpWebRequest)WebRequest.Create(url);
            
            string address = null;
            webRequestObject.ServicePoint.BindIPEndPointDelegate = (servicepoint, remoteendpoint, retrycount) =>
                                                                       {
                                                                           address = remoteendpoint.Address.ToString();
                                                                           return new IPEndPoint(IPAddress.Any, 0);
                                                                       };
            try
            {
                // Request response:
                using (var response = webRequestObject.GetResponse())
                {
                    // Open data stream:
                    using (var webStream = response.GetResponseStream())
                    {
                        // Create reader object:
                        using (var memStream = new MemoryStream())
                        {
                            webStream.CopyTo(memStream);
                            memStream.Seek(0, SeekOrigin.Begin);
                            var htmlDocument = new HtmlDocument();
                            htmlDocument.Load(memStream);
                            if (htmlDocument != null && address != null)
                            {
                                AddToHarvest(url, address);
                            }
                            return htmlDocument;
                            // Cleanup
                        }
                    }
                }

            }
            finally
            {
                webRequestObject.ServicePoint.BindIPEndPointDelegate = null;
            }
        }

        private string GetSomeUrlForCrawling()
        {
            lock (this)
            {
                if (UrlsForCrawling.Count == 0) return null;
                var index = random.Next(UrlsForCrawling.Count);
                var url = UrlsForCrawling[index];
                UrlsForCrawling.RemoveAt(index);
                if (CrawledUrls.Count >= MaxCrawledUrls) CrawledUrls.RemoveAt(random.Next(CrawledUrls.Count));
                CrawledUrls.Add(url);
                return url;
            }
        }

        private void AddToHarvest(string url, string hostAddress)
        {
            lock (this)
            {
                var host = UrlTools.GetServerOnly(url);
                if (host.EndsWith(".cz") && !Harvest.Contains(host))
                {
                    Harvest.Add(host);
                    Console.WriteLine("Harvested #{0} {1} {2} {3}", Harvest.Count, host, hostAddress, url);
                    this.Log(string.Format("Harvested\t{0}\t{1}\t{2}\t{3}", Harvest.Count, host, hostAddress, url));
                }
            }
        }
    }
}