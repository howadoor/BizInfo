using System.Text.RegularExpressions;

namespace BizInfo.Harvesting.Services.Processing.Fragments
{
    public class MailFragmentFinder : RegexBasedFragmentsFinder
    {
        public MailFragmentFinder()
            : base(@"\b[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b", RegexOptions.IgnoreCase | RegexOptions.Compiled)
        {
        }
    }
}