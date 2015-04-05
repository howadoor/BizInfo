using System;

namespace Perenis.Core.Component.Progress
{
    /// <summary>
    /// Server-side implementation of <see cref="IProgressTransaction"/> interface.
    /// </summary>
    public class ProxyProgressTransaction : ProgressTransactionBase, IProgressTransaction
    {
        public ProxyProgressTransaction(string taskName, Guid transactionId, int scaleMin, int scaleMax, int progress)
        {
            TaskName = taskName;
            TransactionId = transactionId;
            ScaleMin = scaleMin;
            ScaleMax = scaleMax;
            ProgressServiceUri = Component.Progress.Progress.DefaultProgressServiceUri;
            this.progress = progress;
            progressService = Component.Progress.Progress.GetService(ProgressServiceUri);
        }

        #region ------ Implementation of IProgressTransaction -------------------------------------

        private int progress;
        public override string TaskName { get; protected set; }

        public override Guid TransactionId { get; protected set; }

        public override int ScaleMin { get; protected set; }

        public override int ScaleMax { get; protected set; }

        public override void Start()
        {
            throw new NotImplementedException();
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }

        public override bool IsRunning
        {
            get { throw new NotImplementedException(); }
            protected set { throw new NotImplementedException(); }
        }


        public override void Advance(int value)
        {
            progress += value;
            IProgressAdvance advance = new ProgressAdvance(value);
            progressService.Add(TransactionId, advance);
        }

        public override int Progress
        {
            get { return progress; }
            set { Advance(value - progress); }
        }

        public override void PostMessage(IProgressMessage message)
        {
            progressService.Add(TransactionId, message);
            LogMessage(message);
        }

        public override IProgressTransaction ParentTransaction
        {
            get { return null; }
            protected set { }
        }

        public override IProgressTransaction CreateInnerTransaction(string taskName, int scaleMin, int scaleMax, int allocatedScale, IProgressNotification progressNotification)
        {
            var result = new ProgressTransaction(taskName, scaleMin, scaleMax, allocatedScale, this, progressNotification);
            LastInnerProgress = ScaleMin;
            return result;
        }

        #endregion

        #region ------ Implementation of the IDisposable interface ----------------------------

        public void Dispose()
        {
            progressService.DropTransactionId(TransactionId);
        }

        #endregion

        #region ------ Internals ------------------------------------------------------------------

        private readonly IProgressService progressService;

        protected internal override int LastInnerProgress { protected get; set; }

        protected internal override int AllocatedScale
        {
            get { throw new NotImplementedException(); }
            protected set { throw new NotImplementedException(); }
        }

        #endregion
    }
}