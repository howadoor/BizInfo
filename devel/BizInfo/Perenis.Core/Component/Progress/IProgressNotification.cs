namespace Perenis.Core.Component.Progress
{
    /// <summary>
    /// Progress notification.
    /// </summary>
    /// <remarks>
    /// Contains methods that may be called as the result of progress state change.
    /// </remarks>
    public interface IProgressNotification
    {
        /// <summary>
        /// Called when progress value is changed.
        /// </summary>
        /// <param name="progressTransaction">Related progress transaction.</param>
        void OnProgressChanged(IProgressTransaction progressTransaction);

        /// <summary>
        /// Called when a progress is started.
        /// </summary>
        /// <param name="progressTransaction">Related progress transaction.</param>
        void OnProgressStarted(IProgressTransaction progressTransaction);

        /// <summary>
        /// Called when a progress is stopped.
        /// </summary>
        /// <param name="progressTransaction">Related progress transaction.</param>
        void OnProgressStopped(IProgressTransaction progressTransaction);

        /// <summary>
        /// Called when progress is disposed.
        /// </summary>
        /// <remarks>
        /// See <see cref="IProgressTransaction"/> for more information about the semantics of 
        /// progress disposal.
        /// </remarks>
        /// <param name="progressTransaction">Related progress transaction.</param>
        void OnProgressDisposed(IProgressTransaction progressTransaction);

        /// <summary>
        /// Called when progress message is posted.
        /// </summary>
        /// <param name="progressTransaction">Related progress transaction.</param>
        /// <param name="message">Message data.</param>
        void OnProgressMessage(IProgressTransaction progressTransaction, IProgressMessage message);
    }
}