using QPC.Core.Models;
using QPC.Core.Repositories;

namespace QPC.DataAccess.Repositories
{
    class LogRepository : Repository<Log>, ILogRepository
    {
        public LogRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
