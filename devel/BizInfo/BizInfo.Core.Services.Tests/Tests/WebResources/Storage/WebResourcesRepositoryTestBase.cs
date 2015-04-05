using System;
using System.IO;
using System.Linq;
using BizInfo.Core.Services.Tests.Tests.Tools;
using BizInfo.Core.Services.WebResources.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Perenis.Testing.Objects;
using Perenis.Testing.Tools;

namespace BizInfo.Core.Services.Tests.Tests.WebResources.Storage
{
    /// <summary>
    /// Abstract base class for tests of <see cref="IWebResourcesRepository"/>. Descendants must define methods for creating and disposing instance of repository.
    /// </summary>
    public abstract class WebResourcesRepositoryTestBase<TRepository> : TestOnInstance<TRepository> where TRepository : class, IWebResourcesRepository
    {
        /// <summary>
        /// Tests if resources which should not exist in the repository really does not exists
        /// </summary>
        [TestMethod]
        public void TestNotExistingResources()
        {
            PerformTest<IWebResourcesRepository>(TestNotExistingResources);
        }

        /// <summary>
        /// Tests if resources which should not exist in the <see cref="repository"/> really does not exists. Expected completely empty <see cref="repository"/> containing no resources at all.
        /// </summary>
        /// <param name="repository">The instance of repository being tested</param>
        protected void TestNotExistingResources(IWebResourcesRepository repository)
        {
            foreach (var resourceId in ResourceRepositoryTestHelpers.GetRandomResourceIds(128))
            {
                Assert.IsFalse(repository.ResourceExists(resourceId));
                IStoredResource resource;
                Assert.IsFalse(repository.TryGetResource(resourceId, out resource));
                Assert.IsNull(resource);
                TestHelpers.AssertThrows<Exception>(() => { var rs = repository[resourceId]; });
            }
        }
        
        /// <summary>
        /// Tests resources creation
        /// </summary>
        [TestMethod]
        public void TestCreatingResources()
        {
            PerformTest<IWebResourcesRepository>(repository => TestCreatingResources(repository));
        }

        /// <summary>
        /// Tests creation of resources and versions in it
        /// </summary>
        [TestMethod]
        public void TestCreatingResourcesAndVersions()
        {
            PerformTest<IWebResourcesRepository>(TestCreatingResourcesAndVersions);
        }

        /// <summary>
        /// Tests <see cref="StoredResourceEx.GetNewestVersion"/> method
        /// </summary>
        [TestMethod]
        public void TestGetNewestVersion()
        {
            PerformTest<IWebResourcesRepository>(TestGetNewestVersion);
        }

        /// <summary>
        /// Tests <see cref="StoredResourceEx.GetNewestVersion"/> method
        /// </summary>
        /// <param name="repository">Repository where test will be performed</param>
        private void TestGetNewestVersion(IWebResourcesRepository repository)
        {
            var resource = TestCreatingResource(13, repository);
            TestCreatingVersions(resource);
            Assert.IsTrue(resource.Versions.Count() > 0);
            var random = new Random();
            var length = random.Next(256 * 1024);
            var bytes = new byte[length];
            random.NextBytes(bytes);
            var properties = ResourceRepositoryTestHelpers.CreateRichProperties();
            var creationTime = DateTime.Now + TimeSpan.FromDays(1);
            IStoredResourceVersion newestVersion;
            using (var stream = new MemoryStream(bytes))
            {
                newestVersion = resource.AddVersion(creationTime, stream, properties);
                Assert.IsNotNull(newestVersion);
            }
            var newestVersionFound = resource.GetNewestVersion();
            ResourceRepositoryTestHelpers.AssertEquals(newestVersion, newestVersionFound);
        }

        /// <summary>
        /// Tests creation of resources and versions in it
        /// </summary>
        /// <param name="repository">Repository</param>
        protected void TestCreatingResourcesAndVersions(IWebResourcesRepository repository)
        {
            foreach (var resource in TestCreatingResources(repository)) TestCreatingVersions(resource); 
        }

        /// <summary>
        /// Tests creation of some versions in <see cref="resource"/>. Expects that <see cref="resource"/> is empty on start, without no existing versions.
        /// </summary>
        /// <param name="resource"></param>
        private void TestCreatingVersions(IStoredResource resource)
        {
            Assert.IsTrue(resource.Versions.Count() == 0);
            for (int i = 0; i < 16; i++)
            {
                TestCreateVersion(resource);
            }
            Assert.IsTrue(resource.Versions.Count() == 16);
        }

        /// <summary>
        /// Tests creation of one version in <see cref="resource"/>
        /// </summary>
        /// <param name="resource"></param>
        private void TestCreateVersion(IStoredResource resource)
        {
            var random = new Random();
            var length = random.Next(256*1024);
            var bytes = new byte[length];
            random.NextBytes(bytes);
            var properties = ResourceRepositoryTestHelpers.CreateRichProperties();
            var creationTime = DateTime.Now;
            using (var stream = new MemoryStream(bytes))
            {
                var version = resource.AddVersion(creationTime, stream, properties);
                Assert.IsNotNull(version);
            }
        }


        /// <summary>
        /// Tests resources creation. Expected completely empty <see cref="repository"/> containing no resources at all.
        /// </summary>
        /// <param name="repository">The instance of repository being tested</param>
        protected IStoredResource[] TestCreatingResources(IWebResourcesRepository repository)
        {
            var resources = ResourceRepositoryTestHelpers.GetRandomResourceIds(128).Select(resourceId => TestCreatingResource(resourceId, repository)).ToArray();
            var resourceIds = resources.Select(resource => resource.Id).ToArray();
            TestHelpers.AssertAreEqual(resourceIds, repository.StoredResourceIds.ToArray());
            return resources;
        }

        /// <summary>
        /// Creates one resource in the repository
        /// </summary>
        /// <param name="resourceId">Identifier of the resource</param>
        /// <param name="repository">Repository where the resource will be created</param>
        private IStoredResource TestCreatingResource(long resourceId, IWebResourcesRepository repository)
        {
            var url = ResourceRepositoryTestHelpers.CreateUrl();
            var properties = ResourceRepositoryTestHelpers.CreateRichProperties();
            var resource = repository.CreateResource(resourceId, url, properties);
            Assert.IsNotNull(resource);
            Assert.AreEqual(resource.Url, url);
            ResourceRepositoryTestHelpers.AssertEquals(properties, resource.Properties);
            Assert.IsTrue(resource.Versions.Count() == 0);
            // check new creation of resource with the same id
            TestHelpers.AssertThrows<Exception>(() => repository.CreateResource(resourceId, url, properties));
            return resource;
        }
    }
}