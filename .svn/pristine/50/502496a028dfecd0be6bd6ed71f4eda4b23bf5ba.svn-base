using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace BizInfo.Harvesting.Services.Scouting
{
    public class HyperInzerceScout : CommonScout
    {
        public HyperInzerceScout() : base(System.Text.Encoding.GetEncoding("Windows-1250"))
        {
            
        }

        protected override IEnumerable<string> ScoutUrls(HtmlDocument page, string pageUrl)
        {
            return page.DocumentNode.Descendants("a").Where(candidate => candidate.HasAttributes && candidate.Attributes.Contains("class") && candidate.Attributes["class"].Value == "bbns").Select(node => node.Attributes["href"].Value);
        }

        public override string GetNextPageUrl(HtmlDocument page, string pageUrl)
        {
            return page.DocumentNode.Descendants("a").Where (node=> node.InnerText.Equals("Další", StringComparison.InvariantCultureIgnoreCase)).Select(node=>node.Attributes["href"].Value).FirstOrDefault();
        }
    }
}