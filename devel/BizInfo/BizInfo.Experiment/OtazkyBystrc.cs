using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BizInfo.Harvesting.Interfaces.Processing;
using BizInfo.Harvesting.Services.Core;
using BizInfo.Harvesting.Services.Processing;
using BizInfo.Harvesting.Services.Processing.Helpers;
using BizInfo.Model.Entities;
using HtmlAgilityPack;

namespace BizInfo.Experiment
{
    internal class OtazkyBystrc : ProcessingBase
    {
        List<OtazkyItem> items = new List<OtazkyItem>();

        private class OtazkyItem
        {
            public string OtazkaDate
            {
                get; set;
            }

            public string OdpovedDate
            {
                get;
                set;
            }

            public string OdpovedAuthor
            {
                get;
                set;
            }

            public bool IsAnswered
            {
                get { return !string.IsNullOrEmpty(OdpovedDate); }
            }
        }

        public OtazkyBystrc() : base(Encoding.GetEncoding("Windows-1250"), UrlLoadOptions.None)
        {
        }

        public OtazkyBystrc CreateStatistics()
        {
            using (var repository = new BizInfoModelContainer())
            {
                var htmlDocument = LoadHtmlDocument(@"http://otazky.umc-bystrc.cz/", new UrlDownloader(repository));
                return CreateStatistics(htmlDocument);
            }
        }

        public OtazkyBystrc CreateStatistics(HtmlDocument htmlDocument)
        {
            var newItems = new List<OtazkyItem>();
            foreach (var otazka in htmlDocument.DocumentNode.Descendants("div").Where(div => div.IdEquals("otazka")))
            {
                newItems.Add(CreateItem(otazka));
            }
            items = newItems;
            return this;
        }

        private OtazkyItem CreateItem(HtmlNode otazkaDiv)
        {
            var odpovedDiv = FindOdpoved(otazkaDiv);
            var otazkaDate = ParseDate(otazkaDiv.InnerText);
            var odpovedDate = odpovedDiv != null ? ParseDate(odpovedDiv.InnerText) : null;
            var odpovedAuthor = odpovedDiv != null ? odpovedDiv.ChildNodes.Last(th => !string.IsNullOrEmpty(th.InnerText.Trim())).InnerText.Trim() : null;
            if (!string.IsNullOrEmpty(odpovedAuthor))
            {
                var pos = odpovedAuthor.IndexOfAny(new char[]{',', '-'});
                if (pos >= 0) odpovedAuthor = odpovedAuthor.Substring(0, pos);
                pos = odpovedAuthor.LastIndexOf('.');
                if (pos >= 0) odpovedAuthor = odpovedAuthor.Substring(pos + 1);
                odpovedAuthor = odpovedAuthor.Trim();
            }
            return new OtazkyItem{OtazkaDate = otazkaDate, OdpovedDate = odpovedDate, OdpovedAuthor = odpovedAuthor};
        }

        private HtmlNode FindOdpoved(HtmlNode otazkaDiv)
        {
            foreach (var sibling in otazkaDiv.GetNextSiblings())
            {
                if (sibling.IdEquals("odpoved")) return sibling;
                if (sibling.Name == "hr") break;
            }
            return null;
        }

        private string ParseDate(string text)
        {
            var match = Regex.Match(text, @"(\d{1,2})\.(\d{1,2})\.(\d\d\d\d)");
            return match.Success ? match.Value : null;
        }

        public void Save(string targetFile)
        {
            using (var outfile = new StreamWriter(targetFile))
            {
                foreach (var item in items)
                {
                    outfile.WriteLine(string.Format("{0}\t{1}\t{2}", item.OtazkaDate, item.OdpovedDate, item.OdpovedAuthor));
                }
            }
            var by_month = new Dictionary<DateTime, int>();
            foreach (var item in items.Where(i => i.IsAnswered))
            {
                var match = Regex.Match(item.OdpovedDate, @"(\d{1,2})\.(\d{1,2})\.(\d\d\d\d)");
                var month = int.Parse(match.Groups[2].Value);
                var year = int.Parse(match.Groups[3].Value);
                var theMmonth = new DateTime(year, month, 1);
                int count = 0;
                if (!by_month.TryGetValue(theMmonth, out count))
                {
                    count = 1;
                }
                else
                {
                    count++;
                }
                by_month[theMmonth] = count;
            }
            using (var outfile = new StreamWriter(targetFile + "by_month.txt"))
            {
                var months = by_month.Keys.ToList();
                months.Sort();
                foreach (var month in months)
                {
                    outfile.WriteLine(string.Format("{0}/{1}\t{2}", month.Month, month.Year, by_month[month]));
                }
            }
            var by_author = new Dictionary<string, int>();
            foreach (var item in items.Where(i => i.IsAnswered))
            {
                var author = item.OdpovedAuthor;
                int count = 0;
                if (!by_author.TryGetValue(author, out count))
                {
                    count = 1;
                }
                else
                {
                    count++;
                }
                by_author[author] = count;
            }
            using (var outfile = new StreamWriter(targetFile + "byauthor.txt"))
            {
                var authors = by_author.Keys.ToList();
                authors.Sort();
                foreach (var author in authors)
                {
                    outfile.WriteLine(string.Format("{0}\t{1}", author, by_author[author]));
                }
            }
        }
    }
}