using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Perenis.Core.Serialization.Exceptions;

namespace Perenis.Core.Serialization
{
    /// <summary>
    /// Serialization strategy which serializes object content in property-by-property, item-by-item manner
    /// </summary>
    internal class ByPropertiesSerializationStrategy : ISerializationStrategy
    {
        #region ISerializationStrategy Members

        public void Write(object @object, ISerializer serializer, IWritingScheme scheme)
        {
            var writingScheme = (WritingScheme) scheme;
            serializer.WriteId(@object);
            foreach (var property in writingScheme.Properties) property.SerializeValue(@object, serializer);
            if (writingScheme.ItemsWritingScheme != null)
            {
                var startElementWritten = false;
                foreach (var item in (IEnumerable) @object)
                {
                    if (!startElementWritten)
                    {
                        serializer.Writer.WriteStartElement(Constants.InfraPrefix, Constants.ItemsName, Constants.InfraNs);
                        startElementWritten = true;
                    }
                    serializer.Write(item);
                }
                if (startElementWritten)
                {
                    serializer.Writer.WriteEndElement();
                }
            }
        }

        public object Read(IDeserializer deserializer, IReadingScheme scheme)
        {
            var @object = ((ReadingScheme) scheme).Instantiator();
            var id = deserializer.Reader.GetAttribute(Constants.IdName);
            if (!string.IsNullOrEmpty(id)) deserializer.AddObject(id, @object);
            var reader = deserializer.Reader;
            // skip any further reading when element has no content
            if (reader.IsEmptyElement) return @object;
            reader.MoveToContent();
            deserializer.ForElements(() =>
                                         {
                                             if (string.IsNullOrEmpty(reader.NamespaceURI))
                                             {
                                                 ReadAndSetProperty(deserializer, scheme, @object);
                                             }
                                             else if (reader.NamespaceURI != Constants.InfraNs)
                                             {
                                                 reader.Skip();
                                             }
                                             else
                                             {
                                                 switch (reader.LocalName)
                                                 {
                                                     case Constants.SchemeName:
                                                         ((Deserializer) deserializer).ReadScheme();
                                                         break;
                                                     case Constants.ItemsName:
                                                         reader.MoveToContent();
                                                         foreach (var item in ((Deserializer) deserializer).ReadObjects())
                                                         {
                                                             ((ReadingScheme)scheme).ItemsAdder(@object, item);
                                                         }
                                                         break;
                                                     default:
                                                         throw new UnexpectedTagException(deserializer, "scheme");
                                                 }
                                             }
                                         });
            return @object;
        }

        public IReadingScheme ReadScheme(IDeserializer deserializer)
        {
            var scheme = new ReadingScheme {Strategy = this};
            deserializer.Reader.MoveToContent();
            deserializer.ForInfrastructureElements(() =>
                                                       {
                                                           switch (deserializer.Reader.LocalName)
                                                           {
                                                               case Constants.ObjectTypeName:
                                                                   var objectTypeString = deserializer.Reader.ReadElementString();
                                                                   scheme.Type = Type.GetType(objectTypeString, false);
                                                                   break;

                                                               case Constants.ItemsName:
                                                                   scheme.ItemsScheme = deserializer.ReadSchemeFromAttribute();
                                                                   deserializer.Reader.Skip();
                                                                   break;

                                                               case Constants.PropertiesName:
                                                                   scheme.Properties = deserializer.ReadProperties();
                                                                   break;
                                                               default:
                                                                   throw new UnexpectedInfrastructureTagException(deserializer);
                                                           }
                                                       });
            try
            {
                Fixup(deserializer, scheme);
            }
            catch (Exception exception)
            {
                throw new SchemeFixupException(deserializer, scheme, exception);
            }
            return scheme;
        }

        public string Name
        {
            get { return Constants.ByPropertiesSerializationStrategyName; }
        }

        #endregion

        private void Fixup(IDeserializer deserializer, ReadingScheme scheme)
        {
            scheme.Instantiator = ((Deserializer) deserializer).InstantiatorProvider.GetInstantiator(scheme.Type);
            if (scheme.ItemsScheme != null) scheme.ItemsAdder = ((Deserializer)deserializer).ItemsAdderProvider.GetItemsAdder(scheme.Type);
            if (scheme.Properties != null)
            {
                foreach (var pair in scheme.Properties)
                {
                    ((PropertyReader) pair.Value).PropertySetter = ((Deserializer) deserializer).PropertySetterProvider.GetPropertySetter(scheme.Type, pair.Key);
                }
            }
        }

        private void ReadProperty(IDeserializer deserializer, IReadingScheme scheme)
        {
            var reader = deserializer.Reader;
            var readingScheme = (ReadingScheme) scheme;
            IPropertyReader propertyReader;
            if (!readingScheme.Properties.TryGetValue(reader.LocalName, out propertyReader)) throw new UnknownPropertyException(deserializer, scheme);
            propertyReader.ReadProperty(deserializer);
        }

        private void ReadAndSetProperty(IDeserializer deserializer, IReadingScheme scheme, object target)
        {
            var reader = deserializer.Reader;
            var readingScheme = (ReadingScheme) scheme;
            IPropertyReader propertyReader;
            if (!readingScheme.Properties.TryGetValue(reader.LocalName, out propertyReader)) throw new UnknownPropertyException(deserializer, scheme);
            ((PropertyReader) propertyReader).PropertySetter (target, propertyReader.ReadProperty(deserializer));
        }
    }
}