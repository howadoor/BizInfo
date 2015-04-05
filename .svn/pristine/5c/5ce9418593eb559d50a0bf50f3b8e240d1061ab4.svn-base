using System;
using System.Configuration;
using Perenis.Core.Reflection;

namespace Perenis.Core.Exceptions.Configuration
{
    /// <summary>
    /// Represents an actual exception handler.
    /// </summary>
    public class ExceptionHandlerRuleElement : ConfigurationElement
    {
        private IExceptionHandler instance;
        private Type type;

        /// <summary>
        /// The type of the exception handler for this rule.
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
        /// The instance of the exception handler for this rule.
        /// </summary>
        public IExceptionHandler Instance
        {
            get
            {
                if (instance == null) instance = TypeFactory<IExceptionHandler>.Instance.CreateInstance(Type);
                return instance;
            }
        }

        /// <summary>
        /// The name of the type of the exception handler for this rule.
        /// </summary>
        [ConfigurationProperty("type", IsKey = true, IsRequired = true)]
        public string TypeName
        {
            get { return (string) base["type"]; }
            set { this["type"] = value; }
        }
    }
}