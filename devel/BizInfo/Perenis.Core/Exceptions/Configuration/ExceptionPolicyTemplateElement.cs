using System.Configuration;
using Perenis.Core.Configuration;

namespace Perenis.Core.Exceptions.Configuration
{
    /// <summary>
    /// Represents an exception policy template, i.e. a group of exceptions policy rules that are 
    /// applied to an exception. 
    /// </summary>
    public class ExceptionPolicyTemplateElement : ConfigurationElementCollectionEx<ExceptionPolicyRuleElement>
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public ExceptionPolicyTemplateElement()
        {
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public ExceptionPolicyTemplateElement(string name)
        {
            Name = name;
        }

        #region ------ Internals: ConfigurationElementCollection overrides ------------------------

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ExceptionPolicyRuleElement) element).Type;
        }

        #endregion

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
    public class ExceptionPolicyTemplateElementCollection : ConfigurationElementCollectionEx<ExceptionPolicyTemplateElement>
    {
        public ExceptionPolicyTemplateElement GetByName(string name)
        {
            return (ExceptionPolicyTemplateElement) BaseGet(name);
        }

        #region ------ Internals: ConfigurationElementCollection overrides ------------------------

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ExceptionPolicyTemplateElement) element).Name;
        }

        #endregion
    }
}