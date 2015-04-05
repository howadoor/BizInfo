using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using BizInfo.App.Services.Tools;
using BizInfo.Harvesting.Services.Processing.Helpers;
using BizInfo.Model.Interfaces;
using HtmlAgilityPack;

namespace BizInfo.Harvesting.Services.Processing
{
    public class InzertExpresParser : CommonOfferParser
    {
        public override void Parse(HtmlDocument htmlDocument, DateTime loadTime, IBizInfoStorage storage, IWebSource webSource, out string summary, out string text, out DateTime offerTime, out int? nativeCategory, out IEnumerable<string> photoUrls,
                                   out IEnumerable<KeyValuePair<string, string>> structured, out DateTime? reloadDate)
        {
            var offerDiv = EmbeddException(webSource, () => htmlDocument.DocumentNode.Descendants("div").WhereAttribute("class", @class => @class == "polozka-vypisu").First(), "Cannot find DIV with offer data");
            var summaryh2 = EmbeddException(webSource, () => offerDiv.Descendants("h2").First(), "Cannot find offer summary H2 element");
            summary = summaryh2.InnerText;
            text = EmbeddException(webSource, () => ParseOfferText(summaryh2), "Cannot parse offer text");
            if (string.IsNullOrEmpty(summary)) summary = text.Substring(0, 48);
            var offerIdDiv = EmbeddException(webSource, () => offerDiv.Descendants("div").WhereAttribute("class", @class => @class == "id-inzeratu").First(), "Cannot find DIV with offer id");
            offerTime = EmbeddException(webSource, () => ParseOfferTime(offerDiv, webSource, loadTime), "Cannot parse offer time");
            photoUrls = ParsePhotoUrls(summaryh2.Ancestors("table").First(), webSource);
            nativeCategory = EmbeddException(webSource, () => ParseNativeCategory(htmlDocument, offerDiv, storage, webSource), "Cannot parse native category");
            structured = ParseStructured(htmlDocument, offerDiv, storage, webSource).Concat(GetMails(text)).Concat(GetPhones(text)).Concat(GetLinks(text));
            reloadDate = null;
        }

        private string ParseOfferText(HtmlNode h2)
        {
            var stringBuilder = new StringBuilder();
            for (var sibling = h2.NextSibling; sibling != null; sibling = sibling.NextSibling)
            {
                stringBuilder.Append(sibling.InnerText);
            }
            return stringBuilder.ToString();
        }

        private DateTime ParseOfferTime(HtmlNode offerDiv, IWebSource webSource, DateTime loadTime)
        {
            var millisecond = webSource.Scouted.Millisecond;
            var second = webSource.Scouted.Second;
            var minute = webSource.Scouted.Minute;
            var hour = webSource.Scouted.Hour;
            var text = offerDiv.InnerText;
            var match = Regex.Match(text, @"^b|\s*vloženo:\s*(\d{1,2})\.(\d{1,2})\.(\d\d\d\d)");
            var day = int.Parse(match.Groups[1].Value);
            var month = int.Parse(match.Groups[2].Value);
            var year = int.Parse(match.Groups[3].Value);
            return NormalizeOfferTime(new DateTime(year, month, day, hour, minute, second, millisecond), webSource);
        }

        private IEnumerable<string> ParsePhotoUrls(HtmlNode inTable, IWebSource webSource)
        {
            return inTable.Descendants("img").Select(img => ProcessingBase.ToAbsoluteUrl(img.Attributes["src"].Value, webSource.Url));
        }
        
        private int? ParseNativeCategory(HtmlDocument htmlDocument, HtmlNode offerDiv, IBizInfoStorage storage, IWebSource webSource)
        {
            var category = storage.GetCategory(UrlTools.GetServerOnly(webSource.Url), null);
            var subCategory = offerDiv.Descendants("h3").First().InnerText.Trim(' ', '\r', '\n', '\t', ':');
            category = storage.GetCategory(subCategory, category);
            var @ref = HttpUtility.ParseQueryString(webSource.Url)["ref"];
            string listingRef = HttpUtility.HtmlDecode(@ref);
            if (string.IsNullOrEmpty(listingRef)) listingRef = @"reality/vymeny";
            foreach (var pathComponent in listingRef.Split('/'))
            {
                category = storage.GetCategory(pathComponent, category);
            }
            return category;
        }
        
        private IEnumerable<KeyValuePair<string, string>> ParseStructured(HtmlDocument htmlDocument, HtmlNode offerDiv, IBizInfoStorage storage, IWebSource webSource)
        {
            var priceDiv = offerDiv.Descendants("div").Where(div => div.InnerText.StartsWith("Cena:")).FirstOrDefault();
            if (priceDiv != null)
            {
                yield return new KeyValuePair<string, string>("Cena", priceDiv.InnerText.Substring(6));
            }
            foreach (var tr in offerDiv.Descendants("tr"))
            {
                foreach (var somethingEndingBySemicolon in tr.Descendants().Where(desc => desc.InnerText.EndsWith(":")))
                {
                    var key = somethingEndingBySemicolon.InnerText;
                    if (key.Contains("SMS služby")) continue;
                    if (somethingEndingBySemicolon.NextSibling == null) continue;
                    var value = ReadUpToPipe(somethingEndingBySemicolon);
                    if (key == "tel.:")
                    {
                        foreach (var phone in GetPhones(value))
                        {
                            yield return phone;
                        }
                        continue;
                    }
                    yield return new KeyValuePair<string, string>(key, value);
                } 
            }
        }

        private string ReadUpToPipe(HtmlNode somethingEndingBySemicolon)
        {
            var stringBuilder = new StringBuilder();
            foreach (var sibling in somethingEndingBySemicolon.GetNextSiblings())
            {
                stringBuilder.Append(sibling.InnerText);
            }
            var text = stringBuilder.ToString();
            var pos = text.IndexOf('|');
            if (pos < 0) return text;
            return text.Substring(0, pos);
        }
    }
}