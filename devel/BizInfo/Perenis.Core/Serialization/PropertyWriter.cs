using System.Reflection;
using Perenis.Core.Reflection;

namespace Perenis.Core.Serialization
{
    internal class PropertyWriter : PropertyWriterBase
    {
        private readonly object @default;

        public PropertyWriter(PropertyInfo propertyInfo, ISerializer serializer)
        {
            PropertyInfo = propertyInfo;
            XmlName = GetXmlName(propertyInfo.Name);
            @default = propertyInfo.PropertyType.Default();
            ValueWritingScheme = serializer.SchemeProvider.GetScheme(propertyInfo.PropertyType, serializer);
        }

        public PropertyInfo PropertyInfo { get; protected set; }

        protected override bool IsDefaultScheme(IWritingScheme writingScheme)
        {
            return writingScheme.Type == PropertyInfo.PropertyType;
        }

        protected override bool IsDefault(object value)
        {
            return value == @default || value.Equals(@default);
        }

        protected override object GetValue(object sourceObject)
        {
            return PropertyInfo.GetValue(sourceObject, null);
        }
    }
}