using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BizInfo.App.Services.Logging;
using BizInfo.Harvesting.Interfaces.Processing;
using BizInfo.Harvesting.Services.Storage;
using BizInfo.Model.Entities;
using BizInfo.Model.Interfaces;

namespace BizInfo.Harvesting.Services.Scouting.Management
{
    public class ScoutingManager
    {
        List<ScoutInfo> scouts = new List<ScoutInfo>();
        private Task scoutingTask;
        private CancellationToken cancellationToken;

        public Func<ScoutingManager, Func<string, IBizInfoStorage, bool>> AcceptorFactory { get; set; }

        public CancellationToken CancellationToken
        {
            get { return cancellationToken; }
        }

        public void AddScout(string id, IScout scout)
        {
            lock (scouts)
            {
                if (scouts.Any(sc => sc.ScoutId == id)) throw new InvalidOperationException("Scout already exists in scout manager");
                var scoutInfo = new ScoutInfo(id, scout) {LastRun = GetLastScoutRun(id)};
                scouts.Add(scoutInfo);
            }
        }

        private ScoutRun GetLastScoutRun(string scoutId)
        {
            using (var container = new BizInfoModelContainer())
            {
                var lastRun = container.ScoutRunSet.Where(sr => sr.ScoutId == scoutId).OrderByDescending(sr => sr.EndTime).FirstOrDefault();
                return lastRun ?? new ScoutRun {ScoutId = scoutId};
            }
        }

        public void StartNewScoutingTask()
        {
            if (scoutingTask != null) throw new InvalidOperationException("Scouting task already running. You have to stop it first.");
            scoutingTask = Task.Factory.StartNew(Scouting, cancellationToken = new CancellationToken());
        }

        private void Scouting()
        {
            using (var scoutingLog = this.LogOperation("Scouting task"))
            {
                for (; !cancellationToken.IsCancellationRequested;)
                {
                    var scout = GetNextScout();
                    WaitIfNecessary(scout);
                    if (cancellationToken.IsCancellationRequested) break;
                    var scoutRun = RunScouting(scout);
                    StoreScoutRun(scoutRun);
                }
            }
        }

        private void WaitIfNecessary(ScoutInfo scout)
        {
            var minTime = scout.LastRun.EndTime + TimeSpan.FromMinutes(5);
            if (DateTime.Now >= minTime) return;
            using (var waitingLog = this.LogOperation(string.Format("Waiting for next scouting up to {0}", minTime)))
            {
                for (; DateTime.Now < minTime;)
                {
                    Thread.Sleep(30);
                    if (cancellationToken.IsCancellationRequested) break;
                }
            }
        }

        private void StoreScoutRun(ScoutRun scoutRun)
        {
            using (var container = new BizInfoModelContainer())
            {
                container.ScoutRunSet.AddObject(scoutRun);
                container.SaveChanges();
            }
        }

        private ScoutRun RunScouting(ScoutInfo scoutInfo)
        {
            using (var logScouting = this.LogOperation(string.Format("Scouting of {0}", scoutInfo.ScoutId)))
            {
                var scoutRun = new ScoutRun {ScoutId = scoutInfo.ScoutId, StartTime = DateTime.Now};
                var acceptor = new ManagedScoutingAcceptor(CancellationToken) {MaximumOldUrlsInLine = 10};
                using (var storage = new EntityStorage())
                {
                    try
                    {
                        scoutInfo.Scout.Scout(storage, acceptor.Accept);
                    }
                    catch (Exception exception)
                    {
                        this.LogException(exception);
                        scoutRun.Exception = exception.ToString();
                    }
                }
                scoutRun.AcceptedCount = acceptor.AcceptedCount;
                scoutRun.NotAcceptedCount = acceptor.NotAcceptedCount;
                scoutRun.LastAcceptedUrl = acceptor.LastAcceptedUrl;
                scoutRun.LastNotAcceptedUrl = acceptor.LastNotAcceptedUrl;
                scoutRun.EndTime = DateTime.Now;
                return scoutRun;
            }
        }

        private int scoutCounter;
        
        private ScoutInfo GetNextScout()
        {
            lock (scouts)
            {
                scoutCounter++;
                return scouts[scoutCounter % scouts.Count];
            }
        }
    }
}