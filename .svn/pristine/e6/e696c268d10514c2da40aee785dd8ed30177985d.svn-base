using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Reflection;
using System.Security;
using System.Security.Principal;
using System.Threading;

namespace Perenis.Core.Exceptions
{
    /// <summary>
    /// Provides common helper methods for exception formatters.
    /// </summary>
    public abstract class ExceptionFormatterBase
    {
        protected static NameValueCollection GetAdditionalInfo()
        {
            var additionalInfo = new NameValueCollection();
            additionalInfo.Add("MachineName", GetMachineName());
            additionalInfo.Add("TimeStamp", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fffzzz", CultureInfo.CurrentCulture));
            additionalInfo.Add("FullName", Assembly.GetExecutingAssembly().FullName);
            additionalInfo.Add("AppDomainName", AppDomain.CurrentDomain.FriendlyName);
            additionalInfo.Add("ThreadIdentity", Thread.CurrentPrincipal.Identity.Name);
            additionalInfo.Add("WindowsIdentity", GetWindowsIdentity());
            return additionalInfo;
        }

        protected static string GetMachineName()
        {
            try
            {
                return Environment.MachineName;
            }
            catch (SecurityException)
            {
                return "N/A Permission Denied";
            }
        }

        protected static string GetWindowsIdentity()
        {
            try
            {
                return WindowsIdentity.GetCurrent().Name;
            }
            catch (SecurityException)
            {
                return "N/A Permission Denied";
            }
        }
    }
}