using System.Configuration;
using Perenis.Core.Configuration;

namespace Perenis.Core.Reflection.Configuration
{
    /// <summary>
    /// Represents specific configuration of assemblies.
    /// </summary>
    public class AssemblyConfigurationSection : ConfigurationSection
    {
        private static AssemblyConfigurationSection _default;

        /// <summary>
        /// The configuration section instance.
        /// </summary>
        /// <remarks>
        /// By default, provides access to the section located in the main config file. Use the 
        /// <see cref="BeeSectionGroup.Load"/> method <b>before first access to this property</b> 
        /// to provide a custom configuration source and override the defaults.
        /// </remarks>
        public static AssemblyConfigurationSection Default
        {
            get
            {
                if (_default == null)
                {
                    _default = (AssemblyConfigurationSection) BeeSectionGroup.Default.Sections["assemblies"];
                }
                return _default;
            }
        }

        /// <summary>
        /// List of assemblies to be pre-loaded upon application start-up.
        /// </summary>
        [ConfigurationProperty("preloads")]
        [ConfigurationCollection(typeof (AssemblyPreloadElement), AddItemName = "preload")]
        public AssemblyPreloadElementCollection PreloadedAssemblies
        {
            get { return (AssemblyPreloadElementCollection) base["preloads"]; }
#if NUNIT
            internal set { base["preloads"] = value; }
#endif
        }
    }
}