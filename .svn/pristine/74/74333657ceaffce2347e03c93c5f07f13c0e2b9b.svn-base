using System;
using System.Globalization;
using System.Reflection;
using Perenis.Core.Reflection;
using Perenis.Core.Serialization.Exceptions;

namespace Perenis.Core.Serialization
{
    internal class ConvertToStringSerializationStrategy : ISerializationStrategy
    {
        #region ISerializationStrategy Members

        public void Write(object @object, ISerializer serializer, IWritingScheme scheme)
        {
            //serializer.WriteId(@object);
            serializer.Writer.WriteString(string.Format(CultureInfo.InvariantCulture, "{0}", @object));
        }

        public object Read(IDeserializer deserializer, IReadingScheme scheme)
        {
            var @string = deserializer.Reader.ReadString();
            object @object;
            if (scheme.Type.IsEnum)
            {
                @object = Enum.Parse(scheme.Type, @string);
            }
            else
            {
                var parseMethod1 = scheme.Type.GetMethod("Parse", BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(string), typeof(IFormatProvider) }, new ParameterModifier[] { });
                if (parseMethod1 != null)
                {
                    @object = parseMethod1.Invoke(null, new object[] { @string, CultureInfo.InvariantCulture });
                }
                else
                {
                    var parseMethod2 = scheme.Type.GetMethod("Parse", BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(string) }, new ParameterModifier[] { });
                    if (parseMethod2 != null)
                    {
                        @object = parseMethod2.Invoke(null, new object[] { @string });
                    }
                    else
                    {
                        @object = Convert.ChangeType(@string, scheme.Type);
                    }
                }
            }
            return @object;
        }

        public string Name
        {
            get { return Constants.ConvertToStringSerializationStrategyName; }
        }

        public IReadingScheme ReadScheme(IDeserializer deserializer)
        {
            deserializer.Reader.MoveToContent();
            Type objectType = null;
            deserializer.ForInfrastructureElements(() =>
                                                       {
                                                           switch (deserializer.Reader.LocalName)
                                                           {
                                                               case Constants.ObjectTypeName:
                                                                   var objectTypeString = deserializer.Reader.ReadElementString();
                                                                   objectType = Type.GetType(objectTypeString, false);
                                                                   break;

                                                               case Constants.ItemsName:
                                                               case Constants.PropertiesName:
                                                                   deserializer.Reader.Skip();
                                                                   break;
                                                               default:
                                                                   throw new UnexpectedInfrastructureTagException(deserializer);
                                                           }
                                                       });
            return new ConvertToStringReadingScheme {Type = objectType};
        }

        #endregion
    }
}