using QPC.Core.DTOs;
using QPC.Core.Models;
using QPC.Core.Repositories;
using QPC.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Http;

namespace QPC.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/defects")]
    public class DefectsController : QualityControlWebApiBaseController
    {
        public DefectsController(IUnitOfWork unitOfWork, QualityControlFactory factory)
            : base(unitOfWork, factory)
        {
        }

        [HttpGet, Authorize]
        [Route("~/api/defect/{id:int}")]
        public async Task<IHttpActionResult> GetById(int id)
        {
            var defect = await _unitOfWork.DefectRepository.FindByIdAsync(id);
            if (defect == null)
                return NotFound();

            var result = _factory.CreateDto(defect);
            return Ok(result);
        }
        [Route("")]
        public async Task<List<DefectDto>> Get()
        {
            var defects = await _unitOfWork.DefectRepository.GetWithProductAsync();
            return defects.Select(d => _factory.CreateDto(d)).ToList();
        }

        [Route("{query}")]
        public async Task<List<DefectDto>> Get([FromUri]string query)
        {
            query = query.ToLower();
            var defects = await _unitOfWork.DefectRepository.GetWithProductAsync();
            defects = defects.Where(d =>
                            d.Name.ToLower().Contains(query) ||
                            d.Description.ToLower().Contains(query)).ToList();
            return defects.Select(d => _factory.CreateDto(d)).ToList();
        }

        [HttpPost][Route("")]
        public async Task<IHttpActionResult> Add([FromBody]DefectDto dto)
        {
            try
            {
                var user = await GetUserAsync();
                var defect = _factory.Create(dto, user);
                _unitOfWork.DefectRepository.Add(defect);
                await _unitOfWork.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                await LogExceptionAsync(ex);
                return BadRequest(ex.Message);
            }
            return Ok();
        }
        [HttpPut, HttpPatch][Route("")]
        public async Task<IHttpActionResult> Update([FromBody]DefectDto dto)
        {
            var defect = await _unitOfWork.DefectRepository.FindByIdAsync(dto.Id);
            if (defect == null)
                return NotFound();
            try
            {
                var user = await GetUserAsync();
                defect.Update(user, dto.Name, dto.Description);
                _unitOfWork.DefectRepository.Update(defect);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await LogExceptionAsync(ex);
                return BadRequest(ex.Message);
            }
            return Ok();
        }
    }
}
