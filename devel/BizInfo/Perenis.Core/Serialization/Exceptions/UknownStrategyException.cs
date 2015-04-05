namespace Perenis.Core.Serialization.Exceptions
{
    public class UknownStrategyException : DeserializationException
    {
        public UknownStrategyException(Deserializer deserializer, string strategyName) : base(deserializer)
        {
            StrategyName = strategyName;
        }

        protected string StrategyName { get; private set; }

        public override string Message
        {
            get { return string.Format("Unknown strategy {0}", StrategyName); }
        }
    }
}