using System;

namespace Perenis.Core.General
{
    /// <summary>
    /// Collects extension methods to <see cref="IPresenter"/> interface
    /// </summary>
    public static class PresenterEx
    {
        /// <summary>
        /// Just a shortcut. Creates a new instance of <see cref="T"/> then calls presenters method <see cref="Present"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="presenter"></param>
        /// <param name="object"></param>
        /// <returns></returns>
        public static T GetPresentation<T>(this IPresenter presenter, object @object) where T : new()
        {
            var presentation = new T();
            presenter.Present(@object, presentation);
            return presentation;
        }

        /// <summary>
        /// If <see cref="@object"/> is kind of <see cref="T"/>, returns object itself. If not, returns newly created
        /// presentation of object, using <see cref="presenter"/> to create it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="presenter"></param>
        /// <param name="object"></param>
        /// <returns></returns>
        public static T GetItselfOrPresentation<T>(this IPresenter presenter, object @object) where T : new()
        {
            return @object is T ? (T) @object : GetPresentation<T>(presenter, @object);
        }

        /// <summary>
        /// If <see cref="@object"/> is kind of <see cref="T"/>, returns clone of object (uses <see cref="Clone"/> method of it). 
        /// If not, returns newly created presentation of object, using <see cref="presenter"/> to create it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="presenter"></param>
        /// <param name="object"></param>
        /// <returns></returns>
        public static T GetItselfCloneOrPresentation<T>(this IPresenter presenter, object @object) where T : new()
        {
            return @object is T ? (T) @object.Clone() : GetPresentation<T>(presenter, @object);
        }

        public static T GetItselfOrPresentation<T>(this IPresenter presenter, object @object, Func<T> creation)
        {
            return @object is T ? (T) @object : GetPresentation(presenter, @object, creation);
        }

        private static T GetPresentation<T>(IPresenter presenter, object @object, Func<T> creation)
        {
            T presentation = creation();
            presenter.Present(@object, presentation);
            return presentation;
        }
    }
}