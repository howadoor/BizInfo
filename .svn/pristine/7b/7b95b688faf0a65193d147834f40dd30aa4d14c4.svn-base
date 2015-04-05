using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Perenis.Core.Serialization
{
    /// <summary>
    /// Colects static methods related to XML serialization
    /// </summary>
    public static class XmlSerialization
    {
        /// <summary>
        /// Converts <see cref="@object"/> to XML string
        /// </summary>
        /// <typeparam name="TType">Type of the object to be converted to XML string</typeparam>
        /// <param name="object">Object to be converted to XML string</param>
        /// <returns>XML string</returns>
        public static string ToXmlString<TType>(this TType @object)
        {
            var serializer = new XmlSerializer(typeof (TType));
            var stringBuilder = new StringBuilder();
            using (var writer = XmlWriter.Create(stringBuilder, new XmlWriterSettings {NamespaceHandling = NamespaceHandling.OmitDuplicates, OmitXmlDeclaration = true}))
            {
                serializer.Serialize(writer, @object);
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Creates an object of type <see cref="TType"/> then loads its content from XML String <see cref="xmlString"/>
        /// </summary>
        /// <typeparam name="TType">Type of the object to be created</typeparam>
        /// <param name="xmlString">XML string with content of the object</param>
        /// <returns>New instance of <see cref="TType"/></returns>
        public static TType FromXmlString<TType>(string xmlString)
        {
            var serializer = new XmlSerializer(typeof (TType));
            using (var reader = new StringReader(xmlString))
            {
                return (TType) serializer.Deserialize(reader);
            }
        }

        public static string ToXmlStringByDataContractSerializer<TType>(this TType @object)
        {
            var serializer = new DataContractSerializer(typeof (TType));
            var stringBuilder = new StringBuilder();
            using (var writer = XmlWriter.Create(stringBuilder, new XmlWriterSettings {NamespaceHandling = NamespaceHandling.OmitDuplicates, OmitXmlDeclaration = true}))
            {
                serializer.WriteObject(writer, @object);
            }
            return stringBuilder.ToString();
        }

        public static TType FromXmlStringByDataContractSerializer<TType>(string xmlString)
        {
            return FromXmlStringByDataContractSerializer<TType>(xmlString, new Type[]{});
        }

        public static TType FromXmlStringByDataContractSerializer<TType>(string xmlString, params Type[] knownTypes)
        {
            var serializer = new DataContractSerializer(typeof(TType), knownTypes);
            using (var stringReader = new StringReader(xmlString))
            {
                using (var reader = XmlReader.Create(stringReader))
                {
                    return (TType)serializer.ReadObject(reader);
                }
            }
        }

        /// <summary>
        /// Creates and object from data in XML <see cref="stream"/>
        /// </summary>
        /// <typeparam name="TType">Type of the object to be created</typeparam>
        /// <param name="stream">XML stream</param>
        /// <returns>New instance of <see cref="TType"/></returns>
        public static TType FromXml<TType>(Stream stream)
        {
            var serializer = new DataContractSerializer(typeof (TType));
            using (var reader = XmlReader.Create(stream))
            {
                return (TType) serializer.ReadObject(reader);
            }
        }

        /// <summary>
        /// Serializes <see cref="@object"/> to <see cref="stream"/>
        /// </summary>
        /// <typeparam name="TType">Type of the object being serialized</typeparam>
        /// <param name="object">Objects being serialized</param>
        /// <param name="stream">Target stream</param>
        /// <param name="settings">Settings for writer used to write to XML</param>
        public static void ToXml<TType>(TType @object, Stream stream, XmlWriterSettings settings = null)
        {
            var serializer = new DataContractSerializer(typeof(TType));
            if (settings == null) settings = new XmlWriterSettings {NamespaceHandling = NamespaceHandling.OmitDuplicates, OmitXmlDeclaration = true, ConformanceLevel = ConformanceLevel.Fragment, Indent = true };
            using (var writer = XmlWriter.Create(stream, settings))
            {
                serializer.WriteObject(writer, @object);
            }
        }

        /// <summary>
        /// Stores <see cref="@object"/> to XML string then computes hash from it
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <param name="object"></param>
        /// <returns></returns>
        public static Guid HashToGuid<TType>(TType @object)
        {
            var xmlString = ToXmlStringByDataContractSerializer(@object);
                return ComputeGuidHash(xmlString);
        }

        /// <summary>
        /// Computes <see cref="Guid"/> as a hash of <see cref="hashedString"/>
        /// </summary>
        /// <param name="hashedString"></param>
        /// <returns></returns>
        public static Guid ComputeGuidHash(string hashedString)
        {
            using (var hashProvider = new System.Security.Cryptography.MD5CryptoServiceProvider())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(hashedString);
                var hash = hashProvider.ComputeHash(bytes);
                return new Guid(hash);
            }
        }
    }
}