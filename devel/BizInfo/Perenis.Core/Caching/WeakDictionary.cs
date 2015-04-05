using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Perenis.Core.General;

namespace Perenis.Core.Caching
{
    /// <summary>
    /// This is a dictionary that allow values to be collected.
    /// </summary>
    [Serializable]
    public class WeakDictionary<TKey, TValue> :
        ThreadSafeDisposable,
        IDictionary<TKey, TValue>,
        ISerializable
        where
            TValue : class
    {
        #region Private dictionary of EquatableWeakReferences

        private volatile Dictionary<TKey, ExtendedGCHandle> fDictionary;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates the dictionary.
        /// </summary>
        public WeakDictionary()
        {
            fDictionary = new Dictionary<TKey, ExtendedGCHandle>();
            GCUtils.Collected += p_Collected;
        }

        /// <summary>
        /// Creates the dictionary using given IEqualityComparer.
        /// </summary>
        public WeakDictionary(IEqualityComparer<TKey> equalityComparer)
        {
            fDictionary = new Dictionary<TKey, ExtendedGCHandle>(equalityComparer);
            GCUtils.Collected += p_Collected;
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Frees all handles used to know if an item was collected or not.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                GCUtils.Collected -= p_Collected;

            Dictionary<TKey, ExtendedGCHandle> dictionary = fDictionary;
            if (dictionary != null)
            {
                fDictionary = null;

                foreach (ExtendedGCHandle wr in dictionary.Values)
                    wr.Free();
            }

            base.Dispose(disposing);
        }

        #endregion

        #region p_Collected

        private void p_Collected()
        {
            lock (DisposeLock)
            {
                if (WasDisposed)
                    return;

                GCUtils.Collected += p_Collected;

                Dictionary<TKey, ExtendedGCHandle> oldDictionary = fDictionary;
                var newDictionary = new Dictionary<TKey, ExtendedGCHandle>();
                foreach (var pair in oldDictionary)
                {
                    ExtendedGCHandle wr = pair.Value;

                    if (wr.IsAlive)
                        newDictionary.Add(pair.Key, pair.Value);
                    else
                        wr.Free();
                }

                fDictionary = newDictionary;
            }
        }

        #endregion

        #region Properties

        #region Count

        /// <summary>
        /// Gets the number of items in this dictionary.
        /// </summary>
        public int Count
        {
            get
            {
                lock (DisposeLock)
                {
                    CheckUndisposed();

                    return fDictionary.Count;
                }
            }
        }

        #endregion

        #region this[]

        /// <summary>
        /// Gets or sets a value for the specified key.
        /// Returns null if the item does not exist. The indexer, when
        /// used as an IDictionary throws an exception when the item does
        /// not exist.
        /// </summary>
        public TValue this[TKey key]
        {
            get
            {
                if (key == null)
                    throw new ArgumentNullException("key");

                lock (DisposeLock)
                {
                    CheckUndisposed();

                    ExtendedGCHandle wr;
                    if (!fDictionary.TryGetValue(key, out wr))
                        return null;

                    return (TValue) wr.Target;
                }
            }
            set
            {
                if (key == null)
                    throw new ArgumentNullException("key");

                lock (DisposeLock)
                {
                    CheckUndisposed();

                    if (value == null)
                        p_Remove(key);
                    else
                    {
                        ExtendedGCHandle wr;

                        Dictionary<TKey, ExtendedGCHandle> dictionary = fDictionary;
                        if (dictionary.TryGetValue(key, out wr))
                            wr.Target = value;
                        else
                            p_Add(key, value, dictionary);
                    }
                }
            }
        }

        #endregion

        #region Keys

        /// <summary>
        /// Gets the Keys that exist in this dictionary.
        /// </summary>
        public ICollection<TKey> Keys
        {
            get
            {
                lock (DisposeLock)
                {
                    CheckUndisposed();

                    return fDictionary.Keys.ToArray();
                }
            }
        }

        #endregion

        #region Values

        /// <summary>
        /// Gets the values that exist in this dictionary.
        /// </summary>
        public ICollection<TValue> Values
        {
            get
            {
                lock (DisposeLock)
                {
                    CheckUndisposed();

                    Dictionary<TKey, ExtendedGCHandle> dictionary = fDictionary;

                    var result = new List<TValue>();
                    foreach (ExtendedGCHandle wr in dictionary.Values)
                    {
                        var item = (TValue) wr.TargetAllowingExpiration;
                        if (item != null)
                            result.Add(item);
                    }
                    return result;
                }
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Clear

        /// <summary>
        /// Clears all items in this dictionary.
        /// </summary>
        public void Clear()
        {
            lock (DisposeLock)
            {
                CheckUndisposed();

                Dictionary<TKey, ExtendedGCHandle> dictionary = fDictionary;
                foreach (ExtendedGCHandle wr in dictionary.Values)
                    wr.Free();

                dictionary.Clear();
            }
        }

        #endregion

        #region Add

        /// <summary>
        /// Adds an item to this dictionary. Throws an exception if an item
        /// with the same key already exists.
        /// </summary>
        public void Add(TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            if (value == null)
                throw new ArgumentNullException("value");

            lock (DisposeLock)
            {
                CheckUndisposed();

                Dictionary<TKey, ExtendedGCHandle> dictionary = fDictionary;
                ExtendedGCHandle wr;
                if (dictionary.TryGetValue(key, out wr))
                {
                    if (wr.IsAlive)
                        throw new ArgumentException("An element with the same key \"" + key + "\" already exists.");

                    wr.Target = value;
                }
                else
                    p_Add(key, value, dictionary);
            }
        }

        #endregion

        #region Remove

        /// <summary>
        /// Removes an item with the given key from the dictionary.
        /// </summary>
        public bool Remove(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            lock (DisposeLock)
            {
                CheckUndisposed();

                return p_Remove(key);
            }
        }

        #endregion

        #region ContainsKey

        /// <summary>
        /// Gets a value indicating if an item with the specified key exists.
        /// </summary>
        public bool ContainsKey(TKey key)
        {
            return fDictionary.ContainsKey(key);
        }

        #endregion

        #region GetEnumerator

        /// <summary>
        /// Gets an enumerator with all key/value pairs that exist in
        /// this dictionary.
        /// </summary>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return ToList().GetEnumerator();
        }

        #endregion

        #region ToList

        /// <summary>
        /// Gets a list with all non-collected key/value pairs.
        /// </summary>
        public List<KeyValuePair<TKey, TValue>> ToList()
        {
            lock (DisposeLock)
            {
                CheckUndisposed();

                Dictionary<TKey, ExtendedGCHandle> dictionary = fDictionary;
                var result = new List<KeyValuePair<TKey, TValue>>();
                foreach (var pair in dictionary)
                {
                    var target = (TValue) pair.Value.TargetAllowingExpiration;
                    if (target != null)
                        result.Add(new KeyValuePair<TKey, TValue>(pair.Key, target));
                }
                return result;
            }
        }

        #endregion

        #region p_Add

        private static void p_Add(TKey key, TValue value, Dictionary<TKey, ExtendedGCHandle> dictionary)
        {
            try
            {
            }
            finally
            {
                var wr = new ExtendedGCHandle(value);
                try
                {
                    dictionary.Add(key, wr);
                }
                catch
                {
                    wr.Free();
                    throw;
                }
            }
        }

        #endregion

        #region p_Remove

        private bool p_Remove(TKey key)
        {
            Dictionary<TKey, ExtendedGCHandle> dictionary = fDictionary;
            ExtendedGCHandle wr;
            if (!dictionary.TryGetValue(key, out wr))
                return false;

            wr.Free();
            return dictionary.Remove(key);
        }

        #endregion

        #endregion

        #region Interfaces

        /// <summary>
        /// Creates the dictionary from serialization info.
        /// Actually, it does not load anything, as if everything was
        /// collected.
        /// </summary>
        protected WeakDictionary(SerializationInfo info, StreamingContext context) :
            this()
        {
        }

        #region IDictionary<TKey,TValue> Members

        public bool TryGetValue(TKey key, out TValue value)
        {
            value = this[key];
            return value != null;
        }

        TValue IDictionary<TKey, TValue>.this[TKey key]
        {
            get
            {
                TValue result = this[key];
                if (result == null)
                    throw new KeyNotFoundException("The given key \"" + key + "\" was not found in the dictionary.");

                return result;
            }
            set { this[key] = value; }
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            if (item.Value == null)
                return false;

            ExtendedGCHandle wr;
            if (!fDictionary.TryGetValue(item.Key, out wr))
                return false;

            return Equals(wr.TargetAllowingExpiration, item.Value);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ToList().CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get { return false; }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            if (item.Value == null)
                return false;

            lock (DisposeLock)
            {
                CheckUndisposed();

                ExtendedGCHandle wr;
                Dictionary<TKey, ExtendedGCHandle> dictionary = fDictionary;
                if (!dictionary.TryGetValue(item.Key, out wr))
                    return false;

                if (!Equals(wr.TargetAllowingExpiration, item.Value))
                    return false;

                return p_Remove(item.Key);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region ISerializable Members

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            lock (DisposeLock)
            {
                CheckUndisposed();

                GetObjectData(info, context);
            }
        }

        #endregion

        /// <summary>
        /// It is here to be inherited. Actually, this does not
        /// add anything, as if everything was collected.
        /// </summary>
        protected virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }

        #endregion
    }
}