using System;
using System.Threading;
using System.Threading.Tasks;

namespace BizInfo.Harvesting.Services.Core
{
    /// <summary>
    /// <see cref="Daemon"/> periodically starts some process
    /// </summary>
    public class Daemon
    {
        private DaemonStatus status;

        /// <summary>
        /// <see cref="Task"/> used to run this <see cref="Daemon"/>.
        /// </summary>
        private Task task;

        public Daemon()
        {
            Status = DaemonStatus.Stopped;
        }

        /// <summary>
        /// Period of starting <see cref="Action"/>
        /// </summary>
        public TimeSpan Period { get; set; }

        /// <summary>
        /// Action which is started periodically by this <see cref="Daemon"/>.
        /// </summary>
        public Action Action { get; set; }

        /// <summary>
        /// Token used for cancellation
        /// </summary>
        public CancellationToken CancellationToken { get; set; }

        /// <summary>
        /// Time of last start of <see cref="Action"/>
        /// </summary>
        public DateTime LastStart { get; private set; }

        /// <summary>
        /// Current status of the <see cref="Daemon"/>
        /// </summary>
        public DaemonStatus Status
        {
            get { return status; }
            private set
            {
                if (status == value) return;
                var oldStatus = status;
                status = value;
                if (StatusChanged != null)
                {
                    StatusChanged.Invoke(this, new DaemonStatusChangedEventArgs(oldStatus, status));
                }
            }
        }

        /// <summary>
        /// <see cref="Exception"/> thrown in the last <see cref="Daemon.Action"/> run. <c>null</c> if no exception occured.
        /// </summary>
        public Exception LastRunException { get; private set; }

        public event EventHandler<DaemonStatusChangedEventArgs> StatusChanged;

        /// <summary>
        /// Starts periodic invoking of <see cref="Action"/>. Does nothing if <see cref="Daemon"/> already runs.
        /// </summary>
        public void Start()
        {
            lock (this)
            {
                if (Status != DaemonStatus.Stopped) return;
                task = Task.Factory.StartNew(Run);
            }
        }

        /// <summary>
        /// Main method which runs periodicaly the <see cref="Action"/>.
        /// </summary>
        private void Run()
        {
            Status = DaemonStatus.Running;
            while (!CancellationToken.IsCancellationRequested)
            {
                WaitForNextRun();
                if (CancellationToken.IsCancellationRequested) break;
                try
                {
                    LastRunException = null;
                    LastStart = DateTime.Now;
                    Status = DaemonStatus.InAction;
                    Action();
                }
                catch (Exception exception)
                {
                    LastRunException = exception;
                }
                finally
                {
                    Status = DaemonStatus.Running;
                }
            }
            Status = DaemonStatus.Stopped;
        }

        /// <summary>
        /// Waits necessarry time defined by <see cref="Period"/> before next run of <see cref="Action"/>.
        /// </summary>
        private void WaitForNextRun()
        {
            var minTime = LastStart + Period;
            if (minTime < DateTime.Now) return;
            var waitTime = minTime - DateTime.Now;
            Status = DaemonStatus.Waiting;
            CancellationToken.WaitHandle.WaitOne(waitTime);
            Status = DaemonStatus.Running;
        }

        /// <summary>
        /// Stops all actions within <see cref="Daemon"/>
        /// </summary>
        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}