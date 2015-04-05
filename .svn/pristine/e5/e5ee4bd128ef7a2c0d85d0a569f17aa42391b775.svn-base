using System.Collections;
using System.Configuration;

namespace Perenis.Core.Configuration
{
    /// <summary>
    /// An extended collection of configuration elements implementing the <see cref="IList"/> interface.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ConfigurationElementCollectionEx<T> : ConfigurationElementCollection, IList
        where T : ConfigurationElement, new()
    {
        #region ------ Internals: ConfigurationElementCollection overrides ------------------------

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.AddRemoveClearMap; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new T();
        }

        #endregion

        #region ------ Implementation of the IList interface --------------------------------------

        public int Add(object value)
        {
            BaseAdd((T) value);
            return BaseIndexOf((T) value);
        }

        public bool Contains(object value)
        {
            return BaseIndexOf((T) value) != -1;
        }

        public void Clear()
        {
            BaseClear();
        }

        public int IndexOf(object value)
        {
            return BaseIndexOf((T) value);
        }

        public void Insert(int index, object value)
        {
            BaseAdd(index, (T) value);
        }

        public void Remove(object value)
        {
            BaseRemove(value);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public object this[int index]
        {
            get { return BaseGet(index); }
            set
            {
                BaseRemoveAt(index);
                BaseAdd(index, (T) value);
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool IsFixedSize
        {
            get { return false; }
        }

        #endregion
    }
}