using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BizInfo.App.Services.Tools;
using BizInfo.Harvesting.Services.Processing.Fragments;
using BizInfo.Harvesting.Services.Processing.Helpers;
using BizInfo.Model.Interfaces;
using HtmlAgilityPack;

namespace BizInfo.Harvesting.Services.Processing
{
    public class SBazarParser : CommonOfferParser
    {
        public override void Parse(HtmlDocument htmlDocument, DateTime loadTime, IBizInfoStorage storage, IWebSource webSource, out string summary, out string text, out DateTime offerTime, out int? nativeCategory, out IEnumerable<string> photoUrls, out IEnumerable<KeyValuePair<string, string>> structured, out DateTime? reloadDate)
        {
            summary = EmbeddException(webSource, () => htmlDocument.DocumentNode.Descendants("head").Single().Descendants("title").Single().InnerText.Trim(), "Cannot parse offer summary");
            var infoDiv = EmbeddException(webSource, () => htmlDocument.DocumentNode.Descendants("div").WhereAttribute("id", id => id.Equals("info")).Single(), "Cannot find info DIV"); ;
            var infoDetails = EmbeddException(webSource, () => GetInfoDetailsNode(infoDiv), "Cannot find info details DIV"); ;
            var basicText = EmbeddException(webSource, () => infoDiv.Descendants("div").WhereAttribute("id", id => id.Equals("ad_description")).Single().InnerText.Trim(), "Cannot parse offer text");
            var textBuilder = new StringBuilder(basicText);
            foreach (var p7Div in infoDiv.Descendants("div").WhereAttribute("class", cls => cls.Equals("p7")))
            {
                textBuilder.AppendFormat("\r\n{0}", p7Div.InnerText);
            }
            text = textBuilder.ToString();
            offerTime = EmbeddException(webSource, () => ParseOfferTime(webSource, infoDetails, loadTime), "Cannot parse offer time");
            nativeCategory = EmbeddException(webSource, () => ParseNativeCategory(htmlDocument, storage, webSource, infoDetails), "Exception parsing native category"); ;
            photoUrls = EmbeddException(webSource, () => ParseOfferPhotoUrls(htmlDocument, webSource), "Exception parsing offer photo urls");
            var txt = text;
            structured = EmbeddException(webSource, () => ParseStructured(htmlDocument).Concat(GetMails(txt)).Concat(GetPhones(txt)).Concat(GetLinks(txt)), "Cannot parse structured content");
            reloadDate = null;
        }

        private IEnumerable<KeyValuePair<string, string>> ParseStructured(HtmlDocument htmlDocument)
        {
            var buyForm = htmlDocument.DocumentNode.Descendants("form").WhereAttribute("action", action => action.Contains("koupit-na-inzerat.html")).FirstOrDefault();
            if (buyForm != null)
            {
                var outerDiv = buyForm.Ancestors("div").First();
                var price = HtmlEntity.DeEntitize(outerDiv.Descendants("strong").First().InnerText);
                yield return new KeyValuePair<string, string>("Cena", price);
            }
        }

        private int? ParseNativeCategory(HtmlDocument htmlDocument, IBizInfoStorage storage, IWebSource webSource, HtmlNode infoDetails)
        {
            var navPath = htmlDocument.DocumentNode.Descendants("p").WhereAttribute("class", cls => cls.Equals("nav-path lone")).First();
            var category = storage.GetCategory(UrlTools.GetServerOnly(webSource.Url), null);
            var isNabidka = infoDetails.InnerText.Contains("Nabídka");
            category = storage.GetCategory(isNabidka ? "Nabídka" : "Poptávka", category);
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
            var photoP = htmlDocument.DocumentNode.Descendants("p").WhereAttribute("id", id => id.Equals("thumbs")).FirstOrDefault();
            if (photoP != null)
            {
                foreach (var photoAnchor in photoP.Descendants("a"))
                {
                    yield return photoAnchor.Attributes["href"].Value;
                }
            }
        }

        private HtmlNode GetInfoDetailsNode(HtmlNode infoDiv)
        {
            foreach (var px7Div in infoDiv.Descendants("div").WhereAttribute("class", cls => cls.Equals("px7")))
            {
                var innerText = px7Div.InnerText.Trim();
                var match = Regex.Match(innerText, @"Inzerce z (\d{1,2})\.(\d{1,2})\.(\d\d\d\d)");
                if (match.Success) return px7Div;
            }
            throw new InvalidOperationException("Cannot find info details node");
        }
        
        private DateTime ParseOfferTime(IWebSource webSource, HtmlNode infoDetails, DateTime loadTime)
        {
            var match = Regex.Match(infoDetails.InnerText, @"Inzerce z (\d{1,2})\.(\d{1,2})\.(\d\d\d\d)");
            int millisecond, second, hour, minute, day, month, year;
            millisecond = webSource.Scouted.Millisecond;
            second = webSource.Scouted.Second;
            minute = webSource.Scouted.Minute;
            hour = webSource.Scouted.Hour;
            day = int.Parse(match.Groups[1].Value);
            month = int.Parse(match.Groups[2].Value);
            year = int.Parse(match.Groups[3].Value);
            return NormalizeOfferTime(new DateTime(year, month, day, hour, minute, second, millisecond), webSource);
        }
    }
}