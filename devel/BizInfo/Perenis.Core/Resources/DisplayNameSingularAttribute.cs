using System;
using System.Resources;

namespace Perenis.Core.Resources
{
    /// <summary>
    /// Binds a display name (in singular form) to an element.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class DisplayNameSingularAttribute : TextResourceAttribute
    {
        #region ------ Constructors ---------------------------------------------------------------

        public DisplayNameSingularAttribute(ResourceManager resourceManager, string resourceID)
            : base(resourceManager, resourceID)
        {
        }

        public DisplayNameSingularAttribute(Type resourceManagerType, string resourceID)
            : base(resourceManagerType, resourceID)
        {
        }

        #endregion
    }
}