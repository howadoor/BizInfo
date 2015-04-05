using System.Runtime.InteropServices;

namespace Perenis.Core.Caching
{
    internal struct UnsafeWeakArray<T>
        where
            T : class
    {
        public static readonly UnsafeWeakArray<T> Empty = new UnsafeWeakArray<T>(0);

        private readonly GCHandle[] fArray;

        public UnsafeWeakArray(int length)
        {
            fArray = new GCHandle[length];
            int index = 0;
            try
            {
                try
                {
                }
                finally
                {
                    // does not stop here by an Abort().
                    for (index = 0; index < length; index++)
                        fArray[index] = GCHandle.Alloc(null, GCHandleType.Weak);
                }
            }
            catch
            {
                for (int i = 0; i < index; i++)
                    fArray[i].Free();

                throw;
            }
        }

        public int Length
        {
            get { return fArray.Length; }
        }

        public T this[int index]
        {
            get
            {
                var result = (T) fArray[index].Target;
                GCUtils.KeepAlive(result);
                return result;
            }
            set
            {
                GCHandle handle = fArray[index];
                GCUtils.Expire(handle.Target);
                handle.Target = value;
                GCUtils.KeepAlive(value);
            }
        }

        public void Free()
        {
            foreach (GCHandle handle in fArray)
                handle.Free();
        }

        public bool IsAlive(int index)
        {
            return fArray[index].Target != null;
        }

        public T GetAllowingExpiration(int index)
        {
            return (T) fArray[index].Target;
        }

        public override bool Equals(object obj)
        {
            if (obj is UnsafeWeakArray<T>)
                return this == (UnsafeWeakArray<T>) obj;

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return fArray.GetHashCode();
        }

        public static bool operator ==(UnsafeWeakArray<T> a, UnsafeWeakArray<T> b)
        {
            return a.fArray == b.fArray;
        }

        public static bool operator !=(UnsafeWeakArray<T> a, UnsafeWeakArray<T> b)
        {
            return a.fArray != b.fArray;
        }
    }
}