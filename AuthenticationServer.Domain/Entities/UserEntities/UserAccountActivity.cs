using AuthenticationServer.Shared.Enums;

namespace AuthenticationServer.Domain.Entities.UserEntities
{
    public class UserAccountActivity: BaseEntity
    {
        public AccountActivityType AccountActivityType { get; set; }
        public string? Note { get; set; }
        public User User { get; set; }
        public long UserId { get; set; }
    }
}
