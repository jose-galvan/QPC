using Microsoft.AspNet.Identity;
using QPC.Core.DTOs;
using QPC.Core.Repositories;
using QPC.Web.Helpers;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace QPC.Web.Controllers.Api
{
    [Authorize]
    public class InspectionsController : ApiController
    {
        private IUnitOfWork _unitOfWork;
        private QualityControlFactory _factory;

        public InspectionsController(IUnitOfWork unitOfWork, QualityControlFactory factory)
        {
            _factory = factory;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IHttpActionResult> AddDesicion([FromBody] InspectionDto dto)
        {
            var qualityControl = await _unitOfWork.QualityControlRepository.FindByIdAsync(dto.QualityControlId);
            var desicion = await _unitOfWork.DesicionRepository.FindByIdAsync(dto.QualityControlId);
            
            if (qualityControl == null)
                return NotFound();

            if (desicion == null)
                return BadRequest();

            var user = await _unitOfWork.UserRepository.FindByIdAsync(User.Identity.GetUserId());
            
            try
            {
                var inspection = _factory.Create(dto);
                inspection.Desicion = desicion;
                qualityControl.SetInspection(inspection, user);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        [HttpPost]
        public async Task<IHttpActionResult> Add()
        {
            await Task.Delay(1);
            return Ok();

        }
    }
}
