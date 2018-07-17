using QPC.Core.Repositories;
using QPC.Core.DTOs;
using System.Web.Http;
using System.Threading.Tasks;
using QPC.Core.Models;
using Microsoft.AspNet.Identity;
using System;

namespace QPC.Web.Controllers.Api
{
    [Authorize]
    public class InstructionController : ApiController
    {
        private IUnitOfWork _unitOfWork;

        public InstructionController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IHttpActionResult> AddInstructionAsync([FromBody]InstructionDto dto)
        {
            var qualityControl = await _unitOfWork.QualityControlRepository.FindByIdAsync(dto.QualityControlId);

            if (qualityControl == null)
                return NotFound();

            var user = await _unitOfWork.UserRepository.FindByIdAsync(User.Identity.GetUserId());
            
            try { 
                //qualityControl.AddInstruction(dto, user);
                await _unitOfWork.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        [HttpPut, HttpPatch]
        public async Task<IHttpActionResult> UpdateInstructionAsync([FromBody]InstructionDto dto)
        {
            var instruction = await _unitOfWork.InstructionRepository.FindByIdAsync(dto.Id);
            if (instruction == null)
                return NotFound();

            var user = await _unitOfWork.UserRepository.FindByIdAsync(User.Identity.GetUserId());
            
            try
            {
                instruction.Update(dto, user);
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

            var user = await _unitOfWork.UserRepository.FindByIdAsync(User.Identity.GetUserId());

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
