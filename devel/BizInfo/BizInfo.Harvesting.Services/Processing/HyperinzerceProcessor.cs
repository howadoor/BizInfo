using System.IO;
using System.Text;
using BizInfo.Harvesting.Interfaces.Processing;
using BizInfo.Harvesting.Services.Core;
using BizInfo.Harvesting.Services.Storage;
using BizInfo.Model.Entities;

namespace BizInfo.Harvesting.Services.Processing
{
    public class HyperinzerceProcessor : CommonOffersProcessor
    {
        public HyperinzerceProcessor() : base(Encoding.GetEncoding("Windows-1250"))
        {
        }
    }
}