using System;
using BizInfo.Harvesting.Services.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Perenis.Core.Serialization;

namespace BizInfo.Harvesting.Services.Tests
{
    [TestClass]
    public class BlobStorageManifestTest
    {
        [TestMethod]
        public void TestManifest()
        {
            var manifest = CreateManifest();
            var xml = manifest.ToXmlStringByDataContractSerializer();
            var deserialized = XmlSerialization.FromXmlStringByDataContractSerializer<BlobsStorageManifest>(xml);
        }

        private BlobsStorageManifest CreateManifest()
        {
            var manifest = new BlobsStorageManifest();
            var blobFile = new BlobFile {BlobFilename = "thefile.1", StoredTime = DateTime.Now};
            blobFile.ExtraInfo["scouted"] = DateTime.Now;
            blobFile.ExtraInfo["extra"] = "koko";
            manifest.AddBlobFile("http://someurl.xxx", blobFile);
            return manifest;
        }
    }
}