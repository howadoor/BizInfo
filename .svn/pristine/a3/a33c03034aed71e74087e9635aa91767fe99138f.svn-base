using System;

namespace Perenis.Core.Component.Progress
{
    /// <summary>
    /// A general-purpose progress observer interface.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This interface allows the observer to watch progress in a process-specified scale from the
    /// <see cref="Minimum"/> to the <see cref="Maximum"/> value.
    /// </para>
    /// <para>
    /// In simple scenarios the target object that consumes the actuall progress (i.e. to show
    /// a progress bar to a user) implements this interface and is responsible for holding the
    /// progress scale boundaries as well as the current (last) progress.
    /// </para>
    /// </remarks>
    /// <seealso cref="IProgressPercentageConsumer"/>
    /// <seealso cref="IProgressController"/>
    [Obsolete("Use IProgressTransaction & IProgressNotification instead.")]
    public interface IProgressObserver
    {
        /// <summary>
        /// The lowest progress value of the given scale.
        /// </summary>
        uint Minimum { get; }

        /// <summary>
        /// The highest progress value of the given scale.
        /// </summary>
        uint Maximum { get; }

        /// <summary>
        /// The length of the given scale (i.e. <c>Scale == Maximum - Minimum</c>).
        /// </summary>
        uint Scale { get; }

        /// <summary>
        /// Current progress value in the given scale.
        /// </summary>
        /// <remarks>
        /// The default value is equal to <see cref="Minimum"/> at the time <see cref="Minimum"/>
        /// and <see cref="Maximum"/> are initialized.
        /// </remarks>
        uint Current { get; }

        /// <summary>
        /// Called just before a process whose progress is to be observed is started.
        /// </summary>
        /// <param name="sender">The object that executes a process.</param>
        /// <remarks>
        /// The lowest progress (i.e. the <see cref="Minimum"/> value) and highest progress (i.e. the 
        /// <see cref="Maximum"/> value) are assumed to be defined by the implementor of this interface.
        /// </remarks>
        void OnStartProgress(object sender);

        /// <summary>
        /// Called just before a process whose progress is to be observed is started.
        /// </summary>
        /// <param name="sender">The object that executes a process.</param>
        /// <param name="minimum">The lowes progress of the process.</param>
        /// <param name="maximum">The highest progress of the process.</param>
        /// <exception cref="ArgumentException">The given <paramref name="minimum"/> and 
        /// <paramref name="maximum"/> are not a valid range.</exception>
        void OnStartProgress(object sender, uint minimum, uint maximum);

        /// <summary>
        /// Called just before a process whose progress is to be observed is started.
        /// </summary>
        /// <param name="sender">The object that executes a process.</param>
        /// <param name="total">The expected total amount of steps of the process.</param>
        /// <remarks>
        /// The lowest progress (i.e. the <see cref="Minimum"/> value) is assumed to be zero. The
        /// highest progress (i.e. the <see cref="Maximum"/> value) is set to the given 
        /// <paramref name="total"/>.
        /// </remarks>
        void OnStartProgress(object sender, uint total);

        /// <summary>
        /// Called when progress is made during execution of a process.
        /// </summary>
        /// <param name="sender">The object that executes a process.</param>
        /// <param name="progress">The current progress of a process. The value of 
        /// <paramref name="progress"/> has to fit between the specified <see cref="Minimum"/>
        /// and <see cref="Maximum"/>.</param>
        /// <remarks>
        /// When the given <paramref name="progress"/> is out of range, the call shall be ignored.
        /// </remarks>
        void OnProgress(object sender, uint progress);

        /// <summary>
        /// Called just after a process whose progress were to be observed has been finished.
        /// </summary>
        /// <param name="sender">The object that executes a process.</param>
        void OnStopProgress(object sender);
    }
}