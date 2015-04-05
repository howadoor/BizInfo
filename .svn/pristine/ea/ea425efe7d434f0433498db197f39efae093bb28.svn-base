using System;
using Perenis.Core.Resources;

namespace Perenis.Core.Reflection
{
    /// <summary>
    /// Decorator to acces description attributes of enumeration
    /// </summary>
    public class DescriptionEnumDecorator<T> : EnumDecorator<T>
    {
        #region ------ Public properties -------------------------------------------

        /// <summary>
        /// Enum description
        /// </summary>
        public string DisplayDescription
        {
            get
            {
                var att = (DisplayDescriptionAttribute) Attribute.GetCustomAttribute(Field, typeof (DisplayDescriptionAttribute));
                return (att == null) ? String.Empty : att.Text;
            }
        }

        /// <summary>
        /// Enum name in plural
        /// </summary>
        public string DisplayNamePlural
        {
            get
            {
                var att = (DisplayNamePluralAttribute) Attribute.GetCustomAttribute(Field, typeof (DisplayNamePluralAttribute));
                return (att == null) ? String.Empty : att.Text;
            }
        }

        /// <summary>
        /// Enum name in singular
        /// </summary>
        public string DisplayNameSingular
        {
            get
            {
                var att = (DisplayNameSingularAttribute) Attribute.GetCustomAttribute(Field, typeof (DisplayNameSingularAttribute));
                return (att == null) ? String.Empty : att.Text;
            }
        }

        #endregion

        public override string ToString()
        {
            string result = DisplayNameSingular;
            return string.IsNullOrEmpty(result) ? Name : result;
        }
    }
}