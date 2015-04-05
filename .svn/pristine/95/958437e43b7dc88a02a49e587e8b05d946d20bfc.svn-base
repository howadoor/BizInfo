using System;
using BizInfo.App.Services.Logging;
using BizInfo.Harvesting.Services.Core;
using BizInfo.Harvesting.Services.Processing.Fragments;

namespace BizInfo.Harvesting.Services.Messaging
{
    /// <summary>
    /// Periodicaly starts database maintenance tasks
    /// </summary>
    public class DbMaintainer
    {
        private Daemon maintainerDaemon;
        private DaemonStatusLogger maintainerDaemonLogger;

        /// <summary>
        /// Creates new instance of <see cref="DbMaintainer"/> and starts deamon which periodicaly starts database maintenance
        /// </summary>
        public DbMaintainer()
        {
            Start();
        }

        protected bool MaintenanceShouldBePerformedRightNow
        {
            get
            {
                var now = DateTime.Now;
                return now.Hour == 2;
            }
        }

        /// <summary>
        /// Starts deamon which periodicaly looks for new items in db and sendes it to the users
        /// </summary>
        private void Start()
        {
            maintainerDaemon = new Daemon {Action = PerformMaintenance, Period = TimeSpan.FromHours(1)};
            maintainerDaemonLogger = new DaemonStatusLogger {DaemonName = "Database maintenance"};
            maintainerDaemonLogger.Attach(maintainerDaemon);
            maintainerDaemon.Start();
        }

        /// <summary>
        /// Performs maintenance tasks
        /// </summary>
        private void PerformMaintenance()
        {
            if (MaintenanceShouldBePerformedRightNow)
            {
                DbMaintenanceTools.RefreshIndexes();
                DbMaintenanceTools.RecomputeOccurencies();
            }
        }
    }
}