using BizInfo.Harvesting.Services.Scouting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BizInfo.Harvesting.Services.Tests.Scouting
{
    [TestClass]
    public class HyperInzerceScoutTest : CommonScoutTestBase<HyperInzerceScout>
    {
        protected override string InitialPageUrl
        {
            get { return @"http://autobazar.hyperinzerce.cz/inzerce/"; }
        }
    }
}