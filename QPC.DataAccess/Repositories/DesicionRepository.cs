
using QPC.Core.Models;
using QPC.Core.Repositories;

namespace QPC.DataAccess.Repositories
{
    internal class DesicionRepository : Repository<Desicion>, IDesicionRepository
    {
        public DesicionRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
