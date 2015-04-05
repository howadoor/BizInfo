using System;
using Perenis.Core.Pattern;

namespace Perenis.Core.Serialization
{
    public class TypeSerializationStrategy : ISerializationStrategy
    {
        #region ISerializationStrategy Members

        public string Name
        {
            get { return Constants.TypeSerializationStrategyName; }
        }

        public void Write(object @object, ISerializer serializer, IWritingScheme scheme)
        {
            serializer.Writer.WriteAttributeString(Constants.IdName, serializer.IdProvider.GetId(@object));
            serializer.Writer.WriteString(((Type) @object).AssemblyQualifiedName);
        }

        public object Read(IDeserializer deserializer, IReadingScheme scheme)
        {
            throw new NotImplementedException();
        }

        public IReadingScheme ReadScheme(IDeserializer deserializer)
        {
            return Singleton<TypeReadingScheme>.Instance;
        }

        #endregion
    }
}