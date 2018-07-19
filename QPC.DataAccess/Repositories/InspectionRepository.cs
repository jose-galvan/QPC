
using QPC.Core.Models;
using QPC.Core.Repositories;

namespace QPC.DataAccess.Repositories
{
    internal class InspectionRepository : Repository<Inspection>, IInspectionRepository
    {
        public InspectionRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
