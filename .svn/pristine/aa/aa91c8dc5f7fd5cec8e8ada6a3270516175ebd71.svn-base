using System;
using System.Collections.Generic;
using Perenis.Core.Spring;

namespace Perenis.Core.Decoupling
{
    /// <summary>
    /// Obtains specialized generics for particular types from Spring context then caches it at dictionary
    /// </summary>
    /// <typeparam name="TGenerics"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class SpecializedGenerics<TGenerics, TValue> : Dictionary<Type, TValue>
        where TValue : class
    {
        /// <summary>
        /// Returns specialized generic type for particular type of @object. If present at cache, returns it,
        /// if not, obtains from Spring context and stores at cache.
        /// </summary>
        /// <param name="object"></param>
        /// <returns></returns>
        public TValue GetSpecialized(object @object)
        {
            return GetSpecializedForType(@object.GetType());
        }

        /// <summary>
        /// Returns specialized generic for given Type. If no exatc specialization exists, tries to find a specialization
        /// for type base type. Caches specialised generics.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public TValue GetSpecializedForType(Type type)
        {
            if (!ContainsKey(type))
            {
                Type genericType = typeof (TGenerics).GetGenericTypeDefinition().MakeGenericType(type);
                var generic = Context.GetObjectOfTypeOrNull(genericType) as TValue;
                if (generic == null && type.BaseType != null) generic = GetSpecializedForType(type.BaseType);
                this[type] = generic;
                return generic;
            }
            return this[type];
        }
    }
}