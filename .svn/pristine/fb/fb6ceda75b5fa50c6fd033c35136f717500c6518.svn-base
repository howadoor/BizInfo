using System;
using Perenis.Core.Pattern;

namespace Perenis.Core.Reflection
{
    /// <summary>
    /// Provides access to attributes of a specific type which is a descendant of the given 
    /// <typeparamref name="TBase"/> type and allows applying dynamically created attributes 
    /// to the type.
    /// </summary>
    /// <typeparam name="TAttributeManager">The descendant of this class.</typeparam>
    /// <typeparam name="TBase">The base type of the supported types.</typeparam>
    /// <remarks>
    /// See <see cref="AttributeManager"/> for detailed description.
    /// </remarks>
    public abstract class TypeAttributeManager<TAttributeManager, TBase> : AttributeManager
        where TAttributeManager : TypeAttributeManager<TAttributeManager, TBase>
        where TBase : class
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <exception cref="ArgumentException"><paramref name="element"/> isn't a descendant of the 
        /// <typeparamref name="TBase"/> type.</exception>
        protected TypeAttributeManager(Type element) : base(element)
        {
            EnsureSupportedType(element);
        }

        #region ------ Internals ------------------------------------------------------------------

        /// <summary>
        /// Ensures that the given type <paramref name="t"/> is descendant of the <typeparamref name="TBase"/> type.
        /// </summary>
        /// <param name="t">The type to be checked.</param>
        /// <exception cref="ArgumentException"><paramref name="t"/> isn't a descendant of the <typeparamref name="TBase"/> type.</exception>
        protected static void EnsureSupportedType(Type t)
        {
            if (!typeof (TBase).IsAssignableFrom(t)) throw new ArgumentException(String.Format("Type “{0}” is not supported; must be assignable from the “{1}” type.", t.FullName, typeof (TBase).FullName), "t");
        }

        #endregion

        /// <summary>
        /// Retrieves a typed instance of an attribute manager for the given type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type whose attribute manager is to be retrieved.</typeparam>
        /// <returns>The retrieved attribute manager.</returns>
        public static TAttributeManager Get<T>() where T : TBase
        {
            return (TAttributeManager) Singleton<AttributeManagerRegistry>.Instance.Get<T>();
        }

        /// <summary>
        /// Retrieves a typed instance of an attribute manager for the given type <paramref name="objType"/>.
        /// </summary>
        /// <param name="objType">The type of the MOSS object whose attribute manager is to be retrieved.</typeparam>
        /// <returns>The retrieved attribute manager.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="objType"/> is a null reference.</exception>
        /// <exception cref="ArgumentException"><paramref name="objType"/> isn't a descendant of the 
        /// <typeparamref name="TBase"/> type.</exception>
        public static TAttributeManager Get(Type objType)
        {
            if (objType == null) throw new ArgumentNullException("objType");
            EnsureSupportedType(objType);
            return (TAttributeManager) Singleton<AttributeManagerRegistry>.Instance.Get(objType);
        }

        /// <summary>
        /// Retrieves a typed instance of an attribute manager for the type of the supplied MOSS object.
        /// </summary>
        /// <param name="obj">A MOSS object whose type's attribute manager is to be retrieved.</param>
        /// <returns>The retrieved attribute manager.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="obj"/> is a null reference.</exception>
        public static TAttributeManager Get(TBase obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            return (TAttributeManager) Singleton<AttributeManagerRegistry>.Instance.Get(obj.GetType());
        }
    }
}