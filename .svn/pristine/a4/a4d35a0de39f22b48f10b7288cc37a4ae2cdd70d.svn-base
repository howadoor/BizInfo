using System;
using System.Resources;

namespace Perenis.Core.Resources
{
    /// <summary>
    /// Binds a display name (in plural form) to an element.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class DisplayNamePluralAttribute : TextResourceAttribute
    {
        #region ------ Constructors ---------------------------------------------------------------

        public DisplayNamePluralAttribute(ResourceManager resourceManager, string resourceID)
            : base(resourceManager, resourceID)
        {
        }

        public DisplayNamePluralAttribute(Type resourceManagerType, string resourceID)
            : base(resourceManagerType, resourceID)
        {
        }

        #endregion
    }
}