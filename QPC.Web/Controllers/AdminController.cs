using System.Web.Mvc;
using QPC.Core.Repositories;
using QPC.Web.Helpers;

namespace QPC.Web.Controllers
{
    public class AdminController : QualityControlMvcController
    {
        public AdminController(IUnitOfWork unitOfWork, QualityControlFactory factory) : base(unitOfWork, factory)
        {

        }

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
    }
}