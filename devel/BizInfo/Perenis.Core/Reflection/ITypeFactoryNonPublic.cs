using System;

namespace Perenis.Core.Reflection
{
    /// <summary>
    /// A provider of a type instance-creation service using non-public constructors.
    /// </summary>
    public interface ITypeFactoryNonPublic<TBase> where TBase : class
    {
        /// <summary>
        /// Creates a new instance of the given type. Calls a parameter-less non-public constructor.
        /// </summary>
        /// <param name="type">The type whose instance is to be created.</param>
        TBase CreateInstanceNonPublic(Type type);

        /// <summary>
        /// Creates a new instance of the given type. Calls a non-public constructor with the given parameters.
        /// </summary>
        /// <param name="type">The type whose instance is to be created.</param>
        /// <param name="args">Arguments to the constructor.</param>
        TBase CreateInstanceNonPublic(Type type, params object[] args);

        /// <summary>
        /// Creates a new instance of the given type. Calls a parameter-less non-public constructor.
        /// </summary>
        /// <typeparam name="T">The type whose instance is to be created.</typeparam>
        T CreateInstanceNonPublic<T>() where T : TBase;

        /// <summary>
        /// Creates a new instance of the given type. Calls a non-public constructor with the given parameters.
        /// </summary>
        /// <typeparam name="T">The type whose instance is to be created.</typeparam>
        /// <param name="args">Arguments to the constructor.</param>
        T CreateInstanceNonPublic<T>(params object[] args) where T : TBase;
    }
}