using System;
using System.Collections.Generic;
using BizInfo.Harvesting.Interfaces.Processing;
using BizInfo.Harvesting.Services.Tests.Tools;
using BizInfo.Model.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Perenis.Testing.Objects;

namespace BizInfo.Harvesting.Services.Tests.Scouting
{
    /// <summary>
    /// Base class for testing implementors of <see cref="IScout"/> interface
    /// </summary>
    /// <typeparam name="TScout">Type implementing <see cref="IScout"/> interface</typeparam>
    [TestClass]
    public abstract class ScoutTestBase<TScout> : TestOnInstance<TScout> where TScout : class, IScout
    {
        /// <summary>
        /// Tests base capabilities of <see cref="IScout"/> interface
        /// </summary>
        [TestMethod]
        public void TestBaseScoutingCapabilities()
        {
            PerformTest<IScout>(TestBaseScoutingCapabilities);
        }

        private void TestBaseScoutingCapabilities(IScout scout)
        {
            using (var storage = CreateBizInfoStorage())
            {
                TestBaseScoutingCapabilities(storage, scout);
            }
        }

        /// <summary>
        /// Calls <see cref="IScout.Scout"/> method of <see cref="scout"/>. Checks if some URLS are found, checks for validity of
        /// returned URLs.
        /// </summary>
        /// <param name="storage">Storage</param>
        /// <param name="scout">Scout to be tested</param>
        /// <returns>List of scouted URLs</returns>
        public static List<string> TestBaseScoutingCapabilities(IBizInfoStorage storage, IScout scout)
        {
            var scoutedUrls = new List<string>();
            scout.Scout(storage, (url, strg) =>
                                     {
                                         TestHelpers.AssertIsValidUrl(url);
                                         scoutedUrls.Add(url);
                                         Console.WriteLine("Scouted #{0} {1}", scoutedUrls.Count, url);
                                         return false;
                                     });
            Assert.IsTrue(scoutedUrls.Count > 0);
            return scoutedUrls;
        }

        public IBizInfoStorage CreateBizInfoStorage()
        {
            return TestEntityStorage.Create();
        }
    }
}