using System;
using System.Collections.Generic;
using System.Reflection;
#if NUNIT
using NUnit.Framework;
#endif

namespace Perenis.Core.Reflection
{
    /// <summary>
    /// Enum extra tools.
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

    #region ------ Unit tests of the EnumEx class -------------------------------------------------

#if NUNIT

    /// <summary>
    /// Unit tests of the <see cref="EnumEx{T}"/> class.
    /// </summary>
    [TestFixture]
    public class EnumEx_Tests
    {
        private enum E
        {
            Z,
            A1,
            B1,
            A2,
            C,
        }

        private enum F
        {
            Z,
            A1 = 10,
            B1,
            A2 = 3,
            C = 4,
        }

        private enum G
        {
            Val1,
            Val2,
        }

        /// <summary>
        /// Tests the <see cref="EnumEx{T}.GetNames"/> method.
        /// </summary>
        [Test]
        public void TestGetNamesE()
        {
            CollectionAssert.AreEqual(new string[]{"Z", "A1", "B1", "A2", "C"}, EnumEx<E>.GetNames());
        }

        /// <summary>
        /// Tests the <see cref="EnumEx{T}.GetNames"/> method.
        /// </summary>
        [Test]
        public void TestGetNamesF()
        {
            CollectionAssert.AreEqual(new string[] { "Z", "A2", "C", "A1", "B1" }, EnumEx<F>.GetNames());
        }

        private class ValueNameComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                if (!(x is string) || !(y is Enum)) throw new ArgumentException();
                return ((string) x).CompareTo(y.ToString());
            }
        }

        /// <summary>
        /// Tests the <see cref="EnumEx{T}.GetValues"/> method.
        /// </summary>
        [Test]
        public void TestGetValuesE()
        {
            CollectionAssert.AreEqual(new string[] { "Z", "A1", "B1", "A2", "C" }, EnumEx<E>.GetValues(), new ValueNameComparer());
        }

        /// <summary>
        /// Tests the <see cref="EnumEx{T}.GetValues"/> method.
        /// </summary>
        [Test]
        public void TestGetValuesF()
        {
            CollectionAssert.AreEqual(new string[] { "Z", "A2", "C", "A1", "B1" }, EnumEx<F>.GetValues(), new ValueNameComparer());
        }

        private class FieldNameComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                if (!(x is string) || !(y is FieldInfo)) throw new ArgumentException();
                return  ((FieldInfo) y).Name.CompareTo(x);
            }
        }

        /// <summary>
        /// Tests the <see cref="EnumEx{T}.GetFields"/> method.
        /// </summary>
        [Test]
        public void TestGetFieldsE()
        {
            CollectionAssert.AreEqual(new string[] { "Z", "A1", "B1", "A2", "C" }, EnumEx<E>.GetFields(), new FieldNameComparer());
        }

        /// <summary>
        /// Tests the <see cref="EnumEx{T}.GetFields"/> method.
        /// </summary>
        [Test]
        public void TestGetFieldsF()
        {
            CollectionAssert.AreEqual(new string[] { "Z", "A2", "C", "A1", "B1" }, EnumEx<F>.GetFields(), new FieldNameComparer());
        }

        /// <summary>
        /// Tests the <see cref="EnumEx{T}.ValueCount"/> property.
        /// </summary>
        [Test]
        public void TestValueCount()
        {
            Assert.AreEqual(5, EnumEx<E>.ValueCount);
            Assert.AreEqual(5, EnumEx<F>.ValueCount);
            Assert.AreEqual(2, EnumEx<G>.ValueCount);
        }
    }

#endif

    #endregion
}