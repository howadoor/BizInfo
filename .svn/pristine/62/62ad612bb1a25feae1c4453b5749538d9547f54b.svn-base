using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Perenis.Core.Collections
{
    /// <summary>
    /// Thread safe envelope arround list
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <typeparam name="TList"></typeparam>
    public class ThreadSafeList<TItem, TList> : IList<TItem> where TList : class, IList<TItem>, new()
    {
        private readonly TList list = new TList();

        /// <summary>
        /// Implementation of list used
        /// </summary>
        [XmlIgnore]
        public TList List
        {
            get { return list; }
        }

        #region IList<TItem> Members

        public IEnumerator<TItem> GetEnumerator()
        {
            lock (list) return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public void Add(TItem item)
        {
            lock (list) list.Add(item);
        }

        public void Clear()
        {
            lock (list) list.Clear();
        }

        public bool Contains(TItem item)
        {
            lock (list) return list.Contains(item);
        }

        public void CopyTo(TItem[] array, int arrayIndex)
        {
            lock (list) list.CopyTo(array, arrayIndex);
        }

        public bool Remove(TItem item)
        {
            lock (list) return list.Remove(item);
        }

        public int Count
        {
            get { lock (list) return list.Count; }
        }

        public bool IsReadOnly
        {
            get { lock (list) return ((IList) list).IsReadOnly; }
        }

        public int IndexOf(TItem item)
        {
            lock (list) return list.IndexOf(item);
        }

        public void Insert(int index, TItem item)
        {
            lock (list) list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            lock (list) list.RemoveAt(index);
        }

        public TItem this[int index]
        {
            get { lock (list) return list[index]; }
            set { lock (list) list[index] = value; }
        }

        #endregion
    }

    public class ThreadSafeList<TItem> : ThreadSafeList<TItem, List<TItem>>
    {
    }
}