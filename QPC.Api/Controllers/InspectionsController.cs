using Microsoft.AspNet.Identity;
using QPC.Core.DTOs;
using QPC.Core.Repositories;
using QPC.Web.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace QPC.Api.Controllers
{
    [Authorize][RoutePrefix("api/inspections")]
    public class InspectionsController : QualityControlWebApiBaseController
    {
        public InspectionsController(IUnitOfWork unitOfWork, QualityControlFactory factory) : base(unitOfWork, factory)
        {
        }

        [HttpPost][Route("")]
        public async Task<IHttpActionResult> AddDesicion([FromBody] InspectionDto dto)
        {
            var qualityControl = await _unitOfWork.QualityControlRepository.FindByIdAsync(dto.QualityControlId);
            var desicion = await _unitOfWork.DesicionRepository.FindByIdAsync(dto.Desicion);
            
            if (qualityControl == null)
                return NotFound();

            if (desicion == null)
                return BadRequest();
            try
            {
                var user = await GetUserAsync();
                var inspection = _factory.Create(dto, user);
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

        [HttpGet][Route("~/api/inspection/desicions")]
        public async Task<IEnumerable<DesicionDto>> GetDesicions()
        {
            var desicions = await _unitOfWork.DesicionRepository.GetAllAsync();
            return desicions.Select(d => _factory.Create(d));
        }

        [HttpGet][Route("~/api/control/{id:int}/inspection")]
        public async Task<IHttpActionResult> GetByControl([FromUri] int id)
        {
            var control = await _unitOfWork.QualityControlRepository.GetWithDetailsAsync(id);
            if (control == null || control.Inspection == null)
                return NotFound();
            
            var inspection = _factory.Create(control.Inspection);
            return Ok(inspection);
        }
    }
}
