using System;
using System.Web.Mvc;
using BizInfo.Model.Entities;
using BizInfo.WebApp.MVC3.Logic;
using BizInfo.WebApp.MVC3.Models.Base;
using BizInfo.WebApp.MVC3.Models.Common;
using BizInfo.WebApp.MVC3.Tools;

namespace BizInfo.WebApp.MVC3.Controllers
{
    public class BaseController : Controller
    {
        private readonly IBussinesLogic logic = new BussinesLogic();

        public ActionResult Index()
        {
            using (this.LogUserAndOperation("Base/Index"))
            {
                return View(LoggedTenant.LastUsedSearchCriteria);
            }
        }

        public ActionResult BaseView()
        {
            using (this.LogUserAndOperation("Base/BaseView"))
            {
                return View(LoggedTenant.LastUsedSearchCriteria);
            }
        }

        public ActionResult Search(SearchingCriteriaModel searchingCriteria, string searchButton, string storeAndWatchButton, int newWatchId, string newWatchName)
        {
            try
            {
                if (searchButton != null)
                {
                    return Search(searchingCriteria);
                }
                if (storeAndWatchButton != null)
                {
                    return StoreAndWatch(searchingCriteria, newWatchId, newWatchName);
                }
                throw new ArgumentException("At least one of buttons have to be not null");
            }
            catch (Exception exception)
            {
                return Request.IsAjaxRequest() ? (ActionResult) PartialView("ExceptionView", exception) : View("ExceptionView", exception);
            }
        }

        public ActionResult StoreWatch()
        {
            using (var repository = new BizInfoModelContainer())
            {
                return PartialView("StoreWatchView", new StoreWatchModel(LoggedTenant.GetLoggedTenant(repository)));
            }
        }

        /// <summary>
        /// Returns view with watched searching criteria
        /// </summary>
        /// <returns></returns>
        public ActionResult Watched()
        {
            using (this.LogUserAndOperation("Base/Watched"))
            {
                // var searchingCriteria = LoggedTenant.WatchedSearchCriteria ?? SearchingCriteriaModel.CreateDefault();
                // return View("Index", searchingCriteria);
                throw new NotImplementedException();
            }
        }

        private ActionResult StoreAndWatch(SearchingCriteriaModel searchingCriteria, int newWatchId, string newWatchName)
        {
            using (this.LogUserAndOperation(string.Format("Base/StoreAndWatch {0}", searchingCriteria)))
            {
                logic.StoreAndWatch(searchingCriteria, newWatchId, newWatchId >= 0 ? null : newWatchName);
                return Request.IsAjaxRequest() ? (ActionResult) PartialView("BaseView", searchingCriteria) : View("BaseView", searchingCriteria);
            }
        }

        private ActionResult Search(SearchingCriteriaModel searchingCriteria)
        {
            using (this.LogUserAndOperation(string.Format("Base/Search {0}", searchingCriteria)))
            {
                return Request.IsAjaxRequest() ? (ActionResult) PartialView("BaseView", searchingCriteria) : View("BaseView", searchingCriteria);
            }
        }

        public ActionResult SearchingCriteria(long id)
        {
            using (this.LogUserAndOperation(string.Format("Base/SearchingCriteria {0}", id)))
            {
                var criteria = SearchCriteriaEx.GetSearchCriteria(id);
                return View("Index", criteria.GetCriteriaModel());
            }
        }

        public ActionResult Detail(long id)
        {
            using (this.LogUserAndOperation(string.Format("Base/Detail {0}", id)))
            {
                var model = BizInfoModel.Create(id);
                return View("DetailView", model);
            }
        }

        public ActionResult GoTo(PagingModel pagingModel)
        {
            using (this.LogUserAndOperation(string.Format("Base/GoTo {0}", pagingModel)))
            {
                return PartialView("PagingView", pagingModel);
            }
        }

        public ActionResult BriefStatus()
        {
            using (this.LogUserAndOperation("Base/BriefStatus"))
            {
                return Request.IsAjaxRequest() ? (ActionResult) PartialView("BriefStatusView", BaseInfoModel.Default) : View("BriefStatusView", BaseInfoModel.Default);
            }
        }

        public ActionResult BaseInfo()
        {
            using (this.LogUserAndOperation("Base/BaseInfo"))
            {
                return Request.IsAjaxRequest() ? (ActionResult) PartialView("BaseInfoView", BaseInfoModel.Default) : View("BaseInfoView", BaseInfoModel.Default);
            }
        }

        public ActionResult SourceStatistics()
        {
            using (this.LogUserAndOperation("Base/SourceStatistics"))
            {
                return Request.IsAjaxRequest() ? (ActionResult) PartialView("SourceStatisticsView", BaseInfoModel.Default) : View("SourceStatisticsView", BaseInfoModel.Default);
            }
        }

        public ActionResult SearchingStatistics()
        {
            using (this.LogUserAndOperation("Base/SourceStatistics"))
            {
                return Request.IsAjaxRequest() ? (ActionResult) PartialView("SearchingStatisticsView", BaseInfoModel.Default) : View("SearchingStatisticsView", BaseInfoModel.Default);
            }
        }

        public ActionResult SearchingMonitor()
        {
            using (this.LogUserAndOperation("Base/SearchingMonitor"))
            {
                return Request.IsAjaxRequest() ? (ActionResult) PartialView("SearchingMonitorView", new SearchingMonitorModel()) : View("SearchingMonitorView", new SearchingMonitorModel());
            }
        }

        public ActionResult MailingMonitor()
        {
            using (this.LogUserAndOperation("Base/SearchingMonitor"))
            {
                return Request.IsAjaxRequest() ? (ActionResult) PartialView("MailingMonitorView", new MailingMonitorModel()) : View("MailingMonitorView", new MailingMonitorModel());
            }
        }

        public MvcHtmlString Important(Int64 infoId)
        {
            using (this.LogUserAndOperation(string.Format("Base/Important {0}", infoId)))
            {
                var bizInfo = BizInfoModel.Create(infoId);
                if (bizInfo == null) return null;
                bizInfo.SwitchImportancyForCurrentUser();
                string targetId;
                return MvcHtmlString.Create(bizInfo.GetImportancyActionLinkContent(out targetId));
            }
        }

        public MvcHtmlString EditNote(long infoId, string note)
        {
            using (this.LogUserAndOperation(string.Format("Base/EditNote {0} {1}", infoId, note)))
            {
                var bizInfo = BizInfoModel.Create(infoId);
                if (bizInfo == null) return MvcHtmlString.Create("Informace neexistuje");
                bizInfo.NoteOfCurrentUser = note;
                return MvcHtmlString.Create("Poznámka byla uložena");
            }
        }
    }
}