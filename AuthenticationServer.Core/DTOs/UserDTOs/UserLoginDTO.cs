namespace AuthenticationServer.Core.DTOs.UserDTOs
{
    public class UserLoginDTO
    {
        [Display(Name = "Username / Email Address")]
        [Required(ErrorMessage = "Enter The {0}")]
        public string UsernameOrEmail { get; set; }


        //Need to be hashed!
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Enter the {0}")]
        public string Password { get; set; }

        //Remember me?
    }
}
