using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Perenis.Core.General;

namespace Perenis.Core.Caching
{
    /// <summary>
    /// This class is similar to the HashSet class, but allows items to
    /// be collected. It was redesigned and rewriten from base and very very wrong implementation. There is
    /// a lot of space for optimalization and more clever approach, but it works now.
    /// </summary>
    [Obsolete]
    [Serializable]
    public class WeakHashSet<TValue> :
        ThreadSafeDisposable,
        IEnumerable<TValue>,
        IEnumerable,
        ISerializable
        where
            TValue : class
    {
        #region Private dictionary

        private volatile Dictionary<int, List<ExtendedGCHandle>> fDictionary = new Dictionary<int, List<ExtendedGCHandle>>();

        #endregion

        #region Constructor

        /// <summary>
        /// Creates the hashset.
        /// </summary>
        public WeakHashSet()
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

            Dictionary<int, List<ExtendedGCHandle>> dictionary = fDictionary;
            if (dictionary != null)
            {
                fDictionary = null;

                foreach (var list in dictionary.Values)
                    foreach (ExtendedGCHandle gcHandle in list)
                        gcHandle.Free();
            }

            base.Dispose(disposing);
        }

        #endregion

        #region OnCollected

        private void OnCollected()
        {
            lock (DisposeLock)
            {
                if (WasDisposed) return;

                ExtendedGCHandle notAliveHandle;
                if (!TryFindHandle(handle => !handle.IsAlive, out notAliveHandle)) return;

                Dictionary<int, List<ExtendedGCHandle>> oldDictionary = fDictionary;
                var newDictionary = new Dictionary<int, List<ExtendedGCHandle>>();

                foreach (var dictionaryPair in oldDictionary)
                {
                    List<ExtendedGCHandle> oldList = dictionaryPair.Value;
                    var newList = new List<ExtendedGCHandle>(oldList.Count);
                    foreach (ExtendedGCHandle gcHandle in oldList)
                    {
                        if (gcHandle.IsAlive)
                            newList.Add(gcHandle);
                        else
                            gcHandle.Free();
                    }

                    if (newList.Count > 0)
                        newDictionary.Add(dictionaryPair.Key, newList);
                }

                fDictionary = newDictionary;
            }
        }

        #endregion

        #region Methods

        #region Contains

        /// <summary>
        /// Verifies if this hashset contains an specific item.
        /// </summary>
        /// <param name="item">The item to check to be part of this hashset.</param>
        /// <returns>true if the item is in this hashset, false otherwise.</returns>
        public bool Contains(TValue item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            int hashCode = item.GetHashCode();
            lock (DisposeLock)
            {
                CheckUndisposed();
                ExtendedGCHandle gcHandle;
                return TryFindHandle(handle => handle.Target.Equals(item), out gcHandle);
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
        public bool Add(TValue item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            int hashCode = item.GetHashCode();
            lock (DisposeLock)
            {
                CheckUndisposed();

                Dictionary<int, List<ExtendedGCHandle>> dictionary = fDictionary;
                List<ExtendedGCHandle> items;
                if (!dictionary.TryGetValue(hashCode, out items))
                {
                    items = new List<ExtendedGCHandle>(1);
                    dictionary.Add(hashCode, items);
                    items.Add(new ExtendedGCHandle(item));
                    return true;
                }

                foreach (ExtendedGCHandle gcHandle in items)
                    if (gcHandle.Target.Equals(item))
                        return false;

                items.Add(new ExtendedGCHandle(item));
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
        public bool Remove(TValue item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            int hashCode = item.GetHashCode();
            lock (DisposeLock)
            {
                CheckUndisposed();

                List<ExtendedGCHandle> items;
                if (!fDictionary.TryGetValue(hashCode, out items))
                    return false;

                int count = items.Count;
                for (int i = 0; i < count; i++)
                {
                    if (items[i].Target == item)
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
        public IEnumerator<TValue> GetEnumerator()
        {
            lock (DisposeLock)
            {
                CheckUndisposed();

                var result = new List<TValue>(fDictionary.Count);

                Dictionary<int, List<ExtendedGCHandle>> dictionary = fDictionary;
                foreach (var items in dictionary.Values)
                    foreach (ExtendedGCHandle gcHandle in items)
                        result.Add((TValue) gcHandle.Target);

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
            GCUtils.Collected += OnCollected;
        }

        #region IEnumerable<TValue> Members

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

        private bool TryFindHandle(Predicate<ExtendedGCHandle> condition, out ExtendedGCHandle handleFound)
        {
            foreach (var list in fDictionary.Values)
                foreach (ExtendedGCHandle gcHandle in list)
                    if (condition(gcHandle))
                    {
                        handleFound = gcHandle;
                        return true;
                    }
            handleFound = default(ExtendedGCHandle);
            return false;
        }
    }
}