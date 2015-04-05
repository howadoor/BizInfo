using System;

namespace Perenis.Core.Component.Progress
{
    /// <summary>
    /// Progress notification using the global progress events <see cref="Events"/>.
    /// </summary>
    public class GlobalProgressNotification : IProgressNotification
    {
        #region IProgressNotification Members

        public void OnProgressChanged(IProgressTransaction progressTransaction)
        {
            if (ProgressChanged != null) ProgressChanged(progressTransaction);
        }

        public void OnProgressStarted(IProgressTransaction progressTransaction)
        {
            if (ProgressStarted != null) ProgressStarted(progressTransaction);
        }

        public void OnProgressStopped(IProgressTransaction progressTransaction)
        {
            if (ProgressStopped != null) ProgressStopped(progressTransaction);
        }

        public void OnProgressDisposed(IProgressTransaction progressTransaction)
        {
            if (ProgressDisposed != null) ProgressDisposed(progressTransaction);
            // Temporarily removing for performance issues
            //Publisher.Unpublish(this);
        }

        public void OnProgressMessage(IProgressTransaction progressTransaction, IProgressMessage message)
        {
            if (ProgressMessage != null) ProgressMessage(progressTransaction, message);
        }

        #endregion

        [StructuredName(Events.ProgressChanged)]
        public event Action<IProgressTransaction> ProgressChanged;

        [StructuredName(Events.ProgressStarted)]
        public event Action<IProgressTransaction> ProgressStarted;

        [StructuredName(Events.ProgressStopped)]
        public event Action<IProgressTransaction> ProgressStopped;

        [StructuredName(Events.ProgressDisposed)]
        public event Action<IProgressTransaction> ProgressDisposed;

        [StructuredName(Events.ProgressMessage)]
        public event Action<IProgressTransaction, IProgressMessage> ProgressMessage;
    }

    /// <summary>
    /// Progress-tracking events.
    /// </summary>
    public enum Events
    {
        // progress id, old value, new value
        ProgressChanged,

        // progress id
        ProgressStarted,

        // progress id
        ProgressStopped,

        // progress id
        ProgressDisposed,

        // message type, text
        ProgressMessage,
    }
}