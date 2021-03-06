using System;

namespace Perenis.Core.Caching
{
    /// <summary>
    /// Strongly typed version of <see cref="WeakReference{TReference}"/>.
    /// </summary>
    /// <typeparam name="TReference"></typeparam>
    public class WeakReference<TReference> : WeakReference
        where TReference : class
    {
        public WeakReference() : this(null)
        {
        }

        public WeakReference(TReference target)
            : base(target)
        {
        }

        public WeakReference(TReference target, bool trackResurrection)
            : base(target, trackResurrection)
        {
        }

        /// <summary>
        /// Strongly typed version of <see cref="WeakReference{TReference}.Target"/>.
        /// </summary>
        public new TReference Target
        {
            get { return base.Target as TReference; }
            set { base.Target = value; }
        }
    }
}