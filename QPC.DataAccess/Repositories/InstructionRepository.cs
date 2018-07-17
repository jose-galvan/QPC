
using QPC.Core.Models;
using QPC.Core.Repositories;

namespace QPC.DataAccess.Repositories
{
    class InstructionRepository : Repository<Instruction>, IInstructionRepository
    {
        public InstructionRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
