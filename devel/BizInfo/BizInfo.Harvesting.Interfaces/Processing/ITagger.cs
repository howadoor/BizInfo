using BizInfo.Model.Entities;
using BizInfo.Model.Interfaces;

namespace BizInfo.Harvesting.Interfaces.Processing
{
    public interface ITagger
    {
        void Tag(IBizInfo bizInfo, IBizInfoStorage storage);
    }
}