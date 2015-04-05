using System;
using Perenis.Core.Pattern;

namespace Perenis.Core.Serialization
{
    public class ConvertToStringReadingScheme : IReadingScheme
    {
        #region IReadingScheme Members

        public Type Type { get; internal set; }

        public object ReadObject(IDeserializer deserializer)
        {
            return Singleton<ConvertToStringSerializationStrategy>.Instance.Read(deserializer, this);
        }

        #endregion
    }
}