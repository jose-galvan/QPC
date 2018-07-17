using QPC.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QPC.Core.Repositories
{
    public interface IDefectRepository: IRepository<Defect>
    {
        Task<List<Defect>> GetByProductAsync(int id);
    }
}
