using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using BizInfo.Harvesting.Interfaces.Processing;
using BizInfo.Model.Interfaces;
using HtmlAgilityPack;

namespace BizInfo.Harvesting.Services.Scouting
{
    public class BazosScout : CommonScout
    {
        public BazosScout() : base(System.Text.Encoding.UTF8)
        {
            
        }
        
        protected override IEnumerable<string> ScoutUrls(HtmlDocument page, string pageUrl)
        {
            foreach (var offersTable in page.DocumentNode.Descendants("table").Where(node => node.Attributes.Contains("class") && node.Attributes["class"].Value == "inzeraty"))
            {
                var title = offersTable.Descendants("span").Where(node => node.Attributes.Contains("class") && node.Attributes["class"].Value == "nadpis").Single();
                {
                    var anchor = title.Descendants("a").First();
                    var offerUrl = ToAbsoluteUrl(anchor.Attributes["href"].Value, pageUrl);
                    yield return offerUrl;
                }
            }
        }

        public override string GetNextPageUrl(HtmlDocument page, string pageUrl)
        {
            var pagination = page.DocumentNode.Descendants("p").Where(node => node.Attributes.Contains("class") && node.Attributes["class"].Value == "strankovani").FirstOrDefault();
            if (pagination == null) return null;
            var nextAnchor = pagination.Descendants("a").Where(node => node.InnerText.Contains("Další")).FirstOrDefault();
            if (nextAnchor == null) return null;
            var nextUrl = nextAnchor.Attributes["href"].Value;
            return ToAbsoluteUrl(nextUrl, pageUrl);
        }
    }
}