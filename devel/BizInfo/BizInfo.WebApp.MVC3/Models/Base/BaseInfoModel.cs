using System;
using BizInfo.App.Services.Tools;

namespace BizInfo.WebApp.MVC3.Models.Base
{
    public class BaseInfoModel
    {
        private static BaseInfoModel defaultModel;

        private readonly OutdatedValidityKeeper baseInfoValidityKeeper;
        private readonly OutdatedValidityKeeper infoBySourcesValidityKeeper;
        private readonly OutdatedValidityKeeper searchingStatisticsValidityKeeper;

        private BaseInfo baseInfo;
        private InfoBySourcesStatistics infoBySources;
        private SearchingStatistics searchingStatistics;

        protected BaseInfoModel()
        {
            baseInfoValidityKeeper = new OutdatedValidityKeeper {MaxAge = TimeSpan.FromMinutes(5), Update = () => baseInfo = new BaseInfo()};
            infoBySourcesValidityKeeper = new OutdatedValidityKeeper {MaxAge = TimeSpan.FromMinutes(20), Update = () => infoBySources = new InfoBySourcesStatistics()};
            searchingStatisticsValidityKeeper = new OutdatedValidityKeeper {MaxAge = TimeSpan.FromMinutes(20), Update = () => searchingStatistics = new SearchingStatistics()};
        }

        public BaseInfo BaseInfo
        {
            get
            {
                baseInfoValidityKeeper.Validate();
                return baseInfo;
            }
        }

        public InfoBySourcesStatistics InfoBySources
        {
            get
            {
                infoBySourcesValidityKeeper.Validate();
                return infoBySources;
            }
        }

        public SearchingStatistics SearchingStatistics
        {
            get
            {
                searchingStatisticsValidityKeeper.Validate();
                return searchingStatistics;
            }
        }

        public static BaseInfoModel Default
        {
            get { return defaultModel ?? (defaultModel = new BaseInfoModel()); }
        }
    }
}