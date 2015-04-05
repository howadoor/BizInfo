using System;
using System.Text;

namespace Perenis.Core
{
    /// <summary>
    /// String manipulation helpers.
    /// </summary>
    public static class StringEx
    {
        /// <summary>
        /// Throws if the given string is a null reference or an empty string.
        /// </summary>
        /// <param name="s">The string to be checked.</param>
        /// <param name="paramName">The original parameter's name.</param>
        /// <exception cref="ArgumentNullException"><paramref name="s"/> is a null reference.</exception>
        /// <exception cref="ArgumentException"><paramref name="s"/> is an empty string.</exception>
        public static void ThrowIfIsNullOrEmpty(string s, string paramName)
        {
            if (s == null) throw new ArgumentNullException(paramName);
            if ((s != null && String.IsNullOrEmpty(s))) throw new ArgumentException(paramName);
        }

        /// <summary>
        /// Throws if the given string is a null reference or an empty string after trimming leading
        /// and trailing spaces.
        /// </summary>
        /// <param name="s">The string to be checked and trimmed.</param>
        /// <param name="paramName">The original parameter's name.</param>
        /// <exception cref="ArgumentNullException"><paramref name="s"/> is a null reference.</exception>
        /// <exception cref="ArgumentException"><paramref name="s"/> is an empty string.</exception>
        public static void ThrowIfIsNullOrTrimmedEmpty(ref string s, string paramName)
        {
            if (s == null) throw new ArgumentNullException(paramName);
            s = s.Trim();
            if ((s != null && String.IsNullOrEmpty(s))) throw new ArgumentException(paramName);
        }

        /// <summary>
        /// Indicates whether the specified string is a null reference or an empty string after 
        /// trimming leading and trailing spaces.
        /// </summary>
        /// <param name="s">The string to be checked.</param>
        /// <returns><c>true</c> if the <paramref name="s"/> is null reference or an empty string ("") 
        /// after trimming the lading and trailing spaces; otherwise, <c>false</c>.</returns>
        public static bool IsNullOrTrimmedEmpty(string s)
        {
            if (s == null) return true;
            if ((s.Trim() != null && String.IsNullOrEmpty(s.Trim()))) return true;
            return false;
        }

        /// <summary>
        /// Wraps the given string to lines of a maximum <paramref name="lineLength"/> length separated by CR LF.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="lineLength"></param>
        /// <returns></returns>
        public static string Wrap(this string s, int lineLength)
        {
            if (s == null) return null;
            var sb = new StringBuilder(s.Length);
            int remaining = s.Length;
            int index = 0;
            while (remaining > 0)
            {
                sb.Append(s, index, Math.Min(remaining, lineLength));
                remaining -= lineLength;
                index += lineLength;
                if (remaining > 0) sb.Append("\r\n");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Unwraps the given string removing all CR, LF, and space characters.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Unwrap(this string s)
        {
            if (s == null) return s;
            var sb = new StringBuilder(s.Length);
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                if (c != '\r' && c != '\n' && c != ' ') sb.Append(c);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Returns <see cref="string.Empty"/> if <paramref name="s"/> is <c>null</c>, otherwise <paramref name="s"/>.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string EmptyIfNull(this string s)
        {
            return s ?? string.Empty;
        }

        /// <summary>
        /// Returns a null reference if <paramref name="s"/> is an empty string, otherwise <paramref name="s"/>.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string NullIfEmpty(this string s)
        {
            return String.IsNullOrEmpty(s) ? null : s;
        }

        /// <summary>
        /// Returns a null reference if <paramref name="s"/> is an empty string after trimming leading 
        /// and trailing spaces, otherwise <paramref name="s"/>.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string NullIfTrimmedEmpty(this string s)
        {
            return IsNullOrTrimmedEmpty(s) ? null : s;
        }

        /// <summary>
        /// Returns <paramref name="defaultValue"/> is <paramref name="s"/> is null or empty string, otherwise returns <paramref name="s"/>.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string DefaultIfEmptyOrNull(this string s, string defaultValue)
        {
            return string.IsNullOrEmpty(s) ? defaultValue : s;
        }

        /// <summary>
        /// Returns true if string contains more than one line.
        /// </summary>
        /// <param name="@string">tested string</param>
        /// <returns>false for "Koko" or "Koko\r\n", true for "Koko\r\nBobo"</returns>
        /// <remarks>
        /// String containing only one newline substring on the end is NOT multiline
        /// </remarks>
        public static bool IsMultiline(this string @string)
        {
            if (string.IsNullOrEmpty(@string)) return false;
            int newLineIndex = @string.IndexOf(Environment.NewLine);
            return newLineIndex > 0 && newLineIndex < @string.Length - Environment.NewLine.Length - 1;
        }

        /// <summary>
        /// Returns first line on @string (not including newline chars on end) or @string itself
        /// if it is not multiline string
        /// </summary>
        public static string GetFirstLine(this string @string)
        {
            if (string.IsNullOrEmpty(@string)) return @string;
            int newLineIndex = @string.IndexOf(Environment.NewLine);
            return newLineIndex < 0 ? @string : @string.Substring(0, newLineIndex);
        }
    }
}