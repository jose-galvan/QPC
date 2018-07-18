using QPC.Core.Models;
using System.Threading.Tasks;

namespace QPC.Core.Repositories
{
    public interface IInstructionRepository: IRepository<Instruction>
    {
        Task<Instruction> GetWithQualityControl(int id);
    }
}
