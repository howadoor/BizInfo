using System;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BizInfo.App.Services.Logging;
using BizInfo.Core.Services.WebResources.Storage;
using BizInfo.Harvesting.Interfaces.Processing;
using BizInfo.Harvesting.Services.Core;
using BizInfo.Harvesting.Services.Processing;
using BizInfo.Model.Entities;
using BizInfo.Model.Interfaces;

namespace BizInfo.Experiment
{
    internal class WebSourcesCopier : IWebSourceProcessor
    {
        WebResourcesRepository webSourceRepository = new WebResourcesRepository(@"c:\BizInfo.WebSources");

        public void Start()
        {
            new ProcessingManager().Process(this, (Expression<Func<WebSource, bool>>) null, new ParallelOptions { MaxDegreeOfParallelism = 1 });
        }

        public void Process(IWebSource webSource, IBizInfoStorage storage, UrlLoadOptions loadOptions)
        {
            try
            {
                if (webSourceRepository.ResourceExists(webSource.Id)) return;
                var request = new UrlDownloadRequest(webSource.Url) { CanLoadDirectly = false, CanLoadFromCache = true, CanStoreToCache = false };
                storage.Loader.Load(request);
                if (request.Content == null || !request.IsContentLoadedFromCache) return;
                var resourceProperties = new ResourceProperties();
                resourceProperties["MIME"] = "text/html";
                var resourceUrl = GetResourceUrl(webSource, ref resourceProperties);
                var resource = webSourceRepository.CreateResource(webSource.Id, resourceUrl, resourceProperties);
                using (var memoryStream = new MemoryStream(request.Content, 0, (int)request.ContentLength))
                {
                    resource.AddVersion(request.LoadTime, memoryStream);
                }
            }
            catch (Exception exception)
            {
                this.LogInfo(exception.ToString());                
                throw;
            }
        }

        private string GetResourceUrl(IWebSource webSource, ref ResourceProperties resourceProperties)
        {
            if (!webSource.Url.Contains(@"inzertexpres.cz")) return webSource.Url;
            var refIndex = webSource.Url.IndexOf(@"ref=");
            if (refIndex < 0) return webSource.Url;
            var scoutInfo = webSource.Url.Substring(refIndex);
            var url = webSource.Url.Substring(0, (webSource.Url[refIndex - 1] == '?') ? refIndex - 1 : refIndex);
            resourceProperties["ScoutInfo"] = scoutInfo;
            return url;
        }
    }
}