using AuthenticationServer.Core.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationServer.Core.DTOs.UserDTOs
{
    public class UserChangePasswordDTO
    {
        [Display(Name = "Current Password")]
        [Required(ErrorMessage = "Enter the {0}")]
        public string CurrentPassword { get; set; }

        [Display(Name = "New Password")]
        [Required(ErrorMessage = "{0} is required")]
        [RegularExpression("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{5,30}$", ErrorMessage = "Password must be at least 5 characters, no more than 30 characters, and must include at least 1 upper case letter, 1 lower case letter, and 1 numeric digit.")]
        public string NewPassword { get; set; }

        [Display(Name = "Confirm New Password")]
        [Required(ErrorMessage = "{0} is required")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "New Passwords are not the same!")]
        public string ConfirmNewPassword { get; set; }
    }
}
