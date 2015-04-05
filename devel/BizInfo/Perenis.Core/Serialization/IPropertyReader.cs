namespace Perenis.Core.Serialization
{
    internal interface IPropertyReader
    {
        string XmlName { get; }
        IReadingScheme PropertyScheme { get; }
        object ReadProperty(IDeserializer deserializer);
    }
}