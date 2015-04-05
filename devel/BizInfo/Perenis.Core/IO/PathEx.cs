using System;
using System.IO;
using System.Text;

#if NUNIT
using NUnit.Framework;
#endif

namespace Perenis.Core.IO
{
    /// <summary>
    /// Path manipulation helpers.
    /// </summary>
    public static class PathEx
    {
        /// <summary>
        /// Checks if the given character is a directory separator character.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsDirectorySeparatorChar(char c)
        {
            return c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar;
        }

        /// <summary>
        /// Combines the given path, file name, and extension.
        /// </summary>
        /// <param name="path">Path portion of the whole name.</param>
        /// <param name="filename">File name portion of the whole name.</param>
        /// <param name="extension">Extension portion of the whole name.</param>
        /// <returns>The combined name in the <c>path/name.ext</c> form.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is a null reference.</exception>
        /// <exception cref="ArgumentException"><paramref name="path"/> is an empty string.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="filename"/> is a null reference.</exception>
        /// <exception cref="ArgumentException"><paramref name="filename"/> is an empty string.</exception>
        public static string Combine(string path, string filename, string extension)
        {
            if (path == null) throw new ArgumentNullException("path");
            if ((path != null && String.IsNullOrEmpty(path))) throw new ArgumentException("path");
            if (filename == null) throw new ArgumentNullException("filename");
            if ((filename != null && String.IsNullOrEmpty(filename))) throw new ArgumentException("filename");

            var sb = new StringBuilder(path);
            if (!IsDirectorySeparatorChar(sb[sb.Length - 1])) sb.Append(Path.DirectorySeparatorChar);
            sb.Append(filename);
            if (!String.IsNullOrEmpty(extension))
            {
                if (sb[sb.Length - 1] == '.' && extension[0] == '.') sb.Length -= 1;
                else if (sb[sb.Length - 1] != '.' && extension[0] != '.') sb.Append('.');
                sb.Append(extension);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Combines the given file name and extension.
        /// </summary>
        /// <param name="filename">File name portion of the whole name.</param>
        /// <param name="extension">Extension portion of the whole name.</param>
        /// <returns>The combined name in the <c>name.ext</c> form.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="filename"/> is a null reference.</exception>
        /// <exception cref="ArgumentException"><paramref name="filename"/> is an empty string.</exception>
        public static string CombineNameAndExtension(string filename, string extension)
        {
            if (filename == null) throw new ArgumentNullException("filename");
            if ((filename != null && String.IsNullOrEmpty(filename))) throw new ArgumentException("filename");

            var sb = new StringBuilder(filename);
            if (!String.IsNullOrEmpty(extension))
            {
                if (sb[sb.Length - 1] == '.' && extension[0] == '.') sb.Length -= 1;
                else if (sb[sb.Length - 1] != '.' && extension[0] != '.') sb.Append('.');
                sb.Append(extension);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Checks if the filename has the specified extension.
        /// </summary>
        /// <param name="filename">File name.</param>
        /// <param name="extension">Extension to be checked for.</param>
        /// <returns><c>true</c> if <paramref name="filename"/> has the given <paramref name="extension"/>; 
        /// otherwise, <c>false</c>.</returns>
        public static bool HasExtension(string filename, string extension)
        {
            if (filename == null) throw new ArgumentNullException("filename");
            if ((filename != null && String.IsNullOrEmpty(filename))) throw new ArgumentException("filename");
            if (extension == null) throw new ArgumentNullException("extension");
            if ((extension != null && String.IsNullOrEmpty(extension))) throw new ArgumentException("extension");

            string currentExtension = Path.GetExtension(filename);
            if (extension[0] == '.')
                return currentExtension == extension;
            else
                return currentExtension.Length - 1 == extension.Length && currentExtension.EndsWith(extension);
        }

        /// <summary>
        /// Checks if the given file name has the provided extension, and if not appends the extension
        /// to the file name.
        /// </summary>
        /// <param name="filename">File name.</param>
        /// <param name="extension">Required extension.</param>
        /// <returns>The <paramref name="filename"/> optionally extended with the given <paramref name="extension"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="filename"/> is a null reference.</exception>
        /// <exception cref="ArgumentException"><paramref name="filename"/> is an empty string.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="extension"/> is a null reference.</exception>
        /// <exception cref="ArgumentException"><paramref name="extension"/> is an empty string.</exception>
        public static string EnsureExtension(string filename, string extension)
        {
            if (filename == null) throw new ArgumentNullException("filename");
            if ((filename != null && String.IsNullOrEmpty(filename))) throw new ArgumentException("filename");
            if (extension == null) throw new ArgumentNullException("extension");
            if ((extension != null && String.IsNullOrEmpty(extension))) throw new ArgumentException("extension");

            if (HasExtension(filename, extension))
                return filename;
            else
                return CombineNameAndExtension(filename, extension);
        }

        /// <summary>
        /// Builds the <c>*.ext</c> mask from the given <paramref name="extension"/>.
        /// </summary>
        /// <param name="extension">The extension to be used in the mask.</param>
        /// <returns><c>*.ext</c> or just <c>*</c> when <paramref name="extension"/> is null or an empty string.</returns>
        public static string GetAllMask(string extension)
        {
            return CombineNameAndExtension("*", extension);
        }

        /// <summary>
        /// Builds the <c>prefix*.ext</c> mask from the given <paramref name="extension"/> and <paramref name="prefix"/>.
        /// </summary>
        /// <param name="extension">The extension to be used in the mask.</param>
        /// <param name="prefix">The prefix of the mask.</param>
        /// <returns><c>prefix*.ext</c> or just <c>prefix*</c> when <paramref name="extension"/> is null or an empty string.</returns>
        public static string GetAllMask(string prefix, string extension)
        {
            return CombineNameAndExtension(String.Format("{0}*", prefix), extension);
        }
    }
}