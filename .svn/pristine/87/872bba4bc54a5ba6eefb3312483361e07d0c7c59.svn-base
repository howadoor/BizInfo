using System;

namespace BizInfo.Harvesting.Services.Core
{
    /// <summary>
    /// Arguments of <see cref="Daemon.StatusChanged"/> event
    /// </summary>
    public class DaemonStatusChangedEventArgs : EventArgs
    {
        public DaemonStatusChangedEventArgs(DaemonStatus oldStatus, DaemonStatus newStatus)
        {
            OldStatus = oldStatus;
            NewStatus = newStatus;
        }

        /// <summary>
        /// New status of the <see cref="Daemon"/>
        /// </summary>
        public DaemonStatus NewStatus
        {
            get; private set;
        }

        /// <summary>
        /// Old status of the daemon
        /// </summary>
        public DaemonStatus OldStatus
        {
            get; private set;
        }
    }
}