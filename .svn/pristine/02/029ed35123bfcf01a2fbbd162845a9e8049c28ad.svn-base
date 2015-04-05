using System;
using System.ComponentModel;

namespace Perenis.Core.Component.Progress
{
    /// <summary>
    /// End-user progress advance.
    /// </summary>
    /// <remarks>
    /// Progress advance is usually displayed as part of the GUI task progress indicators.
    /// </remarks>
    [Serializable]
    public class ProgressAdvance : IProgressAdvance
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="advance"></param>
        public ProgressAdvance(int advance)
        {
            Timestamp = DateTime.Now;
            Advance = advance;
        }

        #region IProgressAdvance Members

        /// <summary>
        /// The creation-timestamp of this message; set to <see cref="DateTime.Now"/> when the instance 
        /// is created.
        /// </summary>
        [Browsable(true)]
        public DateTime Timestamp { get; private set; }

        /// <summary>
        /// Current progress advance value.
        /// </summary>
        [Browsable(true)]
        public int Advance { get; private set; }

        #endregion
    }
}