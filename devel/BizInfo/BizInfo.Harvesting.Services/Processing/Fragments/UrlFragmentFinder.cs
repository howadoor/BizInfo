using System.Text.RegularExpressions;

namespace BizInfo.Harvesting.Services.Processing.Fragments
{
    public class UrlFragmentFinder : RegexBasedFragmentsFinder
    {
        public UrlFragmentFinder()
            : base(@"\b(?:(?:https?|ftp|file)://|www\.|ftp\.)(?:\([-A-Z0-9+&@#/%=~_|?!:,.]*\)|[-A-Z0-9+&@#/%=~_|$?!:,.])*(?:\([-A-Z0-9+&@#/%=~_|$?!:,.]*\)|[A-Z0-9+&@#/%=~_|$])", RegexOptions.IgnoreCase | RegexOptions.Compiled)
        {
        }
    }
}