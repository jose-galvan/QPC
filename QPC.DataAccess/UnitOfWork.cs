using QPC.Core.Repositories;
using QPC.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPC.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Fields
        private readonly ApplicationDbContext _context;
        private IExternalLoginRepository _externalLoginRepository;
        private IRoleRepository _roleRepository;
        private IUserRepository _userRepository;
        private IQualityControlRepository _qualityControlRepository;
        private IInstructionRepository _instructionRepository;
        private ILogRepository _logRepository;
        private IDefectRepository _defectRepository;
        private IProductRepository _productRepository;
        private IInspectionRepository _inspectionRepository;
        private IDesicionRepository _desicionRepository;
        #endregion

        #region Constructors
        public UnitOfWork()
        {
            _context = new ApplicationDbContext();
        }
        #endregion

        #region IUnitOfWork Members
        public IExternalLoginRepository ExternalLoginRepository
        {
            get { return _externalLoginRepository ?? (_externalLoginRepository = new ExternalLoginRepository(_context)); }
        }

        public IRoleRepository RoleRepository
        {
            get { return _roleRepository ?? (_roleRepository = new RoleRepository(_context)); }
        }

        public IUserRepository UserRepository
        {
            get { return _userRepository ?? (_userRepository = new UserRepository(_context)); }
        }

        public IQualityControlRepository QualityControlRepository
        {
            get
            {
                return _qualityControlRepository ?? (_qualityControlRepository = new QualityControlRepository(_context));
            }
        }
        public IInstructionRepository InstructionRepository
        {
            get
            {
                return _instructionRepository ?? (_instructionRepository = new InstructionRepository(_context));
            }
        }
        public ILogRepository LogRepository
        {
            get
            {
                return _logRepository ?? (_logRepository = new LogRepository(_context));
            }
        }

        public IDefectRepository DefectRepository
        {
            get
            {
                return _defectRepository ?? (_defectRepository = new DefectRepository(_context));
            }
        }

        public IProductRepository ProductRepository
        {
            get
            {
                return _productRepository ?? (_productRepository = new ProductRepository(_context));
            }
        }

        public IInspectionRepository InspectionRepository
        {
            get
            {
                return _inspectionRepository ?? (_inspectionRepository = new InspectionRepository(_context));
            }
        }
        public IDesicionRepository DesicionRepository
        {
            get
            {
                return _desicionRepository ?? (_desicionRepository = new DesicionRepository(_context));
            }
        }
        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            _externalLoginRepository = null;
            _roleRepository = null;
            _userRepository = null;
            _qualityControlRepository = null;
            _context.Dispose();
        }
        #endregion
    }
}
