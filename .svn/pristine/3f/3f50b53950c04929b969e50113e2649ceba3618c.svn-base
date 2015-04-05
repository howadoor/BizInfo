using System;
using System.Collections.Generic;
using System.Threading;

namespace Perenis.Core.Caching
{
    /// <summary>
    /// This class acts as a cache for function evalutations. This is inspired
    /// by the functional and immutable programming model, so calling a function
    /// twice with the same parameter, which will generate the same result, will 
    /// be evaluated only once.
    /// As a cache, such generated results can be garbage collected, but the
    /// cache is able to generate the same value again, given the same
    /// parameter. And, of course, the parameter can be an struct, so this 
    /// enables multi-parameters with a little extra effort.
    /// 
    /// To use this cache, you must override and implement the method Generate.
    /// It is common to create the cache as an inheritor of this class with 
    /// a private constructor, and make it acessible by a static function call.
    /// </summary>
    public abstract class ResultCache<TParam, TResult>
    {
        #region Private fields

        private readonly ReaderWriterLockSlim fLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        private Dictionary<TParam, Item> fDictionary = new Dictionary<TParam, Item>();

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes the functional cache, and register it in the Collected event.
        /// </summary>
        public ResultCache()
        {
            GCUtils.Collected += p_Collected;
        }

        #endregion

        #region p_Collected

        private void p_Collected()
        {
            GCUtils.Collected += p_Collected;

            fLock.EnterWriteLock();
            try
            {
                Dictionary<TParam, Item> oldDictionary = fDictionary;
                fDictionary = new Dictionary<TParam, Item>(oldDictionary.Count);
                int lastGeneration = GC.CollectionCount(GC.MaxGeneration) - 1;
                foreach (var pair in oldDictionary)
                    if (pair.Value.CollectionCount >= lastGeneration)
                        fDictionary.Add(pair.Key, pair.Value);
            }
            finally
            {
                fLock.ExitWriteLock();
            }
        }

        #endregion

        #region DoExecute

        /// <summary>
        /// You must implement this method to effectivelly execute the function.
        /// Remember that such function must always return the same value considering
        /// the same input values.
        /// </summary>
        /// <param name="parameter">The parameter value.</param>
        /// <returns>Your generated result value.</returns>
        protected abstract TResult DoExecute(TParam parameter);

        #endregion

        #region Execute

        /// <summary>
        /// Evaluates the function, or uses the cached value if this function
        /// was already evaluated with the same parameters.
        /// 
        /// This method is virtual so it is possible to clone the result in cases
        /// where the result is modifiable just before returning it.
        /// As the value is cached, you must guarantee that the value is not modifiable 
        /// or, if it is, at least clone it so the returned value, if modified, will
        /// not destroy the cached result.
        /// </summary>
        /// <param name="parameter">The parameter value.</param>
        /// <returns>Your generated result value.</returns>
        public virtual TResult Execute(TParam parameter)
        {
            Item item;

            int collectionCount = GC.CollectionCount(GC.MaxGeneration);
            fLock.EnterReadLock();
            try
            {
                if (fDictionary.TryGetValue(parameter, out item))
                {
                    item.CollectionCount = collectionCount;
                    return item.Result;
                }
            }
            finally
            {
                fLock.ExitReadLock();
            }

            fLock.EnterUpgradeableReadLock();
            try
            {
                // as we released the lock, we need to check-again.
                if (fDictionary.TryGetValue(parameter, out item))
                {
                    item.CollectionCount = collectionCount;
                    return item.Result;
                }

                item = new Item();
                item.CollectionCount = collectionCount;
                item.Result = DoExecute(parameter);

                fLock.EnterWriteLock();
                try
                {
                    fDictionary.Add(parameter, item);
                    return item.Result;
                }
                finally
                {
                    fLock.ExitWriteLock();
                }
            }
            finally
            {
                fLock.ExitUpgradeableReadLock();
            }
        }

        #endregion

        #region Clear

        /// <summary>
        /// Clears the cache.
        /// </summary>
        protected void Clear()
        {
            fLock.EnterWriteLock();
            try
            {
                fDictionary.Clear();
            }
            finally
            {
                fLock.ExitWriteLock();
            }
        }

        #endregion

        #region Add

        /// <summary>
        /// Adds a value to the cache. This is here so inherited classes
        /// that know they are generating values valid to be added to the
        /// cache add it. Remember that this method throws an exception
        /// when the value already exists and that Cache in general
        /// must be thread-safe.
        /// </summary>
        protected void Add(TParam parameter, TResult result)
        {
            var item = new Item();
            item.CollectionCount = GC.CollectionCount(GC.MaxGeneration);
            item.Result = result;

            fLock.EnterWriteLock();
            try
            {
                fDictionary.Add(parameter, item);
            }
            finally
            {
                fLock.ExitWriteLock();
            }
        }

        #endregion

        #region AddOrReplace

        /// <summary>
        /// Adds or replaces a value from the cache. This method is here
        /// so inherited classes can replace existing cached values if,
        /// for some reason, such value changes.
        /// If you need to do this, remember that the cache can be used
        /// by many threads and even the cache itself being thread-safe,
        /// you may need your own locking to avoid incoherent states.
        /// </summary>
        protected void AddOrReplace(TParam parameter, TResult result)
        {
            var item = new Item();
            item.CollectionCount = GC.CollectionCount(GC.MaxGeneration);
            item.Result = result;

            fLock.EnterWriteLock();
            try
            {
                fDictionary[parameter] = item;
            }
            finally
            {
                fLock.ExitWriteLock();
            }
        }

        #endregion

        #region Remove

        /// <summary>
        /// Removes a cached result.
        /// </summary>
        protected bool Remove(TParam parameter)
        {
            fLock.EnterWriteLock();
            try
            {
                return fDictionary.Remove(parameter);
            }
            finally
            {
                fLock.ExitWriteLock();
            }
        }

        #endregion

        #region Item - Nested class

        private sealed class Item
        {
            internal int CollectionCount;
            internal TResult Result;
        }

        #endregion
    }
}