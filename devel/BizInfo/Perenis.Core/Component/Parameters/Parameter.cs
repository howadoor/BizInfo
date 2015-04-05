using System;

namespace Perenis.Core.Component.Parameters
{
    /// <summary>
    /// Holds a named value representing a parameter of an object.
    /// </summary>
    /// <remarks>
    /// Parameters are represented as either a string-object pair or as a list of <see cref="Parameter"/>.
    /// </remarks>
    public class Parameter : IComparable, IComparable<Parameter>
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public Parameter(object structuredName, object value)
            : this(ParamsBuilder.GetParameterName(structuredName), structuredName, value)
        {
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        internal Parameter(string name, object structuredName, object value)
        {
            Name = name;
            StructuredName = structuredName;
            Value = value;
        }

        #region ------ Implementation of the IComparable interface --------------------------------

        #region IComparable Members

        int IComparable.CompareTo(object obj)
        {
            if (obj == null) return 1;
            var other = obj as Parameter;
            if (other == null) throw new ArgumentException();
            return Name.CompareTo(other.Name);
        }

        #endregion

        #region IComparable<Parameter> Members

        public int CompareTo(Parameter other)
        {
            if (other == null) return 1;
            return Name.CompareTo(other.Name);
        }

        #endregion

        #endregion

        /// <summary>
        /// The name of the parameter.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The structured name of the parameter.
        /// </summary>
        public object StructuredName { get; private set; }

        /// <summary>
        /// The value of the parameter.
        /// </summary>
        public object Value { get; private set; }
    }
}