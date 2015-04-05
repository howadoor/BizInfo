using System.Collections.Specialized;
using BizInfo.Model.Interfaces;

namespace BizInfo.Harvesting.Interfaces.Processing
{
    /// <summary>
    /// Zpracuje objevený zdroj
    /// </summary>
    public interface IWebSourceProcessor
    {
        /// <summary>
        /// Loads web source document using <see cref="UrlLoadOptions"/> form <see cref="loadOptions"/> argument
        /// </summary>
        /// <param name="webSource"></param>
        /// <param name="storage"></param>
        /// <param name="loadOptions"></param>
        void Process(IWebSource webSource, IBizInfoStorage storage, UrlLoadOptions loadOptions = UrlLoadOptions.LoadFromCacheIfPossible | UrlLoadOptions.StoreToCacheWhenLoaded);
    }
}
