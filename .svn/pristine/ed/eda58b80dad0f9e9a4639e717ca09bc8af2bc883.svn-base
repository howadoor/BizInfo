using System;
using System.Collections.Generic;

namespace Perenis.Core.Collections
{
    /// <summary>
    /// Standardní fronta rozšířená o nové operace.
    /// </summary>
    /// <typeparam name="T">Typ položky fronty.</typeparam>
    [Serializable]
    public class QueueEx<T> : Queue<T>
    {
        /// <summary>
        /// Zařadí do fronty všechny prvky z jiné fronty, ze které je odebere.
        /// </summary>
        /// <param name="queue">Zdrojová fronta poskytující prvky.</param>
        public void Enqueue(QueueEx<T> queue)
        {
            while (!queue.IsEmpty()) Enqueue(queue.Dequeue());
        }

        /// <summary>
        /// Zkusí z fronty vyzvednout prvek.
        /// </summary>
        /// <param name="item">Vyzvednutý prvek nebo <code>default(T)</code>, pokud byla fronta prázdná.</param>
        /// <returns><c>true</c>, je-li k dispozici platný prvek.</returns>
        public bool TryDequeue(out T item)
        {
            if (!IsEmpty())
            {
                item = Dequeue();
                return true;
            }
            else
            {
                item = default(T);
                return false;
            }
        }

        /// <summary>
        /// Zjistí, zda je fronta prázdná.
        /// </summary>
        /// <returns><c>true</c>, pokud je fronta prázdná, v opačném případě <c>false</c>.</returns>
        public bool IsEmpty()
        {
            return Count == 0;
        }

        #region ------ Constructors ---------------------------------------------------------------

        public QueueEx()
        {
        }

        public QueueEx(int capacity) : base(capacity)
        {
        }

        public QueueEx(IEnumerable<T> collection) : base(collection)
        {
        }

        #endregion
    }
}