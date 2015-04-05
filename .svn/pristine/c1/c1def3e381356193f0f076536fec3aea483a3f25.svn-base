using System.Configuration;

namespace Perenis.Core.Configuration
{
    /// <summary>
    /// A configuration element having a name.
    /// </summary>
    public abstract class NamedConfigurationElement : ConfigurationElement
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        protected NamedConfigurationElement()
        {
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        protected NamedConfigurationElement(string name)
        {
            Name = name;
        }

        /// <summary>
        /// The name of this configuration element.
        /// </summary>
        [ConfigurationProperty("name", DefaultValue = "name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string) this["name"]; }
            set { this["name"] = value; }
        }
    }
}