using System;
using Perenis.Core.Log;

namespace Perenis.Core.Exceptions
{
    /// <summary>
    /// Logs an exception; the exception remains unhandled.
    /// </summary>
    public class LoggingExceptionHandler : IExceptionHandler
    {
        #region ------ Implementation of the IExceptionHandler interface --------------------------

        public bool Handle(Exception ex, string policyName, object policyObject)
        {
            this.LogInfo(ex, "Exception occured, the '{0}' exception policy is being applied", policyName);
            return false;
        }

        #endregion
    }
}