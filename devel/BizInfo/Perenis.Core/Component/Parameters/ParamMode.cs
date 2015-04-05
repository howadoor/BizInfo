using System;

namespace Perenis.Core.Component.Parameters
{
    /// <summary>
    /// The modes of a parametr (i.e. input / output).
    /// </summary>
    [Flags]
    public enum ParamMode
    {
        /// <summary>
        /// The mode of the parameter shall be inferred from the semantics of respective the property 
        /// or field.
        /// </summary>
        /// <remarks>
        /// The actual inferred according to the following:
        /// <list>
        ///     <item>A field is always <see cref="ParamMode.InOut"/>.</item>
        ///     <item>A property having a public getter (but not a public setter) is <see cref="ParamMode.In"/>.</item>
        ///     <item>A property having a public setter (but not a public getter) is <see cref="ParamMode.Out"/>.</item>
        ///     <item>A property having both a public getter and setter is <see cref="ParamMode.InOut"/>.</item>
        /// </list>
        /// </remarks>
        Default = 0x00,

        /// <summary>
        /// The parameter is treated an input parameter.
        /// </summary>
        /// <remarks>
        /// A property marked as an <see cref="ParamMode.In"/> parametr must have a public setter.
        /// </remarks>
        In = 0x01,

        /// <summary>
        /// The parameter is treated an output parameter.
        /// </summary>
        /// <remarks>
        /// A property marked as an <see cref="ParamMode.In"/> parametr must have a public getter.
        /// </remarks>
        Out = 0x02,

        /// <summary>
        /// The parameter is treated as both an input and output parameter.
        /// </summary>
        /// <remarks>
        /// A property marked as an <see cref="ParamMode.InOut"/> parametr must have a public getter and setter.
        /// </remarks>
        InOut = In | Out,
    }
}