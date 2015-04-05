using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Perenis.Core.Collections
{
    /// <summary>
    /// Extension methods for <see cref="ObservableCollection[T]"></see>
    /// </summary>
    public static class ObservableCollectionEx
    {
        public static void AddRemoveHandler<T>(this ObservableCollection<T> collection, Action<ObservableCollection<T>, T> added, Action<ObservableCollection<T>, T> removed)
        {
            collection.CollectionChanged += delegate(object sender, NotifyCollectionChangedEventArgs e) { HandleAddRemove(sender, e, added, removed); };
        }

        private static void HandleAddRemove<T>(object sender, NotifyCollectionChangedEventArgs e, Action<ObservableCollection<T>, T> added, Action<ObservableCollection<T>, T> removed)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (T item in e.NewItems) added((ObservableCollection<T>) sender, item);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (T item in e.OldItems) removed((ObservableCollection<T>) sender, item);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    foreach (T item in e.OldItems) removed((ObservableCollection<T>) sender, item);
                    foreach (T item in e.NewItems) added((ObservableCollection<T>) sender, item);
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    if (e.OldItems != null) foreach (T item in e.OldItems) removed((ObservableCollection<T>) sender, item);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}