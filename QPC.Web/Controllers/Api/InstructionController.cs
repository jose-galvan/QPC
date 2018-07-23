using QPC.Core.Repositories;
using QPC.Core.DTOs;
using System.Web.Http;
using System.Threading.Tasks;
using System;
using QPC.Web.Helpers;

namespace QPC.Web.Controllers.Api
{
    [Authorize]
    public class InstructionController : QualityControlWebApiBaseController
    {
        

        public InstructionController(IUnitOfWork unitOfWork, QualityControlFactory factory)
            :base(unitOfWork, factory)
        {
        }

        [HttpPost]
        public async Task<IHttpActionResult> AddInstructionAsync([FromBody]InstructionDto dto)
        {
            var qualityControl = await _unitOfWork.QualityControlRepository.FindByIdAsync(dto.QualityControlId);

            if (qualityControl == null)
                return NotFound();

            
            try {
                var user = await GetUserAsync();
                var instruction = _factory.Create(dto, user);
                qualityControl.AddInstruction(instruction, user);
                await _unitOfWork.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                await LogExceptionAsync(ex);
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
                var user = await GetUserAsync();
                instruction.Update(user);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await LogExceptionAsync(ex);
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


            try
            {
                var user = await GetUserAsync();
                instruction.Cancel(user);
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
