using System;
using System.Collections.Generic;

namespace BizInfo.Harvesting.Services.Processing.Fragments
{
    public interface IFragmentsFinder
    {
        IEnumerable<Tuple<int, int>> FindFragments(string text);
    }
}