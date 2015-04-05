using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using BizInfo.App.Services.Logging;
using BizInfo.Harvesting.Interfaces.Processing;
using BizInfo.Harvesting.Services.Storage;
using BizInfo.Model.Entities;
using BizInfo.Model.Interfaces;

namespace BizInfo.Harvesting.Services.Processing
{
    public class ProcessingManager
    {
        private ParallelOptions defaultParallelOptions;

        public ParallelOptions DefaultParallelOptions
        {
            get
            {
                if (defaultParallelOptions == null) defaultParallelOptions = new ParallelOptions();
                return defaultParallelOptions;
            }
            set { defaultParallelOptions = null; }
        }

        /// <summary>
        /// Used to cancel any processing
        /// </summary>
        public CancellationToken CancellationToken
        {
            get; set;
        }

        public void Process(IWebSourceProcessor processor, Expression<Func<WebSource, bool>> filter, ParallelOptions parallelOptions = null)
        {
            if (parallelOptions == null) parallelOptions = DefaultParallelOptions;
            var succeeded = 0;
            var failed = 0;
            long maxId = 0;
            for (; !CancellationToken.IsCancellationRequested; )
            {
                long[] webSourcesToProcess;
                using (var storage = new EntityStorage())
                {
                    storage.modelContainer.CommandTimeout = 120;
                    var _maxId = maxId;
                    IQueryable<WebSource> webSources = storage.modelContainer.WebSourceSet.Where(ws => ws.Id > _maxId);
                    if (filter != null) webSources = webSources.Where(filter);
                    var webSourcesToProcessQuery = webSources.Select(ws => ws.Id).OrderBy(id => id);
                    webSourcesToProcess = webSourcesToProcessQuery.ToArray();
                    if (webSourcesToProcess.Length == 0) break;
                }
                parallelOptions.CancellationToken = CancellationToken;
                Parallel.ForEach(webSourcesToProcess, parallelOptions, (webSourceId) =>
                                                                           {
                                                                               using (var storage = new EntityStorage())
                                                                               {
                                                                                   var webSource = storage.modelContainer.WebSourceSet.Where(ws => ws.Id == webSourceId).FirstOrDefault();
                                                                                   if (webSource == null) return;
                                                                                   bool success;
                                                                                   try
                                                                                   {
                                                                                       processor.Process(webSource, storage);
                                                                                       succeeded++;
                                                                                       success = true;
                                                                                   }
                                                                                   catch (Exception exception)
                                                                                   {
                                                                                       this.LogException(exception);
                                                                                       failed++;
                                                                                       success = false;
                                                                                   }
                                                                                   // websource can be deleted so we have to test it
                                                                                   if (webSource.EntityState != EntityState.Deleted)
                                                                                   {
                                                                                       webSource.ResultStatus = success ? ProcessingResult.OK : ProcessingResult.Failed;
                                                                                       webSource.Processed = DateTime.Now;
                                                                                   }
                                                                                   storage.modelContainer.SaveChanges();
                                                                                   this.LogInfo(string.Format("Processed {0} of {3}. Succeeded {1}, failed {2}.", succeeded + failed, succeeded, failed, webSourcesToProcess.Length));
                                                                               }
                                                                           });
                maxId = webSourcesToProcess.Last();
            }
        }

        public void Process(IWebSourceProcessor processor, Expression<Func<Info, bool>> filter, ParallelOptions parallelOptions = null)
        {
            if (parallelOptions == null) parallelOptions = DefaultParallelOptions;
            var succeeded = 0;
            var failed = 0;
            long maxId = 0;
            for (; !CancellationToken.IsCancellationRequested; )
            {
                long[] infoToProcess;
                using (var storage = new EntityStorage())
                {
                    storage.modelContainer.CommandTimeout = 120;
                    var _maxId = maxId;
                    var wsQuery = storage.modelContainer.InfoSet.Where(info => info.Id > _maxId && info.WebSourceId.HasValue);
                    if (filter != null) wsQuery = wsQuery.Where(filter);
                    var infoToProcessQuery = wsQuery.Select(i => i.WebSourceId.Value).OrderBy(id => id);
                    infoToProcess = infoToProcessQuery.ToArray();
                    if (infoToProcess.Length == 0) break;
                }
                parallelOptions.CancellationToken = CancellationToken;
                Parallel.ForEach(infoToProcess, parallelOptions, (info) =>
                                                                         {
                                                                             using (var storage = new EntityStorage())
                                                                             {
                                                                                 var webSource = storage.modelContainer.WebSourceSet.Where(ws => ws.Id == info).FirstOrDefault();
                                                                                 bool success;
                                                                                 try
                                                                                 {
                                                                                     processor.Process(webSource, storage);
                                                                                     succeeded++;
                                                                                     success = true;
                                                                                 }
                                                                                 catch (Exception exception)
                                                                                 {
                                                                                     this.LogException(exception);
                                                                                     failed++;
                                                                                     success = false;
                                                                                 }
                                                                                 // websource can be deleted so we have to test it
                                                                                 if (webSource.EntityState != EntityState.Deleted)
                                                                                 {
                                                                                     webSource.ResultStatus = success ? ProcessingResult.OK : ProcessingResult.Failed;
                                                                                     webSource.Processed = DateTime.Now;
                                                                                 }
                                                                                 storage.modelContainer.SaveChanges();
                                                                                 this.LogInfo(string.Format("Processed {0} of {3}. Succeeded {1}, failed {2}.", succeeded + failed, succeeded, failed, infoToProcess.Length));
                                                                             }
                                                                         });
                    maxId = infoToProcess.Last();
            }
        }

