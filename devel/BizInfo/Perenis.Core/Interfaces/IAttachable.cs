namespace Perenis.Core.Interfaces
{
    /// <summary>
    /// Object which can be attached to other object for some purposes
    /// </summary>
    public interface IAttachable
    {
        /// <summary>
        /// Attaches instance of <see cref="IAttachable"/> to <see cref="attachTarget"/>
        /// </summary>
        /// <param name="attachTarget"></param>
        void Attach(object attachTarget);

        /// <summary>
        /// Detaches instance of <see cref="IAttachable"/> from <see cref="attachTarget"/>.
        /// </summary>
        /// <param name="attachTarget"></param>
        void Detach(object attachTarget);
    }


    /// <summary>
    /// Object which can be attached to other object for some purposes. Typed version of <see cref="IAttachable"/>.
    /// </summary>
    public interface IAttachable<TTarget>
    {
        /// <summary>
        /// Attaches instance of <see cref="IAttachable"/> to <see cref="attachTarget"/>
        /// </summary>
        /// <param name="attachTarget"></param>
        void Attach(TTarget attachTarget);

        /// <summary>
        /// Detaches instance of <see cref="IAttachable"/> from <see cref="attachTarget"/>.
        /// </summary>
        /// <param name="attachTarget"></param>
        void Detach(TTarget attachTarget);
    }
}