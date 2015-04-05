using System;

namespace Perenis.Core.Collections
{
    /// <summary>
    /// Bag of values of TEnum. Collects enum values, each value can be added many times.
    /// </summary>
    /// <typeparam name="TEnum">enum type</typeparam>
    public class EnumBag<TEnum> : EnumDictionary<TEnum, int>
    {
        protected int _totalCount;

        /// <summary>
        /// Total count of values
        /// </summary>
        public int Count
        {
            get { return _totalCount; }
        }

        /// <summary>
        /// Array of counts of TEnum values
        /// </summary>
        public int[] Counts
        {
            get { return Values; }
        }

        /// <summary>
        /// Remove everything from bag
        /// </summary>
        public override void RemoveAll()
        {
            base.RemoveAll();
            _totalCount = 0;
        }

        /// <summary>
        /// Remove all occurences of value from bag
        /// </summary>
        /// <param name="value">TEnum value</param>
        public void RemoveAll(TEnum value)
        {
            int index = EnumTypeDescription.IndexOf(value);
            _totalCount -= Counts[index];
            RemoveAt(value);
        }

        /// <summary>
        /// Returns count of occurences of given TEnum value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int CountOf(TEnum value)
        {
            return Counts[EnumTypeDescription.IndexOf(value)];
        }

        /// <summary>
        /// Adds TEnum value to bag
        /// </summary>
        /// <param name="value"></param>
        public void Add(TEnum value)
        {
            Add(value, 1);
        }

        /// <summary>
        /// Adds TEnum valu to bag count times
        /// </summary>
        /// <param name="value"></param>
        /// <param name="count">count of occurences of value to add to bag</param>
        public void Add(TEnum value, int count)
        {
            int index = EnumTypeDescription.IndexOf(value);
            _totalCount += count;
            Counts[index] += count;
        }

        /// <summary>
        /// Removes one occurence of value from bag
        /// </summary>
        /// <param name="value"></param>
        public void Remove(TEnum value)
        {
            Remove(value, 1);
        }

        /// <summary>
        /// Removes count of occurences of value from bag
        /// </summary>
        /// <param name="value"></param>
        /// <param name="count">Count of occurences to remove</param>
        /// <remarks>
        /// No check for count if performed (it can be greater than current count of occurences, results to negative count)
        /// </remarks>
        public void Remove(TEnum value, int count)
        {
            int index = EnumTypeDescription.IndexOf(value);
            _totalCount -= count;
            Counts[index] -= count;
        }

        /// <summary>
        /// Performs an action on each value and count (only for count > 0)
        /// </summary>
        /// <param name="action"></param>
        public void WithValuesAndCounts(Action<TEnum, int> action)
        {
            WithValuesAndCounts(action, false);
        }

        /// <summary>
        /// Performs an action on each value and count
        /// </summary>
        /// <param name="action"></param>
        /// <param name="includingZeroCounts">should be included values with no occurency?</param>
        public void WithValuesAndCounts(Action<TEnum, int> action, bool includingZeroCounts)
        {
            WithKeysAndValues(action, includingZeroCounts);
        }

        /// <summary>
        /// Adds whole content of other bag
        /// </summary>
        /// <param name="enumBag"></param>
        public void Add(EnumBag<TEnum> enumBag)
        {
            enumBag.WithValuesAndCounts(Add);
        }
    }
}