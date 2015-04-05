using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BizInfo.App.Services.Tools;
using BizInfo.Harvesting.Services.Processing.Helpers;
using BizInfo.Model.Interfaces;
using HtmlAgilityPack;
using Enumerable = System.Linq.Enumerable;

namespace BizInfo.Harvesting.Services.Processing
{
    public class BezRealitkyParser : CommonOfferParser
    {
        public override void Parse(HtmlDocument htmlDocument, DateTime loadTime, IBizInfoStorage storage, IWebSource webSource, out string summary, out string text, out DateTime offerTime, out int? nativeCategory, out IEnumerable<string> photoUrls, out IEnumerable<KeyValuePair<string, string>> structured, out DateTime? reloadDate)
        {
            summary = EmbeddException(webSource, () => ParseSummary(htmlDocument), "Cannot parse offer summary");
            text = EmbeddException(webSource, () => ParseOfferText(htmlDocument), "Cannot parse offer text");
            var contact = EmbeddException(webSource, () => ParseContact(htmlDocument), "Failed during contact parsing");
            if (!string.IsNullOrEmpty(contact))
            {
                text = string.Format("{0}\r\n{1}", text, contact);
            }
            // bezrealitky.cz gives no informations about offer creation
            offerTime = webSource.Scouted;
            nativeCategory = EmbeddException(webSource, () => ParseNativeCategory(htmlDocument, storage, webSource), "Exception parsing native category"); ;
            photoUrls = EmbeddException(webSource, () => ParseOfferPhotoUrls(htmlDocument, webSource), "Exception parsing offer photo urls");

            var txt = text;
            structured = EmbeddException(webSource, () => ParseStructured(htmlDocument).Concat(GetMails(txt)).Concat(GetPhones(txt)).Concat(GetLinks(txt)), "Cannot parse structured content");
            reloadDate = null;
        }

        private string ParseOfferText(HtmlDocument htmlDocument)
        {
            var descDiv = htmlDocument.DocumentNode.Descendants("div").WhereIdEquals("desctextcz").FirstOrDefault();
            if (descDiv != null)
            {
                return descDiv.InnerText.Trim();
            }
            var textBuilder = new StringBuilder();
            foreach (var div in htmlDocument.DocumentNode.Descendants("div").WhereClassEquals("odsad"))
            {
                foreach (var p in div.Descendants("p"))
                {
                    if (textBuilder.Length > 0) textBuilder.Append("\r\n");
                    textBuilder.Append(p.InnerText.Trim());
                }
            }
            return textBuilder.ToString();
        }

        private string ParseSummary(HtmlDocument htmlDocument)
        {
            var detailDiv = htmlDocument.DocumentNode.Descendants("div").WhereIdEquals("detail-linka").First();
            var h2 = detailDiv.Descendants("h2").First();
            return h2.InnerText;
        }

        private string ParseContact(HtmlDocument htmlDocument)
        {
            var contactDiv = htmlDocument.DocumentNode.Descendants("div").WhereClassEquals("majitel").FirstOrDefault();
            return contactDiv == null ? null : contactDiv.InnerText.Trim();
        }

        private IEnumerable<KeyValuePair<string, string>> ParseStructured(HtmlDocument htmlDocument)
        {
            var descriptionTable = htmlDocument.DocumentNode.Descendants("table").WhereClassEquals("description").First();
            foreach (var tr in descriptionTable.Descendants("tr"))
            {
                var tds = tr.Descendants("td").Take(2).ToArray();
                yield return new KeyValuePair<string, string>(tds [0].InnerText, tds[1].InnerText);
            }
            var priceH3 = htmlDocument.DocumentNode.Descendants("h3").FirstOrDefault(h3 => h3.InnerText.StartsWith("Cena:"));
            if (priceH3 != null)
            {
                yield return new KeyValuePair<string, string>("Cena", priceH3.Descendants("strong").First().InnerText.Trim());
            }
        }

        private int? ParseNativeCategory(HtmlDocument htmlDocument, IBizInfoStorage storage, IWebSource webSource)
        {
            var category = storage.GetCategory(UrlTools.GetServerOnly(webSource.Url), null);
            var descriptionTable = htmlDocument.DocumentNode.Descendants("table").WhereClassEquals("description").First();
            var offerTypeHeader = descriptionTable.Descendants("td").First(td => td.InnerText.Contains("Typ nabídky"));
            var offerType = offerTypeHeader.GetNextSiblings("td").First().InnerText;
            category = storage.GetCategory(offerType, category);
            var estateTypeHeader = descriptionTable.Descendants("td").First(td => td.InnerText.Contains("Typ nemovitosti"));
            var estateType = estateTypeHeader.GetNextSiblings("td").First().InnerText;
            category = storage.GetCategory(estateType, category);
            return category;
        }

        private IEnumerable<string> ParseOfferPhotoUrls(HtmlDocument htmlDocument, IWebSource webSource)
        {
            var galleryDiv = htmlDocument.DocumentNode.Descendants("div").WhereClassEquals("gallery-cont").First();
            if (galleryDiv != null)
            {
                foreach (var img in galleryDiv.Descendants("img"))
                {
                    yield return img.Attributes["src"].Value;
                }
            }
        }
    }
}