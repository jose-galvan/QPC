using Microsoft.AspNet.Identity;
using QPC.Core.DTOs;
using QPC.Core.Repositories;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace QPC.Web.Controllers.Api
{
    [Authorize]
    public class DesicionsController : ApiController
    {
        private IUnitOfWork _unitOfWork;

        public DesicionsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IHttpActionResult> AddDesicion([FromBody] DesicionDto dto)
        {
            var qualityControl = await _unitOfWork.QualityControlRepository.FindByIdAsync(dto.QualityControlId);

            if (qualityControl == null)
                return NotFound();

            var user = await _unitOfWork.UserRepository.FindByIdAsync(User.Identity.GetUserId());
            
            try
            {
                qualityControl.SetFinalDesicion(dto, user);
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
            return Ok();

        }
    }
}
