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
    public class AnnonceParser : CommonOfferParser
    {
        public override void Parse(HtmlDocument htmlDocument, DateTime loadTime, IBizInfoStorage storage, IWebSource webSource, out string summary, out string text, out DateTime offerTime, out int? nativeCategory, out IEnumerable<string> photoUrls,
                                   out IEnumerable<KeyValuePair<string, string>> structured, out DateTime? reloadDate)
        {
            htmlDocument.DocumentNode.RemoveComments();
            summary = EmbeddException(webSource, () => htmlDocument.DocumentNode.Descendants("h1").First().InnerText.Trim(), "Cannot parse offer summary");
            text = EmbeddException(webSource, () => htmlDocument.DocumentNode.Descendants("div").First(div => div.Attributes.Contains("class") && div.Attributes["class"].Value.Contains("margin-bottom-detail")).InnerText.Trim(), "Cannot parse offer text");
            offerTime = EmbeddException(webSource, () => ParseOfferTime(htmlDocument, webSource, loadTime), "Cannot parse offer time");
            nativeCategory = EmbeddException(webSource, () => ParseNativeCategory(htmlDocument, storage, webSource), "Exception parsing native category");
            photoUrls = EmbeddException(webSource, () => ParseOfferPhotoUrls(htmlDocument, webSource), "Exception parsing offer photo urls");
            var txt = text;
            structured = EmbeddException(webSource, () => ParseStructured(htmlDocument).Concat(GetMails(txt)).Concat(GetPhones(txt)).Concat(GetLinks(txt)), "Cannot parse structured content");
            reloadDate = GetReloadDate(htmlDocument, loadTime);
        }

        private DateTime? GetReloadDate(HtmlDocument htmlDocument, DateTime loadTime)
        {
            var contactDetails = htmlDocument.DocumentNode.Descendants("div").FirstOrDefault(div => div.Attributes.Contains("class") && div.Attributes["class"].Value.Equals("ad-detail-contact"));
            if (contactDetails != null)
            {
                if (IsContactHidden(contactDetails))
                {
                    return GetReloadDateForHiddenContact(contactDetails, loadTime);
                }
            }
            return null;
        }

        private int? ParseNativeCategory(HtmlDocument htmlDocument, IBizInfoStorage storage, IWebSource webSource)
        {
            var category = storage.GetCategory(UrlTools.GetServerOnly(webSource.Url), null);
            var commodityElements = htmlDocument.DocumentNode.Descendants("table").First(div => div.Attributes.Contains("class") && div.Attributes["class"].Value.Equals("comodity-elements"));
            var type = commodityElements.Descendants("th").First(th => th.InnerText.Contains("Typ:"));
            var typeRow = type.Ancestors("tr").First();
            var offerType = typeRow.Descendants("td").First().InnerText.Trim();
            category = storage.GetCategory(offerType, category);
            var breadCrumbsDiv = htmlDocument.DocumentNode.Descendants("div").First(div => div.Attributes.Contains("class") && div.Attributes["class"].Value.Contains("breadcrumbs"));
            var breadCrumbs = HtmlEntity.DeEntitize(breadCrumbsDiv.InnerText).Trim().Split('>').Select(bc => bc.Trim()).ToArray();
            for (var i = 1; i < breadCrumbs.Length; i++)
            {
                category = storage.GetCategory(breadCrumbs[i], category);
            }
            return category;
        }

        private IEnumerable<string> ParseOfferPhotoUrls(HtmlDocument htmlDocument, IWebSource webSource)
        {
            var photoDiv = htmlDocument.DocumentNode.Descendants("div").FirstOrDefault(div => div.Attributes.Contains("id") && div.Attributes["id"].Value.Equals("ad-detail-photos"));
            if (photoDiv != null)
            {
                var thumbnails = photoDiv.Descendants("div").FirstOrDefault(div => div.Attributes.Contains("class") && div.Attributes["class"].Value.Equals("thumbnails"));
                if (thumbnails != null)
                {
                    return thumbnails.Descendants("a").Select(anchor => ProcessingBase.ToAbsoluteUrl(anchor.Attributes["href"].Value, webSource.Url));
                }
            }
            return new string[] {};
        }

        private DateTime ParseOfferTime(HtmlDocument htmlDocument, IWebSource webSource, DateTime loadTime)
        {
            var dateString = htmlDocument.DocumentNode.Descendants("div").First(div => div.Attributes.Contains("class") && div.Attributes["class"].Value.Contains("publication-form")).InnerText.Trim();
            return ParseOfferTime(dateString, webSource, loadTime);
        }

        private DateTime ParseOfferTime(string offerTimeString, IWebSource webSource, DateTime loadTime)
        {
            var millisecond = webSource.Scouted.Millisecond;
            var second = webSource.Scouted.Second;
            var minute = webSource.Scouted.Minute;
            var hour = webSource.Scouted.Hour;
            var match = Regex.Match(offerTimeString, @"(\d{1,2})\.(\d{1,2})\.(\d\d\d\d)");
            var day = int.Parse(match.Groups[1].Value);
            var month = int.Parse(match.Groups[2].Value);
            var year = int.Parse(match.Groups[3].Value);
            return NormalizeOfferTime(new DateTime(year, month, day, hour, minute, second, millisecond), webSource);
        }

        private IEnumerable<KeyValuePair<string, string>> ParseStructured(HtmlDocument htmlDocument)
        {
            var commodityElements = htmlDocument.DocumentNode.Descendants("table").First(div => div.Attributes.Contains("class") && div.Attributes["class"].Value.Equals("comodity-elements"));
            foreach (var row in commodityElements.Descendants("tr"))
            {
                var key = row.Descendants("th").Single().InnerText;
                if (key == "Typ:") continue;
                var value = HtmlEntity.DeEntitize(row.Descendants("td").Single().InnerText);
                yield return new KeyValuePair<string, string>(key, value);
            }
            var contactDetails = htmlDocument.DocumentNode.Descendants("div").FirstOrDefault(div => div.Attributes.Contains("class") && div.Attributes["class"].Value.Equals("ad-detail-contact"));
            if (contactDetails != null)
            {
                if (IsContactHidden(contactDetails)) yield break;
                foreach (var p in contactDetails.Descendants("p"))
                {
                    var strong = p.Descendants("strong").FirstOrDefault();
                    if (strong != null)
                    {
                        if (strong.InnerText == "Tel. èíslo:" || strong.InnerText == "Další tel. èísla:")
                        {
                            foreach (var phone in phoneFinder.GetFragments(HtmlEntity.DeEntitize(p.InnerText)))
                            {
                                yield return new KeyValuePair<string, string>("Telefon", phone);
                            }
                            continue;
                        }
                        if (strong.InnerText == "Fax:")
                        {
                            foreach (var phone in phoneFinder.GetFragments(HtmlEntity.DeEntitize(p.InnerText)))
                            {
                                yield return new KeyValuePair<string, string>("Fax", phone);
                            }
                            continue;
                        }
                        if (strong.InnerText == "Adresa:")
                        {
                            foreach (var link in p.Descendants("a").Select(anchor => anchor.Attributes["href"].Value))
                            {
                                yield return new KeyValuePair<string, string>("Odkaz", link);
                            }
                            yield return new KeyValuePair<string, string>("Adresa", p.GetTextOfTextChildren());
                            continue;
                        }
                        if (strong.InnerText == "Web:" || strong.InnerText == "E-mail:")
                        {
                            var anchor = p.Descendants("a").First();
                            yield return new KeyValuePair<string, string>(strong.InnerText, anchor.InnerText);
                            continue;
                        }
                    }
                }
                foreach (var company in contactDetails.Descendants("h3").Where(h3 => h3.InnerText == "Spoleènost"))
                {
                    var name = company.GetNextSiblings("strong").FirstOrDefault();
                    if (name != null) yield return new KeyValuePair<string, string>("Inzerent", HtmlEntity.DeEntitize(name.InnerText));
                }
            }
        }

        /// <summary>
        /// Zjistí, jestli jsou kontaktní informace skryty (dosud nevyšla tištìná verze Annonce)
        /// </summary>
        /// <param name="contactDetails"></param>
        /// <returns></returns>
        private bool IsContactHidden(HtmlNode contactDetails)
        {
            return contactDetails.Elements("p").Any(p => p.InnerText.StartsWith("Tento inzerát ještì nevyšel v tištìné ANNONCI a proto je kontakt skryt."));
        }

        /// <summary>
        /// Naplánuje reload inzerátu, protože má skrytý kontakt až do doby zveøejnìní tištìné verze Annonce
        /// </summary>
        /// <param name="contactDetails"></param>
        /// <param name="loadTime"></param>
        /// <param name="date"></param>
        /// <param name="storage"></param>
        private DateTime? GetReloadDateForHiddenContact(HtmlNode contactDetails, DateTime loadTime)
        {
            var firstP = contactDetails.Elements("p").First();
            var match = Regex.Match(firstP.InnerText, @"(\d{1,2})\.(\d{1,2})\.(\d\d\d\d)");
            if (!match.Success)
            {
                // if no date found, reload after 7 days
                return loadTime + TimeSpan.FromDays(7);
            }
            var day = int.Parse(match.Groups[1].Value);
            var month = int.Parse(match.Groups[2].Value);
            var year = int.Parse(match.Groups[3].Value);
            // naèti znovu jeden a pùl dne poté, co vyšla tištìná verze
            return new DateTime(year, month, day) + TimeSpan.FromDays(1.5);
        }
    }
}