using System;

namespace Perenis.Core.Serialization
{
    /// <summary>
    /// WritingScheme is a description of the object content and methods for serialization (writing) object content
    /// </summary>
    public interface IWritingScheme : IScheme
    {
        /// <summary>
        /// XML-conformal name of the scheme. It is usually derived from <see cref="Type"/> name.
        /// </summary>
        string XmlName { get; }

        /// <summary>
        /// Serialization strategy
        /// </summary>
        ISerializationStrategy Strategy { get; }

        /// <summary>
        /// Writes an object to the serializer, including starting and ending tag.
        /// </summary>
        /// <param name="object"></param>
        /// <param name="serializer"></param>
        void Write(object @object, ISerializer serializer);

        /// <summary>
        /// Writes only a content to serializer. It assuming that the starting tag was already written and ending tag will be written
        /// after call to this method by callee.
        /// </summary>
        /// <param name="object"></param>
        /// <param name="serializer"></param>
        void WriteContent(object @object, ISerializer serializer);
    }
}