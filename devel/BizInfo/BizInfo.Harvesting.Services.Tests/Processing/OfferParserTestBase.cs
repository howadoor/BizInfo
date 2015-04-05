using System;
using System.Collections.Generic;
using System.Linq;
using BizInfo.Harvesting.Interfaces.Processing;
using BizInfo.Harvesting.Services.Tests.Scouting;
using BizInfo.Harvesting.Services.Tests.Tools;
using BizInfo.Model.Interfaces;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Perenis.Testing.Objects;

namespace BizInfo.Harvesting.Services.Tests.Processing
{
    /// <summary>
    /// Common base for test of implementors of <see cref="IOfferParser"/> interface
    /// </summary>
    /// <typeparam name="TParser"></typeparam>
    [TestClass]
    public abstract class OfferParserTestBase<TParser> : TestOnInstance<TParser> where TParser : class, IOfferParser
    {
        private Tuple<HtmlDocument, string>[] testDocuments;

        protected IEnumerable<Tuple<HtmlDocument, string>> TestDocuments
        {
            get
            {
                return testDocuments ?? (testDocuments = GetTestDocuments().ToArray());
            }
        }

        /// <summary>
        /// General test of parsing capabilities
        /// </summary>
        [TestMethod]
        public void TestParsing()
        {
            PerformTest<IOfferParser>(TestParsing);
        }

        /// <summary>
        /// General test of parsing capabilities
        /// </summary>
        /// <param name="parser">Parser to be tested</param>
        private void TestParsing(IOfferParser parser)
        {
            using (var storage = CreateBizInfoStorage())
            {
                foreach (var document in TestDocuments)
                {
                    TestParsing(parser, document, storage);
                }
            }
        }

        /// <summary>
        /// General test of parsing capabilities
        /// </summary>
        /// <param name="parser">Parser to be tested</param>
        /// <param name="document">Document to be parsed</param>
        private void TestParsing(IOfferParser parser, Tuple<HtmlDocument, string> document, IBizInfoStorage storage)
        {
            Assert.IsNotNull(document);
            Assert.IsNotNull(parser);
            var webSource = CreateWebSource(document);
            IEnumerable<KeyValuePair<string, string>> structured;
            IEnumerable<string> photoUrls;
            int? nativeCategory;
            DateTime offerTime;
            string text;
            string summary;
            DateTime? reloadDate;
            Console.WriteLine("Parsing {0} {1}", document.Item1, document.Item2);
            parser.Parse(document.Item1, DateTime.Now, storage, webSource, out summary, out text, out offerTime, out nativeCategory, out photoUrls, out structured, out reloadDate);
            Assert.IsFalse(string.IsNullOrEmpty(summary));
            Assert.IsFalse(string.IsNullOrEmpty(text));
            Assert.IsTrue(!reloadDate.HasValue || reloadDate.Value > DateTime.Now);
            Assert.IsTrue(nativeCategory >= 0);
            Assert.IsTrue(offerTime <= DateTime.Now);
            var photos = photoUrls.ToArray();
            TestHelpers.AssertIsValidUrl(photos);
            var structuredData = structured.ToArray();
        }

        private static IWebSource CreateWebSource(Tuple<HtmlDocument, string> document)
        {
            return new TestWebSource(document.Item2);
        }

        /// <summary>
        /// Returns collection of <see cref="Tuple{HtmlDocument, string}"/> instances to be parsed in parsing tests. First member of the tuple is
        /// document itself, second is URL of the document
        /// </summary>
        /// <returns>Collection of <see cref="Tuple{HtmlDocument, string}"/> instances to be parsed in parsing tests</returns>
        protected abstract IEnumerable<Tuple<HtmlDocument, string>> GetTestDocuments();

        protected IBizInfoStorage CreateBizInfoStorage()
        {
            return TestEntityStorage.Create();
        }

        /// <summary>
        /// Tests <see cref="GetTestDocuments"/> implementations
        /// </summary>
        [TestMethod]
        public void TestGetTestDocuments()
        {
            foreach (var document in GetTestDocuments())
            {
                Console.WriteLine("Document {0} {1}", document.Item1, document.Item2);
                Assert.IsNotNull(document);
                Assert.IsNotNull(document.Item1);
                TestHelpers.AssertIsValidUrl(document.Item2);
            }
        }
    }

    /// <summary>
    /// Just a primitive mock of <see cref="IWebSource"/> implementation used for testing
    /// </summary>
    internal class TestWebSource : IWebSource
    {
        public TestWebSource(string url)
        {
            Url = url;
        }

        #region IWebSource Members

        public long Id
        {
            get { return 111; }
        }

        public string Url { get; private set; }

        public DateTime Scouted
        {
            get { return DateTime.Now; }
        }

        public DateTime? Processed { get; set; }

        public int ProcessingResult { get; set; }

        #endregion
    }
}