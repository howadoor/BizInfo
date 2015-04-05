using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BizInfo.Harvesting.Services.Processing.Helpers
{
    /// <summary>
    /// Methods for canonicalization of various info components
    /// </summary>
    public static class Canonicalization
    {
        /// <summary>
        /// Canonicalizes info summary
        /// </summary>
        /// <param name="summary"></param>
        /// <returns></returns>
        public static string CanonicalizeSummary (string summary)
        {
            if (string.IsNullOrEmpty(summary)) return summary;
            return RemoveObsoleteWhitespace(RemoveHtmlComments(summary)).Trim();
        }

        /// <summary>
        /// Canonicalizes info text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string CanonicalizeText(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            return RemoveObsoleteWhitespace(RemoveHtmlComments(text)).Trim();
        }

        public static string RemoveHtmlComments(string summary)
        {
            return Regex.Replace(summary, @"<!--.*?-->", String.Empty, RegexOptions.Singleline);
        }

        public static string RemoveObsoleteWhitespace(string summary)
        {
            summary = Regex.Replace(summary, @" +([,!?])", "$1", RegexOptions.Multiline);
            summary = Regex.Replace(summary, @"  +", " ", RegexOptions.Multiline);
            return Regex.Replace(summary, @"(\n|\r|^) +", "$1", RegexOptions.Multiline);
        }
    }
}
