using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BizInfo.Harvesting.Services.Processing.Helpers;
using HtmlAgilityPack;

namespace BizInfo.Harvesting.Services.Scouting
{
    public class SBazarScout : CommonScout
    {
        public SBazarScout()
            : base(System.Text.Encoding.GetEncoding("iso-8859-2"))
        {

        }

        protected override IEnumerable<string> ScoutUrls(HtmlDocument page, string pageUrl)
        {
            var table = page.DocumentNode.Descendants("table").WhereAttribute("id", attr => attr.Equals("jsHoverRow")).Single();
            foreach (var tableRow in table.Descendants("tr"))
            {
                var h2 = tableRow.Descendants("h2").SingleOrDefault();
                if (h2 == null) continue;
                var a = h2.Descendants("a").First();
                yield return a.Attributes["href"].Value;
            }
        }

        private static Regex nextPageUrlRegex = new Regex("<a href=\"([^\"]+)\"");

        public override string GetNextPageUrl(HtmlDocument page, string pageUrl)
        {
            var nextPageScript = page.DocumentNode.Descendants("script").Where(script => script.InnerText.Contains("Další stránka")).FirstOrDefault();
            if (nextPageScript == null) return null;
            var match = nextPageUrlRegex.Matches(nextPageScript.InnerText);
            return match[0].Groups[1].Value;
        }
    }
}