using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BizInfo.App.Services.Tools;
using BizInfo.Harvesting.Services.Processing.Helpers;
using BizInfo.Model.Interfaces;
using HtmlAgilityPack;

namespace BizInfo.Harvesting.Services.Processing
{
    public class BazosParser : CommonOfferParser
    {
        public override void Parse(HtmlDocument htmlDocument, DateTime loadTime, IBizInfoStorage storage, IWebSource webSource, out string summary, out string text, out DateTime offerTime, out int? nativeCategory, out IEnumerable<string> photoUrls, out IEnumerable<KeyValuePair<string, string>> structured, out DateTime? reloadDate)
        {
            var offerTable = EmbeddException(webSource, () => htmlDocument.DocumentNode.Descendants("table").Where(node => node.Attributes.Contains("class") && node.Attributes["class"].Value == "listainzerat").Single(), "Cannot find single main table containing an offer");
            var h1 = EmbeddException(webSource, () => offerTable.Descendants("h1").Single(), "Cannot find single H1 element containing offer title");
            summary = h1.InnerText;
            var nextTable = Enumerable.First<HtmlNode>(offerTable.GetNextSiblings().Where(sibling => sibling.Name.Equals("table", StringComparison.InvariantCultureIgnoreCase)));
            var textTd = EmbeddException(webSource, () => nextTable.Descendants("td").Where(node => node.Attributes.Contains("colspan") && node.Attributes["colspan"].Value == "3").First(), "Cannot find any TD element containing offer text");
            text = textTd.InnerText;
            var basicsSpan = EmbeddException(webSource, () => offerTable.Descendants("span").Where(node => node.Attributes.Contains("class") && node.Attributes["class"].Value == "velikost10").First(), "Cannot find any SPAN element containing offer date");
            offerTime = EmbeddException(webSource, () => ParseOfferTime(basicsSpan, webSource), "Cannot parse offer time");
            nativeCategory = EmbeddException(webSource, () => ParseNativeCategory(storage, webSource, htmlDocument), "Cannot parse native category");
            photoUrls = EmbeddException(webSource, () => ParsePhotoUrls(nextTable), "Cannot parse offer photo URLs");
            var txt = text;
            structured = EmbeddException(webSource, () => ParseStructured(htmlDocument).Concat(GetMails(txt)).Concat(GetPhones(txt)).Concat(GetLinks(txt)), "Cannot parse structured content"); ;
            reloadDate = null;
        }

        private int? ParseNativeCategory(IBizInfoStorage storage, IWebSource webSource, HtmlDocument htmlDocument)
        {
            var categories = htmlDocument.DocumentNode.Descendants("a").Where(node => node.Attributes.Contains("id") && node.Attributes["id"].Value == "zvyraznenikat").ToArray();
            int? category = storage.GetCategory(UrlTools.GetServerOnly(webSource.Url), null);
            category = storage.GetCategory(categories[0].InnerText, category);
            var section = categories[1].Ancestors("div").Where(node => node.Attributes.Contains("class") && node.Attributes["class"].Value == "barvalmenu").First();
            var sectionTitle = Enumerable.First<HtmlNode>(section.GetPreviousSiblings().Where(node => node.Attributes.Contains("class") && node.Attributes["class"].Value == "nadpismenu"));
            category = storage.GetCategory(sectionTitle.InnerText, category);
            var pieces = htmlDocument.DocumentNode.Descendants("div").Where(node => node.Attributes.Contains("class") && node.Attributes["class"].Value == "drobky");
            var piece = pieces.Single().Descendants("a").Skip(1).First();
            category = storage.GetCategory(piece.InnerText, category);
            category = storage.GetCategory(categories[1].InnerText, category);
            return category;
        }

        private IEnumerable<string> ParsePhotoUrls(HtmlNode offerTable)
        {
            var mainUrl = offerTable.Descendants("img").Where(node => node.Attributes.Contains("id") && node.Attributes["id"].Value == "bobrazek").Select(img => img.Attributes["src"].Value).FirstOrDefault();
            if (mainUrl != null) yield return mainUrl;
            foreach (var imgUrl in
                offerTable.Descendants("a").Where(
                    candidateAnchor =>
                    candidateAnchor.Attributes.Contains("onmouserover") && candidateAnchor.Attributes["onmouseover"].Value.StartsWith("return zobrazek", StringComparison.InvariantCultureIgnoreCase) && candidateAnchor.Attributes.Contains("href")).
                    Select(anchor => anchor.Attributes["href"].Value))
            {
                yield return imgUrl;
            }
        }

        private DateTime ParseOfferTime(HtmlNode basicsSpan, IWebSource webSource)
        {
            var text = basicsSpan.InnerText;
            var timeMatch = Regex.Match(text, @"\[\s*(\d|\d\d).\s*(\d|\d\d).\s*(\d\d\d\d)\]");
            var day = int.Parse(timeMatch.Groups[1].Value);
            var month = int.Parse(timeMatch.Groups[2].Value);
            var year = int.Parse(timeMatch.Groups[3].Value);
            var hour = webSource.Scouted.Hour;
            var minute = webSource.Scouted.Minute;
            var second = webSource.Scouted.Second;
            var millisecond = webSource.Scouted.Millisecond;
            return NormalizeOfferTime (new DateTime(year, month, day, hour, minute, second, millisecond), webSource);
        }

        private IEnumerable<KeyValuePair<string, string>> ParseStructured(HtmlDocument htmlDocument)
        {
            var hodnoceniAnchor = htmlDocument.DocumentNode.Descendants("a").Where(a => a.Attributes["href"].Value.StartsWith("http://www.bazos.cz/hodnoceni.php?mail=")).FirstOrDefault();
            if (hodnoceniAnchor != null)
            {
                var listal = hodnoceniAnchor.Ancestors("table").First();
                var tr1 = listal.Descendants("tr").First();
                var anchor = tr1.Descendants("a").First();
                yield return new KeyValuePair<string, string>("Inzerent", anchor.InnerText);
                yield return new KeyValuePair<string, string>("Odkaz", anchor.Attributes["href"].Value);
                foreach (var phoneFragment in GetPhones(tr1.InnerText)) yield return phoneFragment;
                foreach (var tr in tr1.GetNextSiblings("tr"))
                {
                    if (tr.InnerText.StartsWith("Lokalita"))
                    {
                        anchor = tr.Descendants("a").First();
                        yield return new KeyValuePair<string, string>("Lokalita", anchor.InnerText);
                        yield return new KeyValuePair<string, string>("Odkaz", anchor.Attributes["href"].Value);
                        continue;
                    }
                    if (tr.InnerText.StartsWith("Cena"))
                    {
                        var b = tr.Descendants("b").First();
                        yield return new KeyValuePair<string, string>("Cena", b.InnerText);
                        continue;
                    }
                }
            }
        }
    }
}