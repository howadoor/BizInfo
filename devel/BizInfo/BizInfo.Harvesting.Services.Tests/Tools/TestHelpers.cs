using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BizInfo.Harvesting.Services.Tests.Tools
{
    public static class TestHelpers
    {
        /// <summary>
        /// Checks if <see cref="urls"/> are valid absolute non-empty http or https URL.
        /// </summary>
        /// <param name="url"></param>
        public static void AssertIsValidUrl(params string [] urls)
        {
            foreach (var url in urls)
            {
                Assert.IsFalse(string.IsNullOrEmpty(url));
                var uri = new Uri(url, UriKind.Absolute);
                Assert.IsNotNull(uri);
                Assert.IsTrue(uri.IsAbsoluteUri);
                Assert.IsTrue(uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
            }
        }
    }
}