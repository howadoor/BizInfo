using System;

namespace Perenis.Core.Component.Progress
{
    /// <summary>
    /// Base of progress information interface.
    /// </summary>
    public interface IProgressInformation
    {
        /// <summary>
        /// The creation-timestamp of this message; set to <see cref="DateTime.Now"/> when the instance 
        /// is created.
        /// </summary>
        DateTime Timestamp { get; }
    }
}