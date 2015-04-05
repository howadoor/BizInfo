using System;
using System.Collections.Generic;

namespace Perenis.Core.Serialization
{
    internal class ReadingScheme : Scheme, IReadingScheme
    {
        public IDictionary<string, IPropertyReader> Properties { get; set; }

        public IReadingScheme ItemsScheme { get; set; }

        public Func<object> Instantiator
        {
            get; set;
        }

        public Action<object, object> ItemsAdder
        {
            get; set;
        }

        #region IReadingScheme Members

        public object ReadObject(IDeserializer deserializer)
        {
            return Strategy.Read(deserializer, this);

        }

        #endregion
    }
}