using System;
using System.Collections.Generic;
using System.Linq;
using Perenis.Core.General;

namespace Perenis.Core.Caching
{
    /// <summary>
    /// This class acts as a hashset for delegates, but allows the
    /// Targets to be collected, working as if the Delegates where 
    /// WeakDelegates.
    /// The original idea was to make this a generic class, but it
    /// is not possible to use Delegate as a constraint (where TValue: Delegate).
    /// 
    /// To use, implement your event like:
    /// private WeakDelegateSet fMyEvent = new WeakDelegateSet();
    /// public event EventHandler MyEvent
    /// {
    ///		add
    ///		{
    ///			fMyEvent.Add(value);
    ///		}
    ///		remove
    ///		{
    ///			fMyEvent.Remove(value);
    ///		}
    ///		
    /// And when you want to invoke MyEvent, you call:
    ///		fMyEvent.Invoke(this, EventArgs.Empty);
    /// }
    /// </summary>
    public class WeakDelegateSet :
        ThreadSafeDisposable
    {
        #region Private dictionary

        private volatile Dictionary<int, List<WeakDelegateBase>> fDictionary = new Dictionary<int, List<WeakDelegateBase>>();

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new WeakDelegateSet.
        /// </summary>
        public WeakDelegateSet()
        {
            GCUtils.Collected += p_Collected;
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Unregisters the WeakDelegateSet from GCUtils.Collected.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                GCUtils.Collected -= p_Collected;

            base.Dispose(disposing);
        }

        #endregion

        #region p_Collected

        private void p_Collected()
        {
            lock (DisposeLock)
            {
                if (WasDisposed)
                    return;

                GCUtils.Collected += p_Collected;
                Dictionary<int, List<WeakDelegateBase>> oldDictionary = fDictionary;
                var newDictionary = new Dictionary<int, List<WeakDelegateBase>>();

                foreach (var pair in oldDictionary)
                {
                    List<WeakDelegateBase> oldList = pair.Value;
                    var newList = new List<WeakDelegateBase>(oldList.Count);
                    foreach (WeakDelegateBase handler in oldList)
                        if (handler.Method.IsStatic || handler.Target != null)
                            newList.Add(handler);

                    if (newList.Count > 0)
                        newDictionary.Add(pair.Key, newList);
                }

                fDictionary = newDictionary;
            }
        }

        #endregion

        #region Clear

        /// <summary>
        /// Clears the delegate set.
        /// </summary>
        public void Clear()
        {
            lock (DisposeLock)
            {
                CheckUndisposed();

                fDictionary.Clear();
            }
        }

        #endregion

        #region Add

        /// <summary>
        /// Adds a new handler to the delegate set.
        /// </summary>
        /// <param name="handler">The handler to add.</param>
        /// <returns>true if the delegate was new to the set, false otherwise.</returns>
        public bool Add(Delegate handler)
        {
            if (handler == null)
                throw new ArgumentNullException("handler");

            int hashCode = handler.GetHashCode();

            lock (DisposeLock)
            {
                CheckUndisposed();

                Dictionary<int, List<WeakDelegateBase>> dictionary = fDictionary;

                List<WeakDelegateBase> list;
                if (dictionary.TryGetValue(hashCode, out list))
                {
                    foreach (WeakDelegateBase action in list)
                        if (action.Target == handler.Target && action.Method == handler.Method)
                            return false;
                }
                else
                {
                    list = new List<WeakDelegateBase>(1);
                    dictionary.Add(hashCode, list);
                }

                var weakDelegate = new WeakDelegateBase(handler);
                list.Add(weakDelegate);
                return true;
            }
        }

        #endregion

        #region Remove

        /// <summary>
        /// Removes a handler from the delegate set.
        /// </summary>
        /// <param name="handler">The handler to remove.</param>
        /// <returns>true if the item was in the set, false otherwise.</returns>
        public bool Remove(Delegate handler)
        {
            if (handler == null)
                throw new ArgumentNullException("handler");

            int hashCode = handler.GetHashCode();

            lock (DisposeLock)
            {
                CheckUndisposed();

                Dictionary<int, List<WeakDelegateBase>> dictionary = fDictionary;

                List<WeakDelegateBase> list;
                if (!dictionary.TryGetValue(hashCode, out list))
                    return false;

                int count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    WeakDelegateBase weakDelegate = list[i];
                    if (weakDelegate.Method == handler.Method && weakDelegate.Target == handler.Target)
                    {
                        list.RemoveAt(i);
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion

        #region Invoke

        /// <summary>
        /// Invokes all the handlers in the set with the given parameters.
        /// </summary>
        /// <param name="parameters">The parameters to be used in the invoke of each handler.</param>
        public void Invoke(params object[] parameters)
        {
            List<List<WeakDelegateBase>> copy;

            lock (DisposeLock)
            {
                CheckUndisposed();

                copy = new List<List<WeakDelegateBase>>(fDictionary.Values.ToList());
            }

            foreach (var list in copy)
            {
                foreach (WeakDelegateBase handler in list)
                {
                    object target = handler.Target;
                    if (target != null || handler.Method.IsStatic)
                        handler.Method.Invoke(target, parameters);
                }
            }
        }

        #endregion
    }
}