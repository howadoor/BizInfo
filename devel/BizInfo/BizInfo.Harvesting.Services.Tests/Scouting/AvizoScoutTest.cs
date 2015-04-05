using BizInfo.Harvesting.Services.Scouting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BizInfo.Harvesting.Services.Tests.Scouting
{
    [TestClass]
    public class AvizoScoutTest : CommonScoutTestBase<AvizoScout>
    {
        protected override string InitialPageUrl
        {
            get { return @"http://autobazar.avizo.cz/osobni-automobily/"; }
        }
    }
}