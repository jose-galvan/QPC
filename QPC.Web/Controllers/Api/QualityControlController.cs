
using System.Web.Http;
using QPC.Core.Repositories;
using QPC.Web.Helpers;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using QPC.Core.Models;
using QPC.Core.DTOs;
using System;
using System.Linq;
using QPC.Core.ViewModels;

namespace QPC.Web.Controllers.Api
{
    [RoutePrefix("api/controls")]
    public class QualityControlController : QualityControlWebApiBaseController
    {
        public QualityControlController(IUnitOfWork unitOfWork, QualityControlFactory factory) : base(unitOfWork, factory)
        {

        }

        [HttpGet][Route("")]
        public async Task<IEnumerable<ListItemViewModel>> GetAll()
        {
            var controls = await _unitOfWork
                    .QualityControlRepository.GetAllWithDetailsAsync();

            return controls.Select(c => _factory.CreateItem(c));
        }

        [HttpGet][Route("{query:alpha}")]
        public async Task<IEnumerable<ListItemViewModel>> Get(string query)
        {
            var controls = await _unitOfWork
                    .QualityControlRepository.GetAllWithDetailsAsync();
            controls = controls.Where(c => c.Name.Contains(query)
                                    || c.Description.Contains(query))
                                .ToList();

            return controls.Select(c => _factory.CreateItem(c));
        }

        [HttpGet][Route("~/api/control/{id:int}")]
        public async Task<QualityControl> GetById(int id)
        {
            return await _unitOfWork
                    .QualityControlRepository.FindByIdAsync(id);
        }

        [HttpPost][Route("")]
        public async Task<IHttpActionResult> Create([FromBody] QualityControlDto dto)
        {
            var user = await GetUserAsync();

            try
            {
                var control = _factory.Create(dto, user);
                _unitOfWork.QualityControlRepository.Add(control);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await LogExceptionAsync(ex);
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [Route("")]
        [HttpPut, HttpPatch]
        public async Task<IHttpActionResult> Update([FromBody] QualityControlDto dto)
        {
            var user = await GetUserAsync();
            var control = await _unitOfWork.QualityControlRepository.FindByIdAsync(dto.Id);
            if (control == null)
                return NotFound();

            try
            {
                control.Update(user, dto.Name, dto.Description);
                _unitOfWork.QualityControlRepository.Update(control);
                await _unitOfWork.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                await LogExceptionAsync(ex);
                return BadRequest(ex.Message);
            }

            return Ok();
        }


    }
}
