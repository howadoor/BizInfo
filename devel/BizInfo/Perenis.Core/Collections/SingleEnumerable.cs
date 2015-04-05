using System;
using System.Collections;
using System.Collections.Generic;
using Perenis.Core.Pattern;

namespace Perenis.Core.Collections
{
    /// <summary>
    /// Creates a read-only collection wrapper over a single item
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SingleEnumerable<T> : ICollection<T>, IList<T>
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public SingleEnumerable()
        {
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="item">The single item wrapped by this class.</param>
        public SingleEnumerable(T item)
        {
            Item = item;
        }

        #region ------ Implementation of the IEnumerable interface --------------------------------

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region ------ Implementation of the IEnumerable<T> interface -----------------------------

        public IEnumerator<T> GetEnumerator()
        {
            return new SingleEnumerableEnumerator<T>(this);
        }

        #endregion

        #region ------ Implementation of the ICollection interface --------------------------------

        public void Add(T item)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(T item)
        {
            return Equals(Item, item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            array[arrayIndex] = Item;
        }

        public bool Remove(T item)
        {
            throw new NotSupportedException();
        }

        public int Count
        {
            get { return 1; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        #endregion

        #region ------ Implementation of the IList interface --------------------------------------

        public int IndexOf(T item)
        {
            return Equals(item, Item) ? 0 : -1;
        }

        public void Insert(int index, T item)
        {
            throw new NotSupportedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        T IList<T>.this[int index]
        {
            get
            {
                if (index != 0) throw new ArgumentOutOfRangeException("index");
                return Item;
            }
            set { throw new NotSupportedException(); }
        }

        #endregion

        #region ------ Internals ------------------------------------------------------------------

        /// <summary>
        /// Enumerator of <see cref="SingleEnumerable{T}"/> items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class SingleEnumerableEnumerator<T> : Disposable, IEnumerator<T>
        {
            private int index = -1;
            private SingleEnumerable<T> single;

            /// <summary>
            /// Initializes a new instance of this class.
            /// </summary>
            public SingleEnumerableEnumerator(SingleEnumerable<T> single)
            {
                this.single = single;
            }

            #region ------ Implementation of the IEnumerator interface ----------------------------

            public bool MoveNext()
            {
                index++;
                return index == 0;
            }

            public void Reset()
            {
                index = -1;
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public T Current
            {
                get
                {
                    if (index != 0) throw new InvalidOperationException();
                    return single.Item;
                }
            }

            #endregion

            #region ------ Internals: Disposable overrides ----------------------------------------

            protected override void Dispose(bool disposing)
            {
                if (disposing) single = null;
            }

            #endregion
        }

        #endregion

        /// <summary>
        /// The single item wrapped by this class.
        /// </summary>
        public T Item { get; set; }
    }
}