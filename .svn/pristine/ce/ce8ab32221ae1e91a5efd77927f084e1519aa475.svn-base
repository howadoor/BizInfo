using System.Web.Security;
using BizInfo.App.Services.Logging;

namespace BizInfo.WebApp.MVC3.Tools
{
    public static class LoggingTools
    {
        public static UserStartAndStopLog LogUserAndOperation(this object @object, string operationName)
        {
            return new UserStartAndStopLog(operationName);
        }

        public static void LogCurrentUser(this object @object)
        {
            var user = Membership.GetUser();
            @object.LogInfo(user == null ? "No user logged in" : string.Format("User {0} {1}", user.UserName, user.ProviderUserKey));
        }

        #region Nested type: UserStartAndStopLog

        public class UserStartAndStopLog : App.Services.Logging.LoggingTools.StartAndStopLog
        {
            protected internal UserStartAndStopLog(string operationName)
                : base(operationName)
            {
                this.LogCurrentUser();
            }

            public override void Dispose()
            {
                this.LogCurrentUser();
                base.Dispose();
            }
        }

        #endregion
    }
}