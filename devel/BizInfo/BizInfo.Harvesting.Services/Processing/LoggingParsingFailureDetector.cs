using System;
using System.IO;
using BizInfo.App.Services.Tools;
using BizInfo.Model.Interfaces;
using HtmlAgilityPack;

namespace BizInfo.Harvesting.Services.Processing
{
    public class LoggingParsingFailureDetector : IParsingFailureDetector
    {
        public ParsingFailureReason DetectParsingFailure(IWebSource webSource, IBizInfoStorage storage, HtmlDocument htmlDocument, Exception exception)
        {
            if (exception == null) return ParsingFailureReason.Unknown;
            var filename = string.Format(@"c:\projects\BizInfo.ParsingFailures\{0} [{1}]", webSource.Id, UrlTools.GetHost(webSource.Url));
            using (var logFile = File.OpenWrite(filename + ".exception.txt"))
            {
                using (var logStream = new StreamWriter(logFile))
                {
                    logStream.WriteLine("Url {0}", webSource.Url);
                    logStream.WriteLine("---");
                    logStream.Write(exception.ToString());
                }
            }
            htmlDocument.Save(filename + ".html");
            return ParsingFailureReason.Unknown;
        }
    }
}