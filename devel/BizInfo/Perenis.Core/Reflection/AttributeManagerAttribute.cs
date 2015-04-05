using System;

namespace Perenis.Core.Reflection
{
    /// <summary>
    /// Defines the attribute manager to be used for a specific element.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public sealed class AttributeManagerAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="attributeManagerType"/> is a null reference.</exception>
        /// <exception cref="ArgumentException"><paramref name="attributeManagerType"/> is not a descendant
        /// of the <see cref="AttributeManager"/> type.</exception>
        public AttributeManagerAttribute(Type attributeManagerType)
        {
            if (attributeManagerType == null) throw new ArgumentNullException("attributeManagerType");
            if (!Types.IsDescendantOrEqual(attributeManagerType, typeof (AttributeManager))) throw new ArgumentException("attributeManagerType");
            AttributeManagerType = attributeManagerType;
        }

        /// <summary>
        /// The actual type of the attribute manager to be used for the element this instance is
        /// applied to.
        /// </summary>
        public Type AttributeManagerType { get; set; }
    }
}