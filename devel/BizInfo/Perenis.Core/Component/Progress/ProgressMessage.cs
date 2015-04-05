using System;
using System.ComponentModel;
using System.Reflection;
using Perenis.Core.Exceptions;

namespace Perenis.Core.Component.Progress
{
    /// <summary>
    /// End-user progress message.
    /// </summary>
    /// <remarks>
    /// Progress messages are usually displayed as part of the GUI task progress indicators.
    /// </remarks>
    [Serializable]
    public class ProgressMessage : IProgressMessage
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="severity">Message severity.</parparam>
        /// <param name="summary">Message summary.</parparam>
        /// <exception cref="ArgumentNullException"><paramref name="summary"/> is a null reference.</exception>
        /// <exception cref="ArgumentException"><paramref name="summary"/> is an empty string.</exception>
        public ProgressMessage(Severity severity, string summary)
        {
            if (summary == null) throw new ArgumentNullException("summary");
            if ((summary != null && String.IsNullOrEmpty(summary))) throw new ArgumentException("summary");
            Initialize(DateTime.Now, summary, severity, null);
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="severity">Message severity.</parparam>
        /// <param name="summary">Message summary.</parparam>
        /// <param name="description">Optional detailed message description.</param>
        /// <exception cref="ArgumentNullException"><paramref name="summary"/> is a null reference.</exception>
        /// <exception cref="ArgumentException"><paramref name="summary"/> is an empty string.</exception>
        public ProgressMessage(Severity severity, string summary, string description)
        {
            if (summary == null) throw new ArgumentNullException("summary");
            if ((summary != null && String.IsNullOrEmpty(summary))) throw new ArgumentException("summary");
            Initialize(DateTime.Now, summary, severity, description);
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <para
        /// <param name="severity">Message severity.</parparam>
        /// <param name="ex">Exception whose <see cref="Exception.Message"/> is to be used a the message summary.</parparam>
        /// <param name="description">Optional detailed message description.</param>
        /// <exception cref="ArgumentNullException"><paramref name="ex"/> is a null reference.</exception>
        /// <remarks>
        /// Note that the exception is first unboxed (see <see cref="ExceptionEx.Unbox"/>) to retrieve
        /// the real message.
        /// </remarks>
        public ProgressMessage(Severity severity, Exception ex, string description)
        {
            if (ex == null) throw new ArgumentNullException("ex");
            Initialize(DateTime.Now, ex.Unbox<TargetInvocationException>().Message, severity, description);
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <para
        /// <param name="severity">Message severity.</parparam>
        /// <param name="summary">Message summary.</parparam>
        /// <param name="ex">Exception whose <see cref="Exception.Message"/> is to be used a the message detailed description.</parparam>
        /// <exception cref="ArgumentNullException"><paramref name="summary"/> is a null reference.</exception>
        /// <exception cref="ArgumentException"><paramref name="summary"/> is an empty string.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ex"/> is a null reference.</exception>
        /// <remarks>
        /// Note that the exception is first unboxed (see <see cref="ExceptionEx.Unbox"/>) to retrieve
        /// the real message.
        /// </remarks>
        public ProgressMessage(Severity severity, string summary, Exception ex)
        {
            if (summary == null) throw new ArgumentNullException("summary");
            if ((summary != null && String.IsNullOrEmpty(summary))) throw new ArgumentException("summary");
            if (ex == null) throw new ArgumentNullException("ex");
            Initialize(DateTime.Now, summary, severity, ex.Unbox<TargetInvocationException>().Message);
        }

        #region IProgressMessage Members

        /// <summary>
        /// The creation-timestamp of this message; set to <see cref="DateTime.Now"/> when the instance 
        /// is created.
        /// </summary>
        [Browsable(true)]
        public DateTime Timestamp { get; private set; }

        /// <summary>
        /// Message severity.
        /// </summary>
        [Browsable(true)]
        public Severity Severity { get; private set; }

        /// <summary>
        /// Message summary.
        /// </summary>
        [Browsable(true)]
        public string Summary { get; private set; }

        /// <summary>
        /// Optional detailed message description.
        /// </summary>
        [Browsable(true)]
        public string Description { get; private set; }

        #endregion

        #region ------ Internals ------------------------------------------------------------------

        private void Initialize(DateTime timestamp, string summary, Severity severity, string description)
        {
            Timestamp = timestamp;
            Severity = severity;
            Summary = summary;
            Description = description;
        }

        #endregion
    }
}