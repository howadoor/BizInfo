using System;
using System.Reflection;

namespace Perenis.Core.Reflection
{
    /// <summary>
    /// A generic type factory with a specifiable base type.
    /// </summary>
    /// <typeparam name="TBase"></typeparam>
    // TODO Split the implementation of the ITypeFactoryNonPublic into a separate class.
    public class TypeFactory<TBase> : ITypeFactory<TBase>, ITypeFactoryNonPublic<TBase>
        where TBase : class
    {
        private static readonly object lockObject = new object();
        private static TypeFactory<TBase> instance;
        private readonly Type actualType;

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public TypeFactory()
        {
            actualType = typeof (TBase);
        }

        /// <summary>
        /// Constructs runtime-based type factory.
        /// </summary>
        /// <param name="actualType">The actual type of the instance to be created. Must be 
        /// compatible with the <typeparamref name="TBase"/></param>
        public TypeFactory(Type actualType)
        {
            if (!(typeof (TBase).IsAssignableFrom(actualType)))
            {
                throw new ArgumentException("actualType must be compatible with the class' type parameter");
            }

            this.actualType = actualType;
        }

        /// <summary>
        /// A default type factory for any <typeparamref name="TBase"/>-based type.
        /// </summary>
        public static TypeFactory<TBase> Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new TypeFactory<TBase>();
                        }
                    }
                }

                return instance;
            }
        }

        #region ITypeFactory<TBase> Members

        /// <summary>
        /// Creates a new instance of the given type. Calls a parameter-less constructor.
        /// </summary>
        /// <param name="type">The type whose instance is to be created.</param>
        public TBase CreateInstance(Type type)
        {
            return (TBase) type.InvokeMember(null, BindingFlags.CreateInstance, null, null, new object[] {});
        }

        /// <summary>
        /// Creates a new instance of the given type. Calls a constructor with the given parameters.
        /// </summary>
        /// <param name="type">The type whose instance is to be created.</param>
        /// <param name="args">Arguments to the constructor.</param>
        public TBase CreateInstance(Type type, params object[] args)
        {
            return (TBase) type.InvokeMember(null, BindingFlags.CreateInstance, null, null, args);
        }

        /// <summary>
        /// Creates a new instance of the given type. Calls a parameter-less constructor.
        /// </summary>
        /// <typeparam name="T">The type whose instance is to be created.</typeparam>
        public T CreateInstance<T>() where T : TBase
        {
            return (T) typeof (T).InvokeMember(null, BindingFlags.CreateInstance, null, null, new object[] {});
        }

        /// <summary>
        /// Creates a new instance of the given type. Calls a constructor with the given parameters.
        /// </summary>
        /// <typeparam name="T">The type whose instance is to be created.</typeparam>
        /// <param name="args">Arguments to the constructor.</param>
        public T CreateInstance<T>(params object[] args) where T : TBase
        {
            return (T) typeof (T).InvokeMember(null, BindingFlags.CreateInstance, null, null, args);
        }

        /// <summary>
        /// Creates a new instance of the runtime type. Calls a parameter-less constructor.
        /// </summary>
        public TBase CreateInstance()
        {
            return (TBase) actualType.InvokeMember(null, BindingFlags.CreateInstance, null, null, new object[] {});
        }

        /// <summary>
        /// Creates a new instance of the runtime type. Calls a constructor with the given parameters.
        /// </summary>
        /// <param name="args">Arguments to the constructor.</param>
        public TBase CreateInstance(params object[] args)
        {
            return (TBase) actualType.InvokeMember(null, BindingFlags.CreateInstance, null, null, args);
        }

        #endregion

        #region ITypeFactoryNonPublic<TBase> Members

        /// <summary>
        /// Creates a new instance of the given type. Calls a parameter-less non-public constructor.
        /// </summary>
        /// <param name="type">The type whose instance is to be created.</param>
        public TBase CreateInstanceNonPublic(Type type)
        {
            return (TBase) type.InvokeMember(null, BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.NonPublic, null, null, new object[] {});
        }

        /// <summary>
        /// Creates a new instance of the given type. Calls a non-public constructor with the given parameters.
        /// </summary>
        /// <param name="type">The type whose instance is to be created.</param>
        /// <param name="args">Arguments to the constructor.</param>
        public TBase CreateInstanceNonPublic(Type type, params object[] args)
        {
            return (TBase) type.InvokeMember(null, BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.NonPublic, null, null, args);
        }

        /// <summary>
        /// Creates a new instance of the given type. Calls a parameter-less non-public constructor.
        /// </summary>
        /// <typeparam name="T">The type whose instance is to be created.</typeparam>
        public T CreateInstanceNonPublic<T>() where T : TBase
        {
            return (T) typeof (T).InvokeMember(null, BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.NonPublic, null, null, new object[] {});
        }

        /// <summary>
        /// Creates a new instance of the given type. Calls a non-public constructor with the given parameters.
        /// </summary>
        /// <typeparam name="T">The type whose instance is to be created.</typeparam>
        /// <param name="args">Arguments to the constructor.</param>
        public T CreateInstanceNonPublic<T>(params object[] args) where T : TBase
        {
            return (T) typeof (T).InvokeMember(null, BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.NonPublic, null, null, args);
        }

        #endregion
    }
}