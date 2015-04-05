using System;
using System.Collections.Generic;
using BizInfo.Model.Entities;

namespace BizInfo.Model.Interfaces
{
    public interface IWebSourceStorage
    {
        IEnumerable<IWebSource> Sources { get; }
    }

    public interface IBizInfoStorage : IDisposable
    {
        IEnumerable<IBizInfo> Infos { get; }
        IBizInfo CreateInfo(IWebSource webSource, string summary, string text, DateTime creationTime);
        int GetCategory(string name, int? parentCategory);
        IUrlDownloader Loader { get; }
        bool TryAddAssScouted(string url);
        Tag GetTag(string name);
        
        /// <summary>
        /// Delete <see cref="webSource"/> and all associated resources (cached files)
        /// </summary>
        /// <param name="webSource"></param>
        void Delete(IWebSource webSource);

        /// <summary>
        /// Schedules reload of websources constituing <see cref="info"/> after <see cref="reloadDate"/>
        /// </summary>
        /// <param name="info"></param>
        /// <param name="value"></param>
        void ScheduleReload(IBizInfo info, DateTime reloadDate);
    }
}