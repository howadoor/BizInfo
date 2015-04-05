namespace Perenis.Core.Serialization
{
    internal abstract class PropertyWriterBase : IPropertyWriter
    {
        #region IPropertyWriter Members

        public string XmlName { get; protected set; }

        public IWritingScheme ValueWritingScheme { get; protected set; }

        public void SerializeValue(object sourceObject, ISerializer serializer)
        {
            var value = GetValue(sourceObject);
            // do not write default property values
            if (IsDefault(value)) return;
            var scheme = serializer.SchemeProvider.GetScheme(value != null ? value.GetType() : null, serializer);
            serializer.Writer.WriteStartElement(XmlName);
            if (serializer.IdProvider.HasId(value))
            {
                serializer.Writer.WriteAttributeString(Constants.InfraPrefix, Constants.RefName, Constants.InfraNs, serializer.IdProvider.GetId(value));
            }
            else
            {
                if (!IsDefaultScheme(scheme))
                {
                    serializer.Writer.WriteAttributeString(Constants.InfraPrefix, Constants.SchemeName, Constants.InfraNs, scheme.XmlName);
                }
                scheme.WriteContent(value, serializer);
            }
            serializer.Writer.WriteEndElement();
        }

        #endregion

        protected abstract bool IsDefaultScheme(IWritingScheme writingScheme);

        protected abstract bool IsDefault(object value);

        protected abstract object GetValue(object sourceObject);

        protected static string GetXmlName(string name)
        {
            var pos = name.IndexOf('>');
            if (pos < 0) return name;
            return name.Substring(1, pos - 1);
        }
    }
}