using System;
using System.Collections.Generic;

namespace Perenis.Core.Component.Progress
{
    /// <summary>
    /// Provides percentage-scaled progress information to a <see cref="ProgressController.TargetConsumer"/> 
    /// based on the progress reported by several processes (inner progress controllers) to the instance 
    /// of this class.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Progress is reported to the <see cref="ProgressController.TargetConsumer"/> only if, after 
    /// conversion to the percentage scale, it has changed since the last invocation of the 
    /// <see cref="OnProgress"/>. The original sender object is always retained.
    /// </para>
    /// <para>
    /// This class is thread safee, i.e. progress reports from the various inner progress controllers
    /// may be performed from different threads.
    /// </para>
    /// </remarks>
    [Obsolete("Use IProgressTransaction & IProgressNotification instead.")]
    public class CompoundProgressController : ProgressController, IProgressPercentageConsumer
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="targetConsumer">The target consumer accepting percentage-scaled progress.</param>
        public CompoundProgressController(IProgressPercentageConsumer targetConsumer)
            : base(targetConsumer)
        {
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="targetConsumer">The target consumer accepting percentage-scaled progress.</param>
        /// <param name="minimum">The lowest progress value.</param>
        /// <param name="maximum">The highest progress value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="targetConsumer"/> is a null reference.</exception>
        public CompoundProgressController(IProgressPercentageConsumer targetConsumer, uint minimum, uint maximum)
            : base(targetConsumer, minimum, maximum)
        {
        }

        /// <summary>
        /// The portion of the scale  of this progress controller already consumed by inner progress controllers.
        /// </summary>
        public uint AllocatedScale { get; private set; }

        /// <summary>
        /// The portion of the scale of this progress controller remaining for use by inner progress controllers.
        /// </summary>
        public uint RemainingScale
        {
            get { lock (syncRoot) return Scale - AllocatedScale; }
        }

        /// <summary>
        /// Computes the possible requested scale that can be allocated to a single new inner progress
        /// controller in case of equalized distribution of the <see cref="RemainingScale"/> among 
        /// the given number expected inner controllers not yet in scope of this instance (i.e. to 
        /// be created / added).
        /// </summary>
        /// <param name="expectedInnerControllers">The expected number of inner progress controller
        /// (not yet in scope of this instance).</param>
        /// <returns>The portion of the scale of this progress controller that can be allocated
        /// to single new inner progress controller. When no free portion of the scale is available,
        /// this method returns zero (with itself is a valid portion of a scale).</returns>
        /// <remarks>
        /// When creating a new inner progress controller or adding an existing progress controller
        /// into this instance, it should be a common practice to use this method for each inner
        /// progress controller being created / added to avoid <see cref="InvalidOperationException"/> 
        /// exceptions and/or ensure full coverage of the scale of this instance.
        /// </remarks>
        public uint DistributeScale(uint expectedInnerControllers)
        {
            lock (syncRoot)
            {
                if (expectedInnerControllers > RemainingScale) return 0;
                return RemainingScale/expectedInnerControllers;
            }
        }

        /// <summary>
        /// Creates an inner progress controller.
        /// </summary>
        /// <param name="requestedScale">The portion of the scale of this progress controller 
        /// tobe allocated to the inner progress controller created by this method.</param>
        /// <returns>The created inner progress controller casted as an <see cref="IProgressObserver"/>.</returns>
        /// <exception cref="InvalidOperationException">The <see cref="RemainingScale"/> is less 
        /// than the given <paramref name="requestedScale"/>.</exception>
        public IProgressObserver CreateInnerController(uint requestedScale)
        {
            lock (syncRoot)
            {
                AllocateScale(requestedScale);
                var result = new ProgressController(this);
                innerControllers.Add(result, new InnerControllerStatus {AllocatedScale = requestedScale, LastProgress = 0});
                return result;
            }
        }

        /// <summary>
        /// Creates an inner progress controller.
        /// </summary>
        /// <param name="requestedScale">The portion of the scale of this progress controller to be
        /// allocated to the inner progress observer created by this method.</param>
        /// <param name="minimum">The lowest progress value of the progress controller being created.</param>
        /// <param name="maximum">The highest progress value of the progress controller being created.</param>
        /// <returns>The created inner progress controller casted as an <see cref="IProgressObserver"/>.</returns>
        /// <exception cref="InvalidOperationException">The <see cref="RemainingScale"/> is less 
        /// than the given <paramref name="requestedScale"/>.</exception>
        public IProgressObserver CreateInnerController(uint requestedScale, uint minimum, uint maximum)
        {
            lock (syncRoot)
            {
                AllocateScale(requestedScale);
                var result = new ProgressController(this, minimum, maximum);
                innerControllers.Add(result, new InnerControllerStatus {AllocatedScale = requestedScale, LastProgress = 0});
                return result;
            }
        }

        /// <summary>
        /// Adds an inner progress controller to the scope of this progress controller.
        /// </summary>
        /// <param name="requestedScale">The portion of the scale of this progress controller to be
        /// allocated to the inner progress controller supplied to this method.</param>
        /// <param name="innerController"></param>
        /// <exception cref="ArgumentNullException"><paramref name="innerController"/> is a null reference.</exception>
        /// <exception cref="InvalidOperationException">The <see cref="RemainingScale"/> is less 
        /// than the given <paramref name="requestedScale"/>.</exception>
        public void AddInnerController(uint requestedScale, IProgressController innerController)
        {
            if (innerController == null) throw new ArgumentNullException("innerController");
            lock (syncRoot)
            {
                AllocateScale(requestedScale);
                innerControllers.Add(innerController, new InnerControllerStatus
                                                          {
                                                              AllocatedScale = requestedScale,
                                                              LastProgress = Rescale(innerController.Current, innerController.Minimum, innerController.Maximum, 0, requestedScale)
                                                          });
            }
        }

        #region ------ Internals: ProgressController overrides ------------------------------------

        public override void OnStartProgress(object sender)
        {
            lock (syncRoot)
            {
                base.OnStartProgress(sender);
                started = true;
            }
        }

        public override void OnStartProgress(object sender, uint minimum, uint maximum)
        {
            lock (syncRoot)
            {
                base.OnStartProgress(sender, minimum, maximum);
                started = true;
            }
        }

        public override void OnStartProgress(object sender, uint total)
        {
            lock (syncRoot)
            {
                base.OnStartProgress(sender, total);
                started = true;
            }
        }

        #endregion

        #region ------ Implementation of the IProgressPercentageConsumer interface ----------------

        void IProgressPercentageConsumer.OnStartProgress(IProgressController progressController, object sender)
        {
            lock (syncRoot)
            {
                // retrieve status and verify arguments
                InnerControllerStatus status = GetInnerControllerStatus(progressController);

                // report progress start only for the first inner controller
                if (!started) OnStartProgress(sender);
            }
        }

        void IProgressPercentageConsumer.OnProgress(IProgressController progressController, object sender, decimal progressPercentage)
        {
            lock (syncRoot)
            {
                // retrieve status and verify arguments
                InnerControllerStatus status = GetInnerControllerStatus(progressController);

                // compute rescaled progress value
                uint newProgress = Rescale(progressController.Current, progressController.Minimum, progressController.Maximum, 0, status.AllocatedScale);

                // change progress
                if (newProgress != status.LastProgress)
                {
                    OnProgress(sender, Current - status.LastProgress + newProgress);
                    status.LastProgress = newProgress;
                }
            }
        }

        void IProgressPercentageConsumer.OnStopProgress(IProgressController progressController, object sender)
        {
            lock (syncRoot)
            {
                // retrieve status and verify arguments
                InnerControllerStatus status = GetInnerControllerStatus(progressController);

                // report progress stop only if 100 % total progress has been reached before
                if (CurrentPercentage == 100m) OnStopProgress(sender);
            }
        }

        #endregion

        #region ------ Internals ------------------------------------------------------------------

        /// <summary>
        /// Registry of inner progress controllers. 
        /// </summary>
        private readonly Dictionary<IProgressController, InnerControllerStatus> innerControllers = new Dictionary<IProgressController, InnerControllerStatus>();

        /// <summary>
        /// Synchronization root of this instance.
        /// </summary>
        private readonly object syncRoot = new object();

        /// <summary>
        /// Indicates if progress start has already been reported.
        /// </summary>
        private bool started;

        /// <summary>
        /// Retrieves the status of the given inner progress controller.
        /// </summary>
        /// <param name="innerController"></param>
        /// <returns></returns>
        /// <remarks>
        /// This method also validates the eligibility of the passed <paramref name="innerController"/>.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="innerController"/> is a null reference.</exception>
        /// <exception cref="ArgumentException">When the given <paramref name="innerController"/> is not
        /// in the scope of this progress controller.</exception>
        private InnerControllerStatus GetInnerControllerStatus(IProgressController innerController)
        {
            if (innerController == null) throw new ArgumentNullException("innerController");
            if (!innerControllers.ContainsKey(innerController)) throw new ArgumentException("Inner controller not in scope.", "innerController");
            return innerControllers[innerController];
        }

        /// <summary>
        /// Allocates scale of this instance.
        /// </summary>
        /// <param name="requestedScale">The portion of the scale of this progress controller to be
        /// allocated.</param>
        /// <exception cref="InvalidOperationException">The <see cref="RemainingScale"/> is less 
        /// than the given <paramref name="requestedScale"/>.</exception>
        private void AllocateScale(uint requestedScale)
        {
            if (requestedScale > RemainingScale) throw new InvalidOperationException(String.Format("Not enough remining scale; {0} remaining, {1} requested.", RemainingScale, requestedScale));
            AllocatedScale += requestedScale;
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
        private static uint Rescale(uint orgCurrent, uint orgMinimum, uint orgMaximum, uint newMinimum, uint newMaximum)
        {
            if (orgCurrent < orgMinimum) return newMinimum;
            if (orgCurrent > orgMaximum) return newMaximum;
            decimal orgScale = orgMaximum - orgMinimum;
            decimal newScale = newMaximum - newMinimum;
            return newMinimum + (uint) Math.Round(((orgCurrent - orgMinimum)/orgScale)*newScale, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Describes the status of an inner progress controller.
        /// </summary>
        private class InnerControllerStatus
        {
            /// <summary>
            /// The portion of the scale of this progress controller allocated to the inner progress 
            /// controller described by this class.
            /// </summary>
            public uint AllocatedScale { get; set; }

            /// <summary>
            /// The last value of the <see cref="IProgressObserver.Current"/> rescaled into the scale
            /// of this progress controller.
            /// </summary>
            public uint LastProgress { get; set; }
        }

        #endregion
    }
}