using BizInfo.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Perenis.Core.Serialization;
using Perenis.Testing.Objects;

namespace BizInfo.WebApp.MVC3.Tests
{
    [TestClass]
    public class XmlSerializationTestBase<TInstance> : TestOnInstance<TInstance> where TInstance : class, new()
    {
        [TestMethod]
        public void TestGuidHashCreation()
        {
            PerformTest<TInstance>(TestGuidHashCreation);
        }

        public static void TestGuidHashCreation(TInstance instance)
        {
            Assert.IsNotNull(instance);
            var guid = XmlSerialization.HashToGuid(instance);
            Assert.IsNotNull(guid);
            var guid2 = XmlSerialization.HashToGuid(instance);
            Assert.IsNotNull(guid2);
            Assert.AreEqual(guid, guid2);
        }

        [TestMethod]
        public void TestDataContractSerialization()
        {
            PerformTest<TInstance>(TestDataContractSerialization);
        }

        public static void TestDataContractSerialization(TInstance instance)
        {
            var xmlString = XmlSerialization.ToXmlStringByDataContractSerializer(instance);
            Assert.IsFalse(string.IsNullOrEmpty(xmlString));
            var deserialized = XmlSerialization.FromXmlStringByDataContractSerializer<TInstance>(xmlString);
            Assert.IsNotNull(deserialized);
            var xmlString2 = XmlSerialization.ToXmlStringByDataContractSerializer(deserialized);
            Assert.IsTrue(xmlString.Equals(xmlString2));
        }

        public override void DisposeInstance(TInstance instance)
        {
            // intentionaly does nothing;
        }

        public override TInstance CreateInstance()
        {
            return new TInstance();
        }
    }
}