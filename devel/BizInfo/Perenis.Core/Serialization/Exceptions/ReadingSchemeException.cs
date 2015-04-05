using System;

namespace Perenis.Core.Serialization.Exceptions
{
    public class ReadingSchemeException : DeserializationException
    {
        public ReadingSchemeException(IDeserializer deserializer, IReadingScheme scheme)
            : base(deserializer)
        {
            Scheme = scheme;
        }

        public ReadingSchemeException(IDeserializer deserializer, IReadingScheme scheme, Exception innerException)
            : base(deserializer, innerException)
        {
            Scheme = scheme;
        }

        protected IReadingScheme Scheme { get; private set; }
    }

    public class UnknownPropertyException : ReadingSchemeException
    {
        public UnknownPropertyException(IDeserializer deserializer, IReadingScheme scheme) : base(deserializer, scheme)
        {
            PropertyName = deserializer.Reader.Name;
        }

        public override string Message
        {
            get { return string.Format("Cannot find property {0} in scheme {1}", PropertyName, Scheme); }
        }

        protected string PropertyName { get; private set; }
    }
}