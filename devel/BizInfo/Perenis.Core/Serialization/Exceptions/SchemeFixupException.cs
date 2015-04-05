using System;

namespace Perenis.Core.Serialization.Exceptions
{
    public class SchemeFixupException : ReadingSchemeException
    {
        public SchemeFixupException(IDeserializer deserializer, IReadingScheme scheme, Exception innerException) : base(deserializer, scheme, innerException)
        {
        }

        public override string Message
        {
            get { return string.Format("Scheme fixup exception thrown during deserialization of scheme {0}", Scheme); }
        }
    }
}