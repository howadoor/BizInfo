using System;
using System.Diagnostics;

namespace Perenis.Core.Component.Progress
{
    /// <summary>
    /// Implementation of <see cref="IProgressTransaction"/> interface.
    /// </summary>
    internal class ProgressTransaction : ProgressTransactionBase, IProgressTransaction
    {
        public ProgressTransaction(string taskName)
            : this(taskName, 0, 100, 100, null, null)
        {
        }

        public ProgressTransaction(string taskName, int scaleMin, int scaleMax)
            : this(taskName, scaleMin, scaleMax, 100, null, null)
        {
        }

        public ProgressTransaction(string taskName, int scaleMin, int scaleMax, IProgressNotification progressNotification)
            : this(taskName, scaleMin, scaleMax, 100, null, progressNotification)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="taskName">Task name.</param>
        /// <param name="allocatedScale">
        /// Portion of the scale allocated for this transaction measured in percent scale.
        /// Defines, how much of the current progress is contribued to the parent progress transaction.
        /// </param>
        /// <param name="parentTransaction">Parent progress transaction.</param>
        /// <param name="progressNotification">Progress notification interface.</param>
        /// <param name="scaleMin"></param>
        /// <param name="scaleMax"></param>
        internal ProgressTransaction(string taskName, int scaleMin, int scaleMax, int allocatedScale, IProgressTransaction parentTransaction, IProgressNotification progressNotification)
        {
            if (string.IsNullOrEmpty(taskName)) throw new ArgumentNullException("taskName");
            if (allocatedScale < 0 || allocatedScale > 100) throw new ArgumentException("allocatedScale value must be in the range 0-100");

            TaskName = taskName;
            TransactionId = Guid.NewGuid();

            ScaleMin = scaleMin;
            ScaleMax = scaleMax;
            currentProgress = ScaleMin;

            AllocatedScale = allocatedScale;
            ParentTransaction = parentTransaction;
            if (ParentTransaction != null && ParentTransaction is ProgressTransactionBase)
            {
                ((ProgressTransactionBase) ParentTransaction).LastInnerProgress = ScaleMin;
            }

            this.progressNotification = progressNotification ?? new GlobalProgressNotification();
        }

        #region ------ Implementation of IProgressTransaction -------------------------------------

        public override Guid TransactionId { get; protected set; }

        public override string TaskName { get; protected set; }

        public override int ScaleMin { get; protected set; }

        public override int ScaleMax { get; protected set; }

        public override void Start()
        {
            Debug.Assert(!IsRunning);
            IsRunning = true;

            if (progressNotification != null) progressNotification.OnProgressStarted(this);
        }

        public override void Stop()
        {
            Debug.Assert(IsRunning);
            IsRunning = false;

            if (progressNotification != null) progressNotification.OnProgressStopped(this);
        }

        public override bool IsRunning { get; protected set; }

        public override void Advance(int value)
        {
            Progress += value;
        }

        public override int Progress
        {
            get { return currentProgress; }

            set
            {
                // set current progress
                currentProgress = value;

                // set allocated progress
                if (ParentTransaction != null && ParentTransaction is ProgressTransactionBase)
                {
                    ((ProgressTransactionBase) ParentTransaction).ContributeProgress(this, value);
                }

                // notify change of progress
                if (progressNotification != null) progressNotification.OnProgressChanged(this);
            }
        }

        public override IProgressTransaction ParentTransaction { get; protected set; }

        public override IProgressTransaction CreateInnerTransaction(string taskName, int scaleMin, int scaleMax, int allocatedScale, IProgressNotification progressNotification)
        {
            var result = new ProgressTransaction(taskName, scaleMin, scaleMax, allocatedScale, this, progressNotification);
            LastInnerProgress = ScaleMin;
            return result;
        }

        public override void PostMessage(IProgressMessage message)
        {
            if (progressNotification != null) progressNotification.OnProgressMessage(this, message);

            if (ParentTransaction != null)
            {
                ParentTransaction.PostMessage(message);
            }
            else
            {
                LogMessage(message);
            }
        }

        #endregion

        #region ------ Implementation of IDisposable ----------------------------------------------

        public void Dispose()
        {
            if (IsRunning) Stop();
            if (progressNotification != null) progressNotification.OnProgressDisposed(this);
        }

        #endregion

        #region ------ Internals ------------------------------------------------------------------

        private readonly IProgressNotification progressNotification;
        private int currentProgress;
        protected internal override int LastInnerProgress { protected get; set; }

        protected internal override int AllocatedScale { get; protected set; }

        #endregion
    }
}