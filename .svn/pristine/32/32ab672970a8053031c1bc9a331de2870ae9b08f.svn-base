using System.Web.Mvc;
using BizInfo.WebApp.MVC3.Tools;

namespace BizInfo.WebApp.MVC3.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (this.LogUserAndOperation("Home/Index"))
            {
                ViewBag.Message = "Welcome to ASP.NET MVC!";
                return View();
            }
        }

        public ActionResult About()
        {
            using (this.LogUserAndOperation("Home/About"))
            {
                return View();
            }
        }

        public ActionResult Base()
        {
            using (this.LogUserAndOperation("Home/Base"))
            {
                return View("../Base/Index", LoggedTenant.LastUsedSearchCriteria);
            }
        }
    }
}