using System;

namespace Perenis.Core.Serialization.Exceptions
{
    /// <summary>
    /// Raised when something wrong occurs in deserialization using <see cref="Deserializer"/>.
    /// </summary>
    public class DeserializationException : Exception
    {
        /// <summary>
        /// Constructs new instance
        /// </summary>
        /// <param name="deserializer"></param>
        public DeserializationException(IDeserializer deserializer)
        {
            Deserializer = deserializer;
        }

        /// <summary>
        /// Constructs new instance
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="innerException"></param>
        protected DeserializationException(IDeserializer deserializer, Exception innerException) : base(null, innerException)
        {
            Deserializer = deserializer;
        }

        /// <summary>
        /// Instance of <see cref="Deserializer"/> which raised this exception
        /// </summary>
        public IDeserializer Deserializer { get; private set; }
    }
}