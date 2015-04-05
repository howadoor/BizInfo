namespace Perenis.Core.General
{
    /// <summary>
    /// Common presenter
    /// </summary>
    public interface IPresenter
    {
        /// <summary>
        /// Present source object on target
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        void Present(object source, object target);

        /// <summary>
        /// Tests if can present a source object on target object
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        bool CanPresent(object source, object target);
    }

    /// <summary>
    /// Presents TSource on TTarget
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TTarget"></typeparam>
    public interface IPresenter<TSource, TTarget>
    {
        /// <summary>
        /// Creates presentation of source on target
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        void Present(TSource source, TTarget target);
    }
}