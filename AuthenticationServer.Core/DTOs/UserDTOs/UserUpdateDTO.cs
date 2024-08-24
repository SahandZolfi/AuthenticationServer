using AuthenticationServer.Core.Attributes;
using AuthenticationServer.Domain.Entities.UserEntities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationServer.Core.DTOs.UserDTOs
{
    public class UserUpdateDTO
    {
        //?? is EDITABLE?
        [Display(Name = "Username")]
        [MinLength(4, ErrorMessage = "{0} Must be more than {1} characters")]
        public string? Username { get; set; }

        ////?? is EDITABLE?
        //[Display(Name = "Email")]
        //[Required(ErrorMessage = "Please enter {0}")]
        //[MaxLength(320, ErrorMessage = "{0} can not be more than {1} characters")]
        //[EmailAddress(ErrorMessage = "{0} is invalid")]
        //public string EmailAddress { get; set; }

        [Display(Name = "Full Name")]
        [MaxLength(70, ErrorMessage = "{0} can not be more than {1} characters")]
        public string? FullName { get; set; }

        [Display(Name = "Biography")]
        [MaxLength(145, ErrorMessage = "{0} can not be more than {1} characters")]
        public string? Biography { get; set; }

        //?? is EDITABLE?
        [Display(Name = "Birth Date")]
        [DataType(DataType.Date, ErrorMessage = "Incorrect format for {0}")]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "Profile Image File")]
        [AllowedExtensions(new string[] { ".png", ".jpg", ".jpeg" })]
        public IFormFile? ProfileImageFile { get; set; }
    }
}
