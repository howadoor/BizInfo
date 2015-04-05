using System.IO;
using BizInfo.Core.Services.WebResources.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BizInfo.Core.Services.Tests.Tests.Tools
{
    /// <summary>
    /// Tests <see cref="StoredResource"/> class
    /// </summary>
    [TestClass]
    public class StoredResourceTest
    {
        /// <summary>
        /// Tests <see cref="StoredResource.GetVersionEntryName"/> static method
        /// </summary>
        [TestMethod]
        public void TestGetVersionEntryName()
        {
            AssertIsVersionEntryNameCorrect(StoredResource.GetVersionEntryName("http://stackoverflow.com/questions/1105593/get-file-name-from-uri-string-in-c-sharp", 1));
            AssertIsVersionEntryNameCorrect(StoredResource.GetVersionEntryName("http://www.zoobrno.cz/cs/aktuality/foto-poradane-stedrodopoledni-krmeni.html", 124));
            AssertIsVersionEntryNameCorrect(StoredResource.GetVersionEntryName("chrome://updatescan/content/diffPage.xul?id=297&title=YouTube%20-%20%u202Abystrc%u202C%u200F&url=http%3A//www.youtube.com/results%3Fsearch_type%3Dvideos%26search_query%3Dbystrc%26search_sort%3Dvideo_date_uploaded%26suggested_categories%3D2%252C17%252C24%252C10%26uni%3D3&oldDate=p%u0159ed%201%20dnem&newDate=dnes%20v%209%3A52&delay=0", 124));
            AssertIsVersionEntryNameCorrect(StoredResource.GetVersionEntryName("http://mapy.cz/#x=16.460102&y=49.218475&z=15&d=base_1697559_0_1&t=s&q=Helen%C4%8Dina%20stud%C3%A1nka&qp=16.456808_49.229734_16.478282_49.238187_15", 223));
            Assert.IsTrue(StoredResource.GetVersionEntryName("http://www.zoobrno.cz/cs/aktuality/foto-poradane-stedrodopoledni-krmeni.html", 331).EndsWith(".html"));
        }

        /// <summary>
        /// Checks if version netry name is correct
        /// </summary>
        /// <param name="versionEntryName">Version entry name</param>
        private static void AssertIsVersionEntryNameCorrect(string versionEntryName)
        {
            foreach (var @char in versionEntryName)
            {
                Assert.AreNotEqual(@char, ';');
                Assert.AreNotEqual(@char, ':');
                Assert.AreNotEqual(@char, '/');
                Assert.AreNotEqual(@char, '\\');
                Assert.AreNotEqual(@char, '?');
            }
            Assert.IsTrue(versionEntryName.IndexOfAny(Path.GetInvalidPathChars()) < 0);
        }
    }
}