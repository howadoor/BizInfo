using System;
using System.Collections.Generic;
using System.Linq;
using BizInfo.Core.Services.WebResources.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BizInfo.Core.Services.Tests.Tests.Tools
{
    /// <summary>
    /// Collects static helper methods for tests related to web resource repository
    /// </summary>
    internal static class ResourceRepositoryTestHelpers
    {
        /// <summary>
        /// Creates some random and unique URL (http)
        /// </summary>
        /// <returns>New URL</returns>
        public static string CreateUrl()
        {
            return string.Format("http://www.nothing.xxx/{0}", Guid.NewGuid());
        }

        /// <summary>
        /// Create instance of <see cref="ResourceProperties"/> with rich content (some properties)
        /// </summary>
        /// <returns></returns>
        public static ResourceProperties CreateRichProperties()
        {
            var properties = new ResourceProperties();
            properties["ref"] = Guid.NewGuid();
            properties["koko"] = Guid.NewGuid();
            properties[Guid.NewGuid()] = Guid.NewGuid().ToString();
            return properties;
        }

        /// <summary>
        /// Checks manifest has rich set of internal content
        /// </summary>
        /// <param name="manifest">Manifest to be tested</param>
        public static void AssertIsRich(StoredResourceManifest manifest)
        {
            Assert.IsNotNull(manifest);
            Assert.IsFalse(string.IsNullOrEmpty(manifest.Url));
            Assert.IsNotNull(manifest.Properties);
            Assert.IsTrue(manifest.Properties.Count > 0);
            Assert.IsNotNull(manifest.Versions);
            Assert.IsTrue(manifest.Versions.Count > 0);
            foreach (var version in manifest.Versions)
            {
                AssertIsRich(version);
            }
        }

        /// <summary>
        /// Checks manifest has the same content
        /// </summary>
        /// <param name="manifest">First manifest</param>
        /// <param name="manifest2">Second manifest</param>
        public static void AssertEquals(StoredResourceManifest manifest, StoredResourceManifest manifest2)
        {
            Assert.AreEqual(manifest.Url, manifest2.Url);
            AssertEquals(manifest.Properties, manifest2.Properties);
            Assert.AreEqual(manifest.Versions.Count, manifest2.Versions.Count);
            foreach (var version in manifest.Versions)
            {
                var version2 = manifest2.Versions.Where(vs => vs.Filename.Equals(version.Filename)).Single();
                AssertIsRich(version2);
                AssertEquals(version, version2);
            }
        }

        /// <summary>
        /// Checks if two instances of <see cref="ResourceProperties"/> has the same content
        /// </summary>
        /// <param name="properties">First instance</param>
        /// <param name="properties2">Second instance</param>
        public static void AssertEquals(ResourceProperties properties, ResourceProperties properties2)
        {
            Assert.AreEqual(properties.Count, properties2.Count);
            foreach (var property in properties)
            {
                object property2value;
                Assert.IsTrue(properties2.TryGetValue(property.Key, out property2value));
                Assert.AreEqual(property.Value, property2value);
            }
        }

        /// <summary>
        /// Checks if two instances of <see cref="ResourceVersionManifest"/> has the same content
        /// </summary>
        /// <param name="version">First instance</param>
        /// <param name="version2">Second instance</param>
        public static void AssertEquals(ResourceVersionManifest version, ResourceVersionManifest version2)
        {
            Assert.AreEqual(version.Filename, version2.Filename);
            AssertEquals(version.Properties, version2.Properties);
            Assert.AreEqual(version.CreationTime, version2.CreationTime);
        }

        /// <summary>
        /// Checks if <see cref="version"/> has all internal data fields filled
        /// </summary>
        /// <param name="version">Version to be checked</param>
        public static void AssertIsRich(ResourceVersionManifest version)
        {
            Assert.IsNotNull(version);
            Assert.IsTrue(version.Properties.Count > 0);
            Assert.IsFalse(string.IsNullOrEmpty(version.Filename));
            Assert.IsFalse(version.CreationTime > DateTime.Now);
        }

        /// <summary>
        /// Creates new instance of <see cref="StoredResourceManifest"/> and fill its members with full of data
        /// </summary>
        /// <returns></returns>
        public static StoredResourceManifest CreateRichManifest()
        {
            var url = ResourceRepositoryTestHelpers.CreateUrl();
            var manifest = new StoredResourceManifest { Url = url, Properties = ResourceRepositoryTestHelpers.CreateRichProperties() };
            for (int i = 0; i < 8; i++)
            {
                var version = new ResourceVersionManifest { Filename = Guid.NewGuid().ToString(), CreationTime = DateTime.Now, Properties = CreateRichProperties() };
                manifest.Versions.Add(version);
            }
            return manifest;
        }

        /// <summary>
        /// Returns <see cref="count"/> random resource identifiers
        /// </summary>
        /// <param name="count">Count of identifiers</param>
        /// <returns>Sequence of random resource identifiers</returns>
        public static IEnumerable<long> GetRandomResourceIds(int count)
        {
            var random = new Random();
            for (int i = 0; i < count; i++)
            {
                yield return random.Next();
            }
        }

        /// <summary>
        /// Checks two versions equality
        /// </summary>
        /// <param name="version1">First version</param>
        /// <param name="version2">Second version</param>
        public static void AssertEquals(IStoredResourceVersion version1, IStoredResourceVersion version2)
        {
            Assert.AreEqual(version1.CreationTime, version2.CreationTime);
            AssertEquals(version1.Properties, version2.Properties);
        }
    }
}