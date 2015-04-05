using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BizInfo.Harvesting.Services.Processing.Fragments
{
    public class RegexBasedFragmentsFinder : IFragmentsFinder
    {
        public Regex Regex { get; private set; }

        public RegexBasedFragmentsFinder(string pattern, RegexOptions regexOptions) : this (new Regex(pattern, regexOptions))
        {
            
        }

        private RegexBasedFragmentsFinder(Regex regex)
        {
            Regex = regex;
        }

        public IEnumerable<Tuple<int, int>> FindFragments(string text)
        {
            return from Match match in this.Regex.Matches(text) select new Tuple<int, int>(match.Index, match.Length);
        }
    }
}