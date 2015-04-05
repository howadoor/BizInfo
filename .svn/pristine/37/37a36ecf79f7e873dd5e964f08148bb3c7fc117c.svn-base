using System;
using System.Collections.Generic;
using System.ComponentModel;
using Perenis.Core.Pattern;
using Perenis.Core.Reflection;

namespace Perenis.Core.Component
{
    /// <summary>
    /// Class extending <see cref="Type"/> with utility methods concerning Type converting facility (<see cref="TypeConverter"/>).
    /// </summary>
    public static class TypeConverterUtilsTypeEx
    {
        /// <summary>
        /// Finds a suitable <see cref="TypeConverter"/> for the type <paramref name="type"/> capable
        /// of converting to the target type <paramref name="targetType"/>.
        /// </summary>
        /// <remarks>
        /// The method looks for type converters attached dynamically by <see cref="TypeConverterAttribute"/> 
        /// (<see cref="AttributeManagerRegistry"/>) and statically using the <see cref="TypeConverterAttribute"/>.
        /// </remarks>
        /// <param name="type">The reference type.</param>
        /// <param name="targetType">The target type.</param>
        /// <returns>Instance of <see cref="TypeConverter"/> or null if no suitable instance found.</returns>
        public static TypeConverter GetTypeConverterToConvertTo(this Type type, Type targetType)
        {
            // try the dynamic converters first
            TypeConverter result = GetTypeConverterToConvertTo(Singleton<AttributeManagerRegistry>.Instance.Get(type), targetType);
            if (result != null) return result;

            // static converter if nothing found
            result = TypeDescriptor.GetConverter(type);

            return result != null && result.CanConvertTo(targetType) ? result : null;
        }

        /// <summary>
        /// Finds a suitable <see cref="TypeConverter"/> capable
        /// of converting to the target type <paramref name="targetType"/>.
        /// </summary>
        /// <remarks>
        /// The method looks for type converters attached dynamically by <see cref="TypeConverterAttribute"/> 
        /// (<see cref="AttributeManagerRegistry"/>).
        /// </remarks>
        /// <param name="attributeManager">The attribute manager to be looked up for <see cref="TypeConverterAttribute"/> instances.</param>
        /// <param name="targetType">The target type.</param>
        /// <returns>Instance of <see cref="TypeConverter"/> or null if no suitable instance found.</returns>
        public static TypeConverter GetTypeConverterToConvertTo(AttributeManager attributeManager, Type targetType)
        {
            IEnumerable<TypeConverterAttribute> tcas = attributeManager.GetAttributes<TypeConverterAttribute>(true);
            foreach (TypeConverterAttribute tca in tcas)
            {
                TypeConverter tc = TypeFactory<TypeConverter>.Instance.CreateInstance(Type.GetType(tca.ConverterTypeName));
                if (tc != null && tc.CanConvertTo(targetType)) return tc;
            }

            return null;
        }

        /// <summary>
        /// Finds a suitable <see cref="TypeConverter"/> for the type <paramref name="type"/> capable
        /// of converting from the source type <paramref name="sourceType"/>.
        /// </summary>
        /// <remarks>
        /// The method looks for type converters attached dynamically by <see cref="TypeConverterAttribute"/> 
        /// (<see cref="AttributeManagerRegistry"/>) and statically using the <see cref="TypeConverterAttribute"/>.
        /// </remarks>
        /// <param name="type">The reference type.</param>
        /// <param name="sourceType">The source type.</param>
        /// <returns>Instance of <see cref="TypeConverter"/> or null if no suitable instance found.</returns>
        public static TypeConverter GetTypeConverterToConvertFrom(this Type type, Type sourceType)
        {
            // try the dynamic converters first
            TypeConverter result = GetTypeConverterToConvertFrom(Singleton<AttributeManagerRegistry>.Instance.Get(type), sourceType);
            if (result != null) return result;

            // static converter if nothing found
            result = TypeDescriptor.GetConverter(type);
            return result != null && result.CanConvertFrom(sourceType) ? result : null;
        }

        /// <summary>
        /// Finds a suitable <see cref="TypeConverter"/> capable
        /// of converting from the source type <paramref name="sourcetType"/>.
        /// </summary>
        /// <remarks>
        /// The method looks for type converters attached dynamically by <see cref="TypeConverterAttribute"/> 
        /// (<see cref="AttributeManagerRegistry"/>).
        /// </remarks>
        /// <param name="attributeManager">The attribute manager to be looked up for <see cref="TypeConverterAttribute"/> instances.</param>
        /// <param name="sourceType">The source type.</param>
        /// <returns>Instance of <see cref="TypeConverter"/> or null if no suitable instance found.</returns>
        public static TypeConverter GetTypeConverterToConvertFrom(AttributeManager attributeManager, Type sourceType)
        {
            IEnumerable<TypeConverterAttribute> tcas = attributeManager.GetAttributes<TypeConverterAttribute>(true);
            foreach (TypeConverterAttribute tca in tcas)
            {
                var tc = TypeFactory<TypeConverter>.Instance.CreateInstance<TypeConverter>(Type.GetType(tca.ConverterTypeName));
                if (tc != null && tc.CanConvertFrom(sourceType)) return tc;
            }

            return null;
        }
    }
}