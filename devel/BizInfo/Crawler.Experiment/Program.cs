using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crawler.Experiment
{
    class Program
    {
        static void Main(string[] args)
        {
            new Crawler("http://www.bystrcnik.cz").ParallelCrawl();
        }
    }
}
