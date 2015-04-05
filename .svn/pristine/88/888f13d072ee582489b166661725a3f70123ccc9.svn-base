using System;
using Perenis.Core.Pattern;

namespace Perenis.Core.Serialization
{
    internal class StringReadingScheme : IReadingScheme
    {
        #region IReadingScheme Members

        public Type Type
        {
            get { return typeof (string); }
        }

        public object ReadObject(IDeserializer deserializer)
        {
            return Singleton<StringSerializationStrategy>.Instance.Read(deserializer, this);
        }

        #endregion
    }
}