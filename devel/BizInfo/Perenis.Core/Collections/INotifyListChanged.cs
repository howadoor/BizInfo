using System.Collections.Specialized;
using System.ComponentModel;

namespace Perenis.Core.Collections
{
    /// <summary>
    /// Defines similar funcionality as the <see cref="INotifyCollectionChanged"/> interface; however,
    /// uses the <see cref="ListChangedEventArgs"/> event arguments.
    /// </summary>
    public interface INotifyListChanged
    {
        /// <summary>
        /// This event is fired when the list changes in terms of actions defined by the
        /// <see cref="ListChangedType"/> enumeration.
        /// </summary>
        event ListChangedEventHandler ListChanged;
    }
}