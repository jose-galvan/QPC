using Microsoft.AspNet.Identity;
using QPC.Core.Models;
using QPC.Core.Repositories;
using QPC.Core.ViewModels;
using QPC.Web.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QPC.Web.Controllers
{
    [Authorize]
    public class InspectionsController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public Func<Guid> GetUserId;

        public InspectionsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            GetUserId = () => ControllerHelpers.GetGuid(User.Identity.GetUserId());
        }

        [HttpGet]
        public async Task<ActionResult> Inspect(int id)
        {
            var control = await _unitOfWork.QualityControlRepository.GetWithDetails(id);

            if (control == null)
                return HttpNotFound();

            var vm = new InspectionViewModel
            {
                QualityControlId = id, 
                Desicions = await _unitOfWork.DesicionRepository.GetAllAsync()
            };

            if(control.Inspection != null)
            {
                vm.Comments = control.Inspection.Comments;
                vm.FinalDesicison = control.Inspection.Desicion.Id;
                vm.CanInspect = control.Instructions.Any(i => i.Status == InstructionStatus.Pending);
            }

            return View(vm);
        }

        [HttpPost]
        public async Task<ActionResult> Inspect(InspectionViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Data is invalid");
                return View(vm);
            }

            var control = await _unitOfWork.QualityControlRepository.GetWithDetails(vm.QualityControlId);
            if (control == null)
                return HttpNotFound();

            try
            {
                vm.Desicions = await _unitOfWork.DesicionRepository.GetAllAsync();
                var user = await _unitOfWork.UserRepository.FindByIdAsync(GetUserId());
                var inspection = QualityControlFactory.Create(vm);
                control.SetInspection(inspection, user);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex);
            }
            return View(vm);
        }

    }
}