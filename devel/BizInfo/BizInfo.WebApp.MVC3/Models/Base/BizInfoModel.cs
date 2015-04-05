using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Security;
using BizInfo.Harvesting.Services.Processing.CompanyScoreTools;
using BizInfo.Model.Entities;

namespace BizInfo.WebApp.MVC3.Models.Base
{
    public class BizInfoModel
    {
        private static readonly CompanyScoreComputer computer = new CompanyScoreComputer();
        private Category category;
        private bool categoryFound;
        private bool isScheduledReloadFound;
        private ScheduledReload scheduledReload;

        /// <summary>
        /// Cached <see cref="UserAndInfo"/> found for <see cref="userIdFound"/>. Do not use directly, use <see cref="UserAndBizInfoForCurrentUser"/> instead.
        /// </summary>
        private UserAndInfo userAndBizInfoForCurrentUser;

        /// <summary>
        /// Identifier of the user for which was found current <see cref="userAndBizInfoForCurrentUser"/>. We cannot use
        /// <see cref="UserAndInfo.UserId"/> directly because <see cref="userAndBizInfoForCurrentUser"/> can be (and mostly is) <c>null</c>;
        /// </summary>
        private Guid userIdFound;

        private WebSource webSource;
        private bool webSourceFound;
        public Info BizInfo { get; set; }

        public BizInfoModelContainer Container { get; set; }

        public WebSource WebSource
        {
            get
            {
                if (!webSourceFound)
                {
                    webSource = Container.WebSourceSet.Where(ws => ws.Id == BizInfo.WebSourceId).FirstOrDefault();
                    webSourceFound = true;
                }
                return webSource;
            }
        }

        public string WebSourceUrl
        {
            get { return WebSource != null ? WebSource.Url : null; }
        }

        public Category Category
        {
            get
            {
                if (!categoryFound)
                {
                    category = BizInfo.NativeCategoryId.HasValue ? Container.GetCategory(BizInfo.NativeCategoryId.Value) : null;
                    categoryFound = true;
                }
                return category;
            }
        }

        public bool IsInfoImportantForCurrentUser
        {
            get
            {
                var userAndBizInfo = UserAndBizInfoForCurrentUser;
                if (userAndBizInfo == null) return false;
                return userAndBizInfo.IsImportant;
            }
            set
            {
                var user = Membership.GetUser();
                if (user == null) return;
                var userAndBizInfo = UserAndBizInfoForCurrentUser;
                if (userAndBizInfo == null)
                {
                    if (value == false) return;
                    userAndBizInfo = Container.UserAndInfoSet.CreateObject();
                    userAndBizInfo.InfoId = BizInfo.Id;
                    userAndBizInfo.UserId = (Guid) user.ProviderUserKey;
                    Container.UserAndInfoSet.AddObject(userAndBizInfo);
                }
                userAndBizInfo.IsImportant = value;
                if (userAndBizInfo.IsEmpty)
                {
                    Container.UserAndInfoSet.DeleteObject(userAndBizInfo);
                }
                Container.SaveChanges();
            }
        }

        public bool HasNoteOfCurrentUser
        {
            get { return !string.IsNullOrWhiteSpace(NoteOfCurrentUser); }
        }

        public string NoteOfCurrentUser
        {
            get
            {
                var userAndBizInfo = UserAndBizInfoForCurrentUser;
                if (userAndBizInfo == null) return null;
                return userAndBizInfo.Note;
            }
            set
            {
                var user = Membership.GetUser();
                if (user == null) return;
                var userAndBizInfo = UserAndBizInfoForCurrentUser;
                if (userAndBizInfo == null)
                {
                    if (string.IsNullOrWhiteSpace(value)) return;
                    userAndBizInfo = Container.UserAndInfoSet.CreateObject();
                    userAndBizInfo.InfoId = BizInfo.Id;
                    userAndBizInfo.UserId = (Guid) user.ProviderUserKey;
                    Container.UserAndInfoSet.AddObject(userAndBizInfo);
                }
                userAndBizInfo.Note = value.Trim();
                if (userAndBizInfo.IsEmpty)
                {
                    Container.UserAndInfoSet.DeleteObject(userAndBizInfo);
                }
                Container.SaveChanges();
            }
        }

        protected UserAndInfo UserAndBizInfoForCurrentUser
        {
            get
            {
                lock (this)
                {
                    var userId = (Guid) Membership.GetUser().ProviderUserKey;
                    if (!userIdFound.Equals(userId))
                    {
                        userAndBizInfoForCurrentUser = Container.UserAndInfoSet.Where(ubi => ubi.UserId == userId && ubi.InfoId == BizInfo.Id).FirstOrDefault();
                        userIdFound = userId;
                    }
                    return userAndBizInfoForCurrentUser;
                }
            }
        }

        public string DetailUrl
        {
            get { return string.Format("~/Base/Detail?id={0}", BizInfo.Id); }
        }

        public string SourceTagName
        {
            get { return BizInfo.SourceTag != null ? BizInfo.SourceTag.Name : null; }
        }

        public string VerbKindTagName
        {
            get { return BizInfo.VerbKindTag != null ? BizInfo.VerbKindTag.Name : null; }
        }

        public string VerbTagName
        {
            get { return BizInfo.VerbTag != null ? BizInfo.VerbTag.Name : null; }
        }

        public string DomainTagName
        {
            get { return BizInfo.DomainTag != null ? BizInfo.DomainTag.Name : null; }
        }

        /// <summary>
        /// Is <see cref="BizInfo"/> scheduled for reload?
        /// </summary>
        public bool IsScheduledForReload
        {
            get { return ScheduledReload != null; }
        }

        /// <summary>
        /// Instance of <see cref="ScheduledReload"/> if <see cref="BizInfo"/> is scheduled for realoading. <c>null</c> otherwise (is not scheduled).
        /// </summary>
        public ScheduledReload ScheduledReload
        {
            get
            {
                lock (this)
                {
                    if (!isScheduledReloadFound)
                    {
                        scheduledReload = Container.ScheduledReloadSet.Where(sr => sr.InfoId == BizInfo.Id).FirstOrDefault();
                        isScheduledReloadFound = true;
                    }
                    return scheduledReload;
                }
            }
        }

        public static BizInfoModel Create(Int64 bizInfoId)
        {
            var container = new BizInfoModelContainer();
            var bizInfo = container.InfoSet.Where(bi => bi.Id == bizInfoId).FirstOrDefault();
            if (bizInfo == null)
            {
                container.Dispose();
                return null;
            }
            return new BizInfoModel {BizInfo = bizInfo, Container = container};
        }

        public bool SwitchImportancyForCurrentUser()
        {
            var user = Membership.GetUser();
            if (user == null) return false;
            var userAndBizInfo = UserAndBizInfoForCurrentUser;
            if (userAndBizInfo == null)
            {
                userAndBizInfo = Container.UserAndInfoSet.CreateObject();
                userAndBizInfo.InfoId = BizInfo.Id;
                userAndBizInfo.UserId = (Guid) user.ProviderUserKey;
                Container.UserAndInfoSet.AddObject(userAndBizInfo);
            }
            userAndBizInfo.IsImportant = !userAndBizInfo.IsImportant;
            if (userAndBizInfo.IsEmpty)
            {
                Container.UserAndInfoSet.DeleteObject(userAndBizInfo);
            }
            Container.SaveChanges();
            return userAndBizInfo.IsImportant;
        }

        public MvcHtmlString CreateImportancyActionLink(AjaxHelper helper)
        {
            var target = new {InfoId = BizInfo.Id};
            string targetId;
            var content = GetImportancyActionLinkContent(out targetId);
            return MvcHtmlString.Create(helper.ActionLink("***", "Important", target, new AjaxOptions {UpdateTargetId = targetId}).ToString().Replace("***", content));
        }

        public string GetImportancyActionLinkContent(out string targetId)
        {
            targetId = string.Format("imp-{0}", BizInfo.Id);
            return string.Format("<span id=\"{0}\"><img src=\"{1}\" /></span>", targetId, VirtualPathUtility.ToAbsolute(IsInfoImportantForCurrentUser ? "~/Images/important_32.png" : "~/Images/not_important_32.png"));
        }

        public int GetCompanyProbability()
        {
            return (int) (computer.ComputeScore(BizInfo)*100 + 0.5);
        }
    }
}