using System;
using System.IO;
using System.Linq;
using BizInfo.Core.Services.WebResources.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Perenis.Core.Serialization;

namespace BizInfo.Core.Services.Tests.Tests.Tools
{
    /// <summary>
    /// Tests <see cref="StoredResourceManifest"/> class
    /// </summary>
    [TestClass]
    public class StoredResourceManifestTest
    {
        /// <summary>
        /// Tests XML serialization and deserialization
        /// </summary>
        [TestMethod]
        public void TestXmlSerialization()
        {
            var manifest = ResourceRepositoryTestHelpers.CreateRichManifest();
            ResourceRepositoryTestHelpers.AssertIsRich(manifest);
            var xmlString = XmlSerialization.ToXmlStringByDataContractSerializer(manifest);
            Assert.IsFalse(string.IsNullOrEmpty(xmlString));
            var deserializedManifest = XmlSerialization.FromXmlStringByDataContractSerializer<StoredResourceManifest>(xmlString);
            ResourceRepositoryTestHelpers.AssertIsRich(deserializedManifest);
            ResourceRepositoryTestHelpers.AssertEquals(manifest, deserializedManifest);
            using (var stream = new MemoryStream())
            {
                XmlSerialization.ToXml(manifest, stream);
                stream.Seek(0, SeekOrigin.Begin);
                deserializedManifest = XmlSerialization.FromXml<StoredResourceManifest>(stream);
            }
            ResourceRepositoryTestHelpers.AssertIsRich(deserializedManifest);
            ResourceRepositoryTestHelpers.AssertEquals(manifest, deserializedManifest);
        }
    }
}