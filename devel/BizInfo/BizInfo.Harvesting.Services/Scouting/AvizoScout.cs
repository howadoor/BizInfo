using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BizInfo.Harvesting.Services.Processing.Helpers;
using HtmlAgilityPack;
using Enumerable = System.Linq.Enumerable;

namespace BizInfo.Harvesting.Services.Scouting
{
    public class AvizoScout : CommonScout
    {
        public AvizoScout()
            : base(System.Text.Encoding.UTF8)
        {

        }

        protected override IEnumerable<string> ScoutUrls(HtmlDocument page, string pageUrl)
        {
            foreach (var div in page.DocumentNode.Descendants("div").WhereAttribute("class", @class => @class.Equals("AOL")))
            {
                var h2 = div.Descendants("h2").SingleOrDefault();
                if (h2 == null) continue;
                var a = h2.Descendants("a").First();
                yield return ToAbsoluteUrl(a.Attributes["href"].Value, pageUrl);
            }
        }

        public override string GetNextPageUrl(HtmlDocument page, string pageUrl)
        {
            var nextPageAnchor = page.DocumentNode.Descendants("a").WhereAttribute("class", @class => @class.Equals("npagin")).Where(node => node.InnerText.Equals("Další")).FirstOrDefault();
            return nextPageAnchor != null ? nextPageAnchor.Attributes["href"].Value : null;
        }
    }
}