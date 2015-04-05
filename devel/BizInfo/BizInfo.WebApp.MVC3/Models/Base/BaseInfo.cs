using System;
using System.Linq;
using BizInfo.App.Services.Logging;
using BizInfo.Model.Entities;

namespace BizInfo.WebApp.MVC3.Models.Base
{
    public class BaseInfo
    {
        public BaseInfo()
        {
            Update();
        }

        public int WebSourcesCount { get; private set; }

        public int WebSourcesCountLastHour { get; private set; }

        public int WebSourcesCountLastDay { get; private set; }

        public int InfoSentCount { get; private set; }

        public int InfoSentCountLastHour { get; private set; }

        public int InfoSentCountLastDay { get; private set; }

        public int BizInfoCount { get; private set; }

        public int SuccessfulyProcessedWebSourcesCount { get; private set; }

        public int FailedProcessedWebSourcesCount { get; private set; }

        public int NotProcessedWebSourcesCount { get; private set; }

        public int ScheduledSourcesForReload { get; private set; }

        protected void Update()
        {
            using (this.LogOperation("BaseInfoModel.Update"))
            {
                // it is not necessarry to lock here because it is locked by validityKeeper
                using (var container = new BizInfoModelContainer())
                {
                    // count of web sources
                    WebSourcesCount = container.WebSourceSet.Count();
                    var beforeHour = DateTime.Now - TimeSpan.FromHours(1);
                    WebSourcesCountLastHour = container.WebSourceSet.Count(ws => ws.Scouted > beforeHour);
                    var beforeDay = DateTime.Now - TimeSpan.FromDays(1);
                    WebSourcesCountLastDay = container.WebSourceSet.Count(ws => ws.Scouted > beforeDay);
                    // count of info sent
                    var wasSomethingSent = container.SentMessagesSet.FirstOrDefault() != null;
                    InfoSentCount = wasSomethingSent ? container.SentMessagesSet.Select(sm => sm.ContainedInfoCount).Sum() : 0;
                    InfoSentCountLastHour = wasSomethingSent ? container.SentMessagesSet.Where(sm => sm.SendTime > beforeHour).Select(sm => sm.ContainedInfoCount).Sum() : 0;
                    InfoSentCountLastDay = wasSomethingSent ? container.SentMessagesSet.Where(sm => sm.SendTime > beforeDay).Select(sm => sm.ContainedInfoCount).Sum() : 0;
                    // others
                    BizInfoCount = container.InfoSet.Count();
                    SuccessfulyProcessedWebSourcesCount = container.WebSourceSet.Count(ws => ws.Processed.HasValue && ws.ProcessingResult >= 0);
                    FailedProcessedWebSourcesCount = container.WebSourceSet.Count(ws => ws.Processed.HasValue && ws.ProcessingResult < 0);
                    NotProcessedWebSourcesCount = container.WebSourceSet.Count(ws => !ws.Processed.HasValue);
                    ScheduledSourcesForReload = container.ScheduledReloadSet.Count();
                }
                this.LogInfo(ToString());
            }
        }

        /// <summary>
        /// Converts instance to string especially for logging purposes
        /// </summary>
        /// <remarks>
        /// We have deliberately use private members directly. Using properties causes calling <see cref="Update"/> method and endless recursion.
        /// </remarks>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("BizInfo {0} Sources {4} ProcessedSources {3}, FailedSources {1} NotProcessedSources {2} SourcesLastHour {6} SourcesLastDay {5} ScheduledForReload {7}", BizInfoCount, FailedProcessedWebSourcesCount,
                                 NotProcessedWebSourcesCount, SuccessfulyProcessedWebSourcesCount, WebSourcesCount, WebSourcesCountLastDay, WebSourcesCountLastHour, ScheduledSourcesForReload);
        }
    }
}