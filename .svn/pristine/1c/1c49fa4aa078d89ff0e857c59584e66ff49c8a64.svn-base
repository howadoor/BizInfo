using System;
using System.Collections.Generic;
using Perenis.Core.Reflection;

namespace Perenis.Core.Collections
{
    /// <summary>
    /// Dictionary mapping TEnum values to some other value
    /// </summary>
    /// <typeparam name="TEnum">enum type</typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class EnumDictionary<TEnum, TValue>
    {
        protected static EnumDescription _enumDescription;
        protected TValue[] _values;

        /// <summary>
        /// Collection of values
        /// </summary>
        public TValue[] Values
        {
            get
            {
                if (_values == null) _values = NewValues();
                return _values;
            }
        }

        /// <summary>
        /// Description of TEnum type, see EnumDescription
        /// </summary>
        public EnumDescription EnumTypeDescription
        {
            get
            {
                if (_enumDescription == null) _enumDescription = new EnumDescription();
                return _enumDescription;
            }
        }

        /// <summary>
        /// Returns count of keys which values not equals to default TEnum
        /// </summary>
        public int CountOfKeysWithNonDefaultValues
        {
            get
            {
                int count = 0;
                WithKeysAndValues((key, value) => count++);
                return count;
            }
        }

        /// <summary>
        /// Create new array for values
        /// </summary>
        /// <returns></returns>
        private TValue[] NewValues()
        {
            return new TValue[EnumTypeDescription.ValuesCount];
        }

        /// <summary>
        /// Removes everything from dictionary
        /// </summary>
        public virtual void Clear()
        {
            RemoveAll();
        }

        /// <summary>
        /// Removes everything from dictionary
        /// </summary>
        public virtual void RemoveAll()
        {
            _values = null;
        }

        /// <summary>
        /// Removes value of key (replaces it by default value of TValue type)
        /// </summary>
        /// <param name="key"></param>
        public virtual void RemoveAt(TEnum key)
        {
            int index = EnumTypeDescription.IndexOf(key);
            Values[index] = default(TValue);
        }

        /// <summary>
        /// Returns a value of key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual TValue GetAt(TEnum key)
        {
            return Values[EnumTypeDescription.IndexOf(key)];
        }

        /// <summary>
        /// Sets value for given key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public virtual void SetAt(TEnum key, TValue value)
        {
            Values[EnumTypeDescription.IndexOf(key)] = value;
        }

        /// <summary>
        /// Performs an Action for each key and value pair, which value is not equal to default TValue
        /// </summary>
        /// <param name="action"></param>
        public void WithKeysAndValues(Action<TEnum, TValue> action)
        {
            WithKeysAndValues(action, false);
        }

        /// <summary>
        /// Performs an Action for each key and value pair
        /// </summary>
        /// <param name="action"></param>
        /// <param name="includingDefaultValues">should be included pairs with default values too?</param>
        public void WithKeysAndValues(Action<TEnum, TValue> action, bool includingDefaultValues)
        {
            foreach (TEnum key in EnumTypeDescription.Values)
            {
                TValue value = GetAt(key);
                if (!value.Equals(default(TValue)) || includingDefaultValues) action(key, value);
            }
        }

        #region Nested type: EnumDescription

        /// <summary>
        /// Collects iformations describing enum type
        /// </summary>
        public class EnumDescription
        {
            protected TEnum[] _values;
            protected IDictionary<TEnum, int> _valuesToIndex;

            public EnumDescription()
            {
                _values = EnumEx<TEnum>.GetValues();
                _valuesToIndex = EnumEx<TEnum>.GetValuesToIndexesDictionary(_values);
            }

            /// <summary>
            /// Array of values of enum type
            /// </summary>
            public TEnum[] Values
            {
                get { return _values; }
            }

            /// <summary>
            /// Count of values of enum type
            /// </summary>
            public int ValuesCount
            {
                get { return Values.Length; }
            }

            /// <summary>
            /// Returns index of particular enum value
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public int IndexOf(TEnum value)
            {
                return _valuesToIndex[value];
            }
        }

        #endregion
    }
}