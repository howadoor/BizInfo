using System.Collections.Specialized;
using System.ComponentModel;
using Perenis.Core.Component;

namespace Perenis.Core.Collections
{
    /// <summary>
    /// Provides similar funcionality as the <see cref="CollectionChangedEventManager"/> class; however,
    /// uses the <see cref="INotifyListChanged"/> interface.
    /// </summary>
    public class ListChangedEventManager : WeakEventManager<ListChangedEventManager, INotifyListChanged>
    {
        #region ------ Internals ------------------------------------------------------------------

        protected override void StartListening(INotifyListChanged source)
        {
            source.ListChanged += OnListChanged;
        }

        protected override void StopListening(INotifyListChanged source)
        {
            source.ListChanged -= OnListChanged;
        }

        /// <summary>
        /// Forwards the <see cref="INotifyListChanged.ListChanged"/> event to registered listeners.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnListChanged(object sender, ListChangedEventArgs args)
        {
            base.DeliverEvent(sender, args);
        }

        #endregion
    }
}