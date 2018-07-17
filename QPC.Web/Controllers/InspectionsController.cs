using Microsoft.AspNet.Identity;
using QPC.Core.Repositories;
using System;
using System.Web.Mvc;

namespace QPC.Web.Controllers
{
    public class InspectionsController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public Func<Guid> GetUserId;

        public InspectionsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            GetUserId = () => ControllerHelpers.GetGuid(User.Identity.GetUserId());
        }
    }
}