using System;
using System.Linq;
using System.Web.Security;
using BizInfo.Model.Entities;

namespace BizInfo.WebApp.MVC3.Models.User
{
    /// <summary>
    /// Represents tenant settings model
    /// </summary>
    public class TenantSettingsModel
    {
        private readonly MembershipUser membershipUser;
        private Tenant tenant;

        public TenantSettingsModel(Tenant tenant)
        {
            membershipUser = Membership.GetUser(tenant.Uuid);
            this.tenant = tenant;
            UserId = tenant.Id;
            Name = string.Format(membershipUser.UserName);
            Mail = tenant.Mail;
            IsListOfInfoSendingEnabled = tenant.IsListOfInfoSendingEnabled;
            MinimumIntervalOfMailInMinutes = tenant.MinimumIntervalOfMailInMinutes;
            LastMailSentTime = tenant.LastMailSentTime;
            Watches = tenant.WatchSet.ToArray();
            SearchCriteria = Watches.Select(watch => watch.SearchCriteria).ToArray();
        }

        public TenantSettingsModel()
        {
        }

        /// <summary>
        /// Time when last message was sent to tenant
        /// </summary>
        public DateTime LastMailSentTime { get; set; }

        /// <summary>
        /// Minimum interval between mails in minutes
        /// </summary>
        public int MinimumIntervalOfMailInMinutes { get; set; }

        /// <summary>
        /// Can be informations sent to tenant aggregated in lists?
        /// </summary>
        public bool IsListOfInfoSendingEnabled { get; set; }

        /// <summary>
        /// Id of the tenant
        /// </summary>
        public long UserId { get; set; }

        // Full name of the tenant
        public string Name { get; set; }

        // Mail of the tenant
        public string Mail { get; set; }

        public Watch[] Watches { get; private set; }

        public SearchCriteria[] SearchCriteria { get; private set; }
    }
}