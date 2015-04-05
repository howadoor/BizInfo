namespace Perenis.Core.Serialization.Exceptions
{
    /// <summary>
    /// Thrown during deserialization when XML node occurs.
    /// </summary>
    /// <seealso cref="IDeserializer"/>
    /// <seealso cref="Deserializer"/>
    public class UnexpectedXmlNodeException : DeserializationException
    {
        /// <summary>
        /// Creates instance and initializes its members
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="expected"></param>
        public UnexpectedXmlNodeException(IDeserializer deserializer, string expected)
            : base(deserializer)
        {
            Unexpected = deserializer.Reader.Name;
            Expected = expected;
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
            get { return string.Format("Expected {0} but {1} read", Expected, Unexpected); }
        }

        /// <summary>
        /// Unexpected element
        /// </summary>
        public string Unexpected { get; protected set; }
        
        /// <summary>
        /// Expected element (can be <c>null</c>).
        /// </summary>
        public string Expected { get; protected set; }
    }
}