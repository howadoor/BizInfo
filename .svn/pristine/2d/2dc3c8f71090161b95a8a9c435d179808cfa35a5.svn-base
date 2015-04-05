using System;

namespace Perenis.Core.Exceptions
{
    /// <summary>
    /// Represents an exception handler.
    /// </summary>
    public interface IExceptionHandler
    {
        /// <summary>
        /// Handles the given exception.
        /// </summary>
        /// <param name="ex">The exception being handled.</param>
        /// <param name="policyName">The name of the exception policy being applied.</param>
        /// <param name="policyObject">The user-supplied exception policy object.</param>
        /// <returns><c>true</c> when the exception was handled; otherwise, <c>false</c>.</returns>
        bool Handle(Exception ex, string policyName, object policyObject);
    }
}