namespace Perenis.Core.Interfaces
{
    /// <summary>
    /// Named object
    /// </summary>
    /// <remarks>
    /// TODO: This interface is not really good. It is not clear if Name is display name, internal name or a kind of identifier. Rethink and rework.
    /// </remarks>
    public interface IWithName
    {
        /// <summary>
        /// Name of the object
        /// </summary>
        string Name { get; }
    }
}