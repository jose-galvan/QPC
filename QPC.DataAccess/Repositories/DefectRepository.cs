using QPC.Core.Models;
using QPC.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace QPC.DataAccess.Repositories
{
    class DefectRepository : Repository<Defect>, IDefectRepository
    {
        public DefectRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<Defect>> GetByProductAsync(int id)
        {
            return await Set.Where(d => d.ProductId == id).ToListAsync();
        }
    }
}
