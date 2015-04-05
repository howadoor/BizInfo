using System;
using System.Resources;

namespace Perenis.Core.Resources
{
    /// <summary>
    /// Binds a displayable description to an element.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class DisplayDescriptionAttribute : TextResourceAttribute
    {
        #region ------ Constructors ---------------------------------------------------------------

        public DisplayDescriptionAttribute(ResourceManager resourceManager, string resourceID) : base(resourceManager, resourceID)
        {
        }

        public DisplayDescriptionAttribute(Type resourceManagerType, string resourceID) : base(resourceManagerType, resourceID)
        {
        }

        #endregion
    }
}