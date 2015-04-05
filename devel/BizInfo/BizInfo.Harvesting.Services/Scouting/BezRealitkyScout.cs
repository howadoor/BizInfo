using System.Collections.Generic;
using System.Linq;
using BizInfo.Harvesting.Services.Processing.Helpers;
using HtmlAgilityPack;

namespace BizInfo.Harvesting.Services.Scouting
{
    public class BezRealitkyScout : CommonScout
    {
        public BezRealitkyScout()
            : base(System.Text.Encoding.UTF8)
        {

        }

        protected override IEnumerable<string> ScoutUrls(HtmlDocument page, string pageUrl)
        {
            foreach (var div in page.DocumentNode.Descendants("table").WhereAttribute("class", @class => @class.Equals("detail")))
            {
                var h3 = div.Descendants("h3").SingleOrDefault();
                if (h3 == null) continue;
                var a = h3.Ancestors("a").First();
                yield return ToAbsoluteUrl(a.Attributes["href"].Value, pageUrl);
            }
        }

        public override string GetNextPageUrl(HtmlDocument page, string pageUrl)
        {
            var paginationDiv = page.DocumentNode.Descendants("div").WhereAttribute("id", id => id.Equals("paginationControl")).First();
            var activePageSpan = paginationDiv.Descendants("span").WhereAttribute("class", @class => @class.Equals("active")).First();
            var nextPageAnchor = activePageSpan.GetNextSiblings("a").FirstOrDefault();
            return nextPageAnchor != null ? ToAbsoluteUrl(nextPageAnchor.Attributes["href"].Value, pageUrl) : null;
        }
    }
}