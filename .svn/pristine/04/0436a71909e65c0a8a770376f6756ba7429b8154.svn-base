using System;
using System.Linq.Expressions;
using System.Threading;
using BizInfo.App.Services.Logging;
using BizInfo.Harvesting.Interfaces.Processing;
using BizInfo.Harvesting.Services.Processing;
using BizInfo.Harvesting.Services.Scouting.Management;
using BizInfo.Model.Entities;

namespace BizInfo.Harvesting.Services.Core
{
    /// <summary>
    /// Represents main object for harvesting. It does everything - scouting, harvesting, processing...
    /// </summary>
    public class Harvester
    {
        private Daemon processingDaemon;
        private DaemonStatusLogger processingDaemonLogger;
        private ProcessingManager processingManager;
        
        private DispatchingWebSourceProcessor processor;
        private Daemon scheduledReprocessingDaemon;
        private DaemonStatusLogger scheduledReprocessingDaemonLogger;

        /// <summary>
        /// Creates new instance of <see cref="Harvester"/> and starts all deamons and processes immediatelly
        /// </summary>
        public Harvester()
        {
            Create();
        }

        /// <summary>
        /// Token used for cancellation
        /// </summary>
        public CancellationToken CancellationToken { get; set; }

        private void Create()
        {
            // start  scouting
            ScoutingManagerTools.Default.StartNewScoutingTask();
            // start processing of scouted infos
            processingManager = new ProcessingManager{CancellationToken = CancellationToken};
            processor = DispatchingWebSourceProcessor.Create();
            processingDaemon = new Daemon
                                   {
                                       Action = () => Process(ws => !ws.Processed.HasValue),
                                       Period = TimeSpan.FromMinutes(3)
                                   };
            processingDaemonLogger = new DaemonStatusLogger {DaemonName = "Processing new sources"};
            processingDaemonLogger.Attach(processingDaemon);
            processingDaemon.Start();
            // create and start reprocessing of scheduled sources
            scheduledReprocessingDaemon = new Daemon
                                              {
                                                  Action = () => processingManager.ProcessScheduledReloads(processor),
                                                  Period = TimeSpan.FromMinutes(360)
                                              };
            scheduledReprocessingDaemonLogger = new DaemonStatusLogger {DaemonName = "Reprocessing scheduled sources"};
            scheduledReprocessingDaemonLogger.Attach(scheduledReprocessingDaemon);
            scheduledReprocessingDaemon.Start();
        }

        /// <summary>
        /// Simple handy method for reprocessing all sources. Process only such sources which was not processed yet.
        /// </summary>
        public void ProcessAll()
        {
            Process(ws => ws.Processed.HasValue);
        }

        /// <summary>
        /// Process all instances of <see cref="Info"/> in database filtered by <see cref="filter"/>
        /// </summary>
        /// <param name="filter"></param>
        public void Process(Expression<Func<Info, bool>> filter)
        {
            processingManager.Process(processor, filter);
        }

        /// <summary>
        /// Process all instances of <see cref="WebSource"/> in database filtered by <see cref="filter"/>
        /// </summary>
        /// <param name="filter"></param>
        public void Process(Expression<Func<WebSource, bool>> filter)
        {
            processingManager.Process(processor, filter);
        }
    }
}