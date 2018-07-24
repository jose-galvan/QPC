using System;
using System.Linq;
using System.Web.Mvc;
using QPC.Core.Repositories;
using QPC.Web.Helpers;
using System.Threading.Tasks;
using QPC.Core.ViewModels;
using System.Net;
using QPC.Core.Models;

namespace QPC.Web.Controllers
{
    public class DefectsController : QualityControlMvcController
    {
        public DefectsController(IUnitOfWork unitOfWork, QualityControlFactory factory)
                : base(unitOfWork, factory)
        {
        }

        [HttpGet]
        public async Task<ActionResult> Defect(string query = null)
        {
            var defects = await _unitOfWork.DefectRepository
                        .GetWithProductAsync();

            var vm = new DefectViewModel
            {
                Defects = defects.Select(d => _factory.Create(d)).ToList(),
                Products = await _unitOfWork.ProductRepository.GetAllAsync(),
                SearchCriteria = query
            };


            if (!string.IsNullOrEmpty(query))
            {
                vm.Defects = vm.Defects
                    .Where(d => d.Name.ToLower().Contains(query.ToLower())
                        || d.Description.ToLower().Contains(query.ToLower())
                        || d.Product.ToLower().Contains(query.ToLower())
                        )
                    .ToList();
            }

           return View(vm);
        }

        [HttpGet]
        public async Task<ActionResult> UpdateDefect(int id)
        {
            var defects = await _unitOfWork.DefectRepository
            .GetWithProductAsync();

            var defect = defects.SingleOrDefault(d => d.Id == id);
            if (defect == null)
                return HttpNotFound();

            var vm = new DefectViewModel
            {
                Id = id, 
                Name = defect.Name,
                Description = defect.Description,
                Product = defect.ProductId,
                Defects = defects.Select(d => _factory.Create(d)).ToList(),
                Products = await _unitOfWork.ProductRepository.GetAllAsync()
            };
            
            return View("Defect", vm);
        }


        [HttpPost][ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveDefect(DefectViewModel viewModel)
        {
            var defects = await _unitOfWork.DefectRepository
                        .GetWithProductAsync();
            viewModel.Defects = defects.Select(d => _factory.Create(d)).ToList();
            viewModel.Products = await _unitOfWork.ProductRepository.GetAllAsync();
            try
            {
                if (viewModel.Id.HasValue)
                {
                    var defect = await _unitOfWork.DefectRepository.FindByIdAsync(viewModel.Id);
                    if (defect == null)
                        return HttpNotFound();
                    await Update(viewModel, defect);
                }
                else
                    await Create(viewModel);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await LogExceptionAsync(ex);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }

            if (!string.IsNullOrEmpty(viewModel.SearchCriteria))
                viewModel.Products = viewModel.Products
                        .Where(p => p.Name.ToLower().Contains(viewModel.SearchCriteria.ToLower())
                            || p.Description.ToLower().Contains(viewModel.SearchCriteria.ToLower()))
                        .ToList();

            return View("Defect", viewModel);
        }

        private async Task Create(DefectViewModel viewModel)
        {
            var user = await GetUserAsync();
            var defect = _factory.Create(viewModel, user);
            _unitOfWork.DefectRepository.Add(defect);
            viewModel.Defects.Add(_factory.Create(defect));
        }

        private async Task Update(DefectViewModel viewModel, Defect defect)
        {
            var user = await GetUserAsync();
            defect.Update(user, viewModel.Name, viewModel.Description);
        }
    }
}