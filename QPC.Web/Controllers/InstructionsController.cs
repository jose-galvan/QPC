using Microsoft.AspNet.Identity;
using QPC.Core.Models;
using QPC.Core.Repositories;
using QPC.Core.ViewModels;
using System;
using System.Collections;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QPC.Web.Controllers
{
    [Authorize]
    public class InstructionsController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public Func<Guid> GetUserId;

        public InstructionsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            GetUserId = () => ControllerHelpers.GetGuid(User.Identity.GetUserId());
        }

        //receives id of control as parameter
        [HttpGet]
        public async Task<ActionResult> AddInstruction(int id)
        {
            Expression<Func<Instruction, bool>> expr = (i => i.QualityControlId == 1);
            var instructions = await _unitOfWork.InstructionRepository.GetAsync(expr);

            var control = await _unitOfWork.QualityControlRepository.FindByIdAsync(id);
            if (control == null)
                return HttpNotFound();
            var vm = new InstructionViewModel
            {
                QualityControlId = id,
                Instructions = instructions
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
            
            var user = await _unitOfWork.UserRepository.FindByIdAsync(GetUserId());
            try
            {
                control.AddInstruction(vm, user);
                await _unitOfWork.SaveChangesAsync();
                vm.Name = vm.Description = 
                        vm.Comments = string.Empty;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Exception", ex);
            }

            vm.Instructions = control.Instructions;
            return View(vm);
        }
        
    }
}