namespace Perenis.Core.Serialization.Exceptions
{
    /// <summary>
    /// Thrown during deserialization when unexpected namespace on the tag occurs.
    /// </summary>
    /// <seealso cref="IDeserializer"/>
    /// <seealso cref="Deserializer"/>
    public class UnexpectedRootNamespaceException : UnexpectedNamespaceException
    {
        /// <summary>
        /// Creates instance and initializes its members
        /// </summary>
        /// <param name="deserializer"></param>
        public UnexpectedRootNamespaceException(IDeserializer deserializer)
            : base(deserializer, Constants.InfraNs)
        {
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
            get { return string.Format("Expected {0} namespace for the root element but {1} read", Expected, Unexpected); }
        }
    }
}