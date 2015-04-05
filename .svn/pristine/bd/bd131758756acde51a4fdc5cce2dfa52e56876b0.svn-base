using BizInfo.Model.Entities;
using BizInfo.WebApp.MVC3.Models.User;

namespace BizInfo.WebApp.MVC3.Logic
{
    /// <summary>
    /// Bussines logic of the web application
    /// </summary>
    public interface IBussinesLogic
    {
        /// <summary>
        /// Stores <see cref="searchCriteria"/> and sets watch on it (messages will be send to user if new info is found)
        /// </summary>
        /// <param name="searchCriteria">Search criteria to be stored</param>
        /// <param name="watchId">Identifier of the watch. If set to value greater than 0, existing watch with this id is overriden, otherwise new watch is created</param>
        /// <param name="watchName">Name of the watch. If <c>nulll</c>, watch name remains untouched.</param>
        void StoreAndWatch(SearchingCriteriaModel searchCriteria, int watchId = -1, string watchName = null);

        TenantSettingsModel UpdateUserSettings(TenantSettingsModel settingsModel);
        
        /// <summary>
        /// Updates watch identified by <see cref="id"/>. Sets its <see cref="Watch.Name"/> to <see cref="name"/> and <see cref="Watch.IsActive"/> to <see cref="isActive"/>.
        /// </summary>
        /// <param name="id">Identifier of the watch</param>
        /// <param name="name">New name of the watch</param>
        /// <param name="isActive">New value of activity</param>
        void UpdateWatch(long id, string name, bool isActive);

        /// <summary>
        /// Removes watch identified by <see cref="id"/>
        /// </summary>
        /// <param name="id">Identifier of the watch</param>
        void RemoveWatch(long id);
    }
}