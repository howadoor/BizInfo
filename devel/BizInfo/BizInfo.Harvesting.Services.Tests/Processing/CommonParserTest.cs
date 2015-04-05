using System;
using System.Collections.Generic;
using BizInfo.Harvesting.Interfaces.Processing;
using BizInfo.Harvesting.Services.Scouting;
using BizInfo.Harvesting.Services.Tests.Scouting;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BizInfo.Harvesting.Services.Tests.Processing
{
    [TestClass]
    public class CommonParserTest<TParser, TScout, TScoutTest> : OfferParserTestBase<TParser> where TParser : class, IOfferParser, new() where TScoutTest : new() where TScout : class, IScout
    {
        public override void DisposeInstance(TParser instance)
        {
            // intentionaly does nothing;
        }

        public override TParser CreateInstance()
        {
            return new TParser();
        }

        protected override IEnumerable<Tuple<HtmlDocument, string>> GetTestDocuments()
        {
            var scoutTest = new TScoutTest() as ScoutTestBase<TScout>;
            var scout = scoutTest.CreateInstance() as CommonScout;
            try
            {
                using (var storage = scoutTest.CreateBizInfoStorage())
                {
                    var urls = ScoutTestBase<TScout>.TestBaseScoutingCapabilities(storage, scout);
                    Assert.IsNotNull(urls);
                    Assert.IsTrue(urls.Count > 0);
                    foreach (var url in urls)
                    {
                        var document = scout.LoadHtmlDocument(url, storage.Loader);
                        yield return new Tuple<HtmlDocument, string>(document, url);
                    }
                }
            }
            finally
            {
                scoutTest.DisposeInstance(scout as TScout);
            }
        }
    }
}