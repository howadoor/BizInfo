using System;
using Perenis.Core.Pattern;

namespace Perenis.Core.Serialization
{
    public class TypeReadingScheme : IReadingScheme
    {
        #region IReadingScheme Members

        public Type Type
        {
            get { return typeof (Type); }
        }

        public object ReadObject(IDeserializer deserializer)
        {
            return Singleton<TypeSerializationStrategy>.Instance.Read(deserializer, this);
        }

        #endregion
    }
}