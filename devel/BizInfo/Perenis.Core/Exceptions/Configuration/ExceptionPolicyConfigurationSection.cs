using System;
using System.Configuration;
using Perenis.Core.Configuration;

namespace Perenis.Core.Exceptions.Configuration
{
    /// <summary>
    /// Represents the configuration of exception handlers and policies.
    /// </summary>
    public class ExceptionPolicyConfigurationSection : ConfigurationSection
    {
        private static ExceptionPolicyConfigurationSection _default;

        /// <summary>
        /// The configuration section instance.
        /// </summary>
        /// <remarks>
        /// By default, provides access to the section located in the main config file. Use the 
        /// <see cref="BeeSectionGroup.Load"/> method <b>before first access to this property</b>
        /// to provide a custom configuration source and override the defaults.
        /// </remarks>
        public static ExceptionPolicyConfigurationSection Default
        {
            get
            {
                if (_default == null)
                {
                    try
                    {
                        _default = (ExceptionPolicyConfigurationSection) BeeSectionGroup.Default.Sections["exceptionPolicies"];
                    }
                    catch (Exception)
                    {
                        // TODO Na hrubý pytel hrubá záplata
                        var policyElem = new ExceptionPolicyElement();
                        policyElem.Name = "Default";
                        policyElem.Template = "Default";

                        var policies = new ExceptionPolicyElementCollection();
                        policies.Add(policyElem);

                        var policyTempl = new ExceptionPolicyTemplateElement();
                        policyTempl.Name = "Default";

                        var policyTemplates = new ExceptionPolicyTemplateElementCollection();
                        policyTemplates.Add(policyTempl);

                        _default = new ExceptionPolicyConfigurationSection();
#if NUNIT
                        _default.ExceptionPolicies = policies;
                        _default.ExceptionPolicyTemplates = policyTemplates;
                        _default.ExceptionHandlers = new ExceptionHandlerElementCollection();
#endif
                    }
                }
                return _default;
            }
        }

        /// <summary>
        /// Exception handlers' configurations.
        /// </summary>
        [ConfigurationProperty("handlers")]
        [ConfigurationCollection(typeof (ExceptionHandlerElement), AddItemName = "handler")]
        public ExceptionHandlerElementCollection ExceptionHandlers
        {
            get { return (ExceptionHandlerElementCollection) base["handlers"]; }
#if NUNIT
            internal set { base["handlers"] = value; }
#endif
        }

        /// <summary>
        /// Exception policies' configurations.
        /// </summary>
        [ConfigurationProperty("policyTemplates")]
        [ConfigurationCollection(typeof (ExceptionPolicyTemplateElement), AddItemName = "policy")]
        public ExceptionPolicyTemplateElementCollection ExceptionPolicyTemplates
        {
            get { return (ExceptionPolicyTemplateElementCollection) base["policyTemplates"]; }
#if NUNIT
            internal set { base["policyTemplates"] = value; }
#endif
        }

        /// <summary>
        /// Exception policies' configurations.
        /// </summary>
        [ConfigurationProperty("policies")]
        [ConfigurationCollection(typeof (ExceptionPolicyElement), AddItemName = "policy")]
        public ExceptionPolicyElementCollection ExceptionPolicies
        {
            get { return (ExceptionPolicyElementCollection) base["policies"]; }
#if NUNIT
            internal set { base["policies"] = value; }
#endif
        }
    }
}