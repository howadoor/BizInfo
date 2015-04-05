namespace Perenis.Core.General
{
    /// <summary>
    /// Implementation of IAware where TSource is the same as TTarget. It returns simply the source object.
    /// </summary>
    /// <typeparam name="TAwared"></typeparam>
    public class SelfAware<TAwared> : IAware<TAwared, TAwared>
    {
        #region Implementation of IAware<TAwared,TAwared>

        /// <summary>
        /// Returns the source object
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public TAwared GetAwared(TAwared source)
        {
            return source;
        }

        #endregion
    }
}