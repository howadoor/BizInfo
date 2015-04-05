using System.Collections.Generic;

namespace Perenis.Core.Component.Progress
{
    /// <summary>
    /// Multiple progress notifications.
    /// </summary>
    public class CompoundProgressNotification : IProgressNotification
    {
        public CompoundProgressNotification()
        {
        }

        public CompoundProgressNotification(IEnumerable<IProgressNotification> progressNotifications)
        {
            this.progressNotifications = progressNotifications != null
                                             ? new List<IProgressNotification>(progressNotifications)
                                             : new List<IProgressNotification>();
        }

        public void AddProgressNotification(IProgressNotification progressNotification)
        {
            progressNotifications.Add(progressNotification);
        }

        #region ------ Implementation of IProgressNotification ------------------------------------

        public void OnProgressChanged(IProgressTransaction progressTransaction)
        {
            foreach (IProgressNotification progressNotification in progressNotifications)
            {
                progressNotification.OnProgressChanged(progressTransaction);
            }
        }

        public void OnProgressStarted(IProgressTransaction progressTransaction)
        {
            foreach (IProgressNotification progressNotification in progressNotifications)
            {
                progressNotification.OnProgressStarted(progressTransaction);
            }
        }

        public void OnProgressStopped(IProgressTransaction progressTransaction)
        {
            foreach (IProgressNotification progressNotification in progressNotifications)
            {
                progressNotification.OnProgressStopped(progressTransaction);
            }
        }

        public void OnProgressDisposed(IProgressTransaction progressTransaction)
        {
            foreach (IProgressNotification progressNotification in progressNotifications)
            {
                progressNotification.OnProgressDisposed(progressTransaction);
            }
        }

        public void OnProgressMessage(IProgressTransaction progressTransaction, IProgressMessage message)
        {
            foreach (IProgressNotification progressNotification in progressNotifications)
            {
                progressNotification.OnProgressMessage(progressTransaction, message);
            }
        }

        #endregion

        #region ------ Internals ------------------------------------------------------------------

        private readonly List<IProgressNotification> progressNotifications;

        #endregion
    }
}