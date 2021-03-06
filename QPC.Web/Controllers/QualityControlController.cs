﻿using QPC.Core.Repositories;
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
    public class QualityControlController : QualityControlMvcController
    {


        public QualityControlController(IUnitOfWork unitOfWork, QualityControlFactory factory)
            : base(unitOfWork, factory)
        {
        }

        // GET: QC
        public async Task<ViewResult> Index(string searchCriteria = null)
        {
            var controls = await _unitOfWork.QualityControlRepository.GetAllWithDetailsAsync();
            var viewModel = new QualityControlIndexViewModel();

            if(!string.IsNullOrEmpty(searchCriteria))
                controls = controls
                                .Where(c => c.Name.Contains(searchCriteria)
                                    || c.Description.Contains(searchCriteria))
                                .ToList();

            viewModel.Header = "Results for: " + searchCriteria ?? "Quality Controls";
            viewModel.SearchCriteria = searchCriteria;
            viewModel.Controls = controls
                    .Select(qc => _factory.CreateViewModel(qc));

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
        public async Task<ActionResult> RequestControl(QualityControlViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                viewModel.Products = await _unitOfWork.ProductRepository.GetAllAsync();
                viewModel.Defects = await _unitOfWork.DefectRepository.GetAllAsync();
                return View(viewModel);
            }

            try
            {
                var user = await GetUserAsync();
                var control = _factory.Create(viewModel, user);
                _unitOfWork.QualityControlRepository.Add(control);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await LogExceptionAsync(ex);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }

            return RedirectToAction("Index", "QualityControl");
        }

        [HttpGet]
        public async Task<ActionResult> UpdateControl(int id)
        {
            var qc = await _unitOfWork.QualityControlRepository.GetWithDetailsAsync(id);
            if (qc == null)
                return HttpNotFound();

            var viewModel = _factory.Create(qc);
            return View(viewModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateControl(QualityControlDetailViewModel model)
        {
            var control = await _unitOfWork.QualityControlRepository.FindByIdAsync(model.Id);
            if (control == null)
                return HttpNotFound();

            var user = await _unitOfWork.UserRepository.FindByIdAsync(GetUserId());
            try
            {
                control.Update(user, model.Name, model.Description);
                await _unitOfWork.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                await LogExceptionAsync(ex);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }
            return RedirectToAction("Index", "QualityControl");
        }



    }
}