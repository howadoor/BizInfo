using System;
using System.Web;

namespace BizInfo.App.Services.Tools
{
    public static class UrlTools
    {
        public static string GetServerOnly(string url)
        {
            var uri = new Uri(url);
            return GetServerOnly(uri);
        }

        public static string GetServerOnly(Uri uri)
        {
            var host = uri.Host;
            var lastPoint = host.LastIndexOf('.');
            var prevPoint = host.LastIndexOf('.', lastPoint - 1, lastPoint);
            if (prevPoint < 0) return host;
            return host.Substring(prevPoint + 1);
        }

        public static string GetSubdomains(string url)
        {
            var uri = new Uri(url);
            return GetSubdomains(uri);
        }

        private static string GetSubdomains(Uri uri)
        {
            var host = uri.Host;
            var lastPoint = host.LastIndexOf('.');
            var prevPoint = host.LastIndexOf('.', lastPoint - 1, lastPoint);
            if (prevPoint <= 0) return null;
            return host.Substring(0, prevPoint);
        }

        public static string GetHost(string url)
        {
            return new Uri(url).Host;
        }
    }
}