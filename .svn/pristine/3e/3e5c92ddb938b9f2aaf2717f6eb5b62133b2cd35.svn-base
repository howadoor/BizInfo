using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Perenis.Core.Reflection
{
    /// <summary>
    /// <see cref="Enum"/> extra tools.
    /// </summary>
    /// <typeparam name="T">The type of the enumeration type.</typeparam>
    public class EnumEx<T>
    {
        private static int? valueCount;

        /// <summary>
        /// The total number of values defined for the enum <typeparamref name="T"/>.
        /// </summary>
        /// <exception cref="ArgumentException">When the <typeparamref name="T"/> is not an enumeration type.</exception>
        public static int ValueCount
        {
            get
            {
                if (valueCount == null) valueCount = Enum.GetValues(typeof (T)).Length;
                return valueCount.Value;
            }
        }

        /// <summary>
        /// Retrieves an array of the values of the constants in the enumeration <typeparamref name="T"/>.
        /// </summary>
        /// <returns></returns>
        public static T[] GetValues()
        {
            return (T[]) Enum.GetValues(typeof (T));
        }

        /// <summary>
        /// Retrieves a dictionary mapping values of the constants in enumeration <typeparamref name="T"/> 
        /// to their indexes in a value array (see <see cref="GetValues"/>), starting from zero.
        /// </summary>
        /// <returns></returns>
        public static IDictionary<T, int> GetValuesToIndexesDictionary()
        {
            return GetValuesToIndexesDictionary(GetValues());
        }

        /// <summary>
        /// Retrieves a dictionary mapping values of the constants in enumeration <typeparamref name="T"/>
        /// to their indexes in a value array (see <see cref="GetValues"/>), starting from zero.
        /// </summary>
        /// <param name="values">Array of enum values to map to indexes</param>
        /// <returns></returns>
        public static IDictionary<T, int> GetValuesToIndexesDictionary(params T[] values)
        {
            var dictionary = new Dictionary<T, int>(values.Length);
            for (int i = 0; i < values.Length; i++)
            {
                dictionary[values[i]] = i;
            }
            return dictionary;
        }

        /// <summary>
        /// Retrieves an array of the object that decorates the values of given enumeration <typeparamref name="T"/>.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<TDecorator> GetDecoratedValues<TDecorator>()
            where TDecorator : EnumDecorator<T>, new()
        {
            return from T value in Enum.GetValues(typeof (T)) select new TDecorator {Value = value};
        }

        /// <summary>
        /// Retrieves an array of the names of the constants in the enumeration <typeparamref name="T"/>.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// The order of the names is the same as of the fields returned by the <see cref="GetNames"/> method.
        /// </remarks>
        public static string[] GetNames()
        {
            return Enum.GetNames(typeof (T));
        }

        /// <summary>
        /// Retrieves an array of <see cref="FieldInfo"/> instances for the constants in the 
        /// enumeration <typeparamref name="T"/>.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// The order of the fields is the same as of the names returned by the <see cref="GetNames"/> method.
        /// </remarks>
        public static FieldInfo[] GetFields()
        {
            string[] names = Enum.GetNames(typeof (T));
            var fields = new FieldInfo[names.Length];
            for (int i = 0; i < names.Length; i++)
            {
                fields[i] = typeof (T).GetField(names[i], BindingFlags.Public | BindingFlags.Static);
            }
            return fields;
        }

        /// <summary>
        /// Provides a type-safe implementation of the <see cref="Enum.Parse(Type,string)"/> method.
        /// </summary>
        public static T Parse(string value)
        {
            return (T) Enum.Parse(typeof (T), value);
        }

        /// <summary>
        /// Provides a type-safe implementation of the <see cref="Enum.Parse(Type,string,bool)"/> method.
        /// </summary>
        public static T Parse(string value, bool ignoreCase)
        {
            return (T) Enum.Parse(typeof (T), value, ignoreCase);
        }

        /// <summary>
        /// Provides a type-safe implementation of the <see cref="Enum.Parse(Type,string)"/> method.
        /// </summary>
        /// <param name="name">The name or value to convert.</param>
        /// <param name="value">The parsed value of the enumeration.</param>
        /// <returns><c>true</c> if the value was parsed successfuly; otherwise, <c>false</c>.</returns>
        public static bool TryParse(string name, out T value)
        {
            return TryParse(name, false, out value);
        }

        /// <summary>
        /// Provides a type-safe implementation of the <see cref="Enum.Parse(Type,string,bool)"/> method.
        /// </summary>
        /// <param name="name">The name or value to convert.</param>
        /// <param name="ignoreCase">If <c>true</c>, ignore case; otherwise, regard case. </param>
        /// <param name="value">The parsed value of the enumeration.</param>
        /// <returns><c>true</c> if the value was parsed successfuly; otherwise, <c>false</c>.</returns>
        public static bool TryParse(string name, bool ignoreCase, out T value)
        {
            StringEx.ThrowIfIsNullOrTrimmedEmpty(ref name, "name");
            try
            {
                value = (T) Enum.Parse(typeof (T), name, ignoreCase);
                return true;
            }
            catch (ArgumentException)
            {
                value = default(T);
                return false;
            }
        }
    }
}