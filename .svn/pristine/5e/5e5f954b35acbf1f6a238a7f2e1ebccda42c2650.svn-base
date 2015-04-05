using System.Configuration;
using Perenis.Core.Configuration;

namespace Perenis.Core.Reflection.Configuration
{
    /// <summary>
    /// Represents an exception policy template, i.e. a group of exceptions policy rules that are 
    /// applied to an exception. 
    /// </summary>
    public class AssemblyPreloadElement : ConfigurationElement
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public AssemblyPreloadElement()
        {
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public AssemblyPreloadElement(string name)
        {
            Name = name;
        }

        /// <summary>
        /// The name of this exception policy.
        /// </summary>
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string) base["name"]; }
            set { this["name"] = value; }
        }
    }

    /// <summary>
    /// Represents a collection of exception policy templates.
    /// </summary>
    public class AssemblyPreloadElementCollection : ConfigurationElementCollectionEx<AssemblyPreloadElement>
    {
        public AssemblyPreloadElement GetByName(string name)
        {
            return (AssemblyPreloadElement) BaseGet(name);
        }

        #region ------ Internals: ConfigurationElementCollection overrides ------------------------

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AssemblyPreloadElement) element).Name;
        }

        #endregion
    }
}