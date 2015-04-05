using System;

namespace Perenis.Core.Component.Events
{
    /// <summary>
    /// Modes of event delivery via the <see cref="AttributedEventRegistry"/>.
    /// </summary>
    public enum EventDeliveryMode
    {
        /// <summary>
        /// Default delivery behavior.
        /// </summary>
        Default,

        /// <summary>
        /// Synchronous event delivery is required.
        /// </summary>
        /// <remarks>
        /// When selecting from two delivery modes, synchronous wins over default.
        /// </remarks>
        Synchronous,

        /// <summary>
        /// Asynchronous event delivery is required.
        /// </summary>
        /// <remarks>
        /// When selecting from two delivery modes, asynchronous wins over synchronous.
        /// </remarks>
        Asynchronous,
    }

    /// <summary>
    /// Defines the required event delivery mode for an event or an event handler.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Event, AllowMultiple = false, Inherited = true)]
    public class EventDeliveryModeAttribute : Attribute
    {
        private static readonly EventDeliveryModeAttribute _default = new EventDeliveryModeAttribute(EventDeliveryMode.Default);

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public EventDeliveryModeAttribute(EventDeliveryMode mode)
        {
            Mode = mode;
        }

        /// <summary>
        /// The event delivery mode.
        /// </summary>
        public EventDeliveryMode Mode { get; set; }

        /// <summary>
        /// When <c>true</c>, indicates that this delivery mode shall take precedence over the other party's delivery mode.
        /// </summary>
        public bool Force { get; set; }

        /// <summary>
        /// A singleton instance representing the <see cref="EventDeliveryMode.Default"/> delivery mode.
        /// </summary>
        public static EventDeliveryModeAttribute Default
        {
            get { return _default; }
        }
    }
}