using System;
using System.Reflection;
using Common.Logging;
using Perenis.Core.Exceptions;
using Perenis.Core.Pattern;

namespace Perenis.Core.Log
{
    /// <summary>
    /// Provides a facility for logging exceptions the first time they reach the logging framework.
    /// </summary>
    public class ExceptionLogger : Singleton<ExceptionLogger>
    {
        /// <summary>
        /// Logs the supplied <paramref name="exception"/> and creates a short indication of it.
        /// </summary>
        /// <param name="source">The source object actually logging the data.</param>
        /// <param name="level">Logging level of this message.</param>
        /// <param name="exception">The exception to be logged.</param>
        /// <returns>Short indication of the exception containing exception type, message, and handling ID.</returns>
        /// <remarks>
        /// Whether the logging <paramref name="level"/> is enabled is evaluated against the instance
        /// of this class and the exception is logged into the logger retrived for this instance;
        /// otherwise, the exception is logged into the logger retrieved for the <paramref name="source"/> 
        /// object.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="exception"/> is a null reference.</exception>
        public string LogException(object source, LogLevel level, Exception exception)
        {
            if (exception == null) throw new ArgumentNullException("exception");

            // unbox the exception to provide relevant summary information
            Exception unboxed = exception.Unbox<TargetInvocationException>();

            // construct the short exception indication
            string shortInfo = String.Format("Exception “{0}” with message “{1}” occured; handling ID “{2}”", unboxed.GetType().FullName, unboxed.Message, unboxed.GetHandlingInstanceId());

            // log full details if not yet logged (use the original not the unboxed exception)
            if (!IsLogged(exception))
            {
                string message = shortInfo + "\r\n" + XmlExceptionFormatter.Default.Format(exception);
                if (this.IsLoggingEnabled(level))
                    this.Log(level, message);
                else
                    source.Log(level, message);

                // mark both the original and the unboxed exception as logged
                exception.Data["Logged"] = true;
                unboxed.Data["Logged"] = true;
            }

            return shortInfo;
        }

        /// <summary>
        /// Checks if the given <paramref name="exception"/> (i.e. its full details) has already been logged.
        /// </summary>
        /// <param name="exception"></param>
        /// <returns><c>true</c> if the exception has alredy been logged; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="exception"/> is a null reference.</exception>
        public static bool IsLogged(Exception exception)
        {
            if (exception == null) throw new ArgumentNullException("exception");

            return exception.Data.Contains("Logged") && (bool) exception.Data["Logged"];
        }
    }
}