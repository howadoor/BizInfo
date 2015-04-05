using Perenis.Core.Pattern;

namespace Perenis.Core.Serialization
{
    internal class StringSerializationStrategy : ISerializationStrategy
    {
        #region ISerializationStrategy Members

        public void Write(object @object, ISerializer serializer, IWritingScheme scheme)
        {
            // serializer.WriteId(@object);
            serializer.Writer.WriteString((string) @object);
        }

        public object Read(IDeserializer deserializer, IReadingScheme scheme)
        {
            var id = deserializer.Reader.GetAttribute(Constants.IdName);
            var @string = deserializer.Reader.ReadElementContentAsString();
            if (!string.IsNullOrEmpty(id)) deserializer.AddObject(id, @string);
            return @string;
        }

        public IReadingScheme ReadScheme(IDeserializer deserializer)
        {
            return Singleton<StringReadingScheme>.Instance;
        }

        public string Name
        {
            get { return Constants.StringSerializationStrategyName; }
        }

        #endregion
    }
}