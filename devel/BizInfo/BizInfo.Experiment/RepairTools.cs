using System;
using System.Linq;
using System.Threading.Tasks;
using BizInfo.Model.Entities;

namespace BizInfo.Experiment
{
    public class RepairTools
    {
        public static void RepairDownloadedBlobs()
        {
            long[] dsIds;
            using (var localContainer = new BizInfoModelContainer())
            {
                dsIds = localContainer.DownloadedBlobSet.Select(db => db.Id).ToArray();
            }
            long added = 0;
            var i = 0;
            using (var masterContainer = new BizInfoModelContainer("name=BizInfoOnMaster"))
            {
                Parallel.ForEach(dsIds, new ParallelOptions{MaxDegreeOfParallelism = 1}, dsId =>
                                            {
                                                using (var localContainer = new BizInfoModelContainer())
                                                {
                                                    var localDownloaded = localContainer.DownloadedBlobSet.Where(db => db.Id == dsId).First();
                                                    var downloadedOnMaster = masterContainer.DownloadedBlobSet.Where(db => db.SourceUrl == localDownloaded.SourceUrl).FirstOrDefault();
                                                    if (downloadedOnMaster == null)
                                                    {
                                                        localContainer.DownloadedBlobSet.Detach(localDownloaded);
                                                        masterContainer.DownloadedBlobSet.Attach(localDownloaded);
                                                        Console.WriteLine("Added #{0} {1} {2}", added, localDownloaded.Id, localDownloaded.SourceUrl);
                                                        added++;
                                                    }
                                                    else
                                                    {
                                                        if (downloadedOnMaster.Id != localDownloaded.Id)
                                                        {
                                                            var q = 11;
                                                        }
                                                        downloadedOnMaster.DownloadDate = localDownloaded.DownloadDate;
                                                    }
                                                    i++;
                                                    if (i%1000 == 0) Console.WriteLine(i);
                                                }
                                            });
                masterContainer.SaveChanges();
            }
        }
        
        public static void RepairWebsources()
        {
            long[] dsIds;
            using (var localContainer = new BizInfoModelContainer())
            {
                dsIds = localContainer.WebSourceSet.Select(db => db.Id).ToArray();
            }
            long added = 0;
            var i = 0;
            using (var masterContainer = new BizInfoModelContainer("name=BizInfoOnMaster"))
            {
                Parallel.ForEach(dsIds, new ParallelOptions { MaxDegreeOfParallelism = 1 }, dsId =>
                {
                    using (var localContainer = new BizInfoModelContainer())
                    {
                        var localDownloaded = localContainer.WebSourceSet.Where(db => db.Id == dsId).First();
                        var downloadedOnMaster = masterContainer.WebSourceSet.Where(db => db.Url == localDownloaded.Url).FirstOrDefault();
                        if (downloadedOnMaster == null)
                        {
                            localContainer.WebSourceSet.Detach(localDownloaded);
                            masterContainer.WebSourceSet.Attach(localDownloaded);
                            Console.WriteLine("Added #{0} {1} {2}", added, localDownloaded.Id, localDownloaded.Url);
                            added++;
                        }
                        i++;
                        if (i % 1000 == 0) Console.WriteLine(i);
                    }
                });
                masterContainer.SaveChanges();
            }
        }
        
        public static void RepairInfos()
        {
            long[] dsIds;
            using (var localContainer = new BizInfoModelContainer())
            {
                dsIds = localContainer.InfoSet.Select(db => db.Id).ToArray();
            }
            long added = 0;
            var i = 0;
            using (var masterContainer = new BizInfoModelContainer("name=BizInfoOnMaster"))
            {
                Parallel.ForEach(dsIds, new ParallelOptions { MaxDegreeOfParallelism = 1 }, dsId =>
                {
                    using (var localContainer = new BizInfoModelContainer())
                    {
                        var localDownloaded = localContainer.InfoSet.Where(db => db.Id == dsId).First();
                        var downloadedOnMaster = masterContainer.InfoSet.Where(db => db.WebSourceId == localDownloaded.WebSourceId).FirstOrDefault();
                        if (downloadedOnMaster == null)
                        {
                            localContainer.InfoSet.Detach(localDownloaded);
                            masterContainer.InfoSet.Attach(localDownloaded);
                            Console.WriteLine("Added #{0} {1} {2}", added, localDownloaded.Id, localDownloaded.Summary);
                            added++;
                        }
                        i++;
                        if (i % 1000 == 0) Console.WriteLine(i);
                    }
                });
                masterContainer.SaveChanges();
            }
        }
    }
}