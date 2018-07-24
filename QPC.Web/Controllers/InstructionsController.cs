using QPC.Core.Models;
using QPC.Core.Repositories;
using QPC.Core.ViewModels;
using QPC.Web.Helpers;
using System;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QPC.Web.Controllers
{
    [Authorize]
    public class InstructionsController : QualityControlMvcController
    {

        public InstructionsController(IUnitOfWork unitOfWork, QualityControlFactory factory)
            :base(unitOfWork, factory)
        {

        }

        //receives id of control as parameter
        [HttpGet]
        public async Task<ActionResult> AddInstruction(int? id)
        {
            if(!id.HasValue)
                return RedirectToAction("Index", "QualityControl");


            Expression<Func<Instruction, bool>> expr = (i => i.QualityControlId == id);
            var instructions = await _unitOfWork.InstructionRepository.GetAsync(expr);

            var control = await _unitOfWork.QualityControlRepository.FindByIdAsync(id);
            if (control == null)
                return HttpNotFound();
            var vm = new InstructionViewModel
            {
                QualityControlId = id.Value,
                Instructions = instructions,
                CanSave = control.Status ==
                        QualityControlStatus.Closed ? false : true
            };
            return View(vm);
        }
        
        [HttpPost]
        public async Task<ActionResult> AddInstruction(InstructionViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Data is invalid");
                return View(vm);
            }
            var control = await _unitOfWork.QualityControlRepository.FindByIdAsync(vm.QualityControlId);
            if (control == null)
                return HttpNotFound();
            
            try
            {
                var user = await GetUserAsync();
                var instruction = _factory.Create(vm, user);
                control.AddInstruction(instruction, user);
                await _unitOfWork.SaveChangesAsync();
                vm.Name = vm.Description = 
                        vm.Comments = string.Empty;
            }
            catch (Exception ex)
            {
                await LogExceptionAsync(ex);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }

            vm.Instructions = control.Instructions;
            return View(vm);
        }
        
    }
}