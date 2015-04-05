namespace Perenis.Core.Component.Progress
{
    /// <summary>
    /// End-user progress message interface.
    /// </summary>
    public interface IProgressMessage : IProgressInformation
    {
        /// <summary>
        /// Message severity.
        /// </summary>
        Severity Severity { get; }

        /// <summary>
        /// Message summary.
        /// </summary>
        string Summary { get; }

        /// <summary>
        /// Optional detailed message description.
        /// </summary>
        string Description { get; }
    }
}