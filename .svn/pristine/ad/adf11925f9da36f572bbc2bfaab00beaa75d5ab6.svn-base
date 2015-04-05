using System.Diagnostics;
using System.Reflection;

namespace Perenis.Core.Reflection
{
    /// <summary>
    /// Base class for enum decorators
    /// </summary>
    ///<typeparam name="T"> Enum type to decorate</typeparam>
    public class EnumDecorator<T>
    {
        #region ------ Private fields ------------------------------------------

        /// <summary>
        /// Enumeration value
        /// </summary>
        private T value;

        #endregion

        #region ------ Public properties -------------------------------------------

        /// <summary>
        /// Enumeration value
        /// </summary>
        public T Value
        {
            get { return value; }
            set
            {
                Debug.Assert(value.GetType() == typeof (T));
                this.value = value;
            }
        }

        /// <summary>
        /// Name of the enumeration
        /// </summary>
        public string Name
        {
            get { return value.ToString(); }
        }

        /// <summary>
        /// Field info of the enumeration
        /// </summary>
        public FieldInfo Field
        {
            get { return typeof (T).GetField(Name, BindingFlags.Public | BindingFlags.Static); }
        }

        #endregion
    }
}