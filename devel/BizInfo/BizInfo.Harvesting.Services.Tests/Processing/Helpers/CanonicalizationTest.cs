using BizInfo.Harvesting.Services.Processing.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BizInfo.Harvesting.Services.Tests.Processing.Helpers
{
    /// <summary>
    /// Tests <see cref="Canonicalization"/> class
    /// </summary>
    [TestClass]
    public class CanonicalizationTest
    {
        [TestMethod] 
        public void TestRemoveHtmlComments()
        {
            var removed = Canonicalization.RemoveHtmlComments("Koko");
            Assert.AreEqual(removed, "Koko");
            removed = Canonicalization.RemoveHtmlComments("Koko-->");
            Assert.AreEqual(removed, "Koko-->");
            removed = Canonicalization.RemoveHtmlComments("<!--Koko-->");
            Assert.AreEqual(removed, "");
            removed = Canonicalization.RemoveHtmlComments("Zumba<!--\r\nKoko-->Bumba");
            Assert.AreEqual(removed, "ZumbaBumba");
            removed = Canonicalization.RemoveHtmlComments("Zumba<!--\r\nKoko-->Bumba-->xxx");
            Assert.AreEqual(removed, "ZumbaBumba-->xxx");
            removed = Canonicalization.RemoveHtmlComments("Zumba<!--\r\nKoko--Bumba--xxx>");
            Assert.AreEqual(removed, "Zumba<!--\r\nKoko--Bumba--xxx>");
        }

        [TestMethod]
        public void TestRemoveObsoleteWhitespace()
        {
            var removed = Canonicalization.RemoveObsoleteWhitespace("   Koko\r\n   Cvoko\r\n                 zzzz");
            Assert.AreEqual(removed, "Koko\r\nCvoko\r\nzzzz");
            removed = Canonicalization.RemoveObsoleteWhitespace("Ahoj ,   jak se  máš  ?    Zumba , bumba   , křumpa  !");
            Assert.AreEqual(removed, "Ahoj, jak se máš? Zumba, bumba, křumpa!");
        }
    }
}