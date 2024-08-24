using AuthenticationServer.Domain.Entities.UserEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthenticationServer.Persistence.EntityConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.CreationDate).IsRequired();
            builder.Property(x => x.ProfileImageName).HasDefaultValue("DefaultProfile.png");
            builder.Property(x => x.PasswordHash).IsRequired().HasMaxLength(128);
            builder.Property(x => x.BirthDate).IsRequired();
            builder.Property(x => x.FullName).IsRequired().HasMaxLength(70);
            builder.Property(x => x.EmailAddress).IsRequired().HasMaxLength(320);
            builder.Property(x => x.Username).IsRequired().HasMaxLength(32);
            builder.Property(x => x.LoweredUsername).IsRequired().HasMaxLength(32);
            builder.Property(x => x.Biography).HasMaxLength(145);
            builder.Property(x => x.IsActive).IsRequired().HasDefaultValue(false);
            builder.Property(x => x.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Property(x => x.IsBanned).IsRequired().HasDefaultValue(false);
            builder.Property(x => x.RoleId).IsRequired().HasDefaultValue(1);
            builder.HasMany(x => x.Reportings).WithOne(x => x.ReportedByUser).HasForeignKey(x => x.ReporterByUserId);
            builder.HasMany(x => x.Reports).WithOne(x => x.Reporting).HasForeignKey(x => x.ReportingId).OnDelete(DeleteBehavior.ClientCascade);
            builder.HasOne(x => x.Role).WithMany(x => x.UsersWithThisRole).HasForeignKey(x => x.RoleId);
            builder.HasMany(x => x.UserTMPUniqueCodes).WithOne(x => x.User).HasForeignKey(x => x.UserId);
        }
    }

    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Title).IsRequired().HasMaxLength(20);
            builder.HasData(
                new Role()
                {
                    Id = 1,
                    Title = "Normal User"
                }, new Role()
                {
                    Id = 2,
                    Title = "Admin"
                });
        }
    }
  
    public class UserReportConfiguration : IEntityTypeConfiguration<UserReport>
    {
        public void Configure(EntityTypeBuilder<UserReport> builder)
        {
            builder.ToTable("UserReports");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.CreationDate).IsRequired();
            builder.Property(x => x.Text).IsRequired().HasMaxLength(200);
        }
    }

    public class TMPUniqueCodeConfiguration : IEntityTypeConfiguration<UserTMPUniqueCode>
    {
        public void Configure(EntityTypeBuilder<UserTMPUniqueCode> builder)
        {
            builder.ToTable("UserTMPUniqueCodes");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.CreationDate).IsRequired();
            builder.Property(x => x.Code).IsRequired().HasMaxLength(6);
            builder.Property(x => x.ExpireTime).IsRequired();
            builder.Property(x=>x.UserTMPCodeType).IsRequired();
        }
    }

    public class UserAccountActivityConfiguration : IEntityTypeConfiguration<UserAccountActivity>
    {
        public void Configure(EntityTypeBuilder<UserAccountActivity> builder)
        {
            builder.ToTable("UserAccountActivities");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.CreationDate).IsRequired();
            builder.Property(x => x.AccountActivityType).IsRequired();
            builder.Property(x => x.Note).HasMaxLength(200);
        }
    }
}
