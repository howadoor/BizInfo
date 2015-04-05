using System.Collections.Generic;

namespace Perenis.Core.Collections
{
    /// <summary>
    /// Extension methods for <see cref="IEnumerator{T}"/>
    /// </summary>
    public static class EnumeratorEx
    {
        /// <summary>
        /// Returns array of <see cref="count"/> items from enumerable enumerated by <see cref="enumerator"/>. First item of the array
        /// is current item of enumerator (enumerator.Current).
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="enumerator"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static TItem[] MoveNext<TItem> (this IEnumerator<TItem> enumerator, int count)
        {
            var array = new TItem[count];
            for (var i = 0; i < count; i++, enumerator.MoveNext() )
            {
                array[i] = enumerator.Current;
            }
            return array;
        }
    }
}