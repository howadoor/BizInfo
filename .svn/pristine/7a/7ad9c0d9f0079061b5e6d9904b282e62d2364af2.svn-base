using System;
using System.Collections;
using Spring.Context.Support;

namespace Perenis.Core.Spring
{
    /// <summary>
    /// Facade to the object context.
    /// </summary>
    public class Context
    {
        /// <summary>
        /// Gets the desired object of type <typeparamref name="TInstance"/> and name <paramref name="objectName"/>.
        /// </summary>
        /// <remarks>
        /// The object must exist in the object context.
        /// </remarks>
        /// <typeparam name="TInstance">The retrieved object must equal or inherit from the type.</typeparam>
        /// <param name="objectName">The name of the objec to retrieve.</param>
        /// <returns>The context object.</returns>
        public static TInstance GetObject<TInstance>(string objectName)
        {
            return (TInstance) ContextRegistry.GetContext().GetObject(objectName, typeof (TInstance));
        }

        /// <summary>
        /// Gets all objects of type <typeparamref name="TInstance"/>.
        /// </summary>
        /// <typeparam name="TInstance">The retrieved objects must equal or inherit from the type.</typeparam>
        /// <returns>The matching objects.</returns>
        public static IDictionary GetObjects<TInstance>()
        {
            return ContextRegistry.GetContext().GetObjectsOfType(typeof (TInstance) /*, false, false*/);
        }

        /// <summary>
        /// Gets the desired object of type <typeparamref name="TInstance"/>.
        /// </summary>
        /// <remarks>
        /// There must be exactly one object of the type <typeparamref name="TInstance"/> in the object context.
        /// </remarks>
        /// <typeparam name="TInstance">The retrieved object must equal or inherit from the type.</typeparam>
        /// <returns>The context object.</returns>
        /// <exception cref="InvalidOperationException">When zero objects or multiple objects of the type <typeparamref name="TInstance"/> are found.</exception>
        public static TInstance GetObject<TInstance>()
        {
            return (TInstance) GetObjectOfType(typeof (TInstance));
        }

        /// <summary>
        /// Gets the desired object of type
        /// </summary>
        /// <remarks>
        /// There must be exactly one object of the type in the object context.
        /// </remarks>
        /// <returns>The context object.</returns>
        /// <exception cref="InvalidOperationException">When zero objects or multiple objects of the type <typeparamref name="TInstance"/> are found.</exception>
        public static object GetObjectOfType(Type type)
        {
            IDictionary result = ContextRegistry.GetContext().GetObjectsOfType(type);
            if (result.Count == 0) throw new InvalidOperationException(String.Format("No object of type “{0}” found", type.FullName));
            if (result.Count > 1) throw new InvalidOperationException(String.Format("Multiple objects of type “{0}” found", type.FullName));

            IEnumerator e = result.Values.GetEnumerator();
            e.MoveNext();
            return e.Current;
        }

        /// <summary>
        /// Gets the desired object of type
        /// </summary>
        /// <remarks>
        /// There must be maximum one object of the type in the object context.
        /// </remarks>
        /// <returns>The context object.</returns>
        /// <exception cref="InvalidOperationException">When multiple objects of the type <typeparamref name="TInstance"/> are found.</exception>
        public static object GetObjectOfTypeOrNull(Type type)
        {
            IDictionary result = ContextRegistry.GetContext().GetObjectsOfType(type);
            if (result.Count == 0) return null;
            if (result.Count > 1) throw new InvalidOperationException(String.Format("Multiple objects of type “{0}” found", type.FullName));

            IEnumerator e = result.Values.GetEnumerator();
            e.MoveNext();
            return e.Current;
        }
    }
}