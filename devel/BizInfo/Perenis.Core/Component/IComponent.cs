namespace Perenis.Core.Component
{
    /// <summary>
    /// Base component interface.
    /// </summary>
    /// <remarks>
    /// Allows to manage the component's life time and to gather component information.  
    /// /// </remarks>
    public interface IComponent
    {
        /// <summary>
        /// Component friendly name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Initialized the component.
        /// </summary>
        /// <remarks>
        /// Is run once to allow the component's initialization.
        /// </remarks>
        void Init();
    }
}