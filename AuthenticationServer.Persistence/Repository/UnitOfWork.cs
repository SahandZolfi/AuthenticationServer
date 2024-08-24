using AuthenticationServer.Core.IRepository;
using AuthenticationServer.Domain.Entities.UserEntities;
using AuthenticationServer.Persistence.DatabaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationServer.Persistence.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext.DataBaseContext _context;

        public UnitOfWork(DataBaseContext context)
        {
            _context = context;
        }

        private IGenericRepository<User> _userRepository;

        private IGenericRepository<Role> _roleRepository;

        private IGenericRepository<UserReport> _userReportRepository;

        private IGenericRepository<UserTMPUniqueCode> _userTMPUniqueCodeRepository;

        private IGenericRepository<UserAccountActivity> _userAccountActivityRepository;
     

        public IGenericRepository<User> UserRepository => _userRepository ??= new GenericRepository<User>(_context);

        public IGenericRepository<Role> RoleRepository => _roleRepository ??= new GenericRepository<Role>(_context);


        public IGenericRepository<UserReport> UserReportRepository => _userReportRepository ??= new GenericRepository<UserReport>(_context);

        public IGenericRepository<UserTMPUniqueCode> UserTMPUniqueCodeRepository => _userTMPUniqueCodeRepository ??= new GenericRepository<UserTMPUniqueCode>(_context);

        public IGenericRepository<UserAccountActivity> UserAccountActivityRepository => _userAccountActivityRepository ??= new GenericRepository<UserAccountActivity>(_context);
        

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _context.Dispose();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
