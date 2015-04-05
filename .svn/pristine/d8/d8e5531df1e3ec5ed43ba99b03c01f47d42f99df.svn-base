using System;

namespace Perenis.Core.Component.Progress
{
    /// <summary>
    /// The event arguments of the <see cref="ProgressPercentageDelegate"/> events.
    /// </summary>
    [Obsolete("Use IProgressTransaction & IProgressNotification instead.")]
    public class ProgressPercentageEventArgs : EventArgs
    {
        /// <summary>
        /// Represents the 100 % progress (i.e. the progress stop).
        /// </summary>
        public static readonly ProgressPercentageEventArgs HundredPercent = new ProgressPercentageEventArgs(100m);

        /// <summary>
        /// Represents the 0 % progress (i.e. the progress start).
        /// </summary>
        public static readonly ProgressPercentageEventArgs ZeroPercent = new ProgressPercentageEventArgs(0m);

        private readonly decimal progressPercentage;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.EventArgs" /> class.
        /// </summary>
        public ProgressPercentageEventArgs(decimal progressPercentage)
        {
            this.progressPercentage = progressPercentage;
        }

        /// <summary>
        /// Progress in percentual scale.
        /// </summary>
        public decimal ProgressPercentage
        {
            get { return progressPercentage; }
        }
    }

    /// <summary>
    /// Represents events related to percentage-scaled progress reporting.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    [Obsolete("Use IProgressTransaction & IProgressNotification instead.")]
    public delegate void ProgressPercentageDelegate(object sender, ProgressPercentageEventArgs args);

    /// <summary>
    /// Provides a default implementation of the <see cref="IProgressPercentageConsumer"/> interface
    /// converting method calls to events.
    /// </summary>
    /// <remarks>
    /// The original sender object is always retained.
    /// </remarks>
    [Obsolete("Use IProgressTransaction & IProgressNotification instead.")]
    public class ProgressPercentageConsumer : IProgressPercentageConsumer
    {
        private ProgressPercentageDelegate progress;
        private ProgressPercentageDelegate startProgress;
        private ProgressPercentageDelegate stopProgress;

        #region ------ Implementation of the IProgressPercentageConsumer --------------------------

        void IProgressPercentageConsumer.OnStartProgress(IProgressController progressController, object sender)
        {
            if (startProgress != null)
            {
                if (Async)
                    startProgress.BeginInvoke(sender, ProgressPercentageEventArgs.ZeroPercent, null, null);
                else
                    startProgress(sender, ProgressPercentageEventArgs.ZeroPercent);
            }
        }

        void IProgressPercentageConsumer.OnProgress(IProgressController progressController, object sender, decimal progressPercentage)
        {
            if (progress != null)
            {
                if (Async)
                    progress.BeginInvoke(sender, new ProgressPercentageEventArgs(progressPercentage), null, null);
                else
                    progress(sender, new ProgressPercentageEventArgs(progressPercentage));
            }
        }

        void IProgressPercentageConsumer.OnStopProgress(IProgressController progressController, object sender)
        {
            if (stopProgress != null)
            {
                if (Async)
                    stopProgress.BeginInvoke(sender, ProgressPercentageEventArgs.HundredPercent, null, null);
                else
                    stopProgress(sender, ProgressPercentageEventArgs.HundredPercent);
            }
        }

        #endregion

        /// <summary>
        /// Indicates if asynchronous event invokation shall be used.
        /// </summary>
        public bool Async { get; set; }

        /// <summary>
        /// Fired when progress start is reported.
        /// </summary>
        /// <remarks>
        /// The event arguments passed to this event are always <see cref="ProgressPercentageEventArgs.ZeroPercent"/>
        /// </remarks>
        public event ProgressPercentageDelegate StartProgress
        {
            add { startProgress += value; }
            remove { startProgress -= value; }
        }

        /// <summary>
        /// Fired when progress is reported.
        /// </summary>
        /// <remarks>
        /// This event is <b>not</b> fired when progress start or stop is reported. To achieve this,
        /// register the same event handler to the <see cref="StartProgress"/> and <see cref="stopProgress"/>
        /// events.
        /// </remarks>
        public event ProgressPercentageDelegate Progress
        {
            add { progress += value; }
            remove { progress -= value; }
        }

        /// <summary>
        /// Fired when progress stop is reported.
        /// </summary>
        /// <remarks>
        /// The event arguments passed to this event are always <see cref="ProgressPercentageEventArgs.HundredPercent"/>
        /// </remarks>
        public event ProgressPercentageDelegate ProgressStop
        {
            add { stopProgress += value; }
            remove { stopProgress -= value; }
        }
    }
}