using System;
using System.Collections.Generic;

namespace Perenis.Core.Rules
{
    public static class ConditionEx
    {
        public static void FilterList<TFiltered>(this ICondition<TFiltered> condition, IEnumerable<TFiltered> list, Action<TFiltered> retainedAction, Action<TFiltered> notRetainedAction)
        {
            foreach (TFiltered filtered in list)
            {
                if (condition.IsMatch(filtered))
                {
                    if (retainedAction != null) retainedAction(filtered);
                }
                else
                {
                    if (notRetainedAction != null) notRetainedAction(filtered);
                }
            }
        }

        public static void FilterList<TFiltered>(this ICondition<TFiltered> condition, IEnumerable<TFiltered> list, IList<TFiltered> retainedList, IList<TFiltered> notRetainedList)
        {
            condition.FilterList(list, (retainedList != null) ? retainedList.Add : (Action<TFiltered>) null, notRetainedList != null ? notRetainedList.Add : (Action<TFiltered>) null);
        }
    }
}