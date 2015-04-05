using System;

namespace Perenis.Core.General
{
    /// <summary>
    /// Allows you to se a different display name for an enum value,
    /// or simple put a DisplayName to fields, properties and other things.
    /// This attribute is only intended to be used in enum values, but the constraint
    /// allows it to be used in custom fields, but this will be useless.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
    public sealed class DisplayNameAttribute :
        Attribute
    {
        #region Constructor

        /// <summary>
        /// Creates the display name attribute setting it's value.
        /// </summary>
        /// <param name="displayName">The displayName to use for this enum.</param>
        public DisplayNameAttribute(string displayName)
        {
            DisplayName = displayName;
        }

        #endregion

        #region DisplayName

        /// <summary>
        /// Gets the display name for the enum value.
        /// </summary>
        public string DisplayName { get; private set; }

        #endregion
    }
}