using BizInfo.Model.Interfaces;

namespace BizInfo.Harvesting.Services.Processing.CompanyScoreTools
{
    public interface ICompanyScoreComputer
    {
        double ComputeScore(IBizInfo bizInfo);
    }
}