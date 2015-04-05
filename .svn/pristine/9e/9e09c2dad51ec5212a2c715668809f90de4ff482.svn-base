using System;
using System.Collections;
using System.Collections.Generic;

namespace Perenis.Core.Collections
{
    public class CascadedEnumerable<TPrimaryItem, TItem> : IEnumerable<TItem>
    {
        public CascadedEnumerable(IEnumerable<TPrimaryItem> primaryItems, Func<TPrimaryItem, IEnumerable<TItem>> secondaryEnumableGetter)
        {
            PrimaryItems = primaryItems;
            SecondaryEnumerableGetter = secondaryEnumableGetter;
        }

        public IEnumerable<TPrimaryItem> PrimaryItems { get; protected set; }
        public Func<TPrimaryItem, IEnumerable<TItem>> SecondaryEnumerableGetter { get; protected set; }

        #region IEnumerable<TItem> Members

        public IEnumerator<TItem> GetEnumerator()
        {
            return new CascadedEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Nested type: CascadedEnumerator

        public class CascadedEnumerator : IEnumerator<TItem>
        {
            private IEnumerator<TPrimaryItem> primaryEnumerator;
            private IEnumerator<TItem> secondaryEnumerator;

            public CascadedEnumerator(CascadedEnumerable<TPrimaryItem, TItem> cascadedEnumerable)
            {
                CascadedEnumerable = cascadedEnumerable;
            }

            public CascadedEnumerable<TPrimaryItem, TItem> CascadedEnumerable { get; protected set; }

            #region IEnumerator<TItem> Members

            public void Dispose()
            {
                Reset();
                CascadedEnumerable = null;
            }

            public bool MoveNext()
            {
                if (secondaryEnumerator != null && secondaryEnumerator.MoveNext()) return true;
                if (primaryEnumerator == null) primaryEnumerator = CascadedEnumerable.PrimaryItems.GetEnumerator();
                if (!primaryEnumerator.MoveNext()) return false;
                secondaryEnumerator = CascadedEnumerable.SecondaryEnumerableGetter(primaryEnumerator.Current).GetEnumerator();
                return secondaryEnumerator.MoveNext();
            }

            public void Reset()
            {
                if (secondaryEnumerator != null) secondaryEnumerator.Dispose();
                if (primaryEnumerator != null) primaryEnumerator.Dispose();
                primaryEnumerator = null;
                secondaryEnumerator = null;
            }

            public TItem Current
            {
                get { return secondaryEnumerator != null ? secondaryEnumerator.Current : default(TItem); }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            #endregion
        }

        #endregion
    }
}