using QPC.Core.Models;
using QPC.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace QPC.DataAccess.Repositories
{
    internal class DefectRepository : Repository<Defect>, IDefectRepository
    {
        public DefectRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<Defect>> GetByProductAsync(int id)
        {
            return await Set.Include(d => d.Product).Where(d => d.ProductId == id).ToListAsync();
        }

        public async Task<List<Defect>> GetWithProductAsync()
        {
            return await Set.Include(d => d.Product).ToListAsync();
        }
    }
}
