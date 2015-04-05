using System;

namespace Perenis.Core.Component.Progress
{
    /// <summary>
    /// Progress transaction represents one level of task progress indication logic.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Every progress transaction (except the root) contains a reference to its parent transaction.
    /// This allows for progress transaction stacking that may reflect the program execution stack (call stack).
    /// </para>
    /// <para>
    /// Parent progress specifies how much the transactions higher in the execution
    /// stack will contribute to the overall progress. This is refered to as scale allocation.
    /// The progress value is contributed not just to the current transaction but also to the parent
    /// transaction and in the portion prescribed by scale allocation.
    /// This mechanism allows to encapsulate progress logic on every execution level of progress indication.
    /// A program method doesn't have to know the parent's scale range and current progress value, it simply
    /// defines it's own range; how much of the progress is contributed to the parent is perfectly in the parent's control.
    /// </para>
    /// <para>
    /// Progress disposal is a result of the call of <see cref="IDisposable.Dispose"/>.
    /// When a progress is disposed, it is stopped if still running.
    /// <see cref="IDisposable"/> allows to use <c>using</c> statement to scope the
    /// progress transactions and to make sure, progress is stopped and disposed when 
    /// its out of its execution scope.
    /// </para>
    /// </remarks>
    public interface IProgressTransaction : IDisposable
    {
        /// <summary>
        /// Human-readable progress task name.
        /// </summary>
        /// <remarks>
        /// The name is usually displayed in the task progress indication GUI.
        /// </remarks>
        string TaskName { get; }

        /// <summary>
        /// Human-readable progress task hierarchy name .
        /// </summary>
        string TaskFullName { get; }

        /// <summary>
        /// Unique ID of transaction.
        /// </summary>
        Guid TransactionId { get; }

        /// <summary>
        /// Progress minimum value.
        /// </summary>
        int ScaleMin { get; }

        /// <summary>
        /// Progress maximum value.
        /// </summary>
        int ScaleMax { get; }

        /// <summary>
        /// Returns true if the progress indication is running, false otherwise.
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Current progress value. Is in range of the defined scale.
        /// </summary>
        int Progress { get; set; }

        /// <summary>
        /// Parent transaction. If null, this is the root transaction.
        /// </summary>
        IProgressTransaction ParentTransaction { get; }

        /// <summary>
        /// Starts the progress indication.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the progress indication.
        /// </summary>
        void Stop();

        /// <summary>
        /// Completes the progress indication.
        /// </summary>
        void Complete();

        /// <summary>
        /// Advances the progress value by <paramref name="value"/>. <paramref name="value"/> may be negative.
        /// </summary>
        /// <param name="value">Progress value to add to the current progress value.</param>
        void Advance(int value);

        /// <summary>
        /// Posts a progress message.
        /// </summary>
        /// <param name="message"></param>
        void PostMessage(IProgressMessage message);

        /// <summary>
        /// Creates inner progress transaction.
        /// </summary>
        /// <remarks>
        /// This proogress transaction will become the parent of the transaction being created.
        /// </remarks>
        /// <param name="taskName">Task name.</param>
        /// <param name="scaleMin">Minimum progress value.</param>
        /// <param name="scaleMax">Maximum progress value.</param>
        /// <param name="allocatedScale">Allocated scale for the inner transaction.</param>
        /// <param name="progressNotification">Progress notification.</param>
        /// <returns>Inner progress transaction.</returns>
        IProgressTransaction CreateInnerTransaction(string taskName, int scaleMin, int scaleMax, int allocatedScale, IProgressNotification progressNotification);
    }
}