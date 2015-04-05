using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;
using Perenis.Core.Serialization;

namespace Perenis.Core.Collections
{
    /// <summary>
    /// Provides a <typeparamref name="TUniqueKey"/>-indexed collection of items of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of items of this collection</typeparam>
    /// <typeparam name="TUniqueKey">The type of the unique key.</typeparam>
    [Serializable]
    public abstract class IndexedList<T, TUniqueKey> : IList<T>
        where T : class
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        protected IndexedList()
        {
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        protected IndexedList(IEnumerable<T> items)
        {
            AddRange(items);
        }

        /// <summary>
        /// Adds the elements of the specified collection to the end of this list.
        /// </summary>
        /// <param name="items">The collection of elements to be added.</param>
        /// <exception cref="ArgumentNullException"><paramref name=Constants.ItemsName/> is a null reference.</exception>
        public void AddRange(IEnumerable<T> items)
        {
            if (items == null) throw new ArgumentNullException("items");
            foreach (T item in items) Add(item);
        }

        #region ------ Interface for descendants --------------------------------------------------

        /// <summary>
        /// Indicates if the collection contains an item with the given unique <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The unique key of an item</param>
        /// <returns><c>true</c> if the collection contains such an item, otherwise <c>false</c>.</returns>
        protected bool InternalContains(TUniqueKey key)
        {
            return itemIndex.ContainsKey(key);
        }

        /// <summary>
        /// Gets the item with the given <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The index of the item to look for.</param>
        /// <returns>The item with the given <paramref name="index"/> or a null reference if 
        /// not found.</returns>
        protected T InternalGet(TUniqueKey index)
        {
            T value;
            return itemIndex.TryGetValue(index, out value) ? value : null;
        }

        /// <summary>
        /// Called when the given <paramref name="item"/> is being added into this collection.
        /// </summary>
        /// <param name="item">The item ti be added to the collection.</param>
        protected virtual void OnItemAdding(T item)
        {
            // intentionally left blank
        }

        /// <summary>
        /// Called when the given <paramref name="item"/> has been added into this collection.
        /// </summary>
        /// <param name="item">The item just added to the collection.</param>
        protected virtual void OnItemAdded(T item)
        {
            // intentionally left blank
        }

        /// <summary>
        /// Called when the given <paramref name="item"/> is being removed from this collection.
        /// </summary>
        /// <param name="item">The item to be removed from the collection.</param>
        protected virtual void OnItemRemoving(T item)
        {
            // intentionally left blank
        }

        /// <summary>
        /// Called when the given <paramref name="item"/> has been removed from this collection.
        /// </summary>
        /// <param name="item">The item just removed from the collection.</param>
        protected virtual void OnItemRemoved(T item)
        {
            // intentionally left blank
        }

        /// <summary>
        /// Called when the given <paramref name=Constants.ItemsName/> are being removed from this collection.
        /// </summary>
        /// <param name=Constants.ItemsName>The items to be removed from the collection.</param>
        protected virtual void OnItemsRemoving(ICollection<T> items)
        {
            // intentionally left blank
        }

        /// <summary>
        /// Called when the given <paramref name=Constants.ItemsName/> have been removed from this collection.
        /// </summary>
        /// <param name=Constants.ItemsName>The items just removed from the collection.</param>
        protected virtual void OnItemsRemoved(ICollection<T> items)
        {
            // intentionally left blank
        }

        /// <summary>
        /// Called when the given <paramref name="item"/> is being changed.
        /// </summary>
        /// <param name="item">The just changed item.</param>
        protected virtual void OnItemChanging(T item)
        {
            // intentionally left blank
        }

        /// <summary>
        /// Called when the given <paramref name="item"/> has been changed.
        /// </summary>
        /// <param name="item">The just changed item.</param>
        protected virtual void OnItemChanged(T item)
        {
            // intentionally left blank
        }

        /// <summary>
        /// Resolved the unique ID of the given <paramref name="item"/>.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected abstract TUniqueKey GetItemUniqueKey(T item);

        #endregion

        #region ------ Internals: Internal notification facility ----------------------------------

        /// <summary>
        /// Called when the given <paramref name="item"/> is being added into this collection.
        /// </summary>
        /// <param name="item">The item to be added to the collection.</param>
        /// <param name="index">The explicit index of the item or -1 if not yet known.</param>
        protected void OnItemAddingInternal(T item, int index)
        {
            OnItemAdding(item);
        }

        /// <summary>
        /// Called when the given <paramref name="item"/> is added into this collection.
        /// </summary>
        /// <param name="item">The item just added to the collection.</param>
        /// <param name="index">The present index of the item.</param>
        /// <remarks>
        /// This method invokes the <see cref="OnItemAdded"/> method and fires the corresponding
        /// <see cref="INotifyListChanged.ListChanged"/> event.
        /// </remarks>
        protected void OnItemAddedInternal(T item, int index)
        {
            OnItemAdded(item);
            if (listChanged != null) listChanged(this, new ListChangedEventArgsEx(ListChangedType.ItemAdded, item, index));
        }

        /// <summary>
        /// Called when the given <paramref name="item"/> is being removed from this collection.
        /// </summary>
        /// <param name="item">The item to be removed from the collection.</param>
        /// <param name="index">The former index of the item.</param>
        protected void OnItemRemovingInternal(T item, int index)
        {
            OnItemRemoving(item);
        }

        /// <summary>
        /// Called when the given <paramref name="item"/> is removed from this collection.
        /// </summary>
        /// <param name="item">The item just removed from the collection.</param>
        /// <param name="index">The former index of the item.</param>
        /// <remarks>
        /// This method invokes the <see cref="OnItemRemoved"/> method and fires the corresponding
        /// <see cref="INotifyListChanged.ListChanged"/> event.
        /// </remarks>
        protected void OnItemRemovedInternal(T item, int index)
        {
            OnItemRemoved(item);
            if (listChanged != null) listChanged(this, new ListChangedEventArgsEx(ListChangedType.ItemDeleted, item, index));
        }

        /// <summary>
        /// Called when the given <paramref name="oldItem"/> is being replaced by the <paramref name="newItem"/>.
        /// </summary>
        /// <param name="oldItem">The item to be removed from the collection.</param>
        /// <param name="newItem">The item to be added to the collection.</param>
        /// <param name="index">The index of the item.</param>
        protected void OnItemReplacingInternal(T oldItem, T newItem, int index)
        {
            OnItemRemoving(oldItem);
            OnItemAdding(newItem);
        }

        /// <summary>
        /// Called when the given <paramref name="oldItem"/> is replaced by the <paramref name="newItem"/>.
        /// </summary>
        /// <param name="oldItem">The item just removed from the collection.</param>
        /// <param name="newItem">The item just added to the collection.</param>
        /// <param name="index">The index of the item.</param>
        /// <remarks>
        /// This method invokes the <see cref="OnItemRemoved"/> and <see cref="OnItemAdded"/> methods 
        /// and fires the corresponding <see cref="INotifyListChanged.ListChanged"/> event.
        /// </remarks>
        protected void OnItemReplacedInternal(T oldItem, T newItem, int index)
        {
            OnItemRemoved(oldItem);
            OnItemAdded(newItem);
            if (listChanged != null) listChanged(this, new ListChangedEventArgsEx(ListChangedType.ItemChanged, newItem, index));
        }

        /// <summary>
        /// Called when the given <paramref name="item"/> is being changed.
        /// </summary>
        /// <param name="item">The just changed item.</param>
        /// <param name="index">The index of the item.</param>
        protected void OnItemChangingInternal(T item, int index)
        {
            OnItemChanging(item);
        }

        /// <summary>
        /// Called when the given <paramref name="item"/> has been changed.
        /// </summary>
        /// <param name="item">The just changed item.</param>
        /// <param name="index">The index of the item.</param>
        /// <remarks>
        /// This method invokes the <see cref="OnItemChanged"/> method and fires the corresponding
        /// <see cref="INotifyListChanged.ListChanged"/> event.
        /// </remarks>
        protected void OnItemChangedInternal(T item, int index)
        {
            OnItemChanged(item);
            if (listChanged != null) listChanged(this, new ListChangedEventArgsEx(ListChangedType.ItemChanged, item, index));
        }

        /// <summary>
        /// Called when the given <paramref name=Constants.ItemsName/> are being removed from this collection.
        /// </summary>
        /// <param name=Constants.ItemsName>The items just removed from the collection.</param>
        protected void OnClearingInternal(ICollection<T> items)
        {
            OnItemsRemoving(items);
        }

        /// <summary>
        /// Called when the given <paramref name=Constants.ItemsName/> have been removed from this collection.
        /// </summary>
        /// <param name=Constants.ItemsName>The items just removed from the collection.</param>
        /// <remarks>
        /// This method invokes the <see cref="OnItemsRemoved"/> method and fires the corresponding
        /// <see cref="INotifyListChanged.ListChanged"/> event.
        /// </remarks>
        protected void OnClearedInternal(ICollection<T> items)
        {
            OnItemsRemoved(items);
            if (listChanged != null) listChanged(this, new ListChangedEventArgsEx(ListChangedType.Reset, null, -1));
        }

        #endregion

        #region ------ Implementation of the IList<T> interface -----------------------------------

        /// <exception cref="ArgumentNullException"><paramref name="item"/> is a null reference.</exception>
        public int IndexOf(T item)
        {
            if (item == null) throw new ArgumentNullException("item");
            return items.IndexOf(item);
        }

        /// <exception cref="ArgumentNullException"><paramref name="item"/> is a null reference.</exception>
        public void Insert(int index, T item)
        {
            if (item == null) throw new ArgumentNullException("item");
            OnItemAddingInternal(item, index);
            items.Insert(index, item);
            itemIndex.Add(GetItemUniqueKey(item), item);
            OnItemAddedInternal(item, index);
        }

        public void RemoveAt(int index)
        {
            T item = items[index];
            OnItemRemovingInternal(item, index);
            itemIndex.Remove(GetItemUniqueKey(items[index]));
            items.RemoveAt(index);
            OnItemRemovedInternal(item, index);
        }

        /// <exception cref="ArgumentNullException"><paramref name="value"/> is a null reference.</exception>
        public T this[int index]
        {
            get { return items[index]; }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                T oldItem = items[index];
                OnItemReplacingInternal(oldItem, value, index);
                itemIndex.Remove(GetItemUniqueKey(items[index]));
                items[index] = value;
                itemIndex.Add(GetItemUniqueKey(value), value);
                OnItemReplacedInternal(oldItem, value, index);
            }
        }

