using System;

namespace Perenis.Core.Component
{
    /// <summary>
    /// Provides name-resolution service upon name-carrying objects.
    /// </summary>
    /// <typeparam name="T">The base type of named objects.</typeparam>
    public interface IStructuredNamingResolver<T>
    {
        /// <summary>
        /// Retrieves the name of represented by the given <paramref name="nameObject"/>.
        /// </summary>
        /// <param name="nameObject">The name-carrying object.</param>
        /// <returns>The name of the object or a null reference when it can't be resolved for 
        /// whatever reason.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="nameObject"/> is a null reference.</exception>
        string GetStructuredName(T nameObject);
    }
}