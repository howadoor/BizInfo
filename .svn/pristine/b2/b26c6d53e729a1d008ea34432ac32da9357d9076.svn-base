using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace BizInfo.App.Services.Logging
{
    /// <summary>
    /// Handy extension methods arround MS Enterprise Library Logging Block
    /// </summary>
    public static class LoggingTools
    {
        private static LogWriter logWriter = EnterpriseLibraryContainer.Current.GetInstance<LogWriter>();

        public class StartAndStopLog : IDisposable
        {
            public DateTime StartTime { get; private set; }
            public string OperationName { get; private set; }

            protected internal StartAndStopLog(string operationName)
            {
                StartTime = DateTime.Now;
                OperationName = operationName;
                this.LogStart(OperationName);
            }

            public virtual void Dispose()
            {
                var duration = DateTime.Now - StartTime;
                var extendedProperties = new Dictionary<string, object>();
                extendedProperties["Duration"] = duration;
                this.LogStop(OperationName, extendedProperties);
            }
        }

        /// <summary>
        /// Used to log start and stop of operation
        /// </summary>
        /// <param name="object"></param>
        /// <param name="operationName">Name of operation</param>
        /// <returns>Newly created instance of <see cref="StartAndStopLog"/>. It should be disposed when operations ends</returns>
        /// <remarks>
        /// Usage:
        /// <example>
        /// <code>
        /// using (this.LogOperation("Doing something")
        /// {
        ///     DoSomething();
        /// }
        /// </code>
        /// </example>
        /// </remarks>
        public static StartAndStopLog LogOperation(this object @object, string operationName)
        {
            return new StartAndStopLog(operationName);
        }
        
        public static void LogException(this object @object, Exception exception)
        {
            var exceptionDictionary = new Dictionary<string, object>();
            exceptionDictionary["Exception"] = exception;
            @object.Log(exception.Message, TraceEventType.Error, new Dictionary<string, object>());
            @object.LogInfo(exception.ToString());
        }

        public static void LogStart(this object @object, string message)
        {
            @object.Log(message, TraceEventType.Start);
        }

        public static void LogStop(this object @object, string message, Dictionary<string, object> extendedProperties)
        {
            @object.Log(message, TraceEventType.Stop, extendedProperties);
        }
        
        public static void LogInfo(this object @object, string message)
        {
            @object.Log(message, TraceEventType.Information);
        }
        
        public static void Log(this object @object, string message, TraceEventType severity = TraceEventType.Information, Dictionary<string, object> extendedProperties = null)
        {
            if (!IsLoggingEnabled) return;
            var logEntry = new LogEntry
                               {
                                   Message = message,
                                   Severity = severity,
                                   ExtendedProperties = extendedProperties,
                               };
            logWriter.Write(logEntry);
        }

        public static bool IsLoggingEnabled
        {
            get
            {
                return logWriter != null && logWriter.IsLoggingEnabled();
            }
        }
    }
}