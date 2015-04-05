using System;

namespace Perenis.Core.Component.Parameters
{
    /// <summary>
    /// Marks a property or field to be available as a parametr of the enclosing object.
    /// </summary>
    /// <remarks>
    /// A parameter holds a value that may be passed bethween objects using a binder.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class ParamAttribute : Attribute, IComparable<ParamAttribute>
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="structuredName">The structured name of the parameter.</param>
        public ParamAttribute(object structuredName)
        {
            StructuredName = structuredName;
            // TODO Resolve the parameter's type
            Name = ParamsBuilder.GetParameterName(structuredName);
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="structuredName">The structured name of the parameter.</param>
        /// <param name="mode">The mode of the parameter.</param>
        public ParamAttribute(object structuredName, ParamMode mode)
            : this(structuredName)
        {
            Mode = mode;
        }

        #region ------ Implementation of the IComparable interface --------------------------------

        public int CompareTo(ParamAttribute other)
        {
            if (other == null) return 1;
            return Order.CompareTo(other.Order);
        }

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
        /// The mode of the parameter.
        /// </summary>
        public ParamMode Mode { get; set; }

        /// <summary>
        /// Indicates that the parameter is optional.
        /// </summary>
        public bool Optional { get; set; }

        /// <summary>
        /// The optional order of setting / getting the value of the parameter.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Instantiates a value of the parametr described by this attribute.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Parameter Instantiate(object value)
        {
            // TODO Value type check
            return new Parameter(StructuredName, value);
        }

        /// <summary>
        /// Checks if this parameter supports the given <paramref name="mode"/>.
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public bool SupportsMode(ParamMode mode)
        {
            return (Mode == ParamMode.Default)
                   || (Mode == ParamMode.In && ((mode & ParamMode.In) > 0))
                   || (Mode == ParamMode.Out && ((mode & ParamMode.Out) > 0))
                   || (Mode == ParamMode.InOut && (mode == ParamMode.InOut || mode == ParamMode.Out || mode == ParamMode.In));
        }
    }
}