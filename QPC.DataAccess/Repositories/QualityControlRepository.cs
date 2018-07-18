using QPC.Core.Models;
using QPC.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace QPC.DataAccess.Repositories
{
    internal class QualityControlRepository : Repository<QualityControl>, IQualityControlRepository
    {
        internal QualityControlRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<QualityControl>> GetAllWithProductsAsync()
        {
            return await Set.Include(qc => qc.Product).ToListAsync();
        }

        public async Task<QualityControl> GetWithDetails(int id)
        {
            return await Set.Include(c => c.Product)
                            .Include(c => c.Defect)
                            .Include(c => c.Instructions)
                            .Include(c => c.UserCreated)
                            .Include(c => c.Desicion)
                            .Include(c => c.LastModificationUser)
                            .SingleOrDefaultAsync(c =>c.Id ==id);
        }
    }
}
