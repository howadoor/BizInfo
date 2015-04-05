using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BizInfo.App.Services.Tools;
using BizInfo.Harvesting.Services.Processing.Helpers;
using BizInfo.Model.Interfaces;
using HtmlAgilityPack;

namespace BizInfo.Harvesting.Services.Processing
{
    public class AvizoParser : CommonOfferParser
    {
        public override void Parse(HtmlDocument htmlDocument, DateTime loadTime, IBizInfoStorage storage, IWebSource webSource, out string summary, out string text, out DateTime offerTime, out int? nativeCategory, out IEnumerable<string> photoUrls, out IEnumerable<KeyValuePair<string, string>> structured, out DateTime? reloadDate)
        {
            htmlDocument.DocumentNode.RemoveComments();
            summary = EmbeddException(webSource, () => htmlDocument.DocumentNode.Descendants("meta").WhereAttribute("property", property => property.Equals("og:title")).Single().Attributes["content"].Value, "Cannot parse offer summary");
            text = EmbeddException(webSource, () => ParseOfferText(htmlDocument), "Cannot parse offer text");
            offerTime = EmbeddException(webSource, () => ParseOfferTime(htmlDocument, webSource, loadTime), "Cannot parse offer time");
            nativeCategory = EmbeddException(webSource, () => ParseNativeCategory(htmlDocument, storage, webSource), "Exception parsing native category"); ;
            photoUrls = EmbeddException(webSource, () => ParseOfferPhotoUrls(htmlDocument, webSource), "Exception parsing offer photo urls");
            
            var txt = text;
            structured = EmbeddException(webSource, () => ParseStructured(htmlDocument).Concat(GetMails(txt)).Concat(GetPhones(txt)).Concat(GetLinks(txt)), "Cannot parse structured content");
            reloadDate = null;
        }

        private string ParseOfferText(HtmlDocument htmlDocument)
        {
            var basicText = htmlDocument.DocumentNode.Descendants("div").WhereAttribute("class", @class => @class.Equals("addet-textm")).Single().InnerText.Trim();
            if (basicText.EndsWith("Pokraèování »"))
            {
                basicText = basicText.Substring(0, basicText.Length - "Pokraèování »".Length).Replace("...", string.Empty).Trim();
            }
            var textBuilder = new StringBuilder(basicText);
            var contact = ParseContact(htmlDocument);
            if (!string.IsNullOrEmpty(contact))
            {
                textBuilder.AppendFormat(@"r\n{0}", contact);
            }
            return textBuilder.ToString();
        }

        private string ParseContact(HtmlDocument htmlDocument)
        {
            var contactDiv = htmlDocument.DocumentNode.Descendants("div").WhereAttribute("class", @class => @class.Equals("contactContent")).FirstOrDefault();
            if (contactDiv == null) return null;
            var addressDiv = contactDiv.Descendants("address").FirstOrDefault();
            if (addressDiv == null) return null;
            return addressDiv.InnerText.Trim();
        }

        private IEnumerable<KeyValuePair<string, string>> ParseStructured(HtmlDocument htmlDocument)
        {
            var structuredTable = htmlDocument.DocumentNode.Descendants("table").WhereAttribute("class", @class => @class.Equals("tblDA")).FirstOrDefault();
            if (structuredTable != null)
            {
                foreach (var row in structuredTable.Descendants("tr"))
                {
                    var key = row.Descendants("th").Single().InnerText;
                    var value = row.Descendants("td").Single().InnerText;
                    yield return new KeyValuePair<string, string>(key, value);
                }
            }
            var priceSpan = htmlDocument.DocumentNode.Descendants("span").WhereAttribute("class", @class => @class.Equals("addet-prinum")).FirstOrDefault();
            if (priceSpan != null)
            {
                yield return new KeyValuePair<string, string>("Cena", priceSpan.InnerText);
            }
        }

        private int? ParseNativeCategory(HtmlDocument htmlDocument, IBizInfoStorage storage, IWebSource webSource)
        {
            var category = storage.GetCategory(UrlTools.GetServerOnly(webSource.Url), null);
            var offerType = htmlDocument.DocumentNode.Descendants("strong").WhereAttribute("class", @class => @class.Equals("addet-type")).Single().InnerText.Trim();
            category = storage.GetCategory(offerType, category);
            var navPath = htmlDocument.DocumentNode.Descendants("p").WhereAttribute("id", id => id.Equals("pathnavigator")).First();
            int categoryAnchorIndex = 0;
            foreach (var categoryAnchor in navPath.Descendants("a"))
            {
                if (categoryAnchorIndex > 0)
                {
                    category = storage.GetCategory(categoryAnchor.InnerText, category);
                }
                categoryAnchorIndex++;
            }
            return category;
        }

        private IEnumerable<string> ParseOfferPhotoUrls(HtmlDocument htmlDocument, IWebSource webSource)
        {
            var imageNode = htmlDocument.DocumentNode.Descendants("meta").WhereAttribute("property", property => property.Equals("og:image")).FirstOrDefault();
            if (imageNode != null) yield return imageNode.Attributes["content"].Value;
        }

        private DateTime ParseOfferTime(HtmlDocument htmlDocument, IWebSource webSource, DateTime loadTime)
        {
            var offerTimeText = htmlDocument.DocumentNode.Descendants("span").WhereAttribute("class", @class => @class.Equals("addet-date")).Single().InnerText.Trim();
            var match = Regex.Match(offerTimeText, @"(\d{1,2})\.(\d{1,2})\.(\d\d\d\d) (\d{1,2}):(\d{1,2})");
            int millisecond, second, hour, minute, day, month, year;
            millisecond = webSource.Scouted.Millisecond;
            second = webSource.Scouted.Second;
            day = int.Parse(match.Groups[1].Value);
            month = int.Parse(match.Groups[2].Value);
            year = int.Parse(match.Groups[3].Value);
            hour = int.Parse(match.Groups[4].Value);
            minute = int.Parse(match.Groups[5].Value);
            return NormalizeOfferTime(new DateTime(year, month, day, hour, minute, second, millisecond), webSource);
        }
    }
}