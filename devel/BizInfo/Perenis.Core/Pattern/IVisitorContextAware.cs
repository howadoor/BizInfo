namespace Perenis.Core.Pattern
{
    public interface IVisitorContextAware<TVisitorContext>
        where TVisitorContext : IVisitorContext
    {
        /// <summary>
        /// The current execution context of the visitor pattern.
        /// </summary>
        TVisitorContext Context { get; set; }
    }
}