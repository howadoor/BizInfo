using System;
using System.IO;

namespace BizInfo.Harvesting.Services.Core
{
    public class BlobsStorage
    {
        public Predicate<string> CompressionTest = DefaulCompressionTest;

        public string RootPath { get; protected set; }

        public BlobsStorage(string rootPath)
        {
            RootPath = rootPath;
        }
        
        private static bool DefaulCompressionTest(string extension)
        {
            switch (extension)
            {
                case "jpeg":
                case "jpg":
                case "png":
                case "gif":
                    return false;
                default:
                    return true;
            }
        }

        public void Store(int blobId, string extension, byte[] content, int length)
        {
            var targetPath = GetBlobTargetPath(blobId, true);
            var targetFileName = GetBlobFilename(blobId);
            if (!CompressionTest(extension))
            {
                targetFileName = string.Format("{0}.{1}", targetFileName, extension);
                using (var file = File.OpenWrite(Path.Combine(targetPath, targetFileName)))
                {
                    file.Write(content, 0, length);
                }
            }
            else
            {
                var targetEntryName = string.Format("{0}.{1}", targetFileName, extension);
                var targetZipFileName = string.Format("{0}.{1}.zip", targetFileName, extension);
                ZipHelpers.Zip(Path.Combine(targetPath, targetZipFileName), content, targetEntryName);
            }
        }

        public bool TryRead(int blobId, string extension, out byte[] content, out int length)
        {
            content = null;
            length = 0;
            var targetPath = GetBlobTargetPath(blobId);
            var targetFileName = GetBlobFilename(blobId);
            if (!CompressionTest(extension))
            {
                targetFileName = Path.Combine(RootPath, Path.Combine(targetPath, string.Format("{0}.{1}", targetFileName, extension)));
                if (!File.Exists(targetFileName)) return false;
                using (var sourceFile = File.OpenRead(targetFileName))
                {
                    length = (int)sourceFile.Length;
                    content = new byte[length];
                    sourceFile.Read(content, 0, length);
                    return true;
                }
            }
            else
            {
                var targetZipFileName = Path.Combine(RootPath, Path.Combine(targetPath, string.Format("{0}.{1}.zip", targetFileName, extension)));
                if (!File.Exists(targetZipFileName)) return false;
                var targetEntryName = string.Format("{0}.{1}", targetFileName, extension);
                ZipHelpers.UnZip(targetZipFileName, targetEntryName, out content, out length);
                return true;
            }
        }

        public bool Exists(int blobId, string extension)
        {
            var targetPath = GetBlobTargetPath(blobId);
            var targetFileName = GetBlobFilename(blobId);
            if (!CompressionTest(extension))
            {
                targetFileName = string.Format("{0}.{1}", targetFileName, extension);
                return File.Exists(Path.Combine(targetPath, targetFileName));
            }
            else
            {
                var targetZipFileName = string.Format("{0}.{1}.zip", targetFileName, extension);
                return File.Exists(Path.Combine(targetPath, targetZipFileName));
            }
        }

        private string GetBlobTargetPath(int blobId, bool create = false)
        {
            var mega = blobId/1000000;
            var kilo = blobId/1000;
            var megaFolder = Path.Combine(RootPath, string.Format("{0}-{1}", mega*10000000, (mega + 1)*10000000 - 1));
            if (create) Directory.CreateDirectory(megaFolder);
            var kiloFolder = Path.Combine(megaFolder, string.Format("{0}-{1}", kilo * 1000, (kilo + 1) * 1000 - 1));
            if (create) Directory.CreateDirectory(kiloFolder);
            return kiloFolder;
        }

        private string GetBlobFilename(int blobId)
        {
            return blobId.ToString();
        }

        public bool TryRemove(int blobId, string extension)
        {
            var targetPath = GetBlobTargetPath(blobId);
            var targetFileName = GetBlobFilename(blobId);
            string fileToDelete;
            if (!CompressionTest(extension))
            {
                targetFileName = string.Format("{0}.{1}", targetFileName, extension);
                fileToDelete = Path.Combine(targetPath, targetFileName);
            }
            else
            {
                var targetZipFileName = string.Format("{0}.{1}.zip", targetFileName, extension);
                fileToDelete = Path.Combine(targetPath, targetZipFileName);
            }
            if (!File.Exists(fileToDelete)) return false;
            File.Delete(fileToDelete);
            return true;
        }
    }
}