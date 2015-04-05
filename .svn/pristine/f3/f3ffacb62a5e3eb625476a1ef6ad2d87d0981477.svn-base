namespace Perenis.Core.Serialization.Exceptions
{
    /// <summary>
    /// Thrown during deserialization when unexpected namespace on the tag occurs.
    /// </summary>
    /// <seealso cref="IDeserializer"/>
    /// <seealso cref="Deserializer"/>
    public class UnexpectedNamespaceException : UnexpectedXmlNodeException
    {
        /// <summary>
        /// Creates instance and initializes its members
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="expected"></param>
        public UnexpectedNamespaceException(IDeserializer deserializer, string expected)
            : base(deserializer, expected)
        {
            Unexpected = deserializer.Reader.NamespaceURI;
        }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        /// <returns>
        /// The error message that explains the reason for the exception, or an empty string("").
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public override string Message
        {
            get { return string.Format("Expected namespace {0} but {1} read", Expected, Unexpected); }
        }
    }
}