using System;

namespace Perenis.Core.Reflection
{
    /// <summary>
    /// A provider of a type instance-creation service.
    /// </summary>
    public interface ITypeFactory<TBase> where TBase : class
    {
        /// <summary>
        /// Creates a new instance of the given type. Calls a parameter-less constructor.
        /// </summary>
        /// <param name="type">The type whose instance is to be created.</param>
        TBase CreateInstance(Type type);

        /// <summary>
        /// Creates a new instance of the given type. Calls a constructor with the given parameters.
        /// </summary>
        /// <param name="type">The type whose instance is to be created.</param>
        /// <param name="args">Arguments to the constructor.</param>
        TBase CreateInstance(Type type, params object[] args);

        /// <summary>
        /// Creates a new instance of the given type. Calls a parameter-less constructor.
        /// </summary>
        /// <typeparam name="T">The type whose instance is to be created.</typeparam>
        T CreateInstance<T>() where T : TBase;

        /// <summary>
        /// Creates a new instance of the given type. Calls a constructor with the given parameters.
        /// </summary>
        /// <typeparam name="T">The type whose instance is to be created.</typeparam>
        /// <param name="args">Arguments to the constructor.</param>
        T CreateInstance<T>(params object[] args) where T : TBase;

        /// <summary>
        /// Creates a new instance of the runtime type. Calls a parameter-less constructor.
        /// </summary>
        TBase CreateInstance();

        /// <summary>
        /// Creates a new instance of the runtime type. Calls a constructor with the given parameters.
        /// </summary>
        /// <param name="args">Arguments to the constructor.</param>
        TBase CreateInstance(params object[] args);
    }
}