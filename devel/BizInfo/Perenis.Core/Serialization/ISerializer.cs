using System;
using System.Xml;

namespace Perenis.Core.Serialization
{
    /// <summary>
    /// Main interface used to serialize objects
    /// </summary>
    public interface ISerializer : IDisposable
    {
        /// <summary>
        /// Writer of the content of the objects to the target XML
        /// </summary>
        XmlDictionaryWriter Writer { get; }

        /// <summary>
        /// Provides schemes of objects
        /// </summary>
        ISchemeProvider SchemeProvider { get; }

        /// <summary>
        /// Provides ids of objects
        /// </summary>
        IIdProvider IdProvider { get; }

        /// <summary>
        /// Provides XML-conformal names of objects
        /// </summary>
        IXmlNameProvider XmlNameProvider { get; }

        /// <summary>
        /// Serializes an object
        /// </summary>
        /// <param name="object"></param>
        void Write(object @object);
    }
}