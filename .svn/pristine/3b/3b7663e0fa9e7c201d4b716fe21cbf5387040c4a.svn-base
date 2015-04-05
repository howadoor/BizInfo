using System;
using System.Configuration;
using Perenis.Core.Reflection;

namespace Perenis.Core.Configuration
{
    public abstract class NamedConfigurationElementCollection<T> : ConfigurationElementCollectionEx<T>
        where T : NamedConfigurationElement, new()
    {
        public T GetByName(string name)
        {
            return (T) BaseGet(name);
        }

        #region ------ Internals: ConfigurationElementCollection overrides ------------------------

        protected override ConfigurationElement CreateNewElement(string elementName)
        {
            return TypeFactory<T>.Instance.CreateInstance(elementName);
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((T) element).Name;
        }

        #endregion
    }
}