using System;
using System.Linq;
using BizInfo.Model.Entities;
using BizInfo.WebApp.MVC3.Models.User;
using BizInfo.WebApp.MVC3.Tools;
using Perenis.Core.Serialization;

namespace BizInfo.WebApp.MVC3.Logic
{
    /// <summary>
    /// Implementation of <see cref="IBussinesLogic"/>
    /// </summary>
    public class BussinesLogic : IBussinesLogic
    {
        #region IBussinesLogic Members

        public void StoreAndWatch(SearchingCriteriaModel searchCriteria, int watchId = -1, string watchName = null)
        {
            using (var repository = new BizInfoModelContainer())
            {
                var tenant = LoggedTenant.GetLoggedTenant(repository);
                if (tenant == null) throw new InvalidOperationException("No tenant is logged");
                var hash = XmlSerialization.HashToGuid(searchCriteria);
                var criteria = repository.SearchCriteriaSet.SingleOrDefault(crt => crt.Hash == hash);
                if (criteria == null)
                {
                    criteria = repository.SearchCriteriaSet.CreateObject();
                    criteria.Hash = hash;
                    criteria.Criteria = XmlSerialization.ToXmlString(searchCriteria);
                    criteria.LastUsedTime = DateTime.Now;
                    repository.SearchCriteriaSet.AddObject(criteria);
                }
                Watch watch;
                if (watchId >= 0)
                {
                    watch = repository.WatchSet.Single(w => w.Id == watchId);
                }
                else
                {
                    watch = repository.WatchSet.CreateObject();
                    watch.IsActive = true;
                    watch.Tenant = tenant;
                    repository.WatchSet.AddObject(watch);
                }
                if (watchName != null) watch.Name = watchName;
                watch.SearchCriteria = criteria;
                repository.SaveChanges();
            }
        }

        public TenantSettingsModel UpdateUserSettings(TenantSettingsModel settingsModel)
        {
            using (var repository = new BizInfoModelContainer())
            {
                var tenant = repository.TenantSet.Single(usr => usr.Id == settingsModel.UserId);
                tenant.Mail = settingsModel.Mail;
                tenant.MinimumIntervalOfMailInMinutes = settingsModel.MinimumIntervalOfMailInMinutes;
                tenant.IsListOfInfoSendingEnabled = settingsModel.IsListOfInfoSendingEnabled;
                repository.SaveChanges();
                return new TenantSettingsModel(tenant);
            }
        }

        public void UpdateWatch(long id, string name, bool isActive)
        {
            using (var repository = new BizInfoModelContainer())
            {
                var watch = repository.WatchSet.Single(w => w.Id == id);
                watch.Name = name;
                watch.IsActive = isActive;
                repository.SaveChanges();
            }
        }

        public void RemoveWatch(long id)
        {
            using (var repository = new BizInfoModelContainer())
            {
                var watch = repository.WatchSet.Single(w => w.Id == id);
                repository.WatchSet.DeleteObject(watch);
                repository.SaveChanges();
            }
        }

        #endregion
    }
}