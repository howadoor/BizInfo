using System;
using System.Collections;

namespace Perenis.Core.Caching.Tags
{
    /// <summary>
    /// Represents dictionary of weakly referenced keys to stringly referenced collection of values.
    /// 
    /// Tags must be based on WeakKeyDictionary, not a WeakDictionary.When WeakDictionary is used, collection of tag
    /// values is garbage collected, because WeakDictionary has weak references to both, keys even values.
    ///  </summary>
    /// <typeparam name="TTagCollection">Type of tag values collection</typeparam>
    public abstract class Tags<TTagCollection> : WeakKeyDictionary<object, TTagCollection>, ITags where TTagCollection : class, IEnumerable, new()
    {
        /// <summary>
        /// Returns first tag which is TRequested or null
        /// </summary>
        /// <typeparam name="TRequested"></typeparam>
        /// <param name="object"></param>
        /// <returns></returns>
        public TRequested GetFirstOrDefault<TRequested>(object @object) where TRequested : class
        {
            return GetFirstOrDefault(@object, tag => tag is TRequested) as TRequested;
        }

        /// <summary>
        /// Returns first tag which matching condition or null
        /// </summary>
        /// <param name="object"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public object GetFirstOrDefault(object @object, Predicate<object> condition)
        {
            TTagCollection tagsOfObject;
            if (!TryGetValue(@object, out tagsOfObject)) return null;
            foreach (object tag in tagsOfObject)
            {
                if (condition(tag)) return tag;
            }
            return null;
        }

        #region Implementation of ITags

        /// <summary>
        /// Finds a tag which type is TRequested
        /// </summary>
        /// <typeparam name="TRequested"></typeparam>
        /// <param name="object"></param>
        /// <returns></returns>
        public TRequested GetTag<TRequested>(object @object) where TRequested : class
        {
            return GetFirstOrDefault<TRequested>(@object);
        }

        /// <summary>
        /// Returns a tag matches condition
        /// </summary>
        /// <param name="object"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public object GetTag(object @object, Predicate<object> condition)
        {
            return GetFirstOrDefault(@object, condition);
        }

        /// <summary>
        /// Performs an action for all tags apended to given object
        /// </summary>
        /// <param name="object"></param>
        /// <param name="action"></param>
        public void ForEachTag(object @object, Action<object> action)
        {
            TTagCollection tagsOfObject;
            if (!TryGetValue(@object, out tagsOfObject)) return;
            foreach (object tag in tagsOfObject) action(tag);
        }

        /// <summary>
        /// Performs an action with all tag values apended to given object and matching condition
        /// </summary>
        /// <param name="object"></param>
        /// <param name="condition"></param>
        /// <param name="action"></param>
        public void ForEachTag(object @object, Predicate<object> condition, Action<object> action)
        {
            TTagCollection tagsOfObject;
            if (!TryGetValue(@object, out tagsOfObject)) return;
            foreach (object tag in tagsOfObject) if (condition(tag)) action(tag);
        }

        /// <summary>
        /// Performs an action with all tag values of TRequested type apended to given object and matching condition
        /// </summary>
        /// <typeparam name="TRequested"></typeparam>
        /// <param name="object"></param>
        /// <param name="action"></param>
        public void ForEachTag<TRequested>(object @object, Action<TRequested> action) where TRequested : class
        {
            ForEachTag(@object, tag => tag is TRequested, tag => action(tag as TRequested));
        }

        /// <summary>
        /// Appends new tag to object
        /// </summary>
        /// <param name="object"></param>
        /// <param name="tag"></param>
        public void SetTag(object @object, object tag)
        {
            TTagCollection tagsOfObject;
            if (!TryGetValue(@object, out tagsOfObject))
            {
                tagsOfObject = new TTagCollection();
                this[@object] = tagsOfObject;
            }
            _SetTag(tagsOfObject, tag);
        }

        /// <summary>
        /// Set of objects which has tag appended by this instance of ITags
        /// </summary>
        public IEnumerable Objects
        {
            get { return Keys; }
        }

        protected abstract void _SetTag(TTagCollection collection, object tag);

        #endregion
    }
}