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
    public class AukroParser : CommonOfferParser
    {
        public override void Parse(HtmlDocument htmlDocument, DateTime loadTime, IBizInfoStorage storage, IWebSource webSource, out string summary, out string text, out DateTime offerTime, out int? nativeCategory, out IEnumerable<string> photoUrls,
                                   out IEnumerable<KeyValuePair<string, string>> structured, out DateTime? reloadDate)
        {
            htmlDocument.DocumentNode.RemoveComments();
            summary = EmbeddException(webSource, () => htmlDocument.DocumentNode.Descendants("h1").First().InnerText.Trim(), "Cannot parse offer summary");
            text = EmbeddException(webSource, () => HtmlEntity.DeEntitize(htmlDocument.DocumentNode.Descendants("fieldset").WhereIdEquals("user_field").Single().InnerText).Trim(), "Cannot parse offer text");
            offerTime = EmbeddException(webSource, () => ParseOfferTime(htmlDocument, webSource, loadTime), "Cannot parse offer time");
            
            nativeCategory = EmbeddException(webSource, () => ParseNativeCategory(htmlDocument, storage, webSource), "Exception parsing native category");
            
            photoUrls = EmbeddException(webSource, () => ParseOfferPhotoUrls(htmlDocument, webSource), "Exception parsing offer photo urls");
            var txt = text;
            structured = EmbeddException(webSource, () => ParseStructured(htmlDocument).Concat(GetMails(txt)).Concat(GetPhones(txt)).Concat(GetLinks(txt)), "Cannot parse structured content");
            reloadDate = GetReloadDate(htmlDocument, loadTime);
        }

        private DateTime? GetReloadDate(HtmlDocument htmlDocument, DateTime loadTime)
        {
            return null;
        }

        private int? ParseNativeCategory(HtmlDocument htmlDocument, IBizInfoStorage storage, IWebSource webSource)
        {
            var category = storage.GetCategory(UrlTools.GetServerOnly(webSource.Url), null);
            category = storage.GetCategory("Prodej", category);
            category = storage.GetCategory(UrlTools.GetSubdomains(webSource.Url), category);
            return category;
        }

        private IEnumerable<string> ParseOfferPhotoUrls(HtmlDocument htmlDocument, IWebSource webSource)
        {
            var photoScript = htmlDocument.DocumentNode.Descendants("script").FirstOrDefault(script => script.InnerText.Contains("photos:"));
            if (photoScript == null) yield break;
            var mediumPart = Regex.Match(photoScript.InnerText, @"medium: \[([^\]]+)").Groups[1].Value;
            foreach (var url in mediumPart.Split('"', ',', ' ', '\r', '\n').Where(urlCandidate => !string.IsNullOrEmpty(urlCandidate)))
            {
                yield return url;
            }
        }

        private DateTime ParseOfferTime(HtmlDocument htmlDocument, IWebSource webSource, DateTime loadTime)
        {
            return webSource.Scouted;
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
            yield break;
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