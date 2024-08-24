using AuthenticationServer.Shared.Enums;

namespace AuthenticationServer.Domain.Entities.UserEntities
{
    public class UserTMPUniqueCode : BaseEntity
    {
        public string Code { get; set; }
        public DateTime ExpireTime { get; set; }

        public UserTMPCodeType UserTMPCodeType { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }
    }
}
