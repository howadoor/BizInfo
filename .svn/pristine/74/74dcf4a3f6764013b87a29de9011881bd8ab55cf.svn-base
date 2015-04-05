namespace Perenis.Core.Serialization.Exceptions
{
    /// <summary>
    /// Thrown when object id during deserialization process reffers to not existing object.
    /// </summary>
    /// <seealso cref="IDeserializer"/>
    /// <seealso cref="Deserializer"/>
    public class InvalidIdException : DeserializationException
    {
        /// <summary>
        /// Creates instance and initializes its members
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="id"></param>
        public InvalidIdException(IDeserializer deserializer, string id) : base(deserializer)
        {
            Id = id;
        }

        /// <summary>
        /// Invalid id
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        /// <returns>
        /// The error message that explains the reason for the exception.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public override string Message
        {
            get { return string.Format("Id {0} does not reffers to the valid object. Object with the proper id must precede any reference to it in the source file.", Id); }
        }
    }
}