using System;
using System.Collections.Generic;

namespace Perenis.Core.Component
{
    /// <summary>
    /// Aggegates name-resolution services provided by several registered 
    /// <see cref="IStructuredNamingResolver{T}"/> instances.
    /// </summary>
    /// <typeparam name="T">The base type of named objects.</typeparam>
    public class StructuredNamingResolver<T> : IStructuredNamingResolver<T>
    {
        private readonly List<IStructuredNamingResolver<T>> resolvers;

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public StructuredNamingResolver()
        {
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public StructuredNamingResolver(params IStructuredNamingResolver<T>[] resolvers)
        {
            this.resolvers = new List<IStructuredNamingResolver<T>>(resolvers);
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public StructuredNamingResolver(IEnumerable<IStructuredNamingResolver<T>> resolvers)
        {
            this.resolvers = new List<IStructuredNamingResolver<T>>(resolvers);
        }

        #region ------ Implementation of the IStructuredNamingResolver interface ------------------

        public string GetStructuredName(T nameObject)
        {
            if (nameObject == null) throw new ArgumentNullException("nameObject");

            // try registered resolvers and yield the first non-null result
            foreach (var resolver in resolvers)
            {
                string result = resolver.GetStructuredName(nameObject);
                if (result != null) return result;
            }
            return null;
        }

        #endregion

        /// <summary>
        /// Registered individual resolvers.
        /// </summary>
        public List<IStructuredNamingResolver<T>> Resolvers
        {
            get { return resolvers; }
        }
    }
}