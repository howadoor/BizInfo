using System;
using BizInfo.Harvesting.Services.Scouting;
using BizInfo.Harvesting.Services.Tests.Tools;
using BizInfo.Model.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BizInfo.Harvesting.Services.Tests.Scouting
{
    public abstract class CommonScoutTestBase<TScout> : ScoutTestBase<TScout> where TScout : CommonScout, new()
    {
        protected abstract string InitialPageUrl { get; }

        public override void DisposeInstance(TScout instance)
        {
            // intentionaly does nothing
        }

        public override TScout CreateInstance()
        {
            return new TScout {InitialPageUrl = InitialPageUrl};
        }

        /// <summary>
        /// Tests <see cref="CommonScout.GetNextPageUrl"/> method
        /// </summary>
        [TestMethod]
        public void TestGetNextPageUrl()
        {
            PerformTest<CommonScout>(TestGetNextPageUrl);
        }

        /// <summary>
        /// Tests <see cref="CommonScout.GetNextPageUrl"/> method
        /// </summary>
        /// <param name="commonScout">Scout to be tested</param>
        protected void TestGetNextPageUrl(CommonScout commonScout)
        {
            using (var storage = CreateBizInfoStorage())
            {
                TestGetNextPageUrl(storage, commonScout);
            }
        }

        private void TestGetNextPageUrl(IBizInfoStorage storage, CommonScout commonScout)
        {
            var document = commonScout.LoadHtmlDocument(commonScout.InitialPageUrl, storage.Loader);
            Assert.IsNotNull(document);
            var nextPageUrl = commonScout.GetNextPageUrl(document, commonScout.InitialPageUrl);
            Console.WriteLine("URL of the page: {0}", commonScout.InitialPageUrl);
            Console.WriteLine("Next page URL: {0}", nextPageUrl);
            TestHelpers.AssertIsValidUrl(nextPageUrl);
        }
    }
}