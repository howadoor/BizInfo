using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using Perenis.Core.Reflection;

namespace Perenis.Core.Serialization
{
    /// <summary>
    /// Various constraints for serialization
    /// </summary>
    public static class Constraints
    {
        public static bool ShouldBeSerialized(PropertyInfo property)
        {
            if (property.GetCustomAttributes(typeof (XmlIgnoreAttribute), false).Length > 0) return false;
            if (property.CanRead && property.CanWrite && property.GetIndexParameters().GetLength(0) == 0) return true;
            if (typeof (IDictionary).IsAssignableFrom(property.ReflectedType) && (property.Name.EndsWith("Keys") || property.Name.EndsWith("Values"))) return false;
            var x = IsSerializableEnumerable(property.PropertyType);
            return x;
        }

        public static bool IsSerializableEnumerable(Type type)
        {
            return typeof (IEnumerable).IsAssignableFrom(type) && ExistsAddMethod(type);
        }

        public static bool CanUseConvertToStringStrategy(Type type)
        {
            if (type.IsEnum) return true;
            return type.IsValueType && ExistsParseMethod(type);
        }

        private static bool ExistsParseMethod(Type type)
        {
            return type.GetMethod("Parse", BindingFlags.Public | BindingFlags.Static, null, new[] {typeof (string), typeof (IFormatProvider)}, new ParameterModifier[] {}) != null
                   || type.GetMethod("Parse", BindingFlags.Public | BindingFlags.Static, null, new[] {typeof (string)}, new ParameterModifier[] {}) != null;
        }

        private static bool ExistsAddMethod(Type type)
        {
            return type.GetAllMethods().Any(methodInfo =>
                                             {
                                                 if (methodInfo.Name != "Add") return false;
                                                 var types = methodInfo.GetParametersTypes();
                                                 return types.Length == 1;
                                             });
        }
    }
}