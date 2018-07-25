using QPC.Core.Models;
using QPC.Core.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace QPC.Web.Controllers.Api
{
    public class DefectsController : ApiController
    {
        private IUnitOfWork _unitOfWork;

        public DefectsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet, Authorize]
        public async Task<IEnumerable<Defect>> GetDefects(int id)
        {
            return await _unitOfWork.DefectRepository.GetByProductAsync(id);
        }
    }
}
