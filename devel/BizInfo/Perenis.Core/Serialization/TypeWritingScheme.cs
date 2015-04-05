using System;
using Perenis.Core.Pattern;

namespace Perenis.Core.Serialization
{
    internal class TypeWritingScheme : WritingScheme
    {
        public TypeWritingScheme(Type type, string xmlName, ISerializer serializer) : base(type, xmlName, serializer)
        {
            Strategy = Singleton<TypeSerializationStrategy>.Instance;
        }
    }
}