namespace Perenis.Core.Serialization.Exceptions
{
    /// <summary>
    /// Thrown during deserialization when scheme id reffers to object which is not instance of <see cref="IReadingScheme"/>.
    /// </summary>
    /// <seealso cref="IDeserializer"/>
    /// <seealso cref="Deserializer"/>
    public class InvalidSchemeException : InvalidIdException
    {
        /// <summary>
        /// Creates instance and initializes its members
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="schemeId"></param>
        /// <param name="object"></param>
        public InvalidSchemeException(IDeserializer deserializer, string schemeId, object @object) : base(deserializer, schemeId)
        {
            Object = @object;
        }

        /// <summary>
        /// Object which is reffered by <see cref="InvalidIdException.Id"/>.
        /// </summary>
        public object Object { get; private set; }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        /// <returns>
        /// The error message that explains the reason for the exception.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public override string Message
        {
            get { return string.Format("Id {0} does not reffers to the valid scheme object but {1} instead", Id, Object); }
        }
    }
}