        public void Process(Action<IBizInfo, IBizInfoStorage> processor, Expression<Func<Info, bool>> filter = null, ParallelOptions parallelOptions = null)
        {
            if (parallelOptions == null) parallelOptions = DefaultParallelOptions;
            var succeeded = 0;
            var failed = 0;
            long maxId = 0;
            for (; !CancellationToken.IsCancellationRequested; )
            {
                long[] infoIdsToProcess;
                using (var storage = new EntityStorage())
                {
                    storage.modelContainer.CommandTimeout = 120;
                    var _maxId = maxId;
                    var infoToProcessQuery = storage.modelContainer.InfoSet.Where(info => info.Id > _maxId && info.WebSourceId.HasValue);
                    if (filter != null) infoToProcessQuery = infoToProcessQuery.Where(filter);
                    infoToProcessQuery = infoToProcessQuery.OrderBy(i => i.Id);
                    var infoToProcessQueryIds = infoToProcessQuery.Select(i => i.Id).OrderBy(id => id).Take(1024);
                    infoIdsToProcess = infoToProcessQueryIds.ToArray();
                    if (infoIdsToProcess.Length == 0) break;
                }
                parallelOptions.CancellationToken = CancellationToken;
                Parallel.ForEach(infoIdsToProcess, parallelOptions, (infoId) =>
                {
                    using (var storage = new EntityStorage())
                    {
                        var info = storage.modelContainer.InfoSet.FirstOrDefault(i => i.Id == infoId);
                        if (info != null)
                        {
                            bool success;
                            try
                            {
                                processor(info, storage);
                                succeeded++;
                                success = true;
                            }
                            catch (Exception exception)
                            {
                                this.LogException(exception);
                                failed++;
                                success = false;
                            }
                            storage.modelContainer.SaveChanges();
                            this.LogInfo(string.Format("Processed {0} of {3}. Succeeded {1}, failed {2}.", succeeded + failed, succeeded, failed, infoIdsToProcess.Length));
                        }
                    }
                });
                maxId = infoIdsToProcess.Last();
            }
        }

        public void ProcessScheduledReloads(IWebSourceProcessor processor, ParallelOptions parallelOptions = null)
        {
            if (parallelOptions == null) parallelOptions = DefaultParallelOptions;
            var succeeded = 0;
            var failed = 0;
            long maxId = 0;
            for (;!CancellationToken.IsCancellationRequested;)
            {
                using (var storage = new EntityStorage())
                {
                    storage.modelContainer.CommandTimeout = 120;
                    var now = DateTime.Now;
                    var scheduledToProcessQuery = storage.modelContainer.ScheduledReloadSet.Where(scheduled => scheduled.ScheduledTime > now);
                    var scheduledToProcess = scheduledToProcessQuery.Take(1024).ToArray();
                    if (scheduledToProcess.Length == 0) break;
                    parallelOptions.CancellationToken = CancellationToken;
                    Parallel.ForEach(scheduledToProcess, parallelOptions, (scheduled) =>
                                                                              {
                                                                                  WebSource webSource;
                                                                                  lock (storage)
                                                                                  {
                                                                                      var infoId = scheduled.InfoId;
                                                                                      var bizInfo = storage.modelContainer.InfoSet.SingleOrDefault(info => info.Id == infoId);
                                                                                      webSource = storage.modelContainer.GetWebSourceOf(bizInfo);
                                                                                      // delete scheduled reload here, later it can be re-scheduled
                                                                                      storage.modelContainer.ScheduledReloadSet.DeleteObject(scheduled);
                                                                                  }
                                                                                  bool success;
                                                                                  try
                                                                                  {
                                                                                      // It is not loaded from the cache! New content replaces the old one.
                                                                                      processor.Process(webSource, storage, UrlLoadOptions.StoreToCacheWhenLoaded);
                                                                                      succeeded++;
                                                                                      success = true;
                                                                                  }
                                                                                  catch (Exception exception)
                                                                                  {
                                                                                      this.LogException(exception);
                                                                                      failed++;
                                                                                      success = false;
                                                                                  }
                                                                                  lock (storage)
                                                                                  {
                                                                                      // websource can be deleted so we have to test it
                                                                                      if (webSource.EntityState != EntityState.Deleted)
                                                                                      {
                                                                                          webSource.ResultStatus = success ? ProcessingResult.OK : ProcessingResult.Failed;
                                                                                          webSource.Processed = DateTime.Now;
                                                                                      }
                                                                                  }
                                                                              });
                    storage.modelContainer.SaveChanges();
                    Console.WriteLine("Succeeded {0}, failed {1}", succeeded, failed);
                }
            }
        }
    }
}