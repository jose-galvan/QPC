using System.Web.Mvc;

namespace auth.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            RedirectToRoute("Help");
            return View();
        }
    }
}
