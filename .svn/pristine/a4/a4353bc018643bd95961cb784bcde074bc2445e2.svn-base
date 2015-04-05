using System;
using System.Linq;
using System.Text;
using BizInfo.Model.Interfaces;
using Perenis.Core.Serialization;

namespace BizInfo.Harvesting.Services.Processing.Helpers
{
    /// <summary>
    /// Collects methods for content hash computing
    /// </summary>
    public static class ContentHashTools
    {
        public static Guid ComputeContentHash(this IBizInfo info)
        {
            return ComputeContentHash(info.Summary, info.Text);
        }

        public static Guid ComputeContentHash(string summary, string text)
        {
            var builder = new StringBuilder(2048);
            if (!string.IsNullOrEmpty(summary))
            {
                foreach (var @char in summary)
                {
                    if (char.IsLetter(@char)) builder.Append(@char);
                }
            }
            if (!string.IsNullOrEmpty(text))
            {
                foreach (var @char in text.Take(512))
                {
                    if (char.IsLetter(@char)) builder.Append(@char);
                }
            }
            var letters = builder.ToString();
            var hash = XmlSerialization.ComputeGuidHash(letters);
            return hash;
        }
    }
}