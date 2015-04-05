using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizInfo.App.Services.Logging;
using BizInfo.Harvesting.Services.Core;
using BizInfo.Harvesting.Services.Messaging;
using BizInfo.Harvesting.Services.Processing;
using BizInfo.Harvesting.Services.Processing.Helpers;
using BizInfo.Harvesting.Services.SpecialServices;
using BizInfo.Harvesting.Services.Storage;
using BizInfo.Model.Entities;
using BizInfo.Model.Interfaces;
using Perenis.Core.Serialization;

namespace BizInfo.Experiment
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // new OtazkyBystrc().CreateStatistics().Save(@"c:\otazky.txt");
            // Adresy.Fix();
            // CanonicalizeInfos();
            // HashInfos();
            // RemoveSameInfos();
            // Console.WriteLine("Program ends");
            // Console.ReadLine();
            // return;
            try
            {
                // SendSampleMail();
                // return;
                // new WebSourcesCopier().Start();
                // SetFailedSourcesAsNotProcessed();

                // start database maintenance
                var maintainer = new DbMaintainer();
                // start regular harvester
                var harvester = new Harvester();
                // start mailing
                var newItemsSender = new NewItemsSender();


                /*
                // create new mails lists
                var processingManager = new ProcessingManager();
                using (var mailExtractor = new MailExtractor())
                {
                    processingManager.Process(mailExtractor.Process);
                    mailExtractor.SaveLists(@"c:\");
                }*/
                Console.ReadLine();
            }
            catch (Exception exception)
            {
                exception.LogException(exception);
                exception.LogInfo(exception.ToString());
                throw;
            }
        }

        private static void RemoveSameInfos()
        {
            var manager = new ProcessingManager();
            manager.Process(RemoveSameInfos);
        }

        private static void RemoveSameInfos(IBizInfo info, IBizInfoStorage storage)
        {
            var currentId = ((Info) info).Id;
            var infoWithSameContent = ((EntityStorage)storage).modelContainer.InfoSet.FirstOrDefault(infoCandidate => infoCandidate.ContentHash == info.ContentHash && infoCandidate.Id != currentId);
            if (infoWithSameContent != null)
            {
                removed++;
                Console.WriteLine("{4}: Info #{0} '{1}' has the same content as #{2} '{3}'", ((Info)info).Id, info.Summary, infoWithSameContent.Id, infoWithSameContent.Summary, removed);
                ((EntityStorage)storage).modelContainer.InfoSet.DeleteObject((Info) info);
            }
        }

        private static void CanonicalizeInfos()
        {
            var manager = new ProcessingManager();
            manager.Process(CanonicalizeInfo);
        }

        private static void CanonicalizeInfo(IBizInfo info, IBizInfoStorage storage)
        {
            var summary = Canonicalization.CanonicalizeSummary(info.Summary);
            var text = Canonicalization.CanonicalizeText(info.Text);
            if (!summary.Equals(info.Summary))
            {
                Console.WriteLine("*** Canonicalized summary of {0}", ((Info)info).Id);
                Console.WriteLine("Old: '{0}'", info.Summary);
                Console.WriteLine("New: '{0}'", summary);
                info.Summary = summary;
            }
            if (!text.Equals(info.Text))
            {
                Console.WriteLine("*** Canonicalized text of {0}", ((Info)info).Id);
                Console.WriteLine("Old: '{0}'", info.Text);
                Console.WriteLine("New: '{0}'", text);
                info.Text = text;
            }
        }


        private static void HashInfos()
        {
            var manager = new ProcessingManager();
            manager.Process(HashInfo, i => i.ContentHash == Guid.Empty);
        }

        private static int tests = 0;
        private static int duplicates = 0;
        private static int removed = 0;

        private static void HashInfo(IBizInfo info, IBizInfoStorage storage)
        {
            info.ContentHash = info.ComputeContentHash();
            tests++;
            if (tests % 1000 == 0) Console.WriteLine("Hashed {0} info", tests);
        }

        private static void TruncateInfos()
        {
            var manager = new ProcessingManager();
            manager.Process(TruncateInfo);
        }

        public static void TruncateInfo(IBizInfo bizInfo, IBizInfoStorage storage)
        {
            if (bizInfo.Summary.Length > 160)
            {
                Console.WriteLine("Truncating summary of info {0}, original length {1}", ((Info) bizInfo).Id, bizInfo.Summary.Length);
                bizInfo.Summary = bizInfo.Summary.Substring(0, 160);
            }
            if (bizInfo.Text.Length > 1536)
            {
                Console.WriteLine("Truncating text of info {0}, original length {1}", ((Info)bizInfo).Id, bizInfo.Text.Length);
                bizInfo.Text = bizInfo.Text.Substring(0, 1536);
            }
        }

        private static void SetFailedSourcesAsNotProcessed()
        {
            using (var repository = new BizInfoModelContainer())
            {
                foreach (var ws in repository.WebSourceSet.Where(w => /*w.Processed.HasValue && w.ProcessingResult < 0 &&*/ w.Url.Contains("bezrealitky.cz")))
                {
                    ws.Processed = null;
                    ws.ProcessingResult = 0;
                }
                repository.SaveChanges();
            }
        }

        private static void SendSampleMail()
        {
            using (var repository = new BizInfoModelContainer())
            {
                var user = repository.TenantSet.SingleOrDefault(usr => usr.Mail.Contains("vi.ki"));
                if (user == null)
                {
                    user = new Tenant() {Uuid = Guid.NewGuid(), IsListOfInfoSendingEnabled = true, LastInfoSentId = 0, Mail = "vi.ki@tiscali.cz", MinimumIntervalOfMailInMinutes = 0};
                }
                var info = repository.InfoSet.OrderByDescending(bi => bi.Id).Take(5);
                var sender = new MailMessageSender {MessageCreator = new MessageCreator()};
                sender.SendInfo(user, info);
            }
        }
    }

    internal class Adresy
    {
        public static void Fix()
        {
            var addresses = ReadLines(@"c:\maily_krap32.txt").Select(line => line.ToLower()).Distinct().ToList();
            addresses.Sort();
            using (var sw = new StreamWriter(@"c:\maily_krap32_seznam.txt"))
            {
                foreach(var address in addresses) sw.WriteLine(address);
            }
        }

        private static IEnumerable<string> ReadLines(string filename)
        {
// Create an instance of StreamReader to read from a file.
            // The using statement also closes the StreamReader.
            using (StreamReader sr = new StreamReader(filename))
            {
                String line;
                // Read and display lines from the file until the end of
                // the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }
    }
}
