using System;
using Perenis.Core.Exceptions.Configuration;

namespace Perenis.Core.Exceptions
{
    /// <summary>
    /// Provides configurable policy-based exception handling capabilities using the
    /// <see cref="ExceptionPolicyConfigurationSection.Default"/> configuration section.
    /// </summary>
    public static class Ex
    {
        /// <summary>
        /// Retrieves a handling ID of the given <see cref="exception"/> occurence.
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        /// <remarks>
        /// The handling ID is stored within the <see cref="Exception.Data"/> and thus is persistent
        /// accross multiple exception handlers applied to the same exception occurence.
        /// </remarks>
        public static Guid GetHandlingInstanceId(this Exception exception)
        {
            // TODO Consider assigning the handling instance ID to all inner exceptions as well; see also ExceptionEx.Unbox()

            ExceptionHandler.SetHandlingInstanceId(exception);
            return (Guid) exception.Data["HandlingInstanceId"];
        }

        /// <summary>
        /// Handles the given exception using the current exception handling configuration.
        /// </summary>
        /// <param name="ex">The exception to be handled.</param>
        /// <param name="policyObject">The exception policy object.</param>
        /// <returns><c>true</c> if the exception shall be re-thrown; otherwise <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="ex"/> is a null reference.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="policyObject"/> is a null reference.</exception>
        public static bool Handle(this Exception ex, object policyObject)
        {
            return ExceptionHandler.Default.Handle(ex, policyObject);
        }

        /// <summary>
        /// Handles the given exception using the current exception handling configuration.
        /// </summary>
        /// <param name="ex">The exception to be handled.</param>
        /// <param name="policyName">The name of the exception policy to be applied.</param>
        /// <param name="policyObject">The exception policy object.</param>
        /// <returns><c>true</c> if the exception has been handled; <c>false</c> if it shall be re-thrown.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="ex"/> is a null reference.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="policyName"/> is a null reference.</exception>
        /// <exception cref="ArgumentException"><paramref name="policyName"/> is an empty string.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="policyObject"/> is a null reference.</exception>
        public static bool Handle(this Exception ex, string policyName, object policyObject)
        {
            return ExceptionHandler.Default.Handle(ex, policyName, policyObject);
        }
    }
}