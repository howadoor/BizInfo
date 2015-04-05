using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BizInfo.Harvesting.Interfaces.Processing;
using BizInfo.Harvesting.Services.Processing.Fragments;
using BizInfo.Model.Interfaces;
using HtmlAgilityPack;

namespace BizInfo.Harvesting.Services.Processing
{
    public abstract class CommonOfferParser : IOfferParser
    {
        protected static PhoneFragmentFinder phoneFinder = new PhoneFragmentFinder();
        protected static MailFragmentFinder mailFinder = new MailFragmentFinder();
        protected static UrlFragmentFinder linkFinder = new UrlFragmentFinder();

        public abstract void Parse(HtmlDocument htmlDocument, DateTime loadTime, IBizInfoStorage storage, IWebSource webSource, out string summary, out string text, out DateTime offerTime, out int? nativeCategory, out IEnumerable<string> photoUrls, out IEnumerable<KeyValuePair<string, string>> structured, out DateTime? reloadDate);

        protected IEnumerable<KeyValuePair<string, string>> GetPhones (string text)
        {
            return GetFragments("Telefon", text, phoneFinder);
        }

        protected IEnumerable<KeyValuePair<string, string>> GetMails(string text)
        {
            return GetFragments("Mail", text, mailFinder);
        }

        protected IEnumerable<KeyValuePair<string, string>> GetLinks(string text)
        {
            return GetFragments("Odkaz", text, linkFinder);
        }

        protected IEnumerable<KeyValuePair<string, string>> GetFragments(string key, string text, IFragmentsFinder finder)
        {
            return finder.GetFragments(text).Select(fragment => new KeyValuePair<string, string>(key, fragment));
        }

        protected static void EmbeddException(IWebSource webSource, Action action, string message)
        {
            try
            {
                action();
            }
            catch (Exception exception)
            {
                throw new OfferParsingException(message, exception, webSource);
            }
        }

        protected static TResult EmbeddException<TResult>(IWebSource webSource, Func<TResult> function, string message)
        {
            try
            {
                return function();
            }
            catch (Exception exception)
            {
                throw new OfferParsingException(message, exception, webSource);
            }
        }

        public static DateTime NormalizeOfferTime(DateTime offerTime, IWebSource webSource)
        {
            if (offerTime > webSource.Scouted) offerTime = webSource.Scouted;
            if (offerTime > DateTime.Now) offerTime = DateTime.Now;
            return offerTime;
        }
    }
    
    public class OfferParsingException : InvalidOperationException
    {
        public IWebSource WebSource { get; private set; }

        public OfferParsingException(string message, Exception innerException, IWebSource webSource)
            : base(message, innerException)
        {
            WebSource = webSource;
        }

        public override string ToString()
        {
            var builder = new StringBuilder(Message);
            builder.AppendFormat("\r\nWebSource {0}", WebSource.Url);
            builder.AppendFormat("\r\nInnerException {0}", base.ToString());
            return builder.ToString();
        }
    }
}