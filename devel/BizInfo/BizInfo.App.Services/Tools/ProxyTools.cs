using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace BizInfo.App.Services.Tools
{
    public static class ProxyTools
    {
        public static List<string> GetFreshProxies()
        {
            var list = new List<string>();
            for (int i = 0; i < 20; i++)
            {
                list.AddRange(GetFreshProxies(string.Format("http://www.cool-proxy.net/index.php?action=proxy-list&page={0}&sort=working_average&sort-type=desc", i)));
                list.AddRange(GetFreshProxies(string.Format("http://www.proxylist.net/list/0/8080/1/0/{0}", i)));
                list.AddRange(GetFreshProxies(string.Format("http://www.proxylist.net/list/0/3128/1/0/{0}", i)));
            }
            return list;
        }

        private static IEnumerable<string> GetFreshProxies(string url)
        {
            var webRequestObject = (HttpWebRequest)WebRequest.Create(url);

            // Request response:
            byte[] pageContent;
            int length = 0;
            using (var response = webRequestObject.GetResponse())
            {
                // Open data stream:
                using (var webStream = response.GetResponseStream())
                {
                    // Create reader object:
                    using (var memStream = new MemoryStream())
                    {
                        webStream.CopyTo(memStream);
                        // Read the entire stream content:
                        pageContent = memStream.GetBuffer();
                        length = (int) memStream.Length;
                        // Cleanup
                        memStream.Close();
                    }
                    webStream.Close();
                }
                response.Close();
            }
            var pageString = Encoding.UTF8.GetString(pageContent, 0, length);
            var matches = Regex.Matches(pageString, @"(\d{1,3})\.(\d{1,3})\.(\d{1,3})\.(\d{1,3}):(\d{1,4})");
            return from Match match in matches select match.Value;
        }
    }
}