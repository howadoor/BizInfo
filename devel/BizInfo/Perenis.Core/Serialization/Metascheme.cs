using System;

namespace Perenis.Core.Serialization
{
    /// <summary>
    /// Metascheme is writingScheme of object schemes
    /// </summary>
    internal class Metascheme : IWritingScheme
    {
        #region IWritingScheme Members

        public ISerializationStrategy Strategy
        {
            get { throw new NotImplementedException(); }
        }

        public Type Type
        {
            get { return typeof (IWritingScheme); }
        }

        public string XmlName
        {
            get { return Constants.SchemeName; }
        }

        public void Write(object @object, ISerializer serializer)
        {
            if (@object == this) return;
            serializer.Writer.WriteStartElement(Constants.InfraPrefix, Constants.SchemeName, Constants.InfraNs);
            WriteContent(@object, serializer);
            serializer.Writer.WriteEndElement();
        }

        public void WriteContent(object @object, ISerializer serializer)
        {
            var scheme = ((IWritingScheme) @object);
            serializer.Writer.WriteAttributeString(Constants.IdName, scheme.XmlName);
            serializer.Writer.WriteAttributeString(Constants.InfraPrefix, Constants.StrategyName, Constants.InfraNs, scheme.Strategy.Name);
            if (scheme.Strategy is StringSerializationStrategy) return;
            /*
            serializer.Writer.WriteStartElement("schemeType");
            serializer.Write(@object.GetType());
            serializer.Writer.WriteEndElement();
            */
            serializer.Writer.WriteElementString(Constants.InfraPrefix, Constants.ObjectTypeName, Constants.InfraNs, scheme.Type.AssemblyQualifiedName);
            if (scheme is WritingScheme)
            {
                if (((WritingScheme) scheme).Properties.Length > 0)
                {
                    serializer.Writer.WriteStartElement(Constants.InfraPrefix, Constants.PropertiesName, Constants.InfraNs);
                    foreach (var property in ((WritingScheme) scheme).Properties)
                    {
                        serializer.Writer.WriteStartElement(property.XmlName);
                        serializer.Writer.WriteAttributeString(Constants.InfraPrefix, Constants.SchemeName, Constants.InfraNs, property.ValueWritingScheme.XmlName);
                        serializer.Writer.WriteEndElement();
                    }
                    serializer.Writer.WriteEndElement();
                }
                if (((WritingScheme) scheme).ItemsWritingScheme != null)
                {
                    serializer.Writer.WriteStartElement(Constants.InfraPrefix, Constants.ItemsName, Constants.InfraNs);
                    serializer.Writer.WriteAttributeString(Constants.InfraPrefix, Constants.SchemeName, Constants.InfraNs, ((WritingScheme) scheme).ItemsWritingScheme.XmlName);
                    serializer.Writer.WriteEndElement();
                }
            }
        }

        #endregion
    }
}