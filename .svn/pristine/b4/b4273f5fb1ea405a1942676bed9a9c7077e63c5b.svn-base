using System;
using Perenis.Core.Log;

namespace Perenis.Core.Component.Progress
{
    /// <summary>
    /// Base class for classes implementing <see cref="IProgressTransaction"/> interface.
    /// </summary>
    public abstract class ProgressTransactionBase
    {
        #region ------ Internals: IProgressTransaction implemented in descendants -----------------

        public abstract string TaskName { get; protected set; }

        public virtual string TaskFullName
        {
            get { return ParentTransaction != null ? TaskFullName + " | " + TaskName : TaskName; }
        }

        public abstract Guid TransactionId { get; protected set; }

        public abstract int ScaleMin { get; protected set; }

        public abstract int ScaleMax { get; protected set; }

        public abstract bool IsRunning { get; protected set; }

        public abstract int Progress { get; set; }

        internal virtual Uri ProgressServiceUri { get; set; }

        public abstract IProgressTransaction ParentTransaction { get; protected set; }

        protected internal abstract int LastInnerProgress { protected get; set; }

        protected internal abstract int AllocatedScale { get; protected set; }
        public abstract void Start();

        public abstract void Stop();

        public virtual void Complete()
        {
            Progress = ScaleMax;
        }

        public abstract void Advance(int value);
        public abstract void PostMessage(IProgressMessage message);
        public abstract IProgressTransaction CreateInnerTransaction(string taskName, int scaleMin, int scaleMax, int allocatedScale, IProgressNotification progressNotification);

        #endregion

        #region ------ Internals ------------------------------------------------------------------

        public const string ProgressLogFormat = "Progress message - {0}: {1:u}; {2}; {3}";

        /// <summary>
        /// Log processed message into event log.
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        protected void LogMessage(IProgressMessage message)
        {
            switch (message.Severity)
            {
                case Severity.Info:
                    this.LogInfo(string.Format(ProgressLogFormat, "Info", message.Timestamp, message.Summary, message.Description));
                    break;
                case Severity.Warning:
                    this.LogWarn(string.Format(ProgressLogFormat, "Warning", message.Timestamp, message.Summary, message.Description));
                    break;
                case Severity.Error:
                    this.LogError(string.Format(ProgressLogFormat, "Error", message.Timestamp, message.Summary, message.Description));
                    break;
            }
        }

        /// <summary>
        /// Contributes progress by <paramref name="value"/> from <paramref name="innerTransaction"/>.
        /// </summary>
        /// <remarks>
        /// The <paramref name="value"/> is rescaled to current scale and to the scale allocated
        /// for inner transaction.
        /// </remarks>
        /// <param name="innerTransaction">Progress transaction.</param>
        /// <param name="value">Progress value delta being contributed.</param>
        protected internal void ContributeProgress(IProgressTransaction innerTransaction, int value)
        {
            // rescale the value to the current scale
            int contributedProgress = Rescale(value, innerTransaction.ScaleMin, innerTransaction.ScaleMax, ScaleMin, ScaleMax);

            // use only the portion defined by inner transaction's allocatedScale
            int allocatedScaleMax = (int) (ScaleMax/(double) 100)*(innerTransaction is ProgressTransactionBase ? ((ProgressTransactionBase) innerTransaction).AllocatedScale : 100);
            int allocatedProgress = Rescale(contributedProgress, ScaleMin, ScaleMax, ScaleMin, allocatedScaleMax);

            // advance by delta
            Progress = Progress + (allocatedProgress - LastInnerProgress);
            LastInnerProgress = allocatedProgress;
        }

        /// <summary>
        /// Rescales the given progress values measured in an original scale into a new scale.
        /// </summary>
        /// <param name="orgCurrent">The progress value measured in the original scale.</param>
        /// <param name="orgMinimum">The lowest value of the original scale.</param>
        /// <param name="orgMaximum">The highest value of the original scale.</param>
        /// <param name="newMinimum">The lowest value of the new scale.</param>
        /// <param name="newMaximum">The highest value of the new scale.</param>
        /// <returns>The progress value measured in the new scale.</returns>
        protected static int Rescale(int orgCurrent, int orgMinimum, int orgMaximum, int newMinimum, int newMaximum)
        {
            if (orgCurrent < orgMinimum) return newMinimum;
            if (orgCurrent > orgMaximum) return newMaximum;
            if (orgMaximum == orgMinimum) return newMaximum;
            decimal orgScale = orgMaximum - orgMinimum;
            decimal newScale = newMaximum - newMinimum;
            return newMinimum + (int) Math.Round(((orgCurrent - orgMinimum)/orgScale)*newScale, MidpointRounding.AwayFromZero);
        }

        #endregion
    }
}