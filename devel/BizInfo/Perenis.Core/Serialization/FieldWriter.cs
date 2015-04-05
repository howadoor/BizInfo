using System.Reflection;
using Perenis.Core.Reflection;

namespace Perenis.Core.Serialization
{
    internal class FieldWriter : PropertyWriterBase
    {
        private readonly object @default;

        internal FieldWriter(FieldInfo fieldInfo, ISerializer serializer)
        {
            FieldInfo = fieldInfo;
            XmlName = GetXmlName(fieldInfo.Name);
            @default = fieldInfo.FieldType.Default();
            ValueWritingScheme = serializer.SchemeProvider.GetScheme(fieldInfo.FieldType, serializer);
        }

        public FieldInfo FieldInfo { get; protected set; }

        protected override bool IsDefaultScheme(IWritingScheme writingScheme)
        {
            return writingScheme.Type == FieldInfo.FieldType;
        }

        protected override bool IsDefault(object value)
        {
            return value == @default || value.Equals(@default);
        }

        protected override object GetValue(object sourceObject)
        {
            return FieldInfo.GetValue(sourceObject);
        }
    }
}