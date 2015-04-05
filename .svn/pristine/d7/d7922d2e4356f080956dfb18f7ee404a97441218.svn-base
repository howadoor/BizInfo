using System;
using BizInfo.App.Services.Logging;
using BizInfo.Harvesting.Services.Storage;
using BizInfo.Model.Interfaces;

namespace BizInfo.Harvesting.Services.Scouting
{
    public class ScoutingAcceptor
    {
        private int notAcceptedCount;
        private string lastNotAcceptedUrl;
        private int acceptedCount;
        private string lastAcceptedUrl;

        public string LastAcceptedUrl
        {
            get { return lastAcceptedUrl; }
        }

        public string LastNotAcceptedUrl
        {
            get { return lastNotAcceptedUrl; }
        }

        public int AcceptedCount
        {
            get { return acceptedCount; }
        }

        public int NotAcceptedCount
        {
            get { return notAcceptedCount; }
        }

        public int MaximumOldUrlsInLine { get; set; }

        private int failsCounter = 0;
        
        public bool Accept(string url, IBizInfoStorage storage)
        {
            var added = storage.TryAddAssScouted(url);
            if (added)
            {
                Console.WriteLine("Found new info {0}", url);
                this.Log(string.Format("Accepted {0}", url));
                failsCounter = 0;
                acceptedCount++;
                lastAcceptedUrl = url;
                return true;
            }
            notAcceptedCount++;
            lastNotAcceptedUrl = url;
            failsCounter++;
            if (MaximumOldUrlsInLine > 0 && failsCounter > MaximumOldUrlsInLine)
            {
                // this.LogInfo(string.Format("Not accepted {0}, maximum {1} not accepted urls exceeded", url, MaximumOldUrlsInLine));
                return false;
            }
            // this.LogInfo(string.Format("Not accepted {0}, {1} not accepted in line", url, failsCounter));
            return true;
        }
    }
}