using AuthenticationServer.Domain.Entities.UserEntities;

namespace AuthenticationServer.Core.DTOs.UserDTOs
{
    public class UserPublicViewDTO
    {
        public string Username { get; set; }

        //??
        public string EmailAddress { get; set; }

        public string FullName { get; set; }

        public string Biography { get; set; }

        //??
        public DateTime? BirthDate { get; set; }

        public string ProfileImageName { get; set; }

        //??
        public int RoleId { get; set; }
        public Role Role { get; set; }  
    }
}
