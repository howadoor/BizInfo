using System;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Resources;

namespace Perenis.Core.Reflection
{
    /// <summary>
    /// Collects extension methods for <see cref="Assembly"/> class
    /// </summary>
    public static class AssemblyEx
    {
        /// <summary>
        /// Returns  <see cref="StreamResourceInfo"/> of the stream determined by <see cref="resourcePathName"/> from given <see cref="assembly"/>. Just a shortcut to encapsulate
        /// <see cref="Uri"/> creation and packing conventions.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="resourcePathName"></param>
        /// <returns></returns>
        public static StreamResourceInfo GetResourceStream(this Assembly assembly, string resourcePathName)
        {
            var resourceLocation = string.Format("pack://application:,,,/{0};component/{1}", assembly.GetName().Name, resourcePathName);
            var uriResource = new Uri(resourceLocation);
            return Application.GetResourceStream(uriResource);
        }

        public static string GetManifestResourceString(this Assembly assembly, string resourcePathName, Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.ASCII;
            using (var resourceStream = assembly.GetManifestResourceStream(resourcePathName))
            {
                var bytes = new byte[resourceStream.Length];
                resourceStream.Read(bytes, 0, (int) resourceStream.Length);
                return encoding.GetString(bytes);
            }
        }
    }
}