using System;

namespace Perenis.Core.Component.Progress
{
    /// <summary>
    /// A general-purpose progress-percentage consumer interface.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This interface allows the observer to watch progress in a percentage scale from zero to 100 %.
    /// </para>
    /// <para>
    /// In advaced scenarios the target object that consumes the actuall progress (i.e. to show
    /// a progress bar to a user) implements this interface and is thus need <b>not</b> to be 
    /// responsible for holding the progress scale boundaries etc. Instead, the actual progress in
    /// the original scale is being converted to the percentage scale by a progress controller. Thus
    /// the controller may implement advanced progress metering logic like progress aggregation.
    /// </para>
    /// </remarks>
    /// <seealso cref="IProgressObserver"/>
    /// <seealso cref="IProgressController"/>
    /// <seealso cref="ProgressController"/>
    /// <seealso cref="CompoundProgressController"/>
    [Obsolete("Use IProgressTransaction & IProgressNotification instead.")]
    public interface IProgressPercentageConsumer
    {
        /// <summary>
        /// Called just before a process whose progress is to be observed is started.
        /// </summary>
        /// <param name="progressController">The controller in charge of progress processing.</param>
        /// <param name="sender">The object that executes a process.</param>
        void OnStartProgress(IProgressController progressController, object sender);

        /// <summary>
        /// Called when progress is made during execution of a process.
        /// </summary>
        /// <param name="progressController">The controller in charge of progress processing.</param>
        /// <param name="sender">The object that executes a process.</param>
        /// <param name="progressPercentage">The current progress of a process in percents.</param>
        /// <remarks>
        /// The value <paramref name="progressPercentage"/> is guaranteed to be between zero and 100 %.
        /// </remarks>
        void OnProgress(IProgressController progressController, object sender, decimal progressPercentage);

        /// <summary>
        /// Called just after a process whose progress were to be observed has been finished.
        /// </summary>
        /// <param name="progressController">The controller in charge of progress processing.</param>
        /// <param name="sender">The object that executes a process.</param>
        void OnStopProgress(IProgressController progressController, object sender);
    }
}