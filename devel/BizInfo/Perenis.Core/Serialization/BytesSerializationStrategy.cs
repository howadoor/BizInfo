using System;
using Perenis.Core.Serialization.Exceptions;

namespace Perenis.Core.Serialization
{
    internal class BytesSerializationStrategy : ISerializationStrategy
    {
        #region ISerializationStrategy Members

        public void Write(object @object, ISerializer serializer, IWritingScheme scheme)
        {
            serializer.WriteId(@object);
            var bytes = (byte[]) @object;
            serializer.Writer.WriteBase64(bytes, 0, bytes.Length);
        }

        public object Read(IDeserializer deserializer, IReadingScheme scheme)
        {
            var id = deserializer.Reader.GetAttribute(Constants.IdName);
            var bytes = deserializer.Reader.ReadElementContentAsBase64();
            if (!string.IsNullOrEmpty(id)) deserializer.AddObject(id, bytes);
            return bytes;
        }

        public IReadingScheme ReadScheme(IDeserializer deserializer)
        {
            deserializer.Reader.MoveToContent();
            Type objectType = null;
            deserializer.ForInfrastructureElements(() =>
                                                       {
                                                           if (deserializer.Reader.LocalName != Constants.ObjectTypeName) throw new UnexpectedTagException(deserializer, Constants.ObjectTypeName);
                                                           var objectTypeString = deserializer.Reader.ReadElementString();
                                                           objectType = Type.GetType(objectTypeString, false);
                                                       });
            return new BytesReadingScheme {Type = objectType};
        }

        public string Name
        {
            get { return Constants.BytesSerializationStrategyName; }
        }

        #endregion
    }
}