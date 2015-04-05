using System;
using Perenis.Core.Reflection;

namespace Perenis.Core.Serialization
{
    /// <summary>
    /// Default imlementation of <see cref="IInstantiatorProvider"/>. It is used by <see cref="Deserializer"/> when no other instatiator provider is set
    /// to <see cref="Deserializer.InstantiatorProvider"/>.
    /// </summary>
    public class BasicInstantiatorProvider : IInstantiatorProvider
    {
        /// <summary>
        /// Provides parametersless function which creates new instance of given <see cref="type"/> each time called
        /// </summary>
        /// <remarks>
        /// Uses <see cref="TypeEx.GetInstantiator"/> to create instantiator.
        /// </remarks>
        /// <param name="type"></param>
        /// <returns>Parametersless function which creates new instance of given <see cref="type"/> each time called</returns>
        public virtual Func<object> GetInstantiator(Type type)
        {
            return type.GetInstantiator();
        }
    }
}