        #endregion

        #region ------ Implementation of the ICollection<T> interface -----------------------------

        // TODO Remove when System.Xml.XmlSerializer is replaced.

        /// <exception cref="ArgumentNullException"><paramref name="item"/> is a null reference.</exception>
        public void Add(T item)
        {
            if (item == null) throw new ArgumentNullException("item");
            OnItemAddingInternal(item, -1);
            itemIndex.Add(GetItemUniqueKey(item), item);
            items.Add(item);
            OnItemAddedInternal(item, items.IndexOf(item));
        }

        public void Clear()
        {
            List<T> oldItems = items;
            OnClearingInternal(oldItems);
            itemIndex.Clear();
            items = new List<T>();
            OnClearedInternal(oldItems);
        }

        /// <exception cref="ArgumentNullException"><paramref name="item"/> is a null reference.</exception>
        public bool Contains(T item)
        {
            if (item == null) throw new ArgumentNullException("item");
            return items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        /// <exception cref="ArgumentNullException"><paramref name="item"/> is a null reference.</exception>
        public bool Remove(T item)
        {
            if (item == null) throw new ArgumentNullException("item");
            int index = items.IndexOf(item);
            if (index >= 0)
            {
                OnItemRemovingInternal(item, index);
                bool check = items.Remove(item);
                Debug.Assert(check);
                itemIndex.Remove(GetItemUniqueKey(item));
                OnItemRemovedInternal(item, index);
                return true;
            }
            else
            {
                return false;
            }
        }

        public int Count
        {
            get { return items.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        [Obsolete("Do not use, for System.Xml.XmlSerializer purposes only.")]
        public void Add(object item)
        {
            Add((T) item);
        }

        #endregion

        #region ------ Implementation of the IEnumerable<T> interface -----------------------------

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return ((IEnumerable<T>) items).GetEnumerator();
        }

        #endregion

        #region ------ Implementation of the IEnumerable interface --------------------------------

        public IEnumerator GetEnumerator()
        {
            return items.GetEnumerator();
        }

        #endregion

        #region ------ Implementation of the INotifyListChanged interface -------------------------

        [NonSerialized] private ListChangedEventHandler listChanged;

        public event ListChangedEventHandler ListChanged
        {
            add { listChanged += value; }
            remove { listChanged -= value; }
        }

        #endregion

        #region ------ Internals: Storage ---------------------------------------------------------

        /// <summary>
        /// A <see cref="Guid"/>-indexed dictionary of items.
        /// </summary>
        [NonSerialized] protected Dictionary<TUniqueKey, T> itemIndex = new Dictionary<TUniqueKey, T>();

        /// <summary>
        /// Items of this collection.
        /// </summary>
        protected List<T> items = new List<T>();

        #endregion

        #region ------ Internals: Deserialization -------------------------------------------------

        [OnDeserialized]
        protected void OnDeserialized(StreamingContext context)
        {
            // [NonSerializable] fields don't get auto-initialized upon deserialization...
            itemIndex = new Dictionary<TUniqueKey, T>();
            foreach (T item in items)
            {
                itemIndex.Add(GetItemUniqueKey(item), item);
            }
        }

        #endregion
    }
}