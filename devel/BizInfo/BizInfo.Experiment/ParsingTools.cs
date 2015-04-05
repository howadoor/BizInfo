using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace BizInfo.Experiment
{
    internal static class ParsingTools
    {
        public static string GetNextPage(byte[] content)
        {
            var stringBuilder = new StringBuilder();

            var htmlDocument = new HtmlDocument();
            htmlDocument.Load(new MemoryStream(content));
            foreach (var node in htmlDocument.DocumentNode.Descendants("a"))
            {
                if (node.InnerText == "Další") return node.Attributes["href"].Value;
            }
            return null;
        }

        public static IEnumerable<string> ParseOffers(byte[] content)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.Load(new MemoryStream(content));
            foreach (var node in htmlDocument.DocumentNode.Descendants("a").Where(candidate => candidate.HasAttributes && candidate.Attributes.Contains("class") && candidate.Attributes["class"].Value == "bbns"))
            {
                yield return node.Attributes["href"].Value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        /// <remarks>
        /// <code>
        ///     <div id="offer-detail-photos">
        /// </code>
        /// </remarks>
        public static IEnumerable<string> ParseOfferPhotos(byte[] content)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.Load(new MemoryStream(content));
            var photosNode = htmlDocument.GetElementbyId("offer-detail-photos");
            if (photosNode != null)
            {
                foreach (var node in photosNode.Descendants("a"))
                {
                    if (!node.Attributes.Contains("href")) continue;
                    var href = node.Attributes["href"].Value;
                    var extension = Path.GetExtension(href);
                    if (string.IsNullOrEmpty(extension)) continue;
                    extension = extension.Substring(1).ToLower();
                    if (IsImageExtension(extension)) yield return href;
                }
            }
        }

        private static bool IsImageExtension(string extension)
        {
            return extension == "jpg" || extension == "jpeg" || extension == "gif" || extension == "png" || extension == "bmp";
        }
    }
}