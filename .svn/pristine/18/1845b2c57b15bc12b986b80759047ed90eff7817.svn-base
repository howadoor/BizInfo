using System;
using System.Collections.Generic;
using System.Linq;
using BizInfo.App.Services.Logging;
using BizInfo.Model.Entities;

namespace BizInfo.WebApp.MVC3.Models.Base
{
    public class InfoBySourceStatistics
    {
        public InfoBySourceStatistics(string source, int addedInLastHout, int addedInLastDay, int addedInLast100Days, int totalCount)
        {
            Source = source;
            AddedInLastHour = addedInLastHout;
            AddedInLastDay = addedInLastDay;
            AddedInLast100Days = addedInLast100Days;
            TotalCount = totalCount;
        }

        public string Source { get; private set; }
        public int AddedInLastHour { get; private set; }
        public int AddedInLastDay { get; private set; }
        public int AddedInLast100Days { get; private set; }
        public int TotalCount { get; private set; }

        public override string ToString()
        {
            return string.Format("{0} Hour {1} Day {2} 100-Days {3} Total {4}", Source, AddedInLastHour, AddedInLastDay, AddedInLast100Days, TotalCount);
        }
    }

    public class InfoBySourcesStatistics : List<InfoBySourceStatistics>
    {
        public InfoBySourcesStatistics()
        {
            Update();
        }

        private void Update()
        {
            using (this.LogOperation("InfoBySourcesStatistics.Update"))
            {
                Clear();
                using (var container = new BizInfoModelContainer {CommandTimeout = 120})
                {
                    AddRange(SearchingCriteriaModelEx.SourceTagIds.Select(tagId => CreateStatistics(tagId, container)));
                }
                Sort((i1, i2) => string.Compare(i1.Source, i2.Source));
                if (LoggingTools.IsLoggingEnabled)
                {
                    foreach (var statistics in this)
                    {
                        this.LogInfo(statistics.ToString());
                    }
                }
            }
        }

        private static InfoBySourceStatistics CreateStatistics(int? tagId, BizInfoModelContainer container)
        {
            var sourceName = !tagId.HasValue ? "Ostatní, neurèeno" : container.TagSet.Where(t => t.Id == tagId.Value).Single().Name;
            var lastHour = DateTime.Now - TimeSpan.FromHours(1);
            var lastDay = DateTime.Now - TimeSpan.FromDays(1);
            var last100Days = DateTime.Now - TimeSpan.FromDays(100);
            var addedInLastHour = container.InfoSet.Where(i => i.SourceTagId == tagId && i.CreationTime > lastHour).Count();
            var addedInLastDay = container.InfoSet.Where(i => i.SourceTagId == tagId && i.CreationTime > lastDay).Count();
            var addedInLast100Days = container.InfoSet.Where(i => i.SourceTagId == tagId && i.CreationTime > last100Days).Count();
            var totalCount = container.InfoSet.Where(i => i.SourceTagId == tagId).Count();
            return new InfoBySourceStatistics(sourceName, addedInLastHour, addedInLastDay, addedInLast100Days, totalCount);
        }
    }
}