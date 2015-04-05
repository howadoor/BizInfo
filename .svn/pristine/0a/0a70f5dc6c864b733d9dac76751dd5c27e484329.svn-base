using System;

namespace Perenis.Core.Component.Events
{
    /// <summary>
    /// Modes of automated event wiring via the <see cref="AttributedEventRegistry"/>.
    /// </summary>
    [Flags]
    public enum EventWiringMode
    {
        /// <summary>
        /// The default wiring mode; wires methods both with and without the <see cref="StructuredNameAttribute"/>.
        /// </summary>
        Default = Anonymous | Named,

        /// <summary>
        /// Wire methods without the <see cref="StructuredNameAttribute"/> based on compatible signatures.
        /// </summary>
        Anonymous = 0x01,

        /// <summary>
        /// Wire methods with the <see cref="StructuredNameAttribute"/> and a compatible signature.
        /// </summary>
        Named = 0x02,
    }

    /// <summary>
    /// Defines the automated event wiring mode for a class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class EventWiringModeAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public EventWiringModeAttribute(EventWiringMode mode)
        {
            Mode = mode;
        }

        /// <summary>
        /// The automated event wiring mode.
        /// </summary>
        public EventWiringMode Mode { get; set; }
    }
}