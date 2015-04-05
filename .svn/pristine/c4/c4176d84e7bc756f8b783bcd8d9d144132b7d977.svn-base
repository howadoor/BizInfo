using System;
using System.Collections;

namespace Perenis.Core.Caching.Tags
{
    /// <summary>
    /// ITags is an interface to specialised dictionary which allows to append value objects to any object
    /// </summary>
    public interface ITags
    {
        /// <summary>
        /// Set of objects which has tag appended by this instance of ITags
        /// </summary>
        IEnumerable Objects { get; }

        /// <summary>
        /// Finds a tag which type is TRequested
        /// </summary>
        /// <typeparam name="TRequested"></typeparam>
        /// <param name="object"></param>
        /// <returns></returns>
        TRequested GetTag<TRequested>(object @object) where TRequested : class;

        /// <summary>
        /// Returns a tag matches condition
        /// </summary>
        /// <param name="object"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        object GetTag(object @object, Predicate<object> condition);

        /// <summary>
        /// Performs an action for all tags apended to given object
        /// </summary>
        /// <param name="object"></param>
        /// <param name="action"></param>
        void ForEachTag(object @object, Action<object> action);

        /// <summary>
        /// Performs an action with all tag values apended to given object and matching condition
        /// </summary>
        /// <param name="object"></param>
        /// <param name="condition"></param>
        /// <param name="action"></param>
        void ForEachTag(object @object, Predicate<object> condition, Action<object> action);

        /// <summary>
        /// Performs an action with all tag values of TRequested type apended to given object and matching condition
        /// </summary>
        /// <typeparam name="TRequested"></typeparam>
        /// <param name="object"></param>
        /// <param name="action"></param>
        void ForEachTag<TRequested>(object @object, Action<TRequested> action) where TRequested : class;

        /// <summary>
        /// Appends new tag to object
        /// </summary>
        /// <param name="object"></param>
        /// <param name="tag"></param>
        void SetTag(object @object, object tag);

        /// <summary>
        /// Clear all tags for all objects, makes ITags collection empty
        /// </summary>
        void Clear();
    }
}