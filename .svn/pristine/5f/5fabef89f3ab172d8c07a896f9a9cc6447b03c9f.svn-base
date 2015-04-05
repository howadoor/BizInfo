using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using HtmlAgilityPack;

namespace BizInfo.Harvesting.Services.Scouting
{
    /// <summary>
    /// Expected initial URL like http://www.annonce.cz/byty-na-prodej$334-filter.html?nabidkovy=2&location_country=&listStyle=table&perPage=50
    /// </summary>
    public class AnnonceScout : CommonScout
    {
        public AnnonceScout() : base(Encoding.UTF8)
        {
        }

        protected override IEnumerable<string> ScoutUrls(HtmlDocument page, string pageUrl)
        {
            var inputs = page.DocumentNode.Descendants("input").Where(input => input.Attributes.Contains("title") && input.Attributes["title"].Value.Equals("Oznaèit inzerát", StringComparison.InvariantCultureIgnoreCase));
            var tableRowsWithInputs = inputs.Select(input => input.Ancestors("tr").First());
            var tableCellsWithTitle = tableRowsWithInputs.Select(input => input.Descendants("td").First(td => td.Attributes.Contains("class") && td.Attributes["class"].Value.Equals("title", StringComparison.InvariantCultureIgnoreCase)));
            return tableCellsWithTitle.Select(td => ToAbsoluteUrl(td.Descendants("a").First().Attributes["href"].Value, pageUrl));
        }

        public override string GetNextPageUrl(HtmlDocument page, string pageUrl)
        {
            var pages = page.DocumentNode.Descendants("p").FirstOrDefault(input => input.Attributes.Contains("class") && input.Attributes["class"].Value.Equals("pages", StringComparison.InvariantCultureIgnoreCase));
            if (pages == null) return null;
            var nextPageAnchor = pages.Descendants("a").Where(anchor =>
                                                                  {
                                                                      var innerTextDeentitized = HtmlEntity.DeEntitize(anchor.InnerText);
                                                                      var trimmed = innerTextDeentitized.Trim('\r', '\n', '\t', ' ');
                                                                      return trimmed.Equals(">");
                                                                  }).FirstOrDefault();
            if (nextPageAnchor == null) return null;
            return ToAbsoluteUrl(HtmlEntity.DeEntitize(nextPageAnchor.Attributes["href"].Value), pageUrl);
        }
    }
}