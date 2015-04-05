using System.Configuration;

namespace Perenis.Core.Configuration
{
    /// <summary>
    /// Provides static access to the <c>bee</c> configuration section group.
    /// </summary>
    public static class BeeSectionGroup
    {
        private static ConfigurationSectionGroup _default;

        /// <summary>
        /// The <c>bee</c> configuration section group.
        /// </summary>
        /// <remarks>
        /// By default, provides access to the section group located in the application's config file.
        /// Use the <see cref="Load"/> method to provide a custom configuration source and override
        /// the defaults.
        /// </remarks>
        public static ConfigurationSectionGroup Default
        {
            get
            {
                if (_default == null)
                {
                    _default = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).GetSectionGroup("bee");
                }
                return _default;
            }
        }

        /// <summary>
        /// Loads the <c>bee</c> configuration section group from the given <see cref="Configuration"/>
        /// and overrides the defaults located in the application's config file.
        /// </summary>
        /// <param name="config">The <see cref="Configuration"/> instance to be used as source of configuration.</param>
        public static void Load(System.Configuration.Configuration config)
        {
            if (_default == null) _default = config.GetSectionGroup("bee");
        }
    }
}