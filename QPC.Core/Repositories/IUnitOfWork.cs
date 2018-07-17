using System;
using System.Threading;
using System.Threading.Tasks;

namespace QPC.Core.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        #region Properties
        IExternalLoginRepository ExternalLoginRepository { get; }
        IRoleRepository RoleRepository { get; }
        IUserRepository UserRepository { get; }
        IQualityControlRepository QualityControlRepository { get; }
        ILogRepository LogRepository { get; }
        IInstructionRepository InstructionRepository { get;}
        IDefectRepository DefectRepository { get; }
        IProductRepository ProductRepository { get; }
        #endregion

        #region Methods
        int SaveChanges();
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        #endregion
    }
}
