using AuthenticationServer.Domain.Entities.UserEntities;

namespace AuthenticationServer.Core.DTOs.UserDTOs
{
    public class UserRegisterDTO
    {
        [Display(Name = "Username")]
        [Required(ErrorMessage = "Enter the {0}")]
        [RegularExpression("^[a-zA-Z0-9][a-zA-Z0-9_]+$", ErrorMessage ="{0} is not in correct format")]
        [MinLength(4, ErrorMessage ="{0} Must be more than {1} characters")]
        //[MaxLength(4, ErrorMessage = "{0} Can not be more than {2} characters")]
        public string Username { get; set; }

        //Need to be hashed!
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Enter the {0}")]
        [RegularExpression("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{5,30}$", ErrorMessage = "Password must be at least 5 characters, no more than 30 characters, and must include at least 1 upper case letter, 1 lower case letter, and 1 numeric digit.")]
        public string Password { get; set; }

        //Need to be hashed!
        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Enter the {0}")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords are not the same!")]
        public string RePassword { get; set; }

        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Enter the {0}")]
        [MaxLength(320,ErrorMessage = "{0} can not be more than {1} characters")]
        [EmailAddress(ErrorMessage = "{0} is invalid")]
        public string EmailAddress { get; set; }

        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "Enter the {0}")]
        [MaxLength(70,ErrorMessage = "{0} can not be more than {1} characters")]
        public string FullName { get; set; }

        [Display(Name = "Biography")]
        [MaxLength(145,ErrorMessage ="{0} can not be more than {1} characters")]
        public string Biography { get; set; }

        [Display(Name = "BirthDate")]
        [DataType(DataType.Date,ErrorMessage = "Incorrect format for {0}")]
        [Required(ErrorMessage = "{0} is Required")]
        public DateTime BirthDate { get; set; }

        [Display(Name = "ProfileImageName")]
        public string? ProfileImageName { get; set; }
    }
}
