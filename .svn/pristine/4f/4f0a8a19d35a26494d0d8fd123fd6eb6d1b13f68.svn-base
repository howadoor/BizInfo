using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using BizInfo.Harvesting.Services.Processing.Helpers;
using HtmlAgilityPack;

namespace BizInfo.Harvesting.Services.Scouting
{
    public class InzertExpresScout : CommonScout
    {
        public InzertExpresScout() : base (Encoding.UTF8) 
        {
            
        }

        protected override IEnumerable<string> ScoutUrls(HtmlDocument page, string pageUrl)
        {
            return page.DocumentNode.Descendants("h2").Select(h2 => AddRef(ToAbsoluteUrl(h2.Descendants("a").FirstOrDefault().Attributes["href"].Value, pageUrl), pageUrl));
        }

        public override string GetNextPageUrl(HtmlDocument page, string pageUrl)
        {
            var arrowsDiv = page.DocumentNode.Descendants("div").WhereAttribute("class", @class => @class == "sipky-color1").FirstOrDefault();
            if (arrowsDiv == null) return null;
            var nextAnchor = arrowsDiv.Descendants("a").Where(anchor => anchor.InnerText == ">").FirstOrDefault();
            if (nextAnchor == null) return null;
            return ToAbsoluteUrl(nextAnchor.Attributes["href"].Value, pageUrl);
        }

        private static string AddRef(string url, string refUrl)
        {
            var uri = new Uri(refUrl);
            var localPath = uri.LocalPath;
            var addedRef = string.Format("{0}{2}ref={1}", url, HttpUtility.UrlEncode(localPath), url.Contains('?') ? "&" : "?");
            return addedRef;
        }
    }
}