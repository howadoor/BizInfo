using System;
using System.Resources;

namespace Perenis.Core.Resources
{
    /// <summary>
    /// Binds a text (string) resource to an element.
    /// </summary>
    public abstract class TextResourceAttribute : ResourceAttribute
    {
        /// <summary>
        /// The text (string) resource bound to the elemenet.
        /// </summary>
        public string Text
        {
            get { return ResourceManager.GetString(ResourceID); }
        }

        #region ------ Constructors ---------------------------------------------------------------

        protected TextResourceAttribute(ResourceManager resourceManager, string resourceID)
            : base(resourceManager, resourceID)
        {
        }

        protected TextResourceAttribute(Type resourceManagerType, string resourceID)
            : base(resourceManagerType, resourceID)
        {
        }

        #endregion
    }
}