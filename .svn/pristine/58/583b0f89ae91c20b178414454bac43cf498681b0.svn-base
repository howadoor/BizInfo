using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Perenis.Core.Collections;

namespace Perenis.Core.Reflection
{
    /// <summary>
    /// A generic registry of types organized by <typeparam name="TName"/> names.
    /// </summary>
    /// <typeparam name="TName">The type of the index.</typeparam>
    /// <typeparam name="TBase">The base type of types registered by this class.</typeparam>
    public abstract class TypeRegistryBase<TBase, TName>
        where TBase : class
    {
        // TODO Improve separation of concerns; move the registration condition out into a policy class
        // TODO Consider refactoring into a more general-purpose registry

        /// <summary>
        /// A list of registered types.
        /// </summary>
        protected Dictionary<TName, Type> RegisteredTypes = new Dictionary<TName, Type>();

        /// <summary>
        /// Registers a type using its full type name.
        /// </summary>
        /// <param name="type"></param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="type"/> doesn't specify
        /// a valid registerable type.</exception>
        public void RegisterType(Type type)
        {
            RegisterType(GetKey(type), type);
        }

        /// <summary>
        /// Searches for all registerable types within the given assembly and registers them.
        /// </summary>
        /// <param name="assembly">An assembly to be searched.</param>
        public void RegisterTypes(Assembly assembly)
        {
            // find all types from the given assembly
            Type[] types = assembly.GetTypes();

            // find all registerable types and register them
            foreach (Type type in types)
            {
                if (IsRegisterable(type)) RegisterType(type);
            }
        }

        /// <summary>
        /// Searches for all registerable types within the given namespace constrained to the specified 
        /// assembly and registers them.
        /// </summary>
        /// <param name="nmspace">A namespace to be searched.</param>
        /// <param name="assembly">An assembly to be searched.</param>
        public void RegisterTypes(string nmspace, Assembly assembly)
        {
            // find all types from the given namespace
            RegisterTypes(Types.GetTypesWithinNamespace(nmspace, assembly));
        }

        public void RegisterTypes(IEnumerable types)
        {
            // find all registerable types and register them
            foreach (Type type in types)
            {
                if (IsRegisterable(type)) RegisterType(type);
            }
        }

        /// <summary>
        /// Searches for all registerable types within a given namespace and registers them.
        /// </summary>
        /// <param name="nmspace">A namespace to be searched.</param>
        /// <param name="ignoredAssemblies">A set of ignored assemblies.</param>
        public void RegisterTypes(string nmspace, Set<string> ignoredAssemblies)
        {
            // find all types from the given namespace
            Type[] types = Types.GetTypesWithinNamespace(nmspace, ignoredAssemblies);

            // find all registerable types and register them
            foreach (Type type in types)
            {
                if (IsRegisterable(type)) RegisterType(type);
            }
        }

        /// <summary>
        /// Tries to find a registered types.
        /// </summary>
        /// <param name="name">The full name of the type.</param>
        /// <returns>The type that corresponds to the given full name or a null reference when not found.</returns>
        public Type TryFindRegisteredType(TName name)
        {
            Type result;
            return RegisteredTypes.TryGetValue(name, out result) ? result : null;
        }

        /// <summary>
        /// Finds a registered types.
        /// </summary>
        /// <param name="name">The full name of the type.</param>
        /// <returns>The type that corresponds to the given full name.</returns>
        /// <exception cref="InvalidOperationException">When there's no such registered type.</exception>
        public Type FindRegisteredType(TName name)
        {
            EnsureRegisteredType(name);
            return RegisteredTypes[name];
        }

        /// <summary>
        /// Finds a registered types.
        /// </summary>
        /// <param name="type">Thetype.</param>
        /// <returns>The type name that corresponds to the given type.</returns>
        /// <exception cref="InvalidOperationException">When there's no such registered type.</exception>
        public TName FindRegisteredTypeKey(Type type)
        {
            foreach (var pair in RegisteredTypes)
            {
                if (pair.Value == type) return pair.Key;
            }
            throw new InvalidOperationException("No such registered type");
        }


        /// <summary>
        /// Checks whether the given type is a registered type.
        /// </summary>
        /// <param name="name">The name of a type.</param>
        /// <returns></returns>
        public bool IsRegisteredType(TName name)
        {
            return RegisteredTypes.ContainsKey(name);
        }

        /// <summary>
        /// Checks whether the given type is a registered type.
        /// </summary>
        /// <param name="type">A type.</param>
        /// <returns></returns>
        public bool IsRegisteredType(Type type)
        {
            return IsRegisteredType(GetKey(type));
        }

        protected virtual void OnTypeRegistered(Type type)
        {
            // intentionally left blank
        }

        protected virtual bool IsRegisterable(Type type)
        {
            return type.IsClass && !type.IsGenericType && (typeof (TBase).IsInterface ? Types.HasInterface(type, typeof (TBase)) : type.IsSubclassOf(typeof (TBase)));
        }

        protected abstract TName GetKey(Type type);

        /// <summary>
        /// Implements the <see cref="RegisterType"/> method.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        protected void RegisterType(TName name, Type type)
        {
            if (!IsRegisterable(type)) throw new ArgumentException(String.Format("Cannot register type “{0}”", type.FullName));
            RegisteredTypes.Add(name, type);
            OnTypeRegistered(type);
        }

        /// <summary>
        /// Ensures that the given type is a registered type.
        /// </summary>
        /// <param name="name">The name of a type.</param>
        /// <exception cref="InvalidOperationException">The given type is not a registered type.</exception>
        protected void EnsureRegisteredType(TName name)
        {
            if (!IsRegisteredType(name))
                throw new InvalidOperationException(String.Format("“{0}” is not a registered type", name));
        }

        /// <summary>
        /// Ensures that the given type is a registered type.
        /// </summary>
        /// <param name="type">A type.</param>
        /// <exception cref="InvalidOperationException">The given type is not a registered type.</exception>
        protected void EnsureRegisteredType(Type type)
        {
            EnsureRegisteredType(GetKey(type));
        }
    }

    /// <summary>
    /// A generic registry of types organized by string names.
    /// </summary>
    /// <typeparam name="TBase"></typeparam>
    public class TypeRegistry<TBase> : TypeRegistryBase<TBase, string>
        where TBase : class
    {
        protected override string GetKey(Type type)
        {
            return type.AssemblyQualifiedName;
        }
    }
}