using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using BizInfo.App.Services.Tools;
using BizInfo.Model.Entities;
using BizInfo.WebApp.MVC3.Models.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Perenis.Core.Serialization;

namespace BizInfo.WebApp.MVC3.Tests
{
    [TestClass]
    public class SearchingCriteriaModelXmlSerializationTest : XmlSerializationTestBase<SearchingCriteriaModel>
    {
        [TestMethod]
        public void TestXmlSerialization()
        {
            TestXmlSerialization(new SearchingCriteriaModel());
        }

        public static void TestXmlSerialization(SearchingCriteriaModel sourceModel)
        {
            var xmlString = XmlSerialization.ToXmlString(sourceModel);
            Assert.IsFalse(string.IsNullOrEmpty(xmlString));
            var deserialized = XmlSerialization.FromXmlString<SearchingCriteriaModel>(xmlString);
            Assert.IsNotNull(deserialized);
            var xmlString2 = XmlSerialization.ToXmlString(deserialized);
            Assert.IsTrue(xmlString.Equals(xmlString2));
        }
    }
}
