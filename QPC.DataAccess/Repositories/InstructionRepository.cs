using System.Threading.Tasks;
using System.Data.Entity;
using QPC.Core.Models;
using QPC.Core.Repositories;

namespace QPC.DataAccess.Repositories
{
    class InstructionRepository : Repository<Instruction>, IInstructionRepository
    {
        public InstructionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Instruction> GetWithQualityControl(int id)
        {
            return await Set
                    .Include(i => i.QualityControl)
                    .SingleOrDefaultAsync(i => i.Id == id);
        }
    }
}
