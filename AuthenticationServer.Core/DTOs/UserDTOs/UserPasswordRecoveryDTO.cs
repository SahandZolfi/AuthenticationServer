namespace AuthenticationServer.Core.DTOs.UserDTOs
{
    public class UserPasswordRecoveryDTO
    {
        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Enter {0}")]
        [MaxLength(320, ErrorMessage = "{0} can not be more than {1} characters")]
        [EmailAddress(ErrorMessage = "{0} is invalid")]
        public string EmailAddress { get; set; }

        [Display(Name = "Recovery Code")]
        [Required(ErrorMessage = "{0} is required")]
        [MaxLength(6,ErrorMessage = "{0} can not be more than {1} characters")]
        public string RecoveryCode { get; set; }

        [Display(Name = "New Password")]
        [Required(ErrorMessage = "{0} is required")]
        [RegularExpression("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{5,30}$", ErrorMessage = "Password must be at least 5 characters, no more than 30 characters, and must include at least 1 upper case letter, 1 lower case letter, and 1 numeric digit.")]
        public string NewPassword { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "{0} is required")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Passwords are not the same!")]
        public string ConfirmPassword { get; set; }
    }
}
