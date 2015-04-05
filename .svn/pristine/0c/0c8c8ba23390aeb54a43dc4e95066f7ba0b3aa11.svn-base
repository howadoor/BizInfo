using System;
using System.Collections.Generic;
using System.Linq;
using BizInfo.Harvesting.Services.Processing;
using BizInfo.Harvesting.Services.Scouting;
using BizInfo.Harvesting.Services.Tests.Scouting;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BizInfo.Harvesting.Services.Tests.Processing
{
    [TestClass]
    public class AnnonceParserTest : CommonParserTest<AnnonceParser, AnnonceScout, AnnonceScoutTest>
    {
        /// <summary>
        /// Because of slowness we will use only max 8 documents
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<Tuple<HtmlDocument, string>> GetTestDocuments()
        {
            return base.GetTestDocuments().Take(8);
        }
    }
}