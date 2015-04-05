namespace Perenis.Core.Serialization.Exceptions
{
    public class ObjectIdNotUniqueException : DeserializationException
    {
        public ObjectIdNotUniqueException(Deserializer deserializer, string id, object @object) : base(deserializer)
        {
            Id = id;
            Object = @object;
        }

        protected object Object { get; private set; }

        protected string Id { get; private set; }

        public override string Message
        {
            get { return string.Format("Id of object must be unique. Id {0} used second time for {1}", Id, Object); }
        }
    }
}