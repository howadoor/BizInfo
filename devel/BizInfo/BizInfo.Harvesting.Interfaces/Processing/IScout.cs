using System;
using BizInfo.Model.Interfaces;

namespace BizInfo.Harvesting.Interfaces.Processing
{
    /// <summary>
    /// Performs scouting = discovering of new informations
    /// </summary>
    public interface IScout
    {
        /// <summary>
        /// Performs scouting = discovering of new informations
        /// </summary>
        /// <param name="storage">Storage used for scouting</param>
        /// <param name="urlAcceptor">Function for accepting scouted URLs. If returns <c>true</c>, scouting continues, otherwise no more scouting is
        /// performed and method returns.</param>
        void Scout(IBizInfoStorage storage, Func<string, IBizInfoStorage, bool> urlAcceptor);
    }
}

