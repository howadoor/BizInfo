using System.IO;
using Ionic.Zip;
using Ionic.Zlib;

namespace BizInfo.Harvesting.Services.Core
{
    public static class ZipHelpers
    {
        public static void Zip ( string zipFileName, byte [] content, string contentFilename)
        {
            using (var zip = new ZipFile(zipFileName) {CompressionLevel = CompressionLevel.BestCompression})
            {
                if (zip.ContainsEntry(contentFilename)) zip.RemoveEntry(contentFilename);
                zip.AddEntry(contentFilename, content);
                zip.Save();
            }
        }

        public static void UnZip(string zipFileName, string contentFileName, out byte [] content, out int length)
        {
            using (var zip = new ZipFile(zipFileName))
            {
                var entry = zip[contentFileName];
                length = (int) entry.UncompressedSize;
                content = new byte[length];
                using (var memStream = new MemoryStream(content))
                {
                    entry.Extract(memStream);
                }
            }
        }
    }
}