using System;

namespace Perenis.Core.Component.Progress
{
    /// <summary>
    /// Provides a remoting-aware facade of an <see cref="IProgressObserver"/> implementation
    /// </summary>
    [Obsolete("Use IProgressTransaction & IProgressNotification instead.")]
    public class RemoteProgressObserver : MarshalByRefObject, IProgressObserverAware, IProgressObserver
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public RemoteProgressObserver()
        {
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public RemoteProgressObserver(IProgressObserver progressObserver)
        {
            ProgressObserver = progressObserver;
        }

        #region ------ Implementation of the IProgressObserverAware interface ---------------------

        public IProgressObserver ProgressObserver { get; set; }

        #endregion

        #region ------ Implementation of the IProgressObserver interface --------------------------

        uint IProgressObserver.Minimum
        {
            get { return ProgressObserver != null ? ProgressObserver.Minimum : 0; }
        }

        uint IProgressObserver.Maximum
        {
            get { return ProgressObserver != null ? ProgressObserver.Maximum : 0; }
        }

        uint IProgressObserver.Scale
        {
            get { return ProgressObserver != null ? ProgressObserver.Scale : 0; }
        }

        uint IProgressObserver.Current
        {
            get { return ProgressObserver != null ? ProgressObserver.Current : 0; }
        }

        void IProgressObserver.OnStartProgress(object sender)
        {
            if (ProgressObserver != null) ProgressObserver.OnStartProgress(sender);
        }

        void IProgressObserver.OnStartProgress(object sender, uint minimum, uint maximum)
        {
            if (ProgressObserver != null) ProgressObserver.OnStartProgress(sender, minimum, maximum);
        }

        void IProgressObserver.OnStartProgress(object sender, uint total)
        {
            if (ProgressObserver != null) ProgressObserver.OnStartProgress(sender, total);
        }

        void IProgressObserver.OnProgress(object sender, uint progress)
        {
            if (ProgressObserver != null) ProgressObserver.OnProgress(sender, progress);
        }

        void IProgressObserver.OnStopProgress(object sender)
        {
            if (ProgressObserver != null) ProgressObserver.OnStopProgress(sender);
        }

        #endregion
    }
}