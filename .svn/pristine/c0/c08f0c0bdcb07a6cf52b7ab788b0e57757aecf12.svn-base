using System;

namespace Perenis.Core.Component.Progress
{
    /// <summary>
    /// A percentage-scaled progress controller interface.
    /// </summary>
    /// <remarks>
    /// Note that several members of this interface are identical to those of the <see cref="IProgressObserver"/>
    /// interface. However, by design, this interface is not derived from <see cref="IProgressObserver"/>
    /// to maintain logical independence of these two interfaces.
    /// </remarks>
    [Obsolete("Use IProgressTransaction & IProgressNotification instead.")]
    public interface IProgressController
    {
        /// <summary>
        /// The precision of percentage calculations.
        /// </summary>
        /// <remarks>
        /// The default precision is 0, i.e. zero decimal places.
        /// </remarks>
        int Precision { get; set; }

        /// <summary>
        /// The lowest progress value in the original scale.
        /// </summary>
        uint Minimum { get; }

        /// <summary>
        /// The highest progress value in the original scale.
        /// </summary>
        uint Maximum { get; }

        /// <summary>
        /// The length of the original scale (i.e. <c>Scale == Maximum - Minimum</c>).
        /// </summary>
        uint Scale { get; }

        /// <summary>
        /// Current progress value in the original scale.
        /// </summary>
        uint Current { get; }

        /// <summary>
        /// Current progress value in the percentage scale.
        /// </summary>
        decimal CurrentPercentage { get; }
    }
}