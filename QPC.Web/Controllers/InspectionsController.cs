using QPC.Core.Models;
using QPC.Core.Repositories;
using QPC.Core.ViewModels;
using QPC.Web.Helpers;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QPC.Web.Controllers
{
    [Authorize]
    public class InspectionsController : QualityControlMvcController
    {

        public InspectionsController(IUnitOfWork unitOfWork, QualityControlFactory factory)
            :base(unitOfWork, factory)
        {
        }

        [HttpGet]
        public async Task<ActionResult> Inspect(int? id)
        {
            if(!id.HasValue)
                return RedirectToAction("Index", "QualityControl");

            var control = await _unitOfWork.QualityControlRepository.GetWithDetailsAsync(id.Value);

            if (control == null)
                return HttpNotFound();

            var vm = new InspectionViewModel
            {
                QualityControlId = id.Value, 
                Desicions = await _unitOfWork.DesicionRepository.GetAllAsync(),
                CanSave = !control.Instructions.Any(i => i.Status == InstructionStatus.Pending)
            };

            if(control.Inspection != null)
            {
                vm.Comments = control.Inspection.Comments;
                vm.FinalDesicison = control.Inspection.Desicion.Id;
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

            var control = await _unitOfWork.QualityControlRepository.GetWithDetailsAsync(vm.QualityControlId);
            if (control == null)
                return HttpNotFound();

            try
            {
                vm.Desicions = await _unitOfWork.DesicionRepository.GetAllAsync();
                var user = await GetUserAsync();
                var inspection = _factory.Create(vm, user);
                control.SetInspection(inspection, user);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await LogExceptionAsync(ex);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }
            return View(vm);
        }

    }
}