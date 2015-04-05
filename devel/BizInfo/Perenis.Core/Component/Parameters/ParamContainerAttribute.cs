using System;

namespace Perenis.Core.Component.Parameters
{
    /// <summary>
    /// Marks a public property to hold a value providing nested parameters.
    /// </summary>
    /// <remarks>
    /// Instructs the parameter accessor to drill-down into the value instance of the marked property when
    /// accessing the instance parameters.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class ParamContainerAttribute : Attribute
    {
    }
}