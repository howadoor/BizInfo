using System;
using System.Reflection;

namespace Perenis.Core.Serialization
{
    /// <summary>
    /// Provides parametersless function which creates new instance of given type each time called
    /// </summary>
    public interface IInstantiatorProvider
    {
        /// <summary>
        /// Provides parametersless function which creates new instance of given <see cref="type"/> each time called
        /// </summary>
        /// <param name="type"></param>
        /// <returns>Parametersless function which creates new instance of given <see cref="type"/> each time called</returns>
        Func<object> GetInstantiator(Type type);
    }
}