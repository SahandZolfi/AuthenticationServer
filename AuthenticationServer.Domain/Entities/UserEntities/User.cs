namespace AuthenticationServer.Domain.Entities.UserEntities
{
    public class User : BaseEntity
    {
        public string Username { get; set; }

        public string LoweredUsername { get; set; }

        public string PasswordHash { get; set; }

        public DateTime LastPassword { get; set; }

        public string EmailAddress { get; set; }

        public string FullName { get; set; }

        public string Biography { get; set; }

        public DateTime BirthDate { get; set; }

        public string ProfileImageName { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }

        public virtual IList<UserReport> Reports { get; set; }

        public virtual IList<UserReport> Reportings { get; set; }

        public virtual IList<UserTMPUniqueCode> UserTMPUniqueCodes { get; set; }

        public virtual IList<UserAccountActivity> UserAccountActivities { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsActive { get; set; }

        public bool IsBanned { get; set; }

        public DateTime? BannedUpTo { get; set; }
    }
}
