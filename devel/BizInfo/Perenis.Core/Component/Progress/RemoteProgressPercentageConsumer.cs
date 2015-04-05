using System;

namespace Perenis.Core.Component.Progress
{
    /// <summary>
    /// Provides a remoting-aware facade of an <see cref="IProgressPercentageConsumer"/> implementation
    /// </summary>
    [Obsolete("Use IProgressTransaction & IProgressNotification instead.")]
    public class RemoteProgressPercentageConsumer : MarshalByRefObject, IProgressPercentageConsumerAware, IProgressPercentageConsumer
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public RemoteProgressPercentageConsumer()
        {
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public RemoteProgressPercentageConsumer(IProgressPercentageConsumer progressPercentageConsumer)
        {
            ProgressPercentageConsumer = progressPercentageConsumer;
        }

        #region ------ Implementation of the IProgressPercentageConsumerAware interface -----------

        public IProgressPercentageConsumer ProgressPercentageConsumer { get; set; }

        #endregion

        #region ------ Implementation of the IProgressPercentageConsumer interface ----------------

        void IProgressPercentageConsumer.OnStartProgress(IProgressController progressController, object sender)
        {
            if (ProgressPercentageConsumer != null) ProgressPercentageConsumer.OnStartProgress(progressController, sender);
        }

        void IProgressPercentageConsumer.OnProgress(IProgressController progressController, object sender, decimal progressPercentage)
        {
            if (ProgressPercentageConsumer != null) ProgressPercentageConsumer.OnProgress(progressController, sender, progressPercentage);
        }

        void IProgressPercentageConsumer.OnStopProgress(IProgressController progressController, object sender)
        {
            if (ProgressPercentageConsumer != null) ProgressPercentageConsumer.OnStopProgress(progressController, sender);
        }

        #endregion
    }
}