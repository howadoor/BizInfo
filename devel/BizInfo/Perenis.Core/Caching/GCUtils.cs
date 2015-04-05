using System;
using System.Collections.Generic;
using System.Threading;

namespace Perenis.Core.Caching
{
    /// <summary>
    /// Some methods and events to interact with garbage collection. You can 
    /// keep an object alive during the next collection or register to know 
    /// when a collection has just happened. This is useful if you don't use
    /// WeakReferences, but know how to free memory if needed. For example, 
    /// you can do a TrimExcess to your lists to free some memory.
    /// 
    /// Caution: GC.KeepAlive keeps the object alive until that line of code,
    /// while GCUtils.KeepAlive keeps the object alive until the next 
    /// generation.
    /// </summary>
    public static class GCUtils
    {
        #region Constructor

        private static readonly ManualResetEvent fCollectedEvent = new ManualResetEvent(false);
        private static bool fFinished;

        static GCUtils()
        {
            AppDomain current = AppDomain.CurrentDomain;
            current.DomainUnload += p_DomainUnload;
            current.ProcessExit += p_ProcessExit;
            new Runner();

            var thread = new Thread(p_ExecuteCollected);
            thread.Name = "Pfz.GCUtils.Collected executor thread.";
            thread.Start();
        }

        #endregion

        #region Finalization event handles

        private static void p_ProcessExit(object sender, EventArgs e)
        {
            fFinished = true;
            fCollectedEvent.Set();
        }

        private static void p_DomainUnload(object sender, EventArgs e)
        {
            fFinished = true;
            fCollectedEvent.Set();
        }

        #endregion

        #region KeepAlive

        private static HashSet<object> fKeepedObjects = new HashSet<object>(fReferenceComparer);

        /// <summary>
        /// Keeps an object alive at the next collection. This is useful for WeakReferences as an way
        /// to guarantee that recently used objects will not be immediatelly collected. At the next
        /// generation, you can call KeepAlive again, making the object alive for another generation.
        /// </summary>
        /// <param name="item"></param>
        public static void KeepAlive(object item)
        {
            if (item == null)
                return;

            HashSet<object> keepedObjects = fKeepedObjects;

            lock (keepedObjects)
                keepedObjects.Add(item);
        }

        /// <summary>
        /// Expires an object. Is the opposite of KeepAlive.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>true if the object was in the KeepAlive list, false otherwise.</returns>
        public static bool Expire(object item)
        {
            if (item == null)
                return false;

            HashSet<object> keepedObjects = fKeepedObjects;

            lock (keepedObjects)
                return keepedObjects.Remove(item);
        }

        #endregion

        #region p_ExecuteCollected

        private static void p_ExecuteCollected()
        {
            Thread thread = Thread.CurrentThread;
            while (true)
            {
                // we are background while waiting.
                thread.IsBackground = true;

                fCollectedEvent.WaitOne();

                if (fFinished)
                    return;

                fCollectedEvent.Reset();

                // but we are not background while running.
                thread.IsBackground = false;

                Dictionary<int, List<WeakDelegateBase>> collected = fCollected;
                lock (collected)
                {
                    // this is the best trimexcess we can do.
                    fCollected = new Dictionary<int, List<WeakDelegateBase>>();

                    foreach (var list in collected.Values)
                        foreach (WeakDelegateBase action in list)
                            action.Invoke(null);
                }
            }
        }

        #endregion

        #region Collected

        private static Dictionary<int, List<WeakDelegateBase>> fCollected = new Dictionary<int, List<WeakDelegateBase>>();

        /// <summary>
        /// This event is called after a GarbageCollection has just finished,
        /// in another thread. As this happens after the collection has finished,
        /// all other threads are running too, so you must guarantee that
        /// your event is thread safe.
        /// </summary>
        public static event Action Collected
        {
            add
            {
                int hashCode = value.GetHashCode();

                Dictionary<int, List<WeakDelegateBase>> collected = fCollected;
                lock (collected)
                {
                    List<WeakDelegateBase> list;

                    // if there is no item with the same hashCode, we
                    // can insert a new one directly.
                    if (collected.TryGetValue(hashCode, out list))
                    {
                        // ok, an item with the same hashCode exists, so
                        // first we check if this is already in the list.
                        // if there is, we simple return.
                        foreach (WeakDelegateBase action in list)
                            if (action.Target == value.Target && action.Method == value.Method)
                                return;
                    }
                    else
                    {
                        list = new List<WeakDelegateBase>(1);
                        collected.Add(hashCode, list);
                    }

                    var weakDelegate = new WeakDelegateBase(value);
                    list.Add(weakDelegate);
                }
            }
            remove
            {
                int hashCode = value.GetHashCode();

                Dictionary<int, List<WeakDelegateBase>> collected = fCollected;
                lock (collected)
                {
                    List<WeakDelegateBase> list;
                    if (!collected.TryGetValue(hashCode, out list))
                        return;

                    int count = list.Count;
                    for (int i = 0; i < count; i++)
                    {
                        WeakDelegateBase weakDelegate = list[i];
                        if (weakDelegate.Method == value.Method && weakDelegate.Target == value.Target)
                        {
                            list.RemoveAt(i);
                            return;
                        }
                    }
                }
            }
        }

        #endregion

        #region Runner - nested class

        private sealed class Runner
        {
            ~Runner()
            {
                // If we don't test, we will keep re-registering forever
                // when the application is finishing.
                if (fFinished)
                    return;

                GC.ReRegisterForFinalize(this);

                // no lock is needed as we simple put a new object and don't
                // even try to read the old object.
                fKeepedObjects = new HashSet<object>(fReferenceComparer);

                fCollectedEvent.Set();
            }
        }

        #endregion

        #region ReferenceComparer - nested class

        private static readonly ReferenceComparer fReferenceComparer = new ReferenceComparer();

        /// <summary>
        /// Class used to compare two references.
        /// They must point to the same instance (not an equal instance) to be
        /// considered equal.
        /// This also helps making comparisons faster for objects that
        /// implement Equals.
        /// </summary>
        private sealed class ReferenceComparer :
            IEqualityComparer<object>
        {
            #region IEqualityComparer<object> Members

            bool IEqualityComparer<object>.Equals(object x, object y)
            {
                return x == y;
            }

            int IEqualityComparer<object>.GetHashCode(object obj)
            {
                return obj.GetHashCode();
            }

            #endregion
        }

        #endregion
    }
}