using System;

namespace Perenis.Core.General
{
    /// <summary>
    /// Exposes static methods retreieving informations about platform where is running
    /// </summary>
    public static class PlatformInfo
    {
        /// <summary>
        /// Returns <c>true</c> when running on 32-bit platform
        /// </summary>
        /// <returns></returns>
        public static bool Is32BitPlatform()
        {
            return IntPtr.Size == 4;
        }

        /// <summary>
        /// Returns <c>true</c> when running on 64-bit platform
        /// </summary>
        /// <returns></returns>
        public static bool Is64BitPlatform()
        {
            return IntPtr.Size == 8;
        }
    }
}