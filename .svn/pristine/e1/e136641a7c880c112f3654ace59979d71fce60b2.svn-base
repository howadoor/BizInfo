using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ionic.Zip;
using Ionic.Zlib;
using Perenis.Core.Serialization;

namespace BizInfo.Core.Services.WebResources.Storage
{
    /// <summary>
    /// Implementation of <see cref="IStoredResource"/> based on ZIP file
    /// </summary>
    public class StoredResource : IStoredResource
    {
        private List<StoredResourceVersion> versions;
        protected const string manifestFilename = "manifest.xml";

        /// <summary>
        /// Creates new instance and loads its content from existing ZIP file
        /// </summary>
        /// <param name="resourceId">Identifier of the resource</param>
        /// <param name="filename">File name of the ZIP file</param>
        internal StoredResource(long resourceId, string filename)
        {
            if (!File.Exists(filename)) throw new ArgumentException("Resource file does not exists", "filename");
            Id = resourceId;
            Filename = filename;
            using (var zipFile = new ZipFile(filename))
            {
                Manifest = ReadResourceManifest(zipFile);
            }
        }

        /// <summary>
        /// Creates new instance and new ZIP file with the content
        /// </summary>
        /// <param name="resourceId">Identifier of the resource</param>
        /// <param name="filename">Filename of the ZIP file</param>
        /// <param name="url">URL to be associated with teh resource</param>
        /// <param name="properties">Optional properties</param>
        internal StoredResource(long resourceId, string filename, string url, ResourceProperties properties = null)
        {
            if (File.Exists(filename)) throw new ArgumentException("Resource file already exists", "filename");
            Id = resourceId;
            Filename = filename;
            Manifest = new StoredResourceManifest {Url = url, Properties = properties ?? new ResourceProperties()};
            using (var zipFile = new ZipFile(filename) {CompressionLevel = CompressionLevel.BestCompression})
            {
                WriteResourceManifest(Manifest, zipFile);
            }
        }

        protected StoredResourceManifest Manifest { get; private set; }

        /// <summary>
        /// Filename of the ZIP file
        /// </summary>
        protected string Filename { get; private set; }

        #region IStoredResource Members

        public string Url
        {
            get { return Manifest.Url; }
        }

        public ResourceProperties Properties
        {
            get { return Manifest.Properties; }
        }

        public IEnumerable<IStoredResourceVersion> Versions
        {
            get
            {
                return versions ?? (versions =  Manifest.Versions.Select(versionManifest => new StoredResourceVersion(this, versionManifest)).ToList());
            }
        }

        public long Id
        {
            get; private set;
        }

        public IStoredResourceVersion AddVersion(DateTime creationTime, Stream data, ResourceProperties properties)
        {
            using (var zipFile = new ZipFile(Filename))
            {
                var entryName = GetVersionEntryName(Url, Versions.Count());
                zipFile.AddEntry(entryName, data);
                var versionManifest = new ResourceVersionManifest {Filename = entryName, CreationTime = creationTime, Properties = properties};
                Manifest.Versions.Add(versionManifest);
                WriteResourceManifest(Manifest, zipFile);
                var version = new StoredResourceVersion(this, versionManifest);
                versions.Add(version);
                return version;
            }
        }

        /// <summary>
        /// Creates ZIP file entry name for version from url and version index
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="index">Index of the version</param>
        /// <returns>Entry name containing from file name and version index</returns>
        public static string GetVersionEntryName(string url, int index)
        {
            var uri = new Uri(url);
            var path = uri.GetComponents(UriComponents.Path, UriFormat.Unescaped);
            var filename = Path.GetFileName(path);
            return string.IsNullOrEmpty(filename) ? index.ToString() : string.Format("{0} {1}", index, filename);
        }

        #endregion

        /// <summary>
        /// Writes content of the <see cref="manifest"/> to the <see cref="zipFile"/> manifest entry
        /// </summary>
        /// <param name="manifest">Manifest to be written</param>
        /// <param name="zipFile">Target ZIP file</param>
        private static void WriteResourceManifest(StoredResourceManifest manifest, ZipFile zipFile)
        {
            if (zipFile.ContainsEntry(manifestFilename)) zipFile.RemoveEntry(manifestFilename);
            using (var memStream = new MemoryStream())
            {
                XmlSerialization.ToXml(manifest, memStream);
                memStream.Seek(0, SeekOrigin.Begin);
                zipFile.AddEntry(manifestFilename, memStream);
                zipFile.Save();
            }
        }

        /// <summary>
        /// Reads resource manifest from the zip file 
        /// </summary>
        /// <param name="zipFile">Zip file from which the manistest will be read</param>
        /// <returns>New instance of <see cref="StoredResourceManifest"/></returns>
        private static StoredResourceManifest ReadResourceManifest(ZipFile zipFile)
        {
            var manifestEntry = zipFile[manifestFilename];
            using (var memStream = new MemoryStream((int) manifestEntry.UncompressedSize))
            {
                manifestEntry.Extract(memStream);
                return XmlSerialization.FromXml<StoredResourceManifest>(memStream);
            }
        }
    }

    /// <summary>
    /// Implementation of <see cref="IStoredResourceVersion"/> for internal use in <see cref="StoredResource"/> class
    /// </summary>
    internal class StoredResourceVersion : IStoredResourceVersion
    {
        private StoredResource storedResource;
        private ResourceVersionManifest versionManifest;

        public StoredResourceVersion(StoredResource storedResource, ResourceVersionManifest versionManifest)
        {
            this.storedResource = storedResource;
            this.versionManifest = versionManifest;
        }

        public ResourceProperties Properties
        {
            get { return versionManifest.Properties; }
        }

        public DateTime CreationTime
        {
            get { return versionManifest.CreationTime; }
        }

        public void GetContent(out byte[] bytes, out long length)
        {
            throw new NotImplementedException();
        }
    }
}