using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BizInfo.App.Services.Tools;
using BizInfo.Harvesting.Services.Processing.Fragments;
using BizInfo.Harvesting.Services.Processing.Helpers;
using BizInfo.Model.Interfaces;
using HtmlAgilityPack;

namespace BizInfo.Harvesting.Services.Processing
{
    public class HyperInzerceParser : CommonOfferParser
    {
        public override void Parse(HtmlDocument htmlDocument, DateTime loadTime, IBizInfoStorage storage, IWebSource webSource, out string summary, out string text, out DateTime offerTime, out int? nativeCategory, out IEnumerable<string> photoUrls, out IEnumerable<KeyValuePair<string, string>> structured, out DateTime? reloadDate)
        {
            var h1 = EmbeddException(webSource, () => htmlDocument.DocumentNode.Descendants("h1").Single(), "Cannot find single H1 element necessary to obtain offer summary.");
            summary = h1.InnerText;
            var firstAfterSummary = EmbeddException<HtmlNode>(webSource, () => Enumerable.First<HtmlNode>(h1.GetNextSiblings().Where(sibling => sibling.Name == "div")), "Cannot find first DIV element after offer summary (H1). No offer basics found.");
            DateTime _offerTime = new DateTime();
            int? _nativeCategory = null;
            EmbeddException(webSource, () => ParseOfferBasics(storage, webSource, loadTime, firstAfterSummary, out _offerTime, out _nativeCategory), "Error when parsing offer basics");
            offerTime = _offerTime;
            nativeCategory = _nativeCategory;
            var textDiv = EmbeddException(webSource, () => htmlDocument.DocumentNode.Descendants("div").Single(node => node.Attributes.Contains("class") && node.Attributes["class"].Value == "lh18 ww"), "Cannot find single DIV element necessary to obtain offer text.");
            text = textDiv.InnerText;
            photoUrls = EmbeddException(webSource, () => ParsePhotoUrls(htmlDocument), "Cannot parse photo URLs");
            var txt = text;
            structured = EmbeddException(webSource, () => ParseStructured(htmlDocument).Concat(GetMails(txt)).Concat(GetPhones(txt)).Concat(GetLinks(txt)), "Cannot parse structured content");
            reloadDate = null;
        }

        private IEnumerable<string> ParsePhotoUrls(HtmlDocument htmlDocument)
        {
            for (int index = 1; ; index++)
            {
                var photoId = string.Format("inzfoto{0}", index);
                var element = htmlDocument.GetElementbyId(photoId);
                if (element == null) break;
                if (!element.Attributes.Contains("href")) continue;
                var url = element.Attributes["href"].Value;
                yield return url;
            }
        }

        private void ParseOfferBasics(IBizInfoStorage storage, IWebSource webSource, DateTime loadTime, HtmlNode firstAfterSummary, out DateTime offerTime, out int? category)
        {
            var innerText = firstAfterSummary.InnerText.Trim();
            var basics = GetOfferBasics(innerText);
            int basicIndex = 0;
            offerTime = default(DateTime);
            category = storage.GetCategory(UrlTools.GetServerOnly(webSource.Url), null);
            foreach (var basic in basics)
            {
                if (basicIndex == 0)
                {
                    offerTime = ParseOfferTime(loadTime, basics.First(), webSource);
                }
                else
                {
                    category = storage.GetCategory(basic, category);
                    if (basicIndex == 1)
                    {
                        category = storage.GetCategory(UrlTools.GetSubdomains(webSource.Url), category);
                    }
                }
                basicIndex++;
            }
        }

        private DateTime ParseOfferTime(DateTime loadTime, string offerTimeString, IWebSource webSource)
        {
            var timeMatch = Regex.Match(offerTimeString, @" (\d\d):(\d\d)");
            int millisecond, second, hour, minute, day, month, year;
            millisecond = webSource.Scouted.Millisecond;
            second = webSource.Scouted.Second;
            if (timeMatch.Length > 0)
            {
                hour = int.Parse(timeMatch.Groups[1].Value);
                minute = int.Parse(timeMatch.Groups[2].Value);
            }
            else
            {
                hour = webSource.Scouted.Hour;
                minute = webSource.Scouted.Second;
            }
            var yearMatch = Regex.Match(offerTimeString, @".(\d\d\d\d)");
            if (yearMatch.Length > 0)
            {
                year = int.Parse(yearMatch.Groups[1].Value);
            }
            else
            {
                year = loadTime.Year;
            }
            var dateMatch = Regex.Match(offerTimeString, @"^(\d\d).(\d\d)");
            if (dateMatch.Length > 0)
            {
                day = int.Parse(dateMatch.Groups[1].Value);
                month = int.Parse(dateMatch.Groups[2].Value);
            }
            else
            {
                if (offerTimeString.StartsWith("dnes"))
                {
                    day = loadTime.Day;
                    month = loadTime.Month;
                    year = loadTime.Year;
                }
                else
                {
                    if (offerTimeString.StartsWith("včera"))
                    {
                        var yesterday = loadTime - new TimeSpan(1, 0, 0, 0);
                        day = yesterday.Day;
                        month = yesterday.Month;
                        year = yesterday.Year;
                    }
                    else
                    {
                        day = loadTime.Day;
                        month = loadTime.Month;
                    }
                }
            }
            return NormalizeOfferTime(new DateTime(year, month, day, hour, minute, second, millisecond), webSource);
        }

        private IEnumerable<string> GetOfferBasics(string basicsText)
        {
            return basicsText.Split('|').Select(basic => basic.Trim());
        }

        private IEnumerable<KeyValuePair<string, string>> ParseStructured(HtmlDocument htmlDocument)
        {
            foreach (var keySpan in htmlDocument.DocumentNode.Descendants("span").Where(span => span.Attributes.Contains("class") && span.Attributes["class"].Value.Contains("blk c-lblue")))
            {
                var valueSpan = keySpan.GetNextSiblings("span").First();
                yield return new KeyValuePair<string, string>(keySpan.InnerText, HtmlEntity.DeEntitize(valueSpan.InnerText));
            }
            foreach (var keySpan in htmlDocument.DocumentNode.Descendants("div").Where(span => span.Attributes.Contains("class") && span.Attributes["class"].Value.Contains("c-lblue fl")))
            {
                var valueSpan = keySpan.GetNextSiblings("div").First();
                yield return new KeyValuePair<string, string>(keySpan.InnerText, HtmlEntity.DeEntitize(valueSpan.InnerText));
            }
        }
    }
}