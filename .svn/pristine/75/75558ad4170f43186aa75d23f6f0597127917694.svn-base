using System.Configuration;
using Perenis.Core.Configuration;

namespace Perenis.Core.Exceptions.Configuration
{
    /// <summary>
    /// Represents an exception policy, i.e. a group of exceptions policy rules that are applied to
    /// an exception. 
    /// </summary>
    public class ExceptionPolicyElement : ExceptionPolicyTemplateElement
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public ExceptionPolicyElement()
        {
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public ExceptionPolicyElement(string name)
        {
            Name = name;
        }

        /// <summary>
        /// The name of this exception policy.
        /// </summary>
        [ConfigurationProperty("template", IsRequired = false)]
        public string Template
        {
            get { return (string) base["template"]; }
            set { this["template"] = value; }
        }
    }

    /// <summary>
    /// Represents a collection of exception policies.
    /// </summary>
    public class ExceptionPolicyElementCollection : ConfigurationElementCollectionEx<ExceptionPolicyElement>
    {
        public ExceptionPolicyElement GetByName(string name)
        {
            return (ExceptionPolicyElement) BaseGet(name);
        }

        #region ------ Internals: ConfigurationElementCollection overrides ------------------------

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ExceptionPolicyElement) element).Name;
        }

        #endregion
    }
}