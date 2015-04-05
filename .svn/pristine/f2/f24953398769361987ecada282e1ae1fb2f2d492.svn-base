using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BizInfo.App.Services.Logging;
using BizInfo.Harvesting.Interfaces.Processing;
using BizInfo.Harvesting.Services.Processing;
using BizInfo.Model.Interfaces;
using HtmlAgilityPack;

namespace BizInfo.Harvesting.Services.Scouting
{
    public abstract class CommonScout : ProcessingBase, IScout
    {
        /// <summary>
        /// Defines strategy for stopping scout
        /// </summary>
        public enum StopStrategy
        {
            StopOnFirstNotAccepted,
            StopWhenLastUrlInPageIsNotAccepted
        }

        public StopStrategy Stop { get; set; }
        
        protected CommonScout(Encoding encoding) : this(encoding, UrlLoadOptions.None)
        {
        }

        protected CommonScout(Encoding encoding, UrlLoadOptions loadOptions) : base(encoding, loadOptions)
        {
            Stop = StopStrategy.StopWhenLastUrlInPageIsNotAccepted;
        }

        public string InitialPageUrl { get; set; }

        public string LastProcessedPageUrl { get; protected set; }

        #region IScout Members

        public void Scout(IBizInfoStorage storage, Func<string, IBizInfoStorage, bool> urlAcceptor)
        {
            string pageUrl = InitialPageUrl;
            for (var page = LoadHtmlDocument(pageUrl, storage.Loader); page != null; pageUrl = GetNextPageUrl(page, pageUrl), page = string.IsNullOrEmpty(pageUrl) ? null : LoadHtmlDocument(pageUrl, storage.Loader))
            {
                LastProcessedPageUrl = pageUrl;
                this.Log(string.Format("Scouting in {0}, saved as", pageUrl));
                switch(Stop)
                {
                    case StopStrategy.StopOnFirstNotAccepted:
                        if (!ScoutUrls(page, pageUrl).All(scoutedUrl => urlAcceptor(scoutedUrl, storage))) return;
                        break;
                    case StopStrategy.StopWhenLastUrlInPageIsNotAccepted:
                        bool accepted = false;
                        foreach (var url in ScoutUrls(page, pageUrl))
                        {
                            accepted = urlAcceptor(url, storage);
                        }
                        if (!accepted) return;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        #endregion

        protected abstract IEnumerable<string> ScoutUrls(HtmlDocument page, string pageUrl);

        public abstract string GetNextPageUrl(HtmlDocument page, string pageUrl);
    }
}