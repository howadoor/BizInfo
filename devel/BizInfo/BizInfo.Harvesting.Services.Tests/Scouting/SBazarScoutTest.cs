using BizInfo.Harvesting.Services.Scouting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BizInfo.Harvesting.Services.Tests.Scouting
{
    [TestClass]
    public class SBazarScoutTest : CommonScoutTestBase<SBazarScout>
    {
        protected override string InitialPageUrl
        {
            get { return @"http://autobazar.sbazar.cz/prodam-koupim-aukce.html?order=date_create&by=desc"; }
        }
    }
}