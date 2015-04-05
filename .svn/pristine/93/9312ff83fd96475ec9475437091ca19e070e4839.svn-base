using System;
using System.IO;
using System.Linq;
using BizInfo.Core.Services.WebResources.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Perenis.Testing.Tools;

namespace BizInfo.Core.Services.Tests.Tests.WebResources.Storage
{
    /// <summary>
    /// Tests <see cref="WebResourcesRepository"/>
    /// </summary>
    [TestClass]
    public class WebResourcesRepositoryTest : WebResourcesRepositoryTestBase<WebResourcesRepository>
    {
        public override void DisposeInstance(WebResourcesRepository instance)
        {
            instance.DestroyRepository();
            var folder = instance.RepositoryFolder;
            Assert.IsFalse(Directory.Exists(folder));
        }

        public override WebResourcesRepository CreateInstance()
        {
            return new WebResourcesRepository(GetNewRepositoryFolder());
        }

        /// <summary>
        /// Returns new folder for the repository (creates new one with unique name in temporrary folder)
        /// </summary>
        /// <returns></returns>
        private string GetNewRepositoryFolder()
        {
            var folder = Path.Combine(Path.GetTempPath(), string.Format("repository {0}", DateTime.Now.Ticks));
            Assert.IsFalse(Directory.Exists(folder));
            Directory.CreateDirectory(folder);
            Assert.IsTrue(Directory.Exists(folder));
            return folder;
        }

        /// <summary>
        /// Creates repository then opens new one on its data. Tests if all data are read properly from the new repository.
        /// </summary>
        [TestMethod]
        public void TestOpeningExistingRepository()
        {
            PerformTest<WebResourcesRepository>(TestOpeningExistingRepository);
        }

        /// <summary>
        /// Creates repository then opens new one on its data. Tests if all data are read properly from the new repository.
        /// </summary>
        /// <param name="repository">First repository (to be created)</param>
        private void TestOpeningExistingRepository(WebResourcesRepository repository)
        {
            TestCreatingResourcesAndVersions(repository);
            var openedRepository = new WebResourcesRepository(repository.RepositoryFolder);
            var ids = repository.StoredResourceIds.ToArray();
            var openedIds = openedRepository.StoredResourceIds.ToArray();
            TestHelpers.AssertAreEqual(ids, openedIds);
        }
    }
}