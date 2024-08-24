using AuthenticationServer.Domain.Entities.UserEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationServer.Persistence.DatabaseContext
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> modelBuilder) : base(modelBuilder) 
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        #region Tables
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserReport> UserReports { get; set; }
        public DbSet<UserTMPUniqueCode> UserTMPUniqueCodes { get; set; }       
        public DbSet<UserAccountActivity> UserAccountActivities { get; set; }
        #endregion
    }
}
