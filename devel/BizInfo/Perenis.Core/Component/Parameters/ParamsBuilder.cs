using System;
using Perenis.Core.Pattern;

namespace Perenis.Core.Component.Parameters
{
    /// <summary>
    /// Maintains a set of parameters and allows new parameters to be added.
    /// </summary>
    public class ParamsBuilder
    {
        private readonly ParameterCollection parameters = new ParameterCollection();

        /// <summary>
        /// The parameters maintained by this instance.
        /// </summary>
        public ParameterCollection Parameters
        {
            get { return parameters; }
        }

        /// <summary>
        /// Adds a new parameter to the set of parameters maintained by this instance.
        /// </summary>
        /// <param name="paramName">The parameter name-object.</param>
        /// <param name="paramValue">The parameter's value.</param>
        /// <returns>Returns <c>this</c> instance for chaining method invocations.</returns>
        public ParamsBuilder Add(object paramName, object paramValue)
        {
            parameters.Add(new Parameter(paramName, paramValue));
            return this;
        }

        #region ------ Parameter name resolution --------------------------------------------------

        /// <summary>
        /// Parameter name resolver.
        /// </summary>
        private static readonly IStructuredNamingResolver<object> parameterNameResolver = new StructuredNamingResolver<object>(Singleton<EnumStructuredNamingResolver>.Instance, Singleton<StringStructuredNamingResolver>.Instance);

        /// <summary>
        /// Retrieves the name of a parameter specified by the given <paramref name="paramName"/> object.
        /// </summary>
        /// <param name="paramName">The parameter name-object.</param>
        /// <returns>The name of the parameter.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="paramName"/> is a null reference.</exception>
        /// <remarks>
        /// Parameter name-resolution is based on the <see cref="IStructuredNamingResolver{T}"/> mechanism.
        /// </remarks>
        public static string GetParameterName(object paramName)
        {
            // TODO Inferring the expected type from the ParamTypeAttribute
            if (paramName == null) throw new ArgumentNullException("paramName");
            return TryGetParameterName(paramName);
        }

        /// <summary>
        /// Retrieves the name of a parameter specified by the given <paramref name="paramName"/> object.
        /// </summary>
        /// <param name="paramName">The parameter name-object.</param>
        /// <returns>The name of the parameter or null if <paramref name="paramName"/> is null.</returns>
        /// <remarks>
        /// Parameter name-resolution is based on the <see cref="IStructuredNamingResolver{T}"/> mechanism.
        /// </remarks>
        public static string TryGetParameterName(object paramName)
        {
            if (paramName == null) return null;
            string result = parameterNameResolver.GetStructuredName(paramName);
            if (result == null) throw new InvalidOperationException(String.Format("Unsupported parameter name object of type '{0}' supplied.", paramName.GetType().FullName));
            return result;
        }

        #endregion
    }
}