namespace Perenis.Core.Pattern
{
    /// <summary>
    /// Provides filtering functionality for the visitor pattern.
    /// </summary>
    public interface IFilter<TVisitorContext> : IVisitorContextAware<TVisitorContext>
        where TVisitorContext : IVisitorContext
    {
    }
}