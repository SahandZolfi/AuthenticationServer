using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthenticationServer.Domain.Entities.UserEntities;


namespace AuthenticationServer.Core.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        #region User related repositories
        public IGenericRepository<User> UserRepository { get; }
        public IGenericRepository<Role> RoleRepository { get; }
        public IGenericRepository<UserReport> UserReportRepository { get; }
        public IGenericRepository<UserTMPUniqueCode> UserTMPUniqueCodeRepository { get; }
        public IGenericRepository<UserAccountActivity> UserAccountActivityRepository { get; }
        #endregion
       
        public Task Save();
    }
}
