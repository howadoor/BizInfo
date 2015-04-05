using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Perenis.Core.General;

namespace Perenis.Core.Caching.Obsolete
{
    /// <summary>
    /// This class is similar to the HashSet class, but allows items to
    /// be collected.
    /// THIS CLASS IS COMPLETELY WRONG AND MUST BE REIMPLEMENTED
    /// WeakDictionary used for storing lists has a weak reference to values, so lists stored
    /// in the dictionary are garbage collected independently on the fact, if containing items
    /// of that lists are alive or not.
    /// </summary>
    [Obsolete]
    [Serializable]
    public class WeakHashSet<T> :
        ThreadSafeDisposable,
        IEnumerable<T>,
        IEnumerable,
        ISerializable
        where
            T : class
    {
        #region Private dictionary

        private volatile WeakDictionary<int, List<T>> fDictionary = new WeakDictionary<int, List<T>>();

        #endregion

        #region Constructor

        /// <summary>
        /// Creates the hashset.
        /// </summary>
        public WeakHashSet()
        {
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Frees all handles.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                WeakDictionary<int, List<T>> dictionary = fDictionary;
                if (dictionary != null)
                {
                    fDictionary = null;

                    dictionary.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        #region Methods

        #region Contains

        /// <summary>
        /// Verifies if this hashset contains an specific item.
        /// </summary>
        /// <param name="item">The item to check to be part of this hashset.</param>
        /// <returns>true if the item is in this hashset, false otherwise.</returns>
        public bool Contains(T item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            int hashCode = item.GetHashCode();
            lock (DisposeLock)
            {
                CheckUndisposed();

                List<T> items = fDictionary[hashCode];
                if (items == null)
                    return false;

                foreach (T otherItem in items)
                    if (otherItem.Equals(item))
                        return true;

                return false;
            }
        }

        #endregion

        #region Add

        /// <summary>
        /// Adds an item to this hashset.
        /// </summary>
        /// <param name="item">The item to add in the hashset.</param>
        /// <returns>
        /// Returns true if the item was added, or false if it was already
        /// in the hashset.
        /// </returns>
        public bool Add(T item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            int hashCode = item.GetHashCode();
            lock (DisposeLock)
            {
                CheckUndisposed();

                WeakDictionary<int, List<T>> dictionary = fDictionary;
                List<T> items = dictionary[hashCode];
                if (items == null)
                {
                    items = new List<T>(1);
                    dictionary.Add(hashCode, items);
                    items.Add(item);
                    return true;
                }

                foreach (T otherItem in items)
                    if (item.Equals(otherItem))
                        return false;

                items.Add(item);
                return true;
            }
        }

        #endregion

        #region Remove

        /// <summary>
        /// Removes an item from this hashset.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns>true if the item was present in the hashset, false otherwise.</returns>
        public bool Remove(T item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            int hashCode = item.GetHashCode();
            lock (DisposeLock)
            {
                CheckUndisposed();

                List<T> items = fDictionary[hashCode];
                if (items == null)
                    return false;

                int count = items.Count;
                for (int i = 0; i < count; i++)
                {
                    if (item == items[i])
                    {
                        items.RemoveAt(i);
                        return true;
                    }
                }

                return false;
            }
        }

        #endregion

        #region GetEnumerator

        /// <summary>
        /// Gets an enumerator with all items that exist in this hashset.
        /// Different from the common HashSet, they are not guaranteed to 
        /// be in the order they were added.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            lock (DisposeLock)
            {
                CheckUndisposed();

                var result = new List<T>(fDictionary.Count);

                WeakDictionary<int, List<T>> dictionary = fDictionary;
                foreach (var items in dictionary.Values)
                    foreach (T item in items)
                        result.Add(item);

                return result.GetEnumerator();
            }
        }

        #endregion

        #endregion

        /// <summary>
        /// Creates the hashset from serialization info.
        /// Actually, it does not load anything, as if everything was
        /// collected.
        /// </summary>
        public WeakHashSet(SerializationInfo info, StreamingContext context) :
            this()
        {
        }

        #region IEnumerable<T> Members

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
    }
}