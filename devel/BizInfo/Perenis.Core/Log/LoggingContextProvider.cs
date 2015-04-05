using System;
using System.Threading;
using Perenis.Core.Pattern;

namespace Perenis.Core.Log
{
    /// <summary>
    /// Provides and manages a per-thread stack of logging contexts.
    /// </summary>
    /// <remarks>
    /// The purpose of logging contexts is to manage a logging session indication which is automatically
    /// attached into every logged message. It is up to the user whether the logging session reflects
    /// an actual user session or is used otherwise.
    /// </remarks>
    public static class LoggingContextProvider
    {
        [ThreadStatic] private static LoggingContext currentLoggingContext;

        /// <summary>
        /// The current per-thread logging context.
        /// </summary>
        public static LoggingContext CurrentLoggingContext
        {
            get { return currentLoggingContext; }
        }

        /// <summary>
        /// The <see cref="LoggingContext.LoggingSessionId"/> of the <see cref="CurrentLoggingContext"/> 
        /// or <see cref="string.Empty"/> if not logging context exists.
        /// </summary>
        public static string CurrentLoggingSessionId
        {
            get
            {
                LoggingContext ctx = currentLoggingContext;
                return ctx != null ? ctx.LoggingSessionId : String.Empty;
            }
        }

        /// <summary>
        /// Gets a unique sequence number.
        /// </summary>
        /// <returns></returns>
        public static int GetSequence()
        {
            return Interlocked.Increment(ref sequence);
        }

        /// <summary>
        /// Creates a new logging context with a new unique logging session ID and sets it as
        /// the current logging context.
        /// </summary>
        /// <returns>An object controlling the life-time of the logging context.</returns>
        /// <remarks>
        /// <para>
        /// Use this method to create and use a one-time logging context with a constrained life-time.
        /// </para>
        /// <para>
        /// When the object returned by this method is disposed, the previous logging context is restored.
        /// </para>
        /// </remarks>
        public static IDisposable Create()
        {
            return new LoggingContextRegion(LoggingContext.Create());
        }

        /// <summary>
        /// Sets the given logging context as the current logging context.
        /// </summary>
        /// <param name="loggingContext">The logging context to be used.</param>
        /// <returns>An object controlling the life-time of the logging context.</returns>
        /// <remarks>
        /// <para>
        /// Use this method to use a long-living logging context created with one of the 
        /// <see cref="LoggingContext"/>'s factory methods.
        /// </para>
        /// <para>
        /// When the object returned by this method is disposed, the previous logging context is restored.
        /// </para>
        /// </remarks>
        public static IDisposable Use(LoggingContext loggingContext)
        {
            if (loggingContext == null) throw new ArgumentNullException("loggingContext");
            return new LoggingContextRegion(loggingContext);
        }

        #region ------ Internals ------------------------------------------------------------------

        /// <summary>
        /// A unique sequence counter.
        /// </summary>
        private static int sequence;

        /// <summary>
        /// Represents an execution region of code that uses a particular <see cref="LoggingContext"/>.
        /// </summary>
        private class LoggingContextRegion : Disposable
        {
            /// <summary>
            /// The saved value of the <see cref="CurrentLoggingContext"/>.
            /// </summary>
            private readonly LoggingContext savedLoggingContext;

            #region ------ Internals: Disposable overrides ----------------------------------------

            /// <summary>
            /// Disposes the current instance by restoring the saved <see cref="CurrentLoggingContext"/>.
            /// </summary>
            /// <param name="disposing"></param>
            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    currentLoggingContext = savedLoggingContext;
                }
            }

            #endregion

            /// <summary>
            /// Initialization.
            /// </summary>
            /// <param name="newLoggingContext"></param>
            public LoggingContextRegion(LoggingContext newLoggingContext)
            {
                if (newLoggingContext == null) throw new ArgumentNullException("newLoggingContext");
                savedLoggingContext = currentLoggingContext;
                currentLoggingContext = newLoggingContext;
            }
        }

        #endregion
    }
}