using System;
using System.Collections.Generic;

namespace Perenis.Core.Collections
{
    /// <summary>
    /// Extended <see cref="ICollection{T}"/> interface.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICollectionEx<T> : ICollection<T>
    {
        /// <summary>
        /// Adds the elements of the specified collection to the current collection.
        /// </summary>
        /// <param name="collection">The collection whose elements should be added to the current collection.</param>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> is a null reference.</exception>
        /// <exception cref="NotSupportedException">When the current collection is read-only.</exception>
        void AddRange(IEnumerable<T> collection);
    }
}