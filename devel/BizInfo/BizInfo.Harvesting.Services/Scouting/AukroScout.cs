using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BizInfo.Harvesting.Services.Processing.Helpers;
using HtmlAgilityPack;

namespace BizInfo.Harvesting.Services.Scouting
{
    /// <summary>
    /// Expected initial URL like http://auto.aukro.cz/listing.php/showcat?id=8503&order=td&p=1
    /// </summary>
    public class AukroScout : CommonScout
    {
        public AukroScout() : base(Encoding.UTF8)
        {
        }

        protected override IEnumerable<string> ScoutUrls(HtmlDocument page, string pageUrl)
        {
            return page.DocumentNode.Descendants("tr").WhereClassContains("itemListResult").Select(row => row.Descendants("a").WhereClassContains("alleLink").First()).Select(a => ToAbsoluteUrl(a.Attributes["href"].Value, pageUrl));
        }

        public override string GetNextPageUrl(HtmlDocument page, string pageUrl)
        {
            var linkToNext = page.DocumentNode.Descendants("a").WhereClassEquals("arrRight-bottom").WhereAttributeEquals("title", "Další").FirstOrDefault();
            return linkToNext == null ? null : ToAbsoluteUrl(linkToNext.Attributes["href"].Value, pageUrl);
        }
    }
}