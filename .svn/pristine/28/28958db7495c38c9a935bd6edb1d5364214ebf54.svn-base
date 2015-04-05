using System.Configuration;
using Perenis.Core.Configuration;

namespace Perenis.Core.Exceptions.Configuration
{
    /// <summary>
    /// Represents a exception handlers, i.e. a group of exception handling rules that are applied
    /// to an exception. 
    /// </summary>
    public class ExceptionHandlerElement : ConfigurationElementCollectionEx<ExceptionHandlerRuleElement>
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public ExceptionHandlerElement()
        {
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public ExceptionHandlerElement(string name)
        {
            Name = name;
        }

        #region ------ Internals: ConfigurationElementCollection overrides ------------------------

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ExceptionHandlerRuleElement) element).Type;
        }

        #endregion

        /// <summary>
        /// The name of this exception handler group.
        /// </summary>
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string) base["name"]; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// Indicates whether to execute all handlers regardless of handling result, or to stop after
        /// first handler's handling the given exception.
        /// </summary>
        [ConfigurationProperty("all", DefaultValue = false, IsRequired = false)]
        public bool All
        {
            get { return (bool) base["all"]; }
            set { this["all"] = value; }
        }
    }

    /// <summary>
    /// Represents a collection of exception handlers.
    /// </summary>
    public class ExceptionHandlerElementCollection : ConfigurationElementCollectionEx<ExceptionHandlerElement>
    {
        public ExceptionHandlerElement GetByName(string name)
        {
            return (ExceptionHandlerElement) BaseGet(name);
        }

        #region ------ Internals: ConfigurationElementCollection overrides ------------------------

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ExceptionHandlerElement) element).Name;
        }

        #endregion
    }
}