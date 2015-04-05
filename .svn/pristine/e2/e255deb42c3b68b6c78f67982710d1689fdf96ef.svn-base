using System;

namespace Perenis.Core.Serialization
{
    internal class Scheme : IScheme
    {
        public ISerializationStrategy Strategy { get; internal set; }

        #region IScheme Members

        public Type Type { get; internal set; }

        #endregion

        public override string ToString()
        {
            return string.Format("{0}, scheme of {1}", GetType().Name, Type);
        }
    }
}