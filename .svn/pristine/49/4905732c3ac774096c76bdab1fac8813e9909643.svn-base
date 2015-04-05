using System;
using System.Linq;
using System.Text.RegularExpressions;
using BizInfo.Harvesting.Services.Processing.Helpers;
using BizInfo.Model.Interfaces;
using HtmlAgilityPack;

namespace BizInfo.Harvesting.Services.Processing
{
    public class ParsingFailureDetector : IParsingFailureDetector
    {
        public ParsingFailureReason DetectParsingFailure(IWebSource webSource, IBizInfoStorage storage, HtmlDocument htmlDocument, Exception exception)
        {
            // proxies
            if (htmlDocument.DocumentNode.Descendants("title").Where(title => title.InnerText.Contains("Welcome to the CoDeeN HTTP CDN Service")).FirstOrDefault() != null
                || htmlDocument.DocumentNode.Descendants("h1").Where(title => title.InnerText.Contains("CodeDiaries IP:Port Proxy Emulator")).FirstOrDefault() != null)
            {
                return ParsingFailureReason.WrongDataTryReload;
            }
            if (Regex.IsMatch(webSource.Url, @"^http://([^\.]+).hyperinzerce.cz/"))
            {
                // hyperinzerce.cz, inzerát nenalezen
                if (htmlDocument.DocumentNode.Descendants("div").Where(div => div.InnerText.StartsWith("Momentálně zde nejsou žádné inzeráty")).FirstOrDefault() != null
                    || htmlDocument.DocumentNode.Descendants("p").Where(div => div.InnerText.StartsWith("Inzeráty 1 až")).FirstOrDefault() != null)
                {
                    return ParsingFailureReason.WrongDataUnrecoverable;
                }
                var link = htmlDocument.DocumentNode.Descendants("link").Where(lnk => lnk.Attributes["href"].Value.Contains("hyperinzerce.cz")).FirstOrDefault();
                if (link == null) return ParsingFailureReason.WrongDataTryReload;
            }
            // Annonce
            if (Regex.IsMatch(webSource.Url, @"^http://www.annonce.cz/detail/"))
            {
                // Deleted offer
                var info = htmlDocument.DocumentNode.Descendants("div").WhereAttribute("class", c => c == "info").FirstOrDefault();
                if (info != null && info.Elements("p").Any(p => p.InnerText.StartsWith("Daný inzerát neexistuje"))) return ParsingFailureReason.WrongDataUnrecoverable;
                // Wrong data
                var title = htmlDocument.DocumentNode.Descendants("title").FirstOrDefault();
                if (title == null || !title.InnerText.Contains("ANNONCE.cz")) return ParsingFailureReason.WrongDataTryReload;
                var facebook = htmlDocument.DocumentNode.Descendants("a").WhereAttribute("href", href => href == "http://www.facebook.com/Annonce.cz").FirstOrDefault();
                if (facebook == null) return ParsingFailureReason.WrongDataTryReload;
            }
            // Bazos.cz
            if (Regex.IsMatch(webSource.Url, @"^http://([^\.]+).bazos.cz/inzerat/[^/]+/"))
            {
                // Deleted offer
                var deleted = htmlDocument.DocumentNode.Descendants("b").Where(b => b.InnerText.Contains("Inzerát byl vymazán.")).FirstOrDefault();
                if (deleted != null) return ParsingFailureReason.WrongDataUnrecoverable;
            }
            // Inzert Expres
            if (Regex.IsMatch(webSource.Url, @"^http://www.inzertexpres.cz/\d+/"))
            {
                // Deleted offer
                var deleted = htmlDocument.DocumentNode.Descendants("p").Where(p => p.InnerText.Contains("Požadovaný inzerát není v databázi.")).FirstOrDefault();
                if (deleted != null) return ParsingFailureReason.WrongDataUnrecoverable;
            }
            return ParsingFailureReason.Unknown;
            // return new LoggingParsingFailureDetector().DetectParsingFailure(webSource, storage, htmlDocument, exception);
        }
    }
}