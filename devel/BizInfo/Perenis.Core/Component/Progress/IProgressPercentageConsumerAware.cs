using System;

namespace Perenis.Core.Component.Progress
{
    /// <summary>
    /// Represents a process with consumable percentage-scaled progress.
    /// </summary>
    /// <remarks>
    /// This interface shall be implemented by classes that encapsulate one or more progress-providing
    /// processes and report their progress as a unified percentage-scaled progress.
    /// </remarks>
    /// <seealso cref="IProgressObserverAware"/>
    [Obsolete("Use IProgressTransaction & IProgressNotification instead.")]
    public interface IProgressPercentageConsumerAware
    {
        /// <summary>
        /// An progress-percentage consumer that's notified about the percentage-scaled progress of 
        /// the process executed by the class implementing this interface.
        /// </summary>
        IProgressPercentageConsumer ProgressPercentageConsumer { get; set; }
    }
}