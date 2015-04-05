using System;
using System.Collections;
using System.Collections.Generic;
using Perenis.Core.General;

namespace Perenis.Core.Caching
{
    /// <summary>
    /// A dictionary were keys are weakreferences. This is useful if you need
    /// to "extend" existing classes. For example, if you want to add a Tag
    /// property to any object. The way this dictionary works, you can add
    /// items to a given object, which will be kept while the object is alive,
    /// but if the object dies (is collected) they will be allowed to be 
    /// with it.
    /// </summary>
    /// <typeparam name="TKey">The type of the key, which must be a class.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public class WeakKeyDictionary<TKey, TValue> :
        ThreadSafeDisposable,
        IDictionary<TKey, TValue>
        where
            TKey : class
    {
        #region Private dictionary

        private volatile Dictionary<int, List<KeyValuePair<ExtendedGCHandle, TValue>>> fDictionary = new Dictionary<int, List<KeyValuePair<ExtendedGCHandle, TValue>>>();

        #endregion

        #region Constructor

        /// <summary>
        /// Creates the WeakKeyDictionary.
        /// </summary>
        public WeakKeyDictionary()
        {
            GCUtils.Collected += OnCollected;
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Frees all handles.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                GCUtils.Collected -= OnCollected;

            Dictionary<int, List<KeyValuePair<ExtendedGCHandle, TValue>>> dictionary = fDictionary;
            if (dictionary != null)
            {
                fDictionary = null;

                foreach (var list in dictionary.Values)
                    foreach (var pair in list)
                        pair.Key.Free();
            }

            base.Dispose(disposing);
        }

        #endregion

        #region OnCollected

        private void OnCollected()
        {
            lock (DisposeLock)
            {
                if (WasDisposed)
                    return;

                KeyValuePair<ExtendedGCHandle, TValue> notAlivePair;
                if (!TryFindPair(pair => !pair.Key.IsAlive, out notAlivePair)) return;

                Dictionary<int, List<KeyValuePair<ExtendedGCHandle, TValue>>> oldDictionary = fDictionary;
                var newDictionary = new Dictionary<int, List<KeyValuePair<ExtendedGCHandle, TValue>>>();

                foreach (var dictionaryPair in oldDictionary)
                {
                    List<KeyValuePair<ExtendedGCHandle, TValue>> oldList = dictionaryPair.Value;
                    var newList = new List<KeyValuePair<ExtendedGCHandle, TValue>>(oldList.Count);
                    foreach (var pair in oldList)
                    {
                        ExtendedGCHandle key = pair.Key;
                        if (key.IsAlive)
                            newList.Add(pair);
                        else
                            key.Free();
                    }

                    if (newList.Count > 0)
                        newDictionary.Add(dictionaryPair.Key, newList);
                }

                fDictionary = newDictionary;
            }
        }

        #endregion

        #region Properties

        #region Count

        /// <summary>
        /// Gets the number of the items in the dictionary. This value
        /// is not that useful, as just after getting it the number of 
        /// items can change by a collection.
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
        /// Gets or sets a value for the given key.
        /// While getting, if the value does not exist an exception is thrown.
        /// This can happen if the value was collected, so avoid using getter,
        /// use TryGetValue instead.
        /// </summary>
        /// <param name="key">The key.</param>
        public TValue this[TKey key]
        {
            get
            {
                if (key == null)
                    throw new ArgumentNullException("key");

                int hashCode = key.GetHashCode();

                lock (DisposeLock)
                {
                    CheckUndisposed();

                    List<KeyValuePair<ExtendedGCHandle, TValue>> list;
                    if (fDictionary.TryGetValue(hashCode, out list))
                    {
                        foreach (var pair in list)
                        {
                            ExtendedGCHandle handle = pair.Key;
                            object target = handle.TargetAllowingExpiration;
                            if (target == key)
                                return pair.Value;
                        }
                    }

                    throw new KeyNotFoundException("The given key \"" + key + "\" was not found in dictionary.");
                }
            }
            set
            {
                if (key == null)
                    throw new ArgumentNullException("key");

                int hashCode = key.GetHashCode();

                lock (DisposeLock)
                {
                    CheckUndisposed();

                    Dictionary<int, List<KeyValuePair<ExtendedGCHandle, TValue>>> dictionary = fDictionary;
                    List<KeyValuePair<ExtendedGCHandle, TValue>> list;
                    if (!dictionary.TryGetValue(hashCode, out list))
                    {
                        list = new List<KeyValuePair<ExtendedGCHandle, TValue>>(1);
                        dictionary.Add(hashCode, list);
                    }

                    int count = list.Count;
                    for (int i = 0; i < count; i++)
                    {
                        KeyValuePair<ExtendedGCHandle, TValue> pair = list[i];

                        ExtendedGCHandle handle = pair.Key;
                        object target = handle.TargetAllowingExpiration;
                        if (target == key)
                        {
                            pair = new KeyValuePair<ExtendedGCHandle, TValue>(pair.Key, value);
                            list[i] = pair;
                            return;
                        }
                    }

                    try
                    {
                    }
                    finally
                    {
                        // this code is in a finally block, so if an abort is
                        // done between newHandle and try we will not leak
                        // memory.
                        var newHandle = new ExtendedGCHandle(key);
                        try
                        {
                            var newPair = new KeyValuePair<ExtendedGCHandle, TValue>(newHandle, value);
                            list.Add(newPair);
                        }
                        catch
                        {
                            newHandle.Free();
                            throw;
                        }
                    }
                }
            }
        }

        #endregion

        #region Keys

        /// <summary>
        /// Returns all the non-collected keys.
        /// </summary>
        public ICollection<TKey> Keys
        {
            get
            {
                lock (DisposeLock)
                {
                    CheckUndisposed();

                    var keys = new List<TKey>();
                    Dictionary<int, List<KeyValuePair<ExtendedGCHandle, TValue>>> dictionary = fDictionary;
                    foreach (var list in dictionary.Values)
                    {
                        foreach (var pair in list)
                        {
                            ExtendedGCHandle keyHandle = pair.Key;
                            object key = keyHandle.TargetAllowingExpiration;
                            if (key != null)
                                keys.Add((TKey) key);
                        }
                    }
                    return keys;
                }
            }
        }

        #endregion

        #region Values

        /// <summary>
        /// Gets all the values still alive in this dictionary.
        /// </summary>
        public ICollection<TValue> Values
        {
            get
            {
                lock (DisposeLock)
                {
                    CheckUndisposed();

                    var result = new List<TValue>();

                    Dictionary<int, List<KeyValuePair<ExtendedGCHandle, TValue>>> dictionary = fDictionary;
                    foreach (var list in dictionary.Values)
                        foreach (var pair in list)
                            result.Add(pair.Value);

                    return result;
                }
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Clear

        /// <summary>
        /// Clears all items in the dictionary.
        /// </summary>
        public void Clear()
        {
            lock (DisposeLock)
            {
                CheckUndisposed();

                Dictionary<int, List<KeyValuePair<ExtendedGCHandle, TValue>>> dictionary = fDictionary;
                foreach (var list in dictionary.Values)
                    foreach (var pair in list)
                        pair.Key.Free();

                dictionary.Clear();
            }
        }

        #endregion

        #region Add

        /// <summary>
        /// Adds an item to the dictionary, or throws an exception if an item
        /// with the same key already exists.
        /// </summary>
        /// <param name="key">The key of the item to add.</param>
        /// <param name="value">The value of the item to add.</param>
        public void Add(TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            int hashCode = key.GetHashCode();

            lock (DisposeLock)
            {
                CheckUndisposed();

                Dictionary<int, List<KeyValuePair<ExtendedGCHandle, TValue>>> dictionary = fDictionary;
                List<KeyValuePair<ExtendedGCHandle, TValue>> list;
                if (!dictionary.TryGetValue(hashCode, out list))
                {
                    list = new List<KeyValuePair<ExtendedGCHandle, TValue>>(1);
                    dictionary.Add(hashCode, list);
                }

                foreach (var pair in list)
                {
                    ExtendedGCHandle handle = pair.Key;
                    object target = handle.TargetAllowingExpiration;
                    if (target == key)
                        throw new ArgumentException("An item with the same key \"" + key + "\" already exists in the dictionary.");
                }

                try
                {
                }
                finally
                {
                    // this code is in a finally block, so if a Thread.Abort()
                    // is done between newHandle and try we will not leak
                    // memory.
                    var newHandle = new ExtendedGCHandle(key);
                    try
                    {
                        var newPair = new KeyValuePair<ExtendedGCHandle, TValue>(newHandle, value);
                        list.Add(newPair);
                    }
                    catch
                    {
                        newHandle.Free();
                        throw;
                    }
                }
            }
        }

        #endregion

        #region Remove

        /// <summary>
        /// Tries to remove an item from the dictionary, and returns a value 
        /// indicating if an item with the specified key existed.
        /// </summary>
        /// <param name="key">The key of the item to remove.</param>
        /// <returns>true if an item with the given key existed, false otherwise.</returns>
        public bool Remove(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            int hashCode = key.GetHashCode();

            lock (DisposeLock)
            {
                CheckUndisposed();

                Dictionary<int, List<KeyValuePair<ExtendedGCHandle, TValue>>> dictionary = fDictionary;
                List<KeyValuePair<ExtendedGCHandle, TValue>> list;
                if (!dictionary.TryGetValue(hashCode, out list))
                    return false;

                int count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    KeyValuePair<ExtendedGCHandle, TValue> pair = list[i];

                    ExtendedGCHandle handle = pair.Key;
                    object target = handle.TargetAllowingExpiration;
                    if (target == key)
                    {
                        // if the item exists, we simple set the handle target
                        // to null. We do not remove the item now, as the
                        // OnCollected does this.
                        handle.TargetAllowingExpiration = null;
                        GCUtils.Expire(key);
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion

        #region ContainsKey

        /// <summary>
        /// Checks if an item with the given key exists in this dictionary.
        /// </summary>
        public bool ContainsKey(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            int hashCode = key.GetHashCode();

            lock (DisposeLock)
            {
                CheckUndisposed();

                Dictionary<int, List<KeyValuePair<ExtendedGCHandle, TValue>>> dictionary = fDictionary;
                List<KeyValuePair<ExtendedGCHandle, TValue>> list;
                if (!dictionary.TryGetValue(hashCode, out list))
                    return false;

                foreach (var pair in list)
                    if (pair.Key.TargetAllowingExpiration == key)
                        return true;
            }

            return false;
        }

        #endregion

        #region TryGetValue

        /// <summary>
        /// Tries to get a value with a given key.
        /// </summary>
        /// <param name="key">The key of the item to try to get.</param>
        /// <param name="value">
        /// The variable that will receive the found value, or the default value 
        /// if an item with the given key does not exist.
        /// </param>
        /// <returns>
        /// true if an item with the given key was found and stored in value
        /// parameter, false otherwise.
        /// </returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            int hashCode = key.GetHashCode();

            lock (DisposeLock)
            {
                CheckUndisposed();

                List<KeyValuePair<ExtendedGCHandle, TValue>> list;

                Dictionary<int, List<KeyValuePair<ExtendedGCHandle, TValue>>> dictionary = fDictionary;
                if (dictionary.TryGetValue(hashCode, out list))
                {
                    foreach (var pair in list)
                    {
                        if (pair.Key.TargetAllowingExpiration == key)
                        {
                            value = pair.Value;
                            return true;
                        }
                    }
                }
            }

            value = default(TValue);
            return false;
        }

        #endregion

        #region ToList

        /// <summary>
        /// Creates a list with all non-collected keys and values.
        /// </summary>
        public List<KeyValuePair<TKey, TValue>> ToList()
        {
            lock (DisposeLock)
            {
                CheckUndisposed();

                var result = new List<KeyValuePair<TKey, TValue>>();

                Dictionary<int, List<KeyValuePair<ExtendedGCHandle, TValue>>> dictionary = fDictionary;
                foreach (var list in dictionary.Values)
                {
                    foreach (var pair in list)
                    {
                        var key = (TKey) pair.Key.TargetAllowingExpiration;
                        if (key == null)
                            continue;

                        var resultItem = new KeyValuePair<TKey, TValue>(key, pair.Value);
                        result.Add(resultItem);
                    }
                }

                return result;
            }
        }

        #endregion

        #region GetEnumerator

        /// <summary>
        /// Gets an enumerator of all non-collected keys and values.
        /// </summary>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return ToList().GetEnumerator();
        }

        #endregion

        #endregion

        #region IDictionary<TKey,TValue> Members

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            TValue value;
            if (TryGetValue(item.Key, out value))
                return Equals(value, item.Value);

            return false;
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
            TKey key = item.Key;
            if (key == null)
                throw new ArgumentNullException("key");

            int hashCode = key.GetHashCode();

            lock (DisposeLock)
            {
                CheckUndisposed();

                Dictionary<int, List<KeyValuePair<ExtendedGCHandle, TValue>>> dictionary = fDictionary;
                List<KeyValuePair<ExtendedGCHandle, TValue>> list;
                if (!dictionary.TryGetValue(hashCode, out list))
                    return false;

                int count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    KeyValuePair<ExtendedGCHandle, TValue> pair = list[i];

                    ExtendedGCHandle handle = pair.Key;
                    object target = handle.TargetAllowingExpiration;
                    if (target == key)
                    {
                        if (Equals(pair.Value, item.Value))
                        {
                            // if the item exists, we simple set the handle target
                            // to null. We do not remove the item now, as the
                            // OnCollected does this.
                            handle.TargetAllowingExpiration = null;
                            GCUtils.Expire(key);
                            return true;
                        }

                        // we already found the key, but the value is not the
                        // one we expected, so we can already return false.
                        return false;
                    }
                }
            }

            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        private bool TryFindPair(Predicate<KeyValuePair<ExtendedGCHandle, TValue>> condition, out KeyValuePair<ExtendedGCHandle, TValue> pairFound)
        {
            foreach (var list in fDictionary.Values)
                foreach (var pair in list)
                    if (condition(pair))
                    {
                        pairFound = pair;
                        return true;
                    }
            pairFound = default(KeyValuePair<ExtendedGCHandle, TValue>);
            return false;
        }
    }
}