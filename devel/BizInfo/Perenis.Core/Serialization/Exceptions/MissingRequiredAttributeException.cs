namespace Perenis.Core.Serialization.Exceptions
{
    /// <summary>
    /// Thrown during deserialization when some required attribute is missing.
    /// </summary>
    /// <seealso cref="IDeserializer"/>
    /// <seealso cref="Deserializer"/>
    public class MissingRequiredAttributeException : DeserializationException
    {
        /// <summary>
        /// Creates instance and initializes its members
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="attributeName"></param>
        public MissingRequiredAttributeException(IDeserializer deserializer, string attributeName) : base(deserializer)
        {
            AttributeName = attributeName;
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
            get { return string.Format("Attribute {0} is missing or empty", AttributeName); }
        }

        /// <summary>
        /// Name of the requiered attribute.
        /// </summary>
        public string AttributeName { get; private set; }
    }
}