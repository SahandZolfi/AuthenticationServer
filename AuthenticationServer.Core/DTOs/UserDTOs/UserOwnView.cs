using AuthenticationServer.Domain.Entities.UserEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationServer.Core.DTOs.UserDTOs
{
    public class UserOwnView
    {
        public string Username { get; set; }

        public string EmailAddress { get; set; }

        public string FullName { get; set; }

        public string Biography { get; set; }

        public DateTime? BirthDate { get; set; }

        public string ProfileImageName { get; set; }

        //??
        public bool IsBanned { get; set; }

        public DateTime? BannedUpTo { get; set; }
    }
}
