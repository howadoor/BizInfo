using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BizInfo.App.Services.Logging;
using BizInfo.Harvesting.Interfaces.Processing;
using BizInfo.Harvesting.Services.Processing.CompanyScoreTools;
using BizInfo.Harvesting.Services.Processing.Helpers;
using BizInfo.Harvesting.Services.Storage;
using BizInfo.Model.Entities;
using BizInfo.Model.Interfaces;
using HtmlAgilityPack;

namespace BizInfo.Harvesting.Services.Processing
{
    public abstract class CommonOffersProcessor : ProcessingBase, IWebSourceProcessor
    {
        protected CommonOffersProcessor(Encoding encoding, UrlLoadOptions loadOptions) : base(encoding, loadOptions)
        {
        }

        protected CommonOffersProcessor(Encoding encoding) : this(encoding, UrlLoadOptions.LoadFromCacheIfPossible | UrlLoadOptions.StoreToCacheWhenLoaded)
        {
        }

        public ICompanyScoreComputer CompanyScoreComputer { get; set; }
        public IOfferParser Parser { get; set; }
        public ITagger Tagger { get; set; }
        public IParsingFailureDetector ParsingFailureDetector { get; set; }

        #region IWebSourceProcessor Members

        public void Process(IWebSource webSource, IBizInfoStorage storage, UrlLoadOptions loadOptions)
        {
            using (this.LogOperation(string.Format("Processing {0} scouted {1}", webSource.Url, webSource.Scouted)))
            {
                if (webSource.Processed.HasValue)
                {
                    this.LogInfo(string.Format("Last processed {0} result {1}", webSource.Processed.Value, webSource.ProcessingResult));
                }
                DoProcess(webSource, storage, loadOptions);
            }
        }

        #endregion

        public void DoProcess(IWebSource webSource, IBizInfoStorage storage, UrlLoadOptions loadOptions)
        {
            DateTime loadTime;
            HtmlDocument htmlDocument = LoadHtmlDocument(webSource.Url, storage.Loader, loadOptions, out loadTime);
            string summary = null;
            string text = null;
            IEnumerable<string> photoUrls = null;
            var offerTime = new DateTime();
            int? nativeCategory = null;
            StructuredContent structuredContent = null;
            DateTime? reloadDate = null;
            for (var retryCounter = 0; retryCounter < 3; retryCounter++)
            {
                try
                {
                    IEnumerable<KeyValuePair<string, string>> structured;
                    Parser.Parse(htmlDocument, loadTime, storage, webSource, out summary, out text, out offerTime, out nativeCategory, out photoUrls, out structured, out reloadDate);
                    summary = Canonicalization.CanonicalizeSummary(summary);
                    text = Canonicalization.CanonicalizeText(text);
                    structuredContent = StructuredContent.Create(structured);
                    if (photoUrls != null) photoUrls = photoUrls.ToArray();
                    if (ParsingFailureDetector != null && ParsingFailureDetector.DetectParsingFailure(webSource, storage, htmlDocument, null) != ParsingFailureReason.Unknown)
                    {
                        throw new InvalidOperationException(string.Format("Malfunction of parsing failure detector on {0}", webSource.Url));
                    }
                    goto success;
                }
                catch (Exception exception)
                {
                    this.LogException(exception);
                    var parsingFailurereason = ParsingFailureDetector != null ? ParsingFailureDetector.DetectParsingFailure(webSource, storage, htmlDocument, exception) : ParsingFailureReason.Unknown;
                    this.LogInfo(string.Format("Reason of failure {0}", parsingFailurereason));
                    switch (parsingFailurereason)
                    {
                        case ParsingFailureReason.WrongDataTryReload:
                            // reload document then retry
                            htmlDocument = LoadHtmlDocument(webSource.Url, storage.Loader, UrlLoadOptions.StoreToCacheWhenLoaded, out loadTime);
                            continue;
                        case ParsingFailureReason.WrongDataUnrecoverable:
                            storage.Delete(webSource);
                            throw;
                    }
                    throw;
                }
                throw new InvalidOperationException("CommonOfferParser: Execution of code should be never here");
            }
            throw new InvalidOperationException("Too much retries in offers parser");
            success:
            this.LogInfo(string.Format("{0} parsed successfuly: {1}", webSource.Url, summary.Length > 64 ? summary.Substring(0, 64) : summary));
            lock (storage)
            {
                IBizInfo info = ((EntityStorage) storage).modelContainer.InfoSet.FirstOrDefault(infoCandidate => infoCandidate.WebSourceId == webSource.Id);
                var contentHash = ContentHashTools.ComputeContentHash(summary, text);
                if (info == null)
                {
                    var infoWithSameContent = ((EntityStorage) storage).modelContainer.InfoSet.FirstOrDefault(infoCandidate => infoCandidate.ContentHash == contentHash);
                    if (infoWithSameContent != null)
                    {
                        this.LogInfo(string.Format("Info with the same content already exists: #{0} '{1}'", infoWithSameContent.Id, infoWithSameContent.Summary));
                        return;
                    }
                    info = storage.CreateInfo(webSource, summary, text, offerTime);
                }
                else
                {
                    info.Summary = summary;
                    info.Text = text;
                    info.CreationTime = offerTime;
                }
                info.ContentHash = contentHash;
                info.PhotoUrlsList = photoUrls;
                info.NativeCategoryId = nativeCategory;
                info.StructuredContentDocument = structuredContent;
                Tagger.Tag(info, storage);
                info.IsCompanyScore = (float) CompanyScoreComputer.ComputeScore(info);
                if (reloadDate.HasValue)
                {
                    this.LogInfo(string.Format("Scheduled for reload at {0}: Info {1} from {2}", reloadDate.Value, ((Info) info).Id, webSource.Url));
                    storage.ScheduleReload(info, reloadDate.Value);
                }
            }
        }
    }
}