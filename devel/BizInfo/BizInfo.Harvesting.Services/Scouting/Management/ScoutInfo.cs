using BizInfo.Harvesting.Interfaces.Processing;
using BizInfo.Model.Entities;

namespace BizInfo.Harvesting.Services.Scouting.Management
{
    internal class ScoutInfo
    {
        public string ScoutId { get; private set; }

        public ScoutInfo(string id, IScout scout)
        {
            ScoutId = id;
            Scout = scout;
        }

        protected internal IScout Scout
        {
            get;
            private set;
        }

        public ScoutRun LastRun { get; set; }
    }
}