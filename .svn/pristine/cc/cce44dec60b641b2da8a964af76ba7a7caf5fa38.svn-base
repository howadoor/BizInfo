using System;
using System.Configuration;

namespace Perenis.Core.Exceptions.Configuration
{
    /// <summary>
    /// The post-handling actions of an exception policy rule.
    /// </summary>
    public enum ExceptionPolicyRethrow
    {
        /// <summary>
        /// Retrow unhandled exceptions only.
        /// </summary>
        Unhandled,

        /// <summary>
        /// Rethrow handled exceptions only.
        /// </summary>
        Handled,

        /// <summary>
        /// Always rethrow.
        /// </summary>
        Always,

        /// <summary>
        /// Never rethrow.
        /// </summary>
        Never,
    }

    /// <summary>
    /// Binds a given type of an exception with a specific handler.
    /// </summary>
    public class ExceptionPolicyRuleElement : ConfigurationElement
    {
        private Type type;

        /// <summary>
        /// The type of the exception matching this rule.
        /// </summary>
        public Type Type
        {
            get
            {
                if (type == null) type = Type.GetType(TypeName, true);
                return type;
            }
        }

        /// <summary>
        /// The name of the type of the exception handled by this rule.
        /// </summary>
        [ConfigurationProperty("type", IsKey = true, IsRequired = true)]
        public string TypeName
        {
            get { return (string) base["type"]; }
            set { this["type"] = value; }
        }

        /// <summary>
        /// The handler applied to the exception.
        /// </summary>
        [ConfigurationProperty("handler", IsRequired = true)]
        public string HandlerName
        {
            get { return (string) base["handler"]; }
            set { base["handler"] = value; }
        }

        /// <summary>
        /// Indicates whether the exception shall be rethrown upon handling.
        /// </summary>
        [ConfigurationProperty("rethrow", IsRequired = true)]
        public ExceptionPolicyRethrow Rethrow
        {
            get { return (ExceptionPolicyRethrow) base["rethrow"]; }
            set { base["rethrow"] = value; }
        }
    }
}