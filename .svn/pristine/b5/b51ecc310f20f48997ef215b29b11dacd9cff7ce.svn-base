using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using BizInfo.App.Services.Logging;
using BizInfo.Harvesting.Interfaces.Processing;
using BizInfo.Harvesting.Services.Core;
using BizInfo.Harvesting.Services.Processing;
using BizInfo.Harvesting.Services.Processing.CompanyScoreTools;
using BizInfo.Harvesting.Services.Scouting.Management;
using BizInfo.Harvesting.Services.Storage;
using BizInfo.Model.Entities;
using BizInfo.Model.Interfaces;

namespace BizInfo.Experiment
{
    /// <summary>
    /// Obsahuje metody realizující různé experimentální procesy ověřující praktiky použitelné pro projekt BizInfo
    /// </summary>
    internal class ExperimentTools
    {
        private UrlDownloader Loader { get; set; }
        protected int PhotosLoaded { get; set; }

        protected int OffersLoaded { get; set; }

        protected int ListsLoaded { get; set; }

        public void Run()
        {
            // Reprocess(info => !info.HasContact && info.SourceTag.Name == "annonce.cz");
            //StartReprocessingScheduledReloads();
            /*
            var processingManager = new ProcessingManager { PortionSize = 64 };
            Expression<Func<WebSource, bool>> predInfo = ws => ws.Url == @"http://www.annonce.cz/detail/doklady-na-audi-prodam-15957894-w486fd.html";
            processingManager.Process(DispatchingWebSourceProcessor.Create(), predInfo, new ParallelOptions { MaxDegreeOfParallelism = 256 });
            return;
            */
            //StartReprocessingFailedSources();
            //OccurencyTools.RecomputeOccurencies();
            //RecomputeIsCompanyScore();
            ScoutingManagerTools.Default.StartNewScoutingTask();
            for (;;)
            {
                using (this.LogOperation("Processing not processed web sources campaign"))
                {
                    try
                    {
                        var processingManager = new ProcessingManager();
                        processingManager.Process(DispatchingWebSourceProcessor.Create(), info => !info.Processed.HasValue, new ParallelOptions {MaxDegreeOfParallelism = 256});
                    }
                    catch (Exception exception)
                    {
                        this.LogException(exception);
                    }
                }
                Thread.Sleep(TimeSpan.FromMinutes(5));
            }
            //TagInfos();
        }

 

        private void StartReprocessingFailedSources()
        {
            Task.Factory.StartNew
                (() =>
                     {
                         for (;;)
                         {
                             var processingManager = new ProcessingManager();
                             processingManager.Process(DispatchingWebSourceProcessor.Create(), info => info.Processed.HasValue && info.ProcessingResult < 0, new ParallelOptions {MaxDegreeOfParallelism = 256});
                             Thread.Sleep(TimeSpan.FromMinutes(5));
                         }
                     }
                );
        }

        /// <summary>
        /// BE EXTREMELY CAREFULL WHEN USING THIS METHOD
        /// It can destroy all binary sources
        /// </summary>
        private void StartReprocessingScheduledReloads()
        {
            // DO NOT USE BEFORE REVISION
            return;
            Task.Factory.StartNew
                (() =>
                     {
                         for (;;)
                         {
                             var processingManager = new ProcessingManager();
                             processingManager.ProcessScheduledReloads(DispatchingWebSourceProcessor.Create(), new ParallelOptions {MaxDegreeOfParallelism = 256});
                             Thread.Sleep(TimeSpan.FromHours(6));
                         }
                     }
                )
                ;
        }

        private void RecomputeIsCompanyScore()
        {
            var processed = 0;
            var computer = new CompanyScoreComputer();
            for (;;)
            {
                using (var storage = new EntityStorage())
                {
                    storage.modelContainer.CommandTimeout = 120;
                    var toProcess = storage.modelContainer.InfoSet.OrderByDescending(i => i.Id).Skip(processed).Take(1024).ToArray();
                    if (toProcess.Count() == 0) return;
                    foreach (var info in toProcess)
                    {
                        info.IsCompanyScore = (float) computer.ComputeScore(info);
                        processed++;
                    }
                    storage.modelContainer.SaveChanges();
                    Console.WriteLine("Processed {0}", processed);
                }
            }
        }

        private void NormalizeOfferTimes()
        {
            var processed = 449000;
            for (;;)
            {
                using (var storage = new EntityStorage())
                {
                    var toProcess = storage.modelContainer.InfoSet.OrderBy(i => i.Id).Skip(processed).Take(1024).ToArray();
                    if (toProcess.Count() == 0) return;
                    foreach (var info in toProcess)
                    {
                        info.CreationTime = CommonOfferParser.NormalizeOfferTime(info.CreationTime, storage.modelContainer.GetWebSourceOf(info));
                        processed++;
                    }
                    storage.modelContainer.SaveChanges();
                    Console.WriteLine("Processed {0}", processed);
                }
            }
        }


        private void RecacheBlobs()
        {
            var targetBlobsStorage = new BlobsStorage(@"c:\projects\BizInfo.Blobs");
            foreach (var sourceFile in Directory.GetFiles(@"c:\projects\BizInfo.Experiment\Cache"))
            {
                int length;
                byte[] content;
                using (var sourceStream = File.OpenRead(sourceFile))
                {
                    length = (int) sourceStream.Length;
                    content = new byte[length];
                    sourceStream.Read(content, 0, length);
                }
                var blobId = int.Parse(Path.GetFileNameWithoutExtension(sourceFile));
                var extension = Path.GetExtension(sourceFile).Substring(1);
                targetBlobsStorage.Store(blobId, extension, content, length);
            }
        }

        private void TagInfos()
        {
            var tagger = new Tagger();
            var processed = 0;
            var failed = 0;
            long maxId = 0;
            var startTime = DateTime.Now;
            for (;;)
            {
                var roundStartTime = DateTime.Now;
                var roundStartCount = processed + failed;
                using (var storage = new EntityStorage())
                {
                    Console.WriteLine("Retrieving...");
                    var id = maxId;
                    var infosToTag = storage.modelContainer.InfoSet.Where(info => info.Id > id && !info.DomainTagId.HasValue).OrderBy(i => i.Id).Take(1024).ToList();
                    if (infosToTag.Count == 0) break;
                    Console.WriteLine("Processing...");
                    Parallel.ForEach(infosToTag, new ParallelOptions {MaxDegreeOfParallelism = 1}, (bizInfo) =>
                                                                                                       {
                                                                                                           bool success;
                                                                                                           try
                                                                                                           {
                                                                                                               tagger.Tag(bizInfo, storage);
                                                                                                               processed++;
                                                                                                               Console.WriteLine(string.Format("#{0} {3} {1}, failed {2}", processed, bizInfo.Summary, failed, bizInfo.Id));
                                                                                                               success = true;
                                                                                                           }
                                                                                                           catch (Exception exception)
                                                                                                           {
                                                                                                               Console.WriteLine(string.Format("FAILED #{0} {4} {1} {3}, failed {2}", processed, bizInfo.Summary, failed, exception.Message, bizInfo.Id));
                                                                                                               failed++;
                                                                                                               success = false;
                                                                                                           }
                                                                                                           //if (processed > 1000) break;
                                                                                                       });
                    maxId = infosToTag.Last().Id;
                    Console.WriteLine("Saving...");
                    storage.modelContainer.SaveChanges();
                }
                Console.WriteLine("Avg throughput {0:0.#}, this round {1:0.#}", (failed + processed)/(DateTime.Now - startTime).TotalMilliseconds*1000.0,
                                  (failed + processed - roundStartCount)/(DateTime.Now - roundStartTime).TotalMilliseconds*1000.0);
            }
            Console.WriteLine(string.Format("Processed {0}, failed {1}", processed, failed));
        }

        public void Reprocess(long bizInfoid)
        {
            Reprocess(i => i.Id == bizInfoid);
        }

        public void Reprocess(Expression<Func<Info, bool>> filter)
        {
            new ProcessingManager().Process(DispatchingWebSourceProcessor.Create(), filter);
        }

        public void ProcessOffers()
        {
            var processor = DispatchingWebSourceProcessor.Create();
            var processed = 0;
            var failed = 0;
            var startTime = DateTime.Now;
            for (;;)
            {
                var roundStartTime = DateTime.Now;
                var roundStartCount = processed + failed;
                using (var storage = new EntityStorage())
                {
                    Console.WriteLine("Retrieving...");
                    storage.modelContainer.CommandTimeout = 120;
                    var annonceTag = storage.GetTag("annonce.cz");
                    var webSourcesToParseQuery =
                        storage.modelContainer.WebSourceSet.Where(sourceCandidate => !sourceCandidate.Processed.HasValue /*|| (sourceCandidate.Processed.Value < startTime)*/).OrderBy(ws => ws.Id).Skip(processed + failed);
                    //storage.modelContainer.InfoSet.Where(info => info.SourceTagId == annonceTag.Id /*&& info.StructuredContent == null*/).OrderByDescending(ws => ws.Id);
                    var webSourcesToParse = webSourcesToParseQuery.Skip(processed + failed).Take(1024).ToList();
                    if (webSourcesToParse.Count == 0) break;
                    Console.WriteLine("Processing...");
                    Parallel.ForEach(webSourcesToParse, new ParallelOptions {MaxDegreeOfParallelism = 256}, (webSource) =>
                                                                                                                {
                                                                                                                    /*
                                                                                                                    WebSource webSource;
                                                                                                                    lock (storage)
                                                                                                                    {
                                                                                                                        webSource = storage.modelContainer.GetWebSourceOf(info);
                                                                                                                    }*/
                                                                                                                    bool success;
                                                                                                                    try
                                                                                                                    {
                                                                                                                        processor.Process(webSource, storage);
                                                                                                                        processed++;
                                                                                                                        Console.WriteLine(string.Format("#{0} {3} {1}, failed {2}", processed, webSource.Url, failed, webSource.Id));
                                                                                                                        success = true;
                                                                                                                    }
                                                                                                                    catch (Exception exception)
                                                                                                                    {
                                                                                                                        Console.WriteLine(string.Format("FAILED #{0} {4} {1} {3}, failed {2}", processed, webSource.Url, failed, exception.Message,
                                                                                                                                                        webSource.Id));
                                                                                                                        failed++;
                                                                                                                        success = false;
                                                                                                                    }
                                                                                                                    lock (storage)
                                                                                                                    {
                                                                                                                        webSource.ResultStatus = success ? ProcessingResult.OK : ProcessingResult.Failed;
                                                                                                                        webSource.Processed = DateTime.Now;
                                                                                                                        if ((processed + failed)%16 == 0)
                                                                                                                        {
                                                                                                                            storage.modelContainer.SaveChanges();
                                                                                                                        }
                                                                                                                    }
                                                                                                                    //if (processed > 1000) break;
                                                                                                                });
                    Console.WriteLine("Saving...");
                    storage.modelContainer.SaveChanges();
                }
                Console.WriteLine("Avg throughput {0:0.#}, this round {1:0.#}", (failed + processed)/(DateTime.Now - startTime).TotalMilliseconds*1000.0,
                                  (failed + processed - roundStartCount)/(DateTime.Now - roundStartTime).TotalMilliseconds*1000.0);
            }
            Console.WriteLine(string.Format("Processed {0}, failed {1}", processed, failed));
        }
    }
}