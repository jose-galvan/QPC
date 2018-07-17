using Microsoft.AspNet.Identity;
using QPC.Core.Repositories;
using QPC.Core.ViewModels;
using QPC.Web.Helpers;
using QPC.Web.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QPC.Web.Controllers
{
    [Authorize]
    public class QualityControlController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public Func<Guid> GetUserId;

        public QualityControlController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            GetUserId =() => ControllerHelpers.GetGuid(User.Identity.GetUserId());
        }

        // GET: QC
        public async Task<ViewResult> Index(string searchCriteria = null)
        {
            var controls = await _unitOfWork.QualityControlRepository.GetAllWithProductsAsync();
            var viewModel = new QualityControlsViewModel();
            viewModel.Controls = controls;
            viewModel.Header = "Results for: " + searchCriteria ?? "Quality Controls";
            viewModel.SearchCriteria = searchCriteria;

            if(string.IsNullOrEmpty(searchCriteria))
                return View(viewModel);

            viewModel.Controls = controls.Where(c => c.Name.Contains(searchCriteria) || c.Description.Contains(searchCriteria))
                            .ToList();

            return View(viewModel);
        }

        [HttpGet][Authorize]
        public async Task<ActionResult> RequestControl()
        {
            var viewModel = new QualityControlViewModel
            {
                Products = await _unitOfWork.ProductRepository.GetAllAsync(),
                Defects = await _unitOfWork.DefectRepository.GetAllAsync()
            };
            return View(viewModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RequestControl(QualityControlViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _unitOfWork.UserRepository.FindByIdAsync(GetUserId());
            var control = QualityControlFactory.Create(model, user);
            _unitOfWork.QualityControlRepository.Add(control);
            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction("Index", "QualityControl");
        }

        [HttpGet]
        public async Task<ActionResult> UpdateControl(int id)
        {
            var qc = await _unitOfWork.QualityControlRepository.GetWithDetails(id);
            if (qc == null)
                return HttpNotFound();

            var viewModel = QualityControlFactory.Create(qc);
            return View(viewModel);
        }
        
        [HttpPut, HttpPatch]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateControl(QualityControlDetailViewModel model)
        {
            var control = await _unitOfWork.QualityControlRepository.FindByIdAsync(model.Id);
            if (control == null)
                return HttpNotFound();

            var user = await _unitOfWork.UserRepository.FindByIdAsync(GetUserId());
            try
            {
                control.Update(model, user);
                await _unitOfWork.SaveChangesAsync();
            }
            catch
            {
                return View(model);
            }
            return RedirectToAction("Index", "QualityControl");
        }



    }
}