using QPC.Core.Repositories;
using QPC.Core.DTOs;
using System.Web.Http;
using System.Threading.Tasks;
using QPC.Core.Models;
using Microsoft.AspNet.Identity;
using System;
using QPC.Web.Helpers;

namespace QPC.Web.Controllers.Api
{
    [Authorize]
    public class InstructionController : ApiController
    {
        private IUnitOfWork _unitOfWork;
        public Func<Guid> GetUserId;
        
        public InstructionController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            GetUserId = () => ControllerHelpers.GetGuid(User.Identity.GetUserId());
        }

        [HttpPost]
        public async Task<IHttpActionResult> AddInstructionAsync([FromBody]InstructionDto dto)
        {
            var qualityControl = await _unitOfWork.QualityControlRepository.FindByIdAsync(dto.QualityControlId);

            if (qualityControl == null)
                return NotFound();

            var user = await _unitOfWork.UserRepository.FindByIdAsync(GetUserId());
            
            try {
                var instruction = QualityControlFactory.Create(dto);
                qualityControl.AddInstruction(instruction, user);
                await _unitOfWork.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        [HttpPut, HttpPatch]
        public async Task<IHttpActionResult> UpdateInstructionAsync([FromUri]int id)
        {
            var instruction = await _unitOfWork.InstructionRepository.GetWithQualityControl(id);
            if (instruction == null)
                return NotFound();
            
            try
            {
                var user = await _unitOfWork.UserRepository.FindByIdAsync(GetUserId());
                instruction.Update(user);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        [HttpDelete]
        public async Task<IHttpActionResult> CancelInstructionAsync(int id)
        {
            var instruction = await _unitOfWork.InstructionRepository.FindByIdAsync(id);
            if (instruction == null)
                return NotFound();

            var user = await _unitOfWork.UserRepository.FindByIdAsync(GetUserId());

            try
            {
                instruction.Cancel(user);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }
    }
}
