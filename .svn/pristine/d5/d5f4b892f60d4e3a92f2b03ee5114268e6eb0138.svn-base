using Perenis.Core.Interfaces;

namespace Perenis.Core.Serialization
{
    public interface ISerializationStrategy : IWithName
    {
        /// <summary>
        /// Write object to the serializer using scheme
        /// </summary>
        /// <param name="object"></param>
        /// <param name="serializer"></param>
        /// <param name="scheme"></param>
        void Write(object @object, ISerializer serializer, IWritingScheme scheme);

        /// <summary>
        /// Read object from deserializer using scheme
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="scheme"></param>
        /// <returns></returns>
        object Read(IDeserializer deserializer, IReadingScheme scheme);

        /// <summary>
        /// Read reading scheme from <see cref="deserializer"/>
        /// </summary>
        /// <param name="deserializer"></param>
        /// <returns></returns>
        IReadingScheme ReadScheme(IDeserializer deserializer);
    }
}