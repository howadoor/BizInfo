using System;
using System.Configuration;
using System.Threading;

namespace Perenis.Core.Log
{
    /// <summary>
    /// Represents a logging context that maintains a unique logging session ID.
    /// </summary>
    /// <remarks>
    /// When a new <see cref="LoggingContext"/> is created, the supplied or auto-assigned
    /// logging session ID is prepended with the optionally specified prefix found in the
    /// <c>appSettings</c> configuration section under the <c>Perenis.Core.Logging.SessionIdPrefix</c>
    /// key.
    /// </remarks>
    public class LoggingContext
    {
        // TODO Move the Perenis.Core.Logging.SessionIdPrefix setting into a configuration section.

        private readonly string loggingSessionId;

        /// <summary>
        /// The unique ID of the current logging context.
        /// </summary>
        /// <remarks>
        /// The ID is guaranteed to be unique per-run of the current application.
        /// </remarks>
        public string LoggingSessionId
        {
            get { return loggingSessionId; }
        }

        /// <summary>
        /// Gets a sequence number unique for the current instance.
        /// </summary>
        /// <returns>The assigned sequence number.</returns>
        public int GetSequence()
        {
            return Interlocked.Increment(ref sequence);
        }

        /// <summary>
        /// Creates a new logging context with am auto-assigned unique logging session ID.
        /// </summary>
        /// <returns>The created logging context.</returns>
        /// <remarks>
        /// The auto-assigned unique logging session ID is an eight-digit decimal sequence number.
        /// </remarks>
        public static LoggingContext Create()
        {
            return new LoggingContext(Interlocked.Increment(ref loggingSessionIdCounter));
        }

        /// <summary>
        /// Creates a new logging context with the given logging session ID.
        /// </summary>
        /// <param name="loggingSessionId">Given logging session ID.</param>
        /// <returns>The created logging context.</returns>
        /// <remarks>
        /// The user is responsible for keeping the <paramref name="loggingSessionId"/> unique.
        /// </remarks>
        public static LoggingContext Create(string loggingSessionId)
        {
            return new LoggingContext(loggingSessionId);
        }

        #region ------ Internals ------------------------------------------------------------------

        /// <summary>
        /// A prefix of the logging session ID.
        /// </summary>
        private static readonly string loggingSessionIdPrefix = ConfigurationManager.AppSettings["Perenis.Core.Logging.SessionIdPrefix"] ?? String.Empty;

        /// <summary>
        /// A counter for the numeric part of the logging session IDs.
        /// </summary>
        private static int loggingSessionIdCounter;

        /// <summary>
        /// A sequence counter unique for the current instance.
        /// </summary>
        private int sequence;

        /// <summary>
        /// Initialization.
        /// </summary>
        /// <param name="loggingSessionId"></param>
        private LoggingContext(string loggingSessionId)
        {
            this.loggingSessionId = loggingSessionId;
        }

        /// <summary>
        /// Initialization.
        /// </summary>
        /// <param name="loggingSessionNumber"></param>
        private LoggingContext(int loggingSessionNumber)
        {
            loggingSessionId = loggingSessionIdPrefix + loggingSessionNumber.ToString("00000000");
        }

        #endregion
    }
}