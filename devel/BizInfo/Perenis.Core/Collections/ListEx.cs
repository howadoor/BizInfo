using System;
using System.Collections.Generic;

namespace Perenis.Core.Collections
{
    /// <summary>
    /// Extensions methods for <see cref="List{T}"/>.
    /// </summary>
    public static class ListEx
    {
        /// <summary>
        /// Performs a stable sort of the given list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="comparison">The comparison method to be used.</param>
        public static void StableSort<T>(this List<T> list, Comparison<T> comparison)
        {
            if (list == null) throw new ArgumentNullException("list");
            if (comparison == null) throw new ArgumentNullException("comparison");

            // TODO This implementation is very straightforward and inefficient; replace with merge sort or alike.
            list.Sort(new StableComparer<T>(list, comparison));
        }

        #region ------ Internals ------------------------------------------------------------------

        /// <summary>
        /// Encapsulates a user-supplied <see cref="comparison{T}"/> and diambiguates comparison
        /// result for equal items by comparing their original order within the source list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class StableComparer<T> : IComparer<T>
        {
            /// <summary>
            /// The primary comparison method.
            /// </summary>
            private readonly Comparison<T> comparison;

            /// <summary>
            /// The original order of items in the list for comparison disambiguation.
            /// </summary>
            private readonly Dictionary<T, int> orgOrder;

            /// <summary>
            /// Initializes a new instance of this class.
            /// </summary>
            public StableComparer(List<T> list, Comparison<T> comparison)
            {
                this.comparison = comparison;

                orgOrder = new Dictionary<T, int>(list.Count);
                for (int i = 0; i < list.Count; i++) orgOrder.Add(list[i], i);
            }

            #region ------ Implementation of the IComparer interface ------------------------------

            public int Compare(T x, T y)
            {
                int result = comparison(x, y);
                if (result != 0) return result;
                return orgOrder[x].CompareTo(orgOrder[y]);
            }

            #endregion
        }

        #endregion
    }
}