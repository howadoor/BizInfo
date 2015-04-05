using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Perenis.Core.Pattern;
using Perenis.Core.Reflection;

namespace Perenis.Core.Serialization
{
    /// <summary>
    /// Standard implementation of <see cref="IWritingScheme"/>
    /// </summary>
    internal class WritingScheme : Scheme, IWritingScheme
    {
        public WritingScheme(Type type, string xmlName, ISerializer serializer)
        {
            Type = type;
            XmlName = xmlName;
            Properties = GetPropertySerializers(Type, serializer).ToArray();
            if (Type == typeof (string))
            {
                Strategy = Singleton<StringSerializationStrategy>.Instance;
            }
            else
            {
                Strategy = Constraints.CanUseConvertToStringStrategy(Type) ? (ISerializationStrategy) Singleton<ConvertToStringSerializationStrategy>.Instance : Singleton<ByPropertiesSerializationStrategy>.Instance;
            }
            if ((typeof (IEnumerable).IsAssignableFrom(Type) && Constraints.IsSerializableEnumerable(Type)))
            {
                var typedEnumerable = Type.FindGenericTypeOfDefinition(typeof (IEnumerable<>)).FirstOrDefault();
                ItemsWritingScheme = serializer.SchemeProvider.GetScheme(typedEnumerable != null ? typedEnumerable.GetGenericArguments()[0] : typeof (object), serializer);
            }
        }

        public IPropertyWriter[] Properties { get; internal set; }

        public IWritingScheme ItemsWritingScheme { get; internal set; }

        #region IWritingScheme Members

        public string XmlName { get; internal set; }

        public void Write(object @object, ISerializer serializer)
        {
            serializer.Writer.WriteStartElement(XmlName);
            WriteContent(@object, serializer);
            serializer.Writer.WriteEndElement();
        }

        public virtual void WriteContent(object @object, ISerializer serializer)
        {
            Strategy.Write(@object, serializer, this);
        }

        #endregion

        private static IEnumerable<IPropertyWriter> GetPropertySerializers(Type type, ISerializer serializer)
        {
            foreach (var field in type.GetAllFields(BindingFlags.Public | BindingFlags.Instance))
            {
                yield return new FieldWriter(field, serializer);
            }
            foreach (var property in type.GetProperties(BindingFlags.Public /*| BindingFlags.NonPublic*/| BindingFlags.Instance))
            {
                if (Constraints.ShouldBeSerialized(property))
                {
                    yield return new PropertyWriter(property, serializer);
                }
            }
            if (type.IsGenericType)
            {
                var definition = type.GetGenericTypeDefinition();
                if (definition == typeof (KeyValuePair<,>))
                {
                    yield return new PropertyWriter(type.GetProperty("Key"), serializer);
                    yield return new PropertyWriter(type.GetProperty("Value"), serializer);
                }
                if ((new[] {typeof (Tuple)}).Contains(definition))
                {
                    foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                    {
                        if (property.DeclaringType == definition && !property.CanWrite)
                        {
                            yield return new PropertyWriter(property, serializer);
                        }
                    }
                }
            }
        }

        protected void WriteId(ISerializer serializer, object @object)
        {
            serializer.Writer.WriteAttributeString(Constants.IdName, serializer.IdProvider.GetId(@object));
        }
    }
}