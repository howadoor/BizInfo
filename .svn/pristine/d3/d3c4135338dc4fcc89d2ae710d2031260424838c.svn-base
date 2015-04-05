namespace Perenis.Core.Serialization.Exceptions
{
    /// <summary>
    /// Thrown during deserialization when unexpected tag occurs.
    /// </summary>
    /// <seealso cref="IDeserializer"/>
    /// <seealso cref="Deserializer"/>
    public class UnexpectedTagException : UnexpectedXmlNodeException
    {
        /// <summary>
        /// Creates instance and initializes its members
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="expected"></param>
        public UnexpectedTagException(IDeserializer deserializer, string expected)
            : base(deserializer, expected)
        {
        }
    }
}