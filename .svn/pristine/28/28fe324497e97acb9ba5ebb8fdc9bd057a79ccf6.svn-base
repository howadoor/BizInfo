namespace Perenis.Core.Pattern
{
    /// <summary>
    /// Simple factory interface.
    /// </summary>
    /// <typeparam name="InstanceT">Compatible instance type of created instances.</typeparam>
    public interface IFactory<InstanceT>
    {
        /// <summary>
        /// Creates an instance of type <typeparamref name="InstanceT"/>.
        /// </summary>
        /// <param name="instance"></param>
        void Create(out InstanceT instance);
    }

    /// <summary>
    /// Factory interface.
    /// </summary>
    /// <typeparam name="InstanceT">Compatible instance type of created instances.</typeparam>
    /// <typeparam name="ContextT">Contextual information type.</typeparam>
    public interface IContextFactory<InstanceT, ContextT>
    {
        /// <summary>
        /// Creates an instance of type <typeparamref name="InstanceT"/>
        /// based on the contextual information <paramref name="context"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="instance"></param>
        void Create(ContextT context, out InstanceT instance);
    }
}