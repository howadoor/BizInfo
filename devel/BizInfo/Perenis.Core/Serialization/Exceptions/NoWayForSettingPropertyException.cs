namespace Perenis.Core.Serialization.Exceptions
{
    public class NoWayForSettingPropertyException : ReadingSchemeException
    {
        public NoWayForSettingPropertyException(IDeserializer deserializer, IReadingScheme scheme, string propertyName, object target, object value) : base(deserializer, scheme)
        {
            PropertyName = propertyName;
            Target = target;
            Value = value;
        }

        protected object Value
        {
            get; private set;
        }

        protected string PropertyName
        {
            get; private set;
        }

        protected object Target
        {
            get; private set;
        }

        public override string Message
        {
            get
            {
                return string.Format("No way found for setting property {0} in object {1} to new value {2}.", PropertyName, Target, Value);
            }
        }
    }
}