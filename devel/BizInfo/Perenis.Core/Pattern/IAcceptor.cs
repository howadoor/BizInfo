namespace Perenis.Core.Pattern
{
    /// <summary>
    /// Provides acceptor functionality for the visitor pattern.
    /// </summary>
    /// <seealso cref="IAcceptor{TVisitorContext,TScope}"/>
    public interface IAcceptor<TVisitorContext> : IVisitorContextAware<TVisitorContext>
        where TVisitorContext : IVisitorContext
    {
    }

    /// <summary>
    /// Provides acceptor functionality for the visitor pattern.
    /// </summary>
    /// <typeparam name="TVisitorContext">The type of the execution context.</typeparam>
    /// <typeparam name="TScope">The type of the scope.</typeparam>
    /// <remarks>
    /// Unlike the non-generic <see cref="IAcceptor"/>, this interface enables scope management.
    /// </remarks>
    public interface IAcceptor<TVisitorContext, TScope> : IAcceptor<TVisitorContext>
        where TVisitorContext : IVisitorContext
    {
        /// <summary>
        /// This method is called when the given <paramref name="scope"/> is being entered.
        /// </summary>
        /// <param name="scope">A user-specific scope-definition object.</param>
        void OnEnterScope(TScope scope);

        /// <summary>
        /// This method is called when the given <paramref name="scope"/> is being left.
        /// </summary>
        /// <param name="scope">A user-specific scope-definition object.</param>
        void OnLeaveScope(TScope scope);
    }
}