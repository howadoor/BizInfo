using System;

namespace Perenis.Core
{
    /// <summary>
    /// <see cref="Guid"/> manipulation helpers.
    /// </summary>
    public static class GuidEx
    {
        /// <summary>
        /// Determines if the given string is a valid GUID.
        /// </summary>
        /// <param name="s">The string to be tested.</param>
        /// <returns><c>true</c> when <paramref name="s"/> denotes a valid GUID; otherwise, <c>false</c>.</returns>
        public static bool IsGuid(string s)
        {
            return TryParse(s) != null;
        }

        /// <summary>
        /// Tries to parse the supplied string as a GUID.
        /// </summary>
        /// <param name="s">The string to be tested.</param>
        /// <returns>A <see cref="Guid"/> instance or a null reference when the given string is not a valid GUID.</returns>
        public static Guid? TryParse(string s)
        {
            if (String.IsNullOrEmpty(s)) return null;
            try
            {
                return new Guid(s);
            }
            catch (FormatException)
            {
                // there's nothing like a native Guid.TryParse method, so catching FormatException is a quite acceptable solution
                return null;
            }
        }
    }
}