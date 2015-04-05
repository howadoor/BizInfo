namespace Perenis.Core.Serialization.Exceptions
{
    /// <summary>
    /// Thrown during deserialization when unexpected tag with local name occured.
    /// </summary>
    /// <seealso cref="IDeserializer"/>
    /// <seealso cref="Deserializer"/>
    public class NotAllowedLocalTagException : UnexpectedXmlNodeException
    {
        /// <summary>
        /// Creates instance and initializes its members
        /// </summary>
        /// <param name="deserializer"></param>
        public NotAllowedLocalTagException(IDeserializer deserializer)
            : base(deserializer, null)
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
            get { return string.Format("Not allowed local tag {0} here", Unexpected); }
        }
    }
}