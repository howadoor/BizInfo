using System;
using System.Linq;
using System.Web.Security;
using BizInfo.Model.Entities;
using Perenis.Core.Serialization;

namespace BizInfo.WebApp.MVC3.Tools
{
    public static class LoggedTenant
    {
        public static SearchingCriteriaModel LastUsedSearchCriteria
        {
            get
            {
                using (var repository = new BizInfoModelContainer())
                {
                    var tenant = GetLoggedTenant(repository);
                    if (tenant == null) return null;
                    var scs = tenant.LastUsedSearchCriteria;
                    if (string.IsNullOrEmpty(scs)) return LastUsedSearchCriteria = SearchingCriteriaModel.CreateDefault();
                    return XmlSerialization.FromXmlString<SearchingCriteriaModel>(scs);
                }
            }
            set
            {
                using (var repository = new BizInfoModelContainer())
                {
                    var tenant = GetLoggedTenant(repository);
                    if (tenant == null) return;
                    tenant.LastUsedSearchCriteria = value != null ? value.ToXmlString() : null;
                    repository.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Returns id of the current tenant. If no tenant is logged in, fails
        /// </summary>
        public static Guid Id
        {
            get
            {
                var currentUser = Membership.GetUser();
                return (Guid) currentUser.ProviderUserKey;
            }
        }

        #region Rights

        /// <summary>
        /// Can user see last searches of other users?
        /// </summary>
        public static bool CanSeeLastSearches
        {
            get { return IsAdministrator; }
        }

        /// <summary>
        /// Is in role of administrator?
        /// </summary>
        /// TODO Do it somehow better
        public static bool IsAdministrator
        {
            get
            {
                var currentUser = Membership.GetUser();
                return currentUser != null && currentUser.UserName.Contains("Viktor");
            }
        }

        /// <summary>
        /// Can see messages sent to other users?
        /// </summary>
        public static bool CanSeeSentMessages
        {
            get { return IsAdministrator; }
        }

        #endregion

        /// <summary>
        /// Initializes values of properties in <see cref="tenant"/> from corresponding <see cref="membershipUser"/>.
        /// </summary>
        /// <param name="membershipUser">Source of the new tenant properties</param>
        /// <param name="tenant">Instance of <see cref="Tenant"/>. Its members will be setup from values of <see cref="membershipUser"/></param>
        private static void InitTenantFromMembershipUser(MembershipUser membershipUser, Tenant tenant)
        {
            var currentUserGuid = (Guid) membershipUser.ProviderUserKey;
            tenant.Uuid = currentUserGuid;
            tenant.Mail = membershipUser.Email;
            tenant.LastMailSentTime = DateTime.Now;
        }

        public static Tenant GetLoggedTenant(BizInfoModelContainer repository)
        {
            var membershipUser = Membership.GetUser();
            if (membershipUser == null) return null;
            var currentUserGuid = (Guid) membershipUser.ProviderUserKey;
            lock (repository)
            {
                var tenant = repository.TenantSet.SingleOrDefault(tnt => tnt.Uuid == currentUserGuid);
                if (tenant == null)
                {
                    tenant = repository.TenantSet.CreateObject();
                    InitTenantFromMembershipUser(membershipUser, tenant);
                    repository.TenantSet.AddObject(tenant);
                    repository.SaveChanges();
                }
                return tenant;
            }
        }
    }
}