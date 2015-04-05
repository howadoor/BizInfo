using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Perenis.Core.Collections
{
    /// <summary>
    /// Collects extension methods for <see cref="IEnumerable{T}"/> interface
    /// </summary>
    public static class EnumerableEx
    {
        /// <summary>
        /// Returns <see cref="IEnumerable{T}"/> of tuples containing pairs of source collections. First pair is constructed from first and second item,
        /// second pair from second and third, third pair from third a fourth etc.
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static IEnumerable<Tuple<TItem, TItem>> Tuples<TItem>(this IEnumerable<TItem> enumerable)
        {
            TItem previousItem = default(TItem);
            return enumerable.Where((item, index) => index > 0).Select((item, index) =>
                                                                           {
                                                                               if (index == 0) previousItem = enumerable.First();
                                                                               var tuple = new Tuple<TItem, TItem>(previousItem, item);
                                                                               previousItem = item;
                                                                               return tuple;
                                                                           });
        }

        /// <summary>
        /// Returns <see cref="IEnumerable{T}"/> of tuples containing pairs of source collections. First pair is constructed from first and second item,
        /// second pair from second and third, third pair from third a fourth etc. Last tuple consist of last and first item in the enumeration.
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static IEnumerable<Tuple<TItem, TItem>> LoopTuples<TItem>(this IEnumerable<TItem> enumerable)
        {
            TItem previousItem = default(TItem);
            int i = 0;
            foreach (var item in enumerable)
            {
                if (i == 0)
                {
                    previousItem = item;
                }
                else
                {
                    yield return new Tuple<TItem, TItem>(previousItem, item);
                    previousItem = item;
                }
                i++;
            }
            yield return new Tuple<TItem, TItem>(previousItem, enumerable.First());
        }
        
        /// <summary>
        /// Returns <see cref="IEnumerable{T}"/> of tuples containing pairs of source collections. First pair is constructed from first and second item,
        /// second pair from third and fourth, third pair from fifth a sixth etc.
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static IEnumerable<Tuple<TItem, TItem>> Pairs<TItem>(this IEnumerable<TItem> enumerable)
        {
            bool isOdd = true;
            TItem previous = default(TItem);
            foreach (var item in enumerable)
            {
                if (isOdd)
                {
                    previous = item;
                }
                else
                {
                    yield return new Tuple<TItem, TItem>(previous, item);
                }
                isOdd = !isOdd;
            }
            if (!isOdd) throw new InvalidOperationException("Count of items in enumerable for Pairs must not be odd");
        }

        /// <summary>
        /// Checks whether a collection is the same as another collection
        /// </summary>
        /// <param name="value">The current instance object</param>
        /// <param name="compareList">The collection to compare with</param>
        /// <param name="comparer">The comparer object to use to compare each item in the collection.  If null uses EqualityComparer(T).Default</param>
        /// <returns>True if the two collections contain all the same items in the same order</returns>
        public static bool IsEqualTo<TSource>(this IEnumerable<TSource> value, IEnumerable<TSource> compareList, IEqualityComparer<TSource> comparer)
        {
            if (value == compareList)
            {
                return true;
            }
            else if (value == null || compareList == null)
            {
                return false;
            }
            else
            {
                if (comparer == null)
                {
                    comparer = EqualityComparer<TSource>.Default;
                }

                IEnumerator<TSource> enumerator1 = value.GetEnumerator();
                IEnumerator<TSource> enumerator2 = compareList.GetEnumerator();

                bool enum1HasValue = enumerator1.MoveNext();
                bool enum2HasValue = enumerator2.MoveNext();

                try
                {
                    while (enum1HasValue && enum2HasValue)
                    {
                        if (!comparer.Equals(enumerator1.Current, enumerator2.Current))
                        {
                            return false;
                        }

                        enum1HasValue = enumerator1.MoveNext();
                        enum2HasValue = enumerator2.MoveNext();
                    }

                    return !(enum1HasValue || enum2HasValue);
                }
                finally
                {
                    if (enumerator1 != null) enumerator1.Dispose();
                    if (enumerator2 != null) enumerator2.Dispose();
                }
            }
        }

        /// <summary>
        /// Checks whether a collection is the same as another collection
        /// </summary>
        /// <param name="value">The current instance object</param>
        /// <param name="compareList">The collection to compare with</param>
        /// <returns>True if the two collections contain all the same items in the same order</returns>
        public static bool IsEqualTo<TSource>(this IEnumerable<TSource> value, IEnumerable<TSource> compareList)
        {
            return IsEqualTo(value, compareList, null);
        }

        /// <summary>
        /// Checks whether a collection is the same as another collection
        /// </summary>
        /// <param name="value">The current instance object</param>
        /// <param name="compareList">The collection to compare with</param>
        /// <returns>True if the two collections contain all the same items in the same order</returns>
        public static bool IsEqualTo(this IEnumerable value, IEnumerable compareList)
        {
            return IsEqualTo<object>(value.OfType<object>(), compareList.OfType<object>());
        }

        /// <summary>
        /// Returns item with type best matching (closest to) <see cref="typeToMatch"/>.
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="items"></param>
        /// <param name="typeGetter"></param>
        /// <param name="typeToMatch"></param>
        /// <returns></returns>
        public static TItem FindBestMatchingType<TItem>(this IEnumerable<TItem> items, Func<TItem, Type> typeGetter, Type typeToMatch)
        {
            TItem bestMatchingItem = default(TItem);
            Type bestMatchingType = null;
            foreach (var item in items)
            {
                var itemType = typeGetter(item);
                if (itemType == typeToMatch) return item;
                if (!itemType.IsAssignableFrom(typeToMatch)) continue;
                if (bestMatchingType == null || bestMatchingType.IsAssignableFrom(itemType))
                {
                    bestMatchingItem = item;
                    bestMatchingType = itemType;
                    continue;
                }
            }
            return bestMatchingItem;
        }
    }
}