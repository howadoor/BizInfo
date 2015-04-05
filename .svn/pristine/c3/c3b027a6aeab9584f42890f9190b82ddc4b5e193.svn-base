using System;

namespace Perenis.Core.Component
{
    /// <summary>
    /// Allows to assign a structured name to a program component.
    /// </summary>
    /// <seealso cref="StructuredNamingResolver{T}"/>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
    public class StructuredNameAttribute : Attribute
    {
        private readonly object structuredName;

        /// <summary>
        /// Initializes a new instance of the <see cref="StructuredNameAttribute" /> class.
        /// </summary>
        public StructuredNameAttribute(object structuredName)
        {
            this.structuredName = structuredName;
        }

        /// <summary>
        /// Strucutred name, <see cref="StructuredNamingResolver{T}"/>.
        /// </summary>
        public object StructuredName
        {
            get { return structuredName; }
        }
    }
}