using System.Web.Mvc;
using QPC.Core.Repositories;
using QPC.Web.Helpers;
using QPC.Core.ViewModels;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Net;
using QPC.Core.Models;
using System.Linq.Expressions;

namespace QPC.Web.Controllers
{
    public class ProductsController : QualityControlMvcController
    {
        public ProductsController(IUnitOfWork unitOfWork, QualityControlFactory factory) : base(unitOfWork, factory)
        {

        }

        [HttpGet]
        public async Task<ActionResult> Product(string searchCriteria = null)
        {
            var vm = new ProductViewModel();
            vm.Products = await _unitOfWork.ProductRepository.GetAllAsync();

            if (!string.IsNullOrEmpty(searchCriteria))
                vm.Products = vm.Products
                        .Where(p => p.Name.ToLower().Contains(searchCriteria.ToLower())
                            || p.Description.ToLower().Contains(searchCriteria.ToLower()))
                        .ToList();
            
            return View(vm);
        }
        [HttpPost][ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveProduct(ProductViewModel viewModel)
        {
            viewModel.Products = await _unitOfWork.ProductRepository.GetAllAsync();
            try
            {
                if (viewModel.Id.HasValue)
                {
                    var product = await  _unitOfWork.ProductRepository.FindByIdAsync(viewModel.Id);
                    if (product == null)
                        return HttpNotFound();
                    await Update(viewModel, product);
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

            return View(viewModel);
        }
        [HttpGet]
        public async Task<ActionResult> UpdateProduct(int id)
        {
            var product = await _unitOfWork.ProductRepository.FindByIdAsync(id);
            if (product == null)
                return HttpNotFound();

            Expression<Func<Defect, bool>> expr = (d => d.ProductId == id);

            var vm = new ProductViewModel
            {
                Id = id,
                Name = product.Name,
                Description = product.Description,
                Products = await _unitOfWork.ProductRepository.GetAllAsync(),
                Defects = await _unitOfWork.DefectRepository.GetAsync(expr)
            };
            return View("Product",vm);
        }


        private async Task Create(ProductViewModel viewModel)
        {
            Expression<Func<Product, bool>> expr =
                (p => p.Name.ToLower().Contains(viewModel.Name.ToLower()));
            var products = await _unitOfWork.ProductRepository.GetAsync(expr);

            if (products.Any())
                throw new Exception("Product Already registered.");
            
            var user = await GetUserAsync();
            var product = _factory.Create(viewModel, user);
            _unitOfWork.ProductRepository.Add(product);
            viewModel.Products.Add(product);
        }
        
        private async Task Update(ProductViewModel viewModel, Product product)
        {
            var user = await GetUserAsync();
            product.Update(user, viewModel.Name, viewModel.Description);
            _unitOfWork.ProductRepository.Update(product);   
        }
    }
}