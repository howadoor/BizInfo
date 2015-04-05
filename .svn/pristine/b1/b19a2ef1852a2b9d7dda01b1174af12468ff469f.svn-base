using System.Web.Security;
using BizInfo.Model.Entities;

namespace BizInfo.WebApp.MVC3.Tools
{
    public static class SearchingLogEx
    {
        public static string GetUserName(this SearchingLog searchingLog)
        {
            var user = GetMembershipUser(searchingLog);
            return user != null ? user.UserName : string.Empty;
        }

        public static MembershipUser GetMembershipUser(this SearchingLog searchingLog)
        {
            return Membership.GetUser(searchingLog.UserId);
        }

        public static string GetSearchUrl(this SearchingLog searchingLog)
        {
            return string.Format("~/Base/SearchingCriteria?id={0}", searchingLog.SearchCriteriaId);
        }

        public static string GetCriteriaDisplayString(this SearchingLog searchingLog)
        {
            var criteria = GetCriteria(searchingLog);
            return criteria != null ? criteria.GetDisplayString() : string.Empty;
        }

        public static SearchCriteria GetCriteria(this SearchingLog searchingLog)
        {
            return SearchCriteriaEx.GetSearchCriteria(searchingLog.SearchCriteriaId);
        }
    }
}