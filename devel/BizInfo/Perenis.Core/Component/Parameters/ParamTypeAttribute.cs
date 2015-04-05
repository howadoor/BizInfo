using System;

namespace Perenis.Core.Component.Parameters
{
    /// <summary>
    /// When applied to an object which is a structured name of a parameter specifies the type of that parameter.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public sealed class ParamTypeAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public ParamTypeAttribute(Type type)
        {
            Type = type;
        }

        /// <summary>
        /// The permitted type of a parameter.
        /// </summary>
        public Type Type { get; set; }
    }
}