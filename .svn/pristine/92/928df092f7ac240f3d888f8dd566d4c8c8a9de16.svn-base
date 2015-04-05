namespace Perenis.Core.General
{
    /// <summary>
    /// Instance implementing IAware is awared obout something in given object
    /// </summary>
    public interface IAware
    {
        /// <summary>
        /// Returns awared object in given object
        /// </summary>
        /// <typeparam name="TRequested"></typeparam>
        /// <param name="object"></param>
        /// <returns></returns>
        TRequested GetAwared<TRequested>(object @object);
    }

    /// <summary>
    /// Specialized implementation of IAware. Knows about awared only in instances of TSource.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TTarget"></typeparam>
    public interface IAware<TSource, TTarget>
    {
        TTarget GetAwared(TSource source);
    }
}