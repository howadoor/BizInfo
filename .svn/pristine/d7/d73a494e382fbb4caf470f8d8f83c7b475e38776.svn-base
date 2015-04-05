using System;

namespace Perenis.Core.Component.Progress
{
    /// <summary>
    /// Represents a process with observable progress.
    /// </summary>
    /// <remarks>
    /// This interface shall be implemented by classes that directly implement a single process and 
    /// report it's custom-scaled progress.
    /// </remarks>
    /// <seealso cref="IProgressPercentageConsumerAware"/>
    [Obsolete("Use IProgressTransaction & IProgressNotification instead.")]
    public interface IProgressObserverAware
    {
        /// <summary>
        /// An observer that's notified about the progress of the process executed by the class
        /// implementing this interface.
        /// </summary>
        IProgressObserver ProgressObserver { get; set; }
    }
}