using System;
using Perenis.Core.Component;
using Perenis.Core.Pattern;

namespace Perenis.Core.Exceptions
{
    /// <summary>
    /// Represents a user-defined exception policy object.
    /// </summary>
    public interface IExceptionPolicy
    {
        /// <summary>
        /// Gets the name of this exception policy.
        /// </summary>
        string ExceptionPolicyName { get; }
    }

    /// <summary>
    /// Provides name-resolution service for objects implementing the <see cref="IExceptionPolicy"/> interface.
    /// </summary>
    internal class ExceptionPolicyStructuredNamingResolver : Singleton<ExceptionPolicyStructuredNamingResolver>, IStructuredNamingResolver<object>
    {
        #region ------ Implementation of the IStructuredNamingResolver interface ------------------

        public string GetStructuredName(object nameObject)
        {
            if (nameObject == null) throw new ArgumentNullException("nameObject");
            if (!(nameObject is IExceptionPolicy)) return null;

            return ((IExceptionPolicy) nameObject).ExceptionPolicyName;
        }

        #endregion
    }
}