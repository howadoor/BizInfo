using System;

namespace Perenis.Core.Component.Progress
{
    /// <summary>
    /// Provides percentage-scaled progress information to a <see cref="TargetConsumer"/> based on
    /// the progress reported by a process to the instance of this class.
    /// </summary>
    /// <remarks>
    /// Progress is reported to the <see cref="TargetConsumer"/> only if, after conversion to the
    /// percentage scale, it has changed since the last invocation of the <see cref="OnProgress"/>.
    /// The original sender object is always retained.
    /// </remarks>
    [Obsolete("Use IProgressTransaction & IProgressNotification instead.")]
    public class ProgressController : IProgressObserver, IProgressController
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="targetConsumer">The target consumer accepting percentage-scaled progress.</param>
        public ProgressController(IProgressPercentageConsumer targetConsumer)
            : this(targetConsumer, 0, 100)
        {
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="targetConsumer">The target consumer accepting percentage-scaled progress.</param>
        /// <param name="minimum">The lowest progress value.</param>
        /// <param name="maximum">The highest progress value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="targetConsumer"/> is a null reference.</exception>
        public ProgressController(IProgressPercentageConsumer targetConsumer, uint minimum, uint maximum)
        {
            if (targetConsumer == null) throw new ArgumentNullException("targetConsumer");
            TargetConsumer = targetConsumer;
            SetRange(minimum, maximum);
        }

        #region ------ Implementation of the IProgressController interface ------------------------

        public int Precision { get; set; }

        public decimal CurrentPercentage
        {
            get { return GetPercentage(Current); }
        }

        #endregion

        #region ------ Implementation of the IProgressObserver interface --------------------------

        public uint Minimum { get; private set; }

        public uint Maximum { get; private set; }

        public uint Scale
        {
            get { return Maximum - Minimum; }
        }

        public uint Current { get; private set; }

        public virtual void OnStartProgress(object sender)
        {
            TargetConsumer.OnStartProgress(this, sender);
        }

        public virtual void OnStartProgress(object sender, uint minimum, uint maximum)
        {
            SetRange(minimum, maximum);
            TargetConsumer.OnStartProgress(this, sender);
        }

        public virtual void OnStartProgress(object sender, uint total)
        {
            SetRange(0, total);
            TargetConsumer.OnStartProgress(this, sender);
        }

        public virtual void OnProgress(object sender, uint progress)
        {
            if (progress < Minimum || progress > Maximum) return;
            decimal oldPercentage = GetPercentage(Current);
            decimal newPercentage = GetPercentage(progress);
            Current = progress;
            if (oldPercentage != newPercentage) TargetConsumer.OnProgress(this, sender, newPercentage);
        }

        public virtual void OnStopProgress(object sender)
        {
            TargetConsumer.OnStopProgress(this, sender);
        }

        #endregion

        #region ------ Internals ------------------------------------------------------------------

        /// <summary>
        /// Sets the progress range to the given <paramref name="minimum"/> and <paramref name="maximum"/> values.
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        protected void SetRange(uint minimum, uint maximum)
        {
            if (minimum > maximum) throw new ArgumentException(String.Format("Invalid range {0}..{1}", minimum, maximum));
            Minimum = minimum;
            Maximum = maximum;
            Current = minimum;
        }

        /// <summary>
        /// Computes the percentage representation of the given <paramref name="progress"/>.
        /// </summary>
        /// <param name="progress"></param>
        /// <returns></returns>
        protected decimal GetPercentage(uint progress)
        {
            if (progress < Minimum) return 0;
            if (progress > Maximum) return 100;

            decimal whole = Maximum - Minimum;
            decimal part = progress - Minimum;

            if (whole == 0) return 0;
            return Math.Round(part/whole*100, Precision, MidpointRounding.AwayFromZero);
        }

        #endregion

        /// <summary>
        /// The target consumer accepting percentage-scaled progress.
        /// </summary>
        public IProgressPercentageConsumer TargetConsumer { get; private set; }
    }
}