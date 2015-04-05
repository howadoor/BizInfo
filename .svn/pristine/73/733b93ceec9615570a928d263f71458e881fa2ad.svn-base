using System;

namespace Perenis.Core.Pattern
{
    /// <summary>
    /// Makes a class a registrator for instances of type <typeparamref name="T"/>.
    /// Registrator allows to register or unregister instances of type <typeparamref name="T"/>,
    /// the actual semantics is fully in control of the implementors.
    /// </summary>
    /// <typeparam name="T">Type of the instances being registered/unregistered.</typeparam>
    public interface IRegistrar<T> where T : class
    {
        /// <summary>
        /// Registers an instance of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="instance"></param>
        void Register(T instance);

        /// <summary>
        /// Unregisters an instance of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="instance"></param>
        void Unregister(T instance);
    }

    /// <summary>
    /// Extends the <see cref="IRegistrar{T}"/> and includes several registrar events.
    /// </summary>
    /// <typeparam name="T"><see cref="IRegistrar{T}"/></typeparam>
    public interface ICallbackRegistrar<T> : IRegistrar<T> where T : class
    {
        /// <summary>
        /// Fired after an instance has been registered.
        /// </summary>
        event Action<T> Registered;

        /// <summary>
        /// Fired after an instance has been unregistered.
        /// </summary>
        event Action<T> Unregistered;
    }
}