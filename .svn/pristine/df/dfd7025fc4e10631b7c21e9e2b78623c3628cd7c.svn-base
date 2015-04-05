using System;
using System.Runtime.InteropServices;

namespace Perenis.Core.Caching
{
    /// <summary>
    /// This struct works like a KeepAliveWeakReference, but it is internal
    /// as it's uses the GCHandle directly, and can leak memory if used
    /// unproperly.
    /// </summary>
    internal struct ExtendedGCHandle
    {
        #region Private handle

        private GCHandle fHandle;

        #endregion

        #region Constructor

        public ExtendedGCHandle(object target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            GCUtils.KeepAlive(target);

            try
            {
            }
            finally
            {
                fHandle = GCHandle.Alloc(target, GCHandleType.Weak);
            }
        }

        #endregion

        #region Free

        public void Free()
        {
            if (fHandle.IsAllocated)
            {
                GCUtils.Expire(fHandle.Target);
                fHandle.Free();
            }
        }

        #endregion

        #region IsAlive

        public bool IsAlive
        {
            get { return fHandle.Target != null; }
        }

        #endregion

        #region Target

        public object Target
        {
            get
            {
                object result = fHandle.Target;
                GCUtils.KeepAlive(result);
                return result;
            }
            set
            {
                GCUtils.Expire(fHandle.Target);
                GCUtils.KeepAlive(value);
                fHandle.Target = value;
            }
        }

        #endregion

        #region TargetAllowingExpiration

        public object TargetAllowingExpiration
        {
            get { return fHandle.Target; }
            set { fHandle.Target = value; }
        }

        #endregion
    }
}