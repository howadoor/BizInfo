namespace Perenis.Core.General
{
    /// <summary>
    /// Presents source object in some context on target object. Used when not only one object (source) is fully defining the situation.
    /// 
    /// One of reasons for intrducing this interface is presentation of instances <see cref="ObjectVisual3D"/> on <see cref="OverCanvas"/>. There is no
    /// way how to distinguish between object visuals created by various objects. Solution is to call <see cref="IContextualPresenter.Present"/> with first
    /// argument of object which created ObjectVisual3D and second ObjectVisual3D instance itself.
    /// 
    /// There is another possible solution here - put service creating OverCanvas items into some property of ObjectVisual3D. This is not good - it tightly coupling
    /// ObjectVisual3D with OverCanvas. Solution with IContextualPresenter keeps ObjectVisual3D totaly agnostic of OverCanvas.
    /// </summary>
    public interface IContextualPresenter
    {
        void Present(object source, object context, object target);
    }

    /// <summary>
    /// Presents TSource in TContext on TTarget. If you want to create a contextual presenter, implement this interface and register your new type in Presenter object, for instance
    /// using Spring xml configuration file.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TTarget"></typeparam>
    /// <typeparam name="TContext"></typeparam>
    public interface IContextualPresenter<in TSource, in TContext, in TTarget>
    {
        /// <summary>
        /// Creates presentation of source on target in some context. See <see cref="IContextualPresenter"/> interface for detailed description.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="context"></param>
        /// <param name="target"></param>
        void Present(TSource source, TContext context, TTarget target);
    }
}