using System;
using System.Linq;
using System.Reflection;
using Perenis.Core.Reflection;
using Perenis.Core.Serialization.Exceptions;

namespace Perenis.Core.Serialization
{
    /// <summary>
    /// Basic implementation of <see cref="IItemsAdderProvider"/>.
    /// </summary>
    class BasicItemsAdderProvider : IItemsAdderProvider
    {
        /// <summary>
        /// Provides action which adds new item to the collection of given type
        /// </summary>
        /// <exception cref="CannotCreateItemsAdderException"></exception>
        /// <remarks>
        /// For interface and abstract types creates do-nothing adder. For concrete types finds "Add" method with one parameter. If no method is found, throws <see cref="InvalidOperationException"/>.
        /// </remarks>
        public Action<object, object> GetItemsAdder(Type type)
        {
            if (type.IsAbstract || type.IsInterface) return (collection, item) => { };
            var addMethod = type.GetAllMethods().FirstOrDefault(methodInfo =>
            {
                if (methodInfo.Name != "Add") return false;
                var types = methodInfo.GetParametersTypes();
                return types.Length == 1;
            });
            if (addMethod == null) throw new CannotCreateItemsAdderException(type);
            return (collection, item) => addMethod.Invoke(collection, new[] {item});
        }
    }
}