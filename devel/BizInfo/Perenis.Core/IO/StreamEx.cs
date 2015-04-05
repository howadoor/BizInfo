using System.IO;

namespace Perenis.Core.IO
{
    public static class StreamEx
    {
        /// <summary>
        /// Copies content of source stream from current position up to the end of stream  to target
        /// </summary>
        /// <returns>Number of bytes copied</returns>
        public static long CopyTo(this Stream source, Stream target)
        {
            const int bufSize = 0x1000;
            long copied = 0;
            var buf = new byte[bufSize];
            int bytesRead = 0;
            while ((bytesRead = source.Read(buf, 0, bufSize)) > 0)
            {
                target.Write(buf, 0, bytesRead);
                copied += bytesRead;
            }
            return copied;
        }
    }
}