using System;
using System.Linq;
using BizInfo.Model.Entities;

namespace BizInfo.WebApp.MVC3.Models.Base
{
    public class SearchingStatistics
    {
        public SearchingStatistics()
        {
            Update();
        }

        public SearchingStatisticsItem LastHour { get; private set; }
        public SearchingStatisticsItem LastDay { get; private set; }

        private void Update()
        {
            using (var repository = new BizInfoModelContainer())
            {
                LastHour = new SearchingStatisticsItem(repository, DateTime.Now - TimeSpan.FromHours(1));
                LastDay = new SearchingStatisticsItem(repository, DateTime.Now - TimeSpan.FromDays(1));
            }
        }
    }

    public class SearchingStatisticsItem
    {
        public SearchingStatisticsItem(BizInfoModelContainer repository, DateTime fromTime)
        {
            TotalCount = repository.SearchingLogSet.Where(sl => sl.Start > fromTime).Count();
            Succeeded = TotalCount > 0 ? repository.SearchingLogSet.Where(sl => sl.Start > fromTime && sl.Succeeded).Count() : 0;
            ItemsFound = TotalCount > 0 ? repository.SearchingLogSet.Where(sl => sl.Start > fromTime).Select(sl => sl.ResultsCount).Sum() : 0;
            TotalDuration = TotalCount > 0 ? TimeSpan.FromMilliseconds(repository.SearchingLogSet.Where(sl => sl.Start > fromTime).Select(sl => sl.Duration).AsEnumerable().Sum(ts => ts.TotalMilliseconds)) : TimeSpan.FromMilliseconds(0);
        }

        public int Succeeded { get; private set; }

        public int Failed
        {
            get { return TotalCount - Succeeded; }
        }

        public TimeSpan AverageDuration
        {
            get { return TimeSpan.FromMilliseconds(TotalCount > 0 ? TotalDuration.TotalMilliseconds/TotalCount : 0); }
        }

        public int TotalCount { get; private set; }
        public int ItemsFound { get; private set; }
        public TimeSpan TotalDuration { get; private set; }
    }
}