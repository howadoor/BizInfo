using System;
using System.Web.Mvc;
using BizInfo.Model.Entities;
using BizInfo.WebApp.MVC3.Logic;
using BizInfo.WebApp.MVC3.Models.User;
using BizInfo.WebApp.MVC3.Tools;

namespace BizInfo.WebApp.MVC3.Controllers
{
    /// <summary>
    /// User-related settings
    /// </summary>
    public class UserController : Controller
    {
        private readonly IBussinesLogic logic = new BussinesLogic();

        public ActionResult Index()
        {
            // Clearing model state is necessary otherwise wrong values are rendered in partial views
            ModelState.Clear();
            using (this.LogUserAndOperation("User/Index"))
            {
                using (var repository = new BizInfoModelContainer())
                {
                    ViewBag.Message = "Uživatelská nastavení";
                    return View(new TenantSettingsModel(LoggedTenant.GetLoggedTenant(repository)));
                }
            }
        }

        public ActionResult UpdateUserSettings(TenantSettingsModel model)
        {
            // Clearing model state is necessary otherwise wrong values are rendered in partial views
            ModelState.Clear();
            using (this.LogUserAndOperation("User/UpdateCurrentSettings"))
            {
                ViewBag.Message = "Uživatelská nastavení";
                return View("Index", logic.UpdateUserSettings(model));
            }
        }

        public ActionResult UpdateWatch(long id, string name, bool isActive, string store, string remove)
        {
            // Clearing model state is necessary otherwise wrong values are rendered in partial views
            ModelState.Clear();
            using (this.LogUserAndOperation(string.Format("User/UpdateWatch id={0} name={1} isActive={2} store={3} removae={4}", id, name, isActive, store, remove)))
            {
                if (store != null)
                {
                    using (var repository = new BizInfoModelContainer())
                    {
                        logic.UpdateWatch(id, name, isActive);
                        ViewBag.Message = "Uživatelská nastavení";
                        return View("Index", new TenantSettingsModel(LoggedTenant.GetLoggedTenant(repository)));
                    }
                }
                if (remove != null)
                {
                    using (var repository = new BizInfoModelContainer())
                    {
                        logic.RemoveWatch(id);
                        ViewBag.Message = "Uživatelská nastavení";
                        return View("Index", new TenantSettingsModel(LoggedTenant.GetLoggedTenant(repository)));
                    }
                }
                throw new ArgumentException("At least one of store and remove arguments must be non-null");
            }
        }
    }
}