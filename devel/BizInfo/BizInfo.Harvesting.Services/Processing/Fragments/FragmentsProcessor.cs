using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BizInfo.Harvesting.Services.Processing.Fragments
{
    public class FragmentsProcessor
    {
        public List<Tuple<IFragmentsFinder, Func<string, string>>> Finders { get; private set; }

        public FragmentsProcessor()
        {
            Finders = new List<Tuple<IFragmentsFinder, Func<string, string>>>();
        }

        public void AddFinder(IFragmentsFinder finder, Func<string, string> processor)
        {
            Finders.Add(new Tuple<IFragmentsFinder, Func<string, string>>(finder, processor));
        }

        public string Process(string text)
        {
            var builder = new StringBuilder(text.Length * 2);
            foreach (var processed in Process(text, 0)) builder.Append(processed);
            return builder.ToString();
        }

        private IEnumerable<string> Process(string text, int finderIndex)
        {
            if (finderIndex >= Finders.Count)
            {
                yield return text;
            }
            else
            {
                var finder = Finders[finderIndex].Item1;
                var processor = Finders[finderIndex].Item2;
                var startChar = 0;
                foreach (var match in finder.FindFragments(text))
                {
                    if (match.Item1 > startChar)
                    {
                        foreach (var overMatch in Process(text.Substring(startChar, match.Item1 - startChar), finderIndex + 1)) yield return overMatch;
                    }
                    yield return processor(text.Substring(match.Item1, match.Item2));
                    startChar = match.Item1 + match.Item2;
                }
                if (startChar < text.Length - 1)
                    foreach (var overMatch in Process(text.Substring(startChar, text.Length - startChar), finderIndex + 1)) yield return overMatch;
            }
        }
    }
}