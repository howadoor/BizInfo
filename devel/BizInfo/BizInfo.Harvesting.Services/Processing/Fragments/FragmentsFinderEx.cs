using System.Collections.Generic;
using System.Linq;

namespace BizInfo.Harvesting.Services.Processing.Fragments
{
    public static class FragmentsFinderEx
    {
        public static IEnumerable<string> GetFragments(this IFragmentsFinder finder, string text)
        {
            return finder.FindFragments(text).Select(fr => text.Substring(fr.Item1, fr.Item2));
        }
    }
}