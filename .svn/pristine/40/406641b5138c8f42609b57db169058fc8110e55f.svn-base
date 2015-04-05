using System;
using System.Collections.Generic;
using System.Windows.Media.Media3D;
using System.Xml;
using Perenis.Core.Serialization.Exceptions;

namespace Perenis.Core.Serialization
{
    internal static class SerializationHelpers
    {
        internal static void WriteId(this ISerializer serializer, object @object)
        {
            serializer.Writer.WriteAttributeString(Constants.IdName, serializer.IdProvider.GetId(@object));
        }

        internal static void ForInfrastructureElements(this IDeserializer deserializer, Action action)
        {
            var reader = deserializer.Reader;
            deserializer.ForElements(() =>
                                         {
                                             if (string.IsNullOrEmpty(reader.NamespaceURI)) throw new NotAllowedLocalTagException(deserializer);
                                             if (reader.NamespaceURI != Constants.InfraNs)
                                             {
                                                 reader.Skip();
                                             }
                                             else
                                             {
                                                 action();
                                             }
                                         });
        }

        internal static void ForElements(this IDeserializer deserializer, Action action)
        {
            var reader = deserializer.Reader;
            while (reader.Read() && reader.NodeType != XmlNodeType.EndElement)
            {
                if (reader.NodeType != XmlNodeType.Element) continue;
                action();
            }
        }

        internal static IDictionary<string, IPropertyReader> ReadProperties(this IDeserializer deserializer)
        {
            var reader = deserializer.Reader;
            var properties = new Dictionary<string, IPropertyReader>();
            while (reader.Read() && reader.NodeType != XmlNodeType.EndElement)
            {
                if (reader.NodeType != XmlNodeType.Element) continue;
                // read local tag
                if (string.IsNullOrEmpty(reader.NamespaceURI))
                {
                    var property = deserializer.ReadProperty();
                    properties[property.XmlName] = property;
                    continue;
                }
                // read infrastructure tag 
                if (reader.NamespaceURI == Constants.InfraNs)
                {
                    throw new UnexpectedTagException(deserializer, null);
                }
                // skip all tags from foreign namespaces
                reader.Skip();
                continue;
            }
            return properties;
        }

        internal static IPropertyReader ReadProperty(this IDeserializer deserializer)
        {
            return new PropertyReader {XmlName = deserializer.Reader.LocalName, PropertyScheme = deserializer.ReadSchemeFromAttribute()};
        }

        internal static IReadingScheme ReadSchemeFromAttribute(this IDeserializer deserializer)
        {
            var reader = deserializer.Reader;
            var schemeId = reader.GetAttribute(Constants.SchemeName, Constants.InfraNs);
            if (string.IsNullOrEmpty(schemeId)) throw new MissingRequiredAttributeException(deserializer, Constants.SchemeName);
            return deserializer.GetScheme(schemeId);
        }

        internal static IReadingScheme GetScheme(this IDeserializer deserializer, string schemeId)
        {
            var scheme = deserializer.GetObject(schemeId) as IReadingScheme;
            if (scheme == null) throw new InvalidSchemeException(deserializer, schemeId, null);
            return scheme;
        }
    }
}