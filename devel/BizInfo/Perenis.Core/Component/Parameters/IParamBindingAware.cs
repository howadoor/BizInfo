namespace Perenis.Core.Component.Parameters
{
    /// <summary>
    /// Called by <see cref="ParamBinder"/> on the target binding instance.
    /// The interface is called on all the instances of the parameter instance tree, that is on the
    /// root instance and all the inner parameter containers.
    /// </summary>
    public interface IParamBindingAware
    {
        /// <summary>
        /// Called before writing of parameters 
        /// </summary>
        void OnBeforeSetParameters();

        /// <summary>
        /// Called after writing of parameters is finished.
        /// </summary>
        void OnAfterSetParameters();

        /// <summary>
        /// Called before reading of parameters.
        /// </summary>
        void OnBeforeGetParameters();

        /// <summary>
        /// Called after reading of parameters is finished.
        /// </summary>
        void OnAfterGetParameters();
    }
}