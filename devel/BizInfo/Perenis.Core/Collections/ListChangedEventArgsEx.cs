using System.ComponentModel;

namespace Perenis.Core.Collections
{
    /// <summary>
    /// Extends the <see cref="ListChangedEventArgs"/> with an option to specify a changed property 
    /// by its name rather than by a <see cref="PropertyDescriptor"/>.
    /// </summary>
    public class ListChangedEventArgsEx : ListChangedEventArgs
    {
        /// <summary>
        /// The actually affected item.
        /// </summary>
        public object Item { get; private set; }

        /// <summary>
        /// The name of the property changed.
        /// </summary>
        public string PropertyName { get; private set; }

        #region ------ Constructors ---------------------------------------------------------------

        public ListChangedEventArgsEx(ListChangedType listChangedType, object item, int newIndex)
            : base(listChangedType, newIndex)
        {
            Item = item;
        }

        public ListChangedEventArgsEx(ListChangedType listChangedType, object item, int newIndex, PropertyDescriptor propDesc)
            : base(listChangedType, newIndex, propDesc)
        {
            Item = item;
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="listChangedType">The type of the change.</param>
        /// <param name="newIndex">The index of the item chagned.</param>
        /// <param name="propName">The name of the property changed.</param>
        public ListChangedEventArgsEx(ListChangedType listChangedType, object item, int newIndex, string propName)
            : base(listChangedType, newIndex)
        {
            Item = item;
            PropertyName = propName;
        }

        public ListChangedEventArgsEx(ListChangedType listChangedType, object item, PropertyDescriptor propDesc)
            : base(listChangedType, propDesc)
        {
            Item = item;
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="listChangedType">The type of the change.</param>
        /// <param name="propName">The name of the property changed.</param>
        public ListChangedEventArgsEx(ListChangedType listChangedType, object item, string propName)
            : base(listChangedType, null)
        {
            Item = item;
            PropertyName = propName;
        }

        public ListChangedEventArgsEx(ListChangedType listChangedType, object item, int newIndex, int oldIndex)
            : base(listChangedType, newIndex, oldIndex)
        {
            Item = item;
        }

        #endregion
    }
}