using System;
using System.Text.RegularExpressions;

namespace BizInfo.Harvesting.Services.Processing.Fragments
{
    public static class TextProcessingTools
    {
        private static FragmentsProcessor fragmentsProcessor;

        public static FragmentsProcessor Processor
        {
            get
            {
                if (fragmentsProcessor == null)
                {
                    fragmentsProcessor = new FragmentsProcessor();
                    fragmentsProcessor.AddFinder(new PhoneFragmentFinder(), ProcessPhoneFragment);
                    fragmentsProcessor.AddFinder(new MailFragmentFinder(), ProcessMailFragment);
                    fragmentsProcessor.AddFinder(new UrlFragmentFinder(), ProcessWebPageFragment);
                }
                return fragmentsProcessor;
            }
        }

        private static string ProcessWebPageFragment(string url)
        {
            return string.Format("{0} {1} {2}", LinkTo(url, Boldify(url)), BizInfoSearchLink(url, url), GoogleSearchLink(url, url));
        }

        private static string ProcessMailFragment(string mail)
        {
            return string.Format("{0} {1} {2}", MailTo(mail, Boldify(mail)), BizInfoSearchLink(mail, mail), GoogleSearchLink(mail, mail));
        }

        private static string ProcessPhoneFragment(string phone)
        {
            var displayPhone = CanonicalizePhone(phone);
            return string.Format("{0} {1} {2}", Boldify(displayPhone), BizInfoSearchLink(BizInfoPhoneSearchPhrase(phone), displayPhone), GoogleSearchLink(displayPhone, displayPhone));
        }

        private static string GoogleSearchLink(string phrase, string displayText)
        {
            return string.Format("<a target=\"_blank\" href=\"http://www.google.cz/search?q={0}\" title=\"Vyhledat na Googlu {1}\">{2}</a>", phrase, displayText, GoogleSearchIcon);
        }

        private static string BizInfoPhoneSearchPhrase(string phone)
        {
            return string.Format("{0:### ### ### ###} OR {0}", Int64.Parse(Regex.Replace(phone, @"\D", "")));
        }

        private static string BizInfoSearchLink(string phrase, string displayText)
        {
            return string.Format("<a data-ajax=\"true\" data-ajax-mode=\"replace\" data-ajax-update=\"#bizinfo-page\" href=\"./Base/Search?Phrase={0}\" title=\"Vyhledat inzeráty obsahující {1}\">{2}</a>", phrase, displayText, BizInfoSearchIcon);
        }

        private static string BizInfoSearchIcon
        {
            get { return "<img src=\"./Images/information_16.png\" class=\"inplace-search-icon\"/>"; }
        }

        private static string GoogleSearchIcon
        {
            get { return "<img src=\"./Images/google_favicon.png\" class=\"inplace-search-icon\"/>"; }
        }

        private static string LinkTo(string url, string text)
        {
            return string.Format("<a target=\"_blank\" href=\"http://{0}\" title=\"{0}\">{1}</a>", url, text);
        }

        private static string MailTo(string email, string text)
        {
            return string.Format("<a href=\"mailto:{0}\" title=\"Napsat mail na adresu {0}\">{1}</a>", email, text);
        }

        private static string CanonicalizePhone(string phone)
        {
            return string.Format("{0:### ### ### ###}", Int64.Parse(Regex.Replace(phone, @"\D", "")));
        }

        private static string Boldify(string str)
        {
            return string.Format("<em>{0}</em>", str);
        }
    }
}