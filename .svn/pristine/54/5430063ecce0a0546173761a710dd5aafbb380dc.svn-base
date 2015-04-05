namespace Perenis.Core.Serialization
{
    internal interface IPropertyWriter
    {
        string XmlName { get; }
        IWritingScheme ValueWritingScheme { get; }
        void SerializeValue(object sourceObject, ISerializer serializer);
    }
}