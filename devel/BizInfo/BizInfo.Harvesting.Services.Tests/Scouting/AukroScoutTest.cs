using BizInfo.Harvesting.Services.Scouting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BizInfo.Harvesting.Services.Tests.Scouting
{
    [TestClass]
    public class AukroScoutTest : CommonScoutTestBase<AukroScout>
    {
        protected override string InitialPageUrl
        {
            get { return @"http://auto.aukro.cz/listing.php/showcat?id=8503&order=td&p=1"; }
        }
    }
}