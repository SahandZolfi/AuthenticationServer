using AutoMapper;
using AuthenticationServer.Core.DTOs.UserDTOs;
using AuthenticationServer.Core.IRepository;
using AuthenticationServer.Core.IServices;
using AuthenticationServer.Core.Security;
using AuthenticationServer.Core.Services;
using AuthenticationServer.Domain.Entities.UserEntities;
using AuthenticationServer.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationServer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthManager _authManager;
        private readonly IFileService _fileService;

        public UserController(ILogger<UserController> logger, IUnitOfWork unitOfWork, IMapper mapper, IAuthManager authManager, IFileService fileService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authManager = authManager;
            _fileService = fileService;
        }

        [HttpGet("ViewUserProfile/{userId:long}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ViewUserProfile([FromRoute] long userId)
        {
            _logger.LogInformation($"GET Attempt for {nameof(ViewUserProfile)}");
            var user = await _unitOfWork.UserRepository.Get(expression: x => x.Id == userId && !x.IsDeleted && !x.IsBanned && x.IsActive);
            if (user == null)
            {
                _logger.LogError($"Invalid GET Attempt for {nameof(ViewUserProfile)}");
                return NotFound("No User with this Id Exists");
            }
            return Ok(user);
        }

        [Authorize]
        [HttpGet("ViewOwnUserProfile")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ViewOwnUserProfile()
        {
            _logger.LogInformation($"GET Attempt for {nameof(ViewOwnUserProfile)}");
            var user = await _unitOfWork.UserRepository.Get(expression: x => x.Username == HttpContext.User.Identity.Name && !x.IsBanned && !x.IsDeleted && x.IsActive);
            if (user == null)
            {
                _logger.LogError($"Invalid GET Attempt for {nameof(ViewOwnUserProfile)}");
                return BadRequest();
            }
            return Ok(user);
        }

        [HttpPost("UserRegister")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO userRegisterDTO)
        {
            _logger.LogInformation($"POST Attempt for {nameof(Register)}");
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST Attempt for {nameof(Register)}");
                return BadRequest(ModelState);
            }
            var existingUser = await _unitOfWork.UserRepository.Get(expression: x => x.Username == userRegisterDTO.Username || x.EmailAddress == userRegisterDTO.EmailAddress);
            if (existingUser != null)
            {
                return BadRequest("Another user with the same Username/Email exists.");
            }
            var user = _mapper.Map<User>(userRegisterDTO);
            user.PasswordHash = await HashHelper.GetHash(userRegisterDTO.Password);
            user.CreationDate = DateTime.Now;
            user.EmailAddress = userRegisterDTO.EmailAddress.ToLower();
            user.LoweredUsername = userRegisterDTO.Username.ToLower();
            await _unitOfWork.UserRepository.Insert(user);
            await _unitOfWork.Save();
            return Ok("User registered successfully");
        }

        [HttpPost("SendActivationCode/{userId:long}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SendEmailActivationCode([FromRoute] long userId)
        {
            _logger.LogInformation($"POST Attempt for {nameof(SendEmailActivationCode)}");
            var user = await _unitOfWork.UserRepository.Get(expression: x => x.Id == userId && !x.IsDeleted && !x.IsBanned);
            if (user == null)
            {
                return BadRequest("Invalid Attempt");
            }
            if (user.IsActive)
            {
                return BadRequest("User is already active!");
            }
            string uniqueCode = HashHelper.CreateUnique6CharCode();
            var userTmpUniqueCode = new UserTMPUniqueCode()
            {
                UserId = user.Id,
                Code = uniqueCode,
                ExpireTime = DateTime.Now.AddHours(2),
                CreationDate = DateTime.Now,
                UserTMPCodeType = UserTMPCodeType.AccountActivation
            };
            var expiredUniqueCodes = await _unitOfWork.UserTMPUniqueCodeRepository.GetFilteredList(expression: x => x.ExpireTime < DateTime.Now || x.UserId == userId);
            _unitOfWork.UserTMPUniqueCodeRepository.Delete(expiredUniqueCodes.ToList());
            await _unitOfWork.UserTMPUniqueCodeRepository.Insert(userTmpUniqueCode);
            await _unitOfWork.Save();
            string userEmail = user.EmailAddress;
            //Send Activation Code to Email Address
            await EmailService.Send(userEmail, "AuthenticationServer Account Activation", $"<p style=\"color:black;font-size:20px;\">Your Activation Code is : <b style=\"color:blue;font-size:20px;\"> {uniqueCode}</b></p>");
            return Ok("The Activation code has been sent!");
        }

        [HttpPost("UserEmailActivation/{userId:long}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UserEmailActivation([FromRoute] long userId, [FromBody] string userUniqueCode)
        {
            _logger.LogInformation($"POST Attempt for {nameof(UserEmailActivation)}");
            var user = await _unitOfWork.UserRepository.Get(expression: x => x.Id == userId && !x.IsDeleted && !x.IsBanned && !x.IsActive, includes: new()
            {
                X=>X.Include(x=>x.Role)
            });
            if (user == null)
            {
                _logger.LogError($"Invalid POST Attempt for {nameof(UserEmailActivation)}");
                return BadRequest("Invalid User Activation.");
            }
            var userTMPUniqueCode = await _unitOfWork.UserTMPUniqueCodeRepository.Get(x => x.Code == userUniqueCode && x.UserId == user.Id && x.UserTMPCodeType == UserTMPCodeType.AccountActivation);
            if (userTMPUniqueCode == null || userTMPUniqueCode.ExpireTime.IsExpired())
            {
                _logger.LogError($"Invalid POST Attempt for {nameof(UserEmailActivation)}");
                return BadRequest("Invalid User Activation.");
            }
            user.IsActive = true;
            _unitOfWork.UserRepository.Update(user);
            _unitOfWork.UserTMPUniqueCodeRepository.Delete(userTMPUniqueCode);
            await _unitOfWork.Save();
            return Ok(new { Message = "User activated successfully", Token = await _authManager.CreateToken(user) });
        }

        [HttpPost("UserLogin")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginDTO)
        {
            _logger.LogInformation($"POST Attempt for {nameof(Login)}");
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST Attempt for {nameof(Login)}");
                return BadRequest(ModelState);
            }
            var user = await _authManager.ValidateUser(userLoginDTO);
            if (user == null)
            {
                _logger.LogError($"Invalid POST LOGIN Attempt for {nameof(Login)}");
                return Unauthorized("Incorrect Username/Email or Password");
            }

            if (!user.IsActive)
            {
                return Accepted(new { Token = await _authManager.CreateToken(), Message = "Login Successful. Activate your ACCOUNT!" });
            }
            return Accepted(new { Token = await _authManager.CreateToken(), Message = $"Login Successful" });
        }

        //need to be improved
        [HttpPost("ForgotPassword")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            _logger.LogInformation($"POST Attempt for {nameof(ForgotPassword)}");
            var user = await _unitOfWork.UserRepository.Get(x => x.EmailAddress.ToLower() == email, includes: new()
            {
                X=>X.Include(x=>x.UserAccountActivities)
            });
            if (user == null)
            {
                _logger.LogError($"Invalid POST Attempt for {nameof(ForgotPassword)}");
                return BadRequest("Email Address is Not valid");
            }

            //Password Change Limit For a user
            var userPasswordChangeActivities = user.UserAccountActivities.Where(x => x.AccountActivityType == AccountActivityType.PasswordChange).ToList();
            var oneDayAgoDateTime = DateTime.Now.AddHours(-24);
            var passwordChangesSinceOneDayAgo = userPasswordChangeActivities.Where(x => x.CreationDate > oneDayAgoDateTime).ToList();
            if (passwordChangesSinceOneDayAgo.Count >= 3)
            {
                _logger.LogError($"Invalid POST Attempt for {nameof(ForgotPassword)}");
                return BadRequest("You have reached the Password Recovery Limit. You are able to change your password 3 times in a day.");
            }

            string uniqueCode = HashHelper.CreateUnique6CharCode();
            var userTmpUniqueCode = new UserTMPUniqueCode()
            {
                UserId = user.Id,
                Code = uniqueCode,
                ExpireTime = DateTime.Now.AddHours(2),
                CreationDate = DateTime.Now,
                UserTMPCodeType = UserTMPCodeType.PasswordRecovery
            };
            var expiredUniqueCodes = await _unitOfWork.UserTMPUniqueCodeRepository.GetFilteredList(expression: x => x.ExpireTime < DateTime.Now || x.UserId == user.Id);
            _unitOfWork.UserTMPUniqueCodeRepository.Delete(expiredUniqueCodes.ToList());
            await _unitOfWork.UserTMPUniqueCodeRepository.Insert(userTmpUniqueCode);
            await _unitOfWork.Save();
            string userEmail = user.EmailAddress;
            //Send Password Recovery Code to Email Address
            await EmailService.Send(userEmail, "AuthenticationServer Account Recovery", $"<p style=\"color:black;font-size:20px;\">Your Recovery Code is : <b style=\"color:blue;font-size:20px;\"> {uniqueCode}</b></p>");
            return Ok("The Recovery code has been sent!");
        }

        [HttpPost("PasswordRecovery")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> PasswordRecovery([FromBody] UserPasswordRecoveryDTO userPasswordRecoveryDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST Attempt for {nameof(PasswordRecovery)}");
                return BadRequest(ModelState);
            }
            var user = await _unitOfWork.UserRepository.Get(x => x.EmailAddress == userPasswordRecoveryDTO.EmailAddress.ToLower(), includes: new()
            {
                X => X.Include(x=>x.UserTMPUniqueCodes)
            });
            if (user == null)
            {
                _logger.LogError($"Invalid POST Attempt for {nameof(PasswordRecovery)}");
                return BadRequest("The email address is not valid");
            }
            var userUniqueCode = user.UserTMPUniqueCodes.SingleOrDefault(x => x.Code == userPasswordRecoveryDTO.RecoveryCode && x.UserTMPCodeType == UserTMPCodeType.PasswordRecovery);
            if (userUniqueCode == null)
            {
                _logger.LogError($"Invalid POST Attempt for {nameof(PasswordRecovery)}");
                return BadRequest("The recovery code is not valid");
            }
            user.PasswordHash = await HashHelper.GetHash(userPasswordRecoveryDTO.NewPassword);
            UserAccountActivity userAccountActivity = new UserAccountActivity()
            {
                AccountActivityType = AccountActivityType.PasswordChange,
                CreationDate = DateTime.Now,
                Note = $"Password Recovery; Password of the user ({user.Username}) has been changed by the user itself",
                UserId = user.Id
            };
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.UserAccountActivityRepository.Insert(userAccountActivity);
            _unitOfWork.UserTMPUniqueCodeRepository.Delete(userUniqueCode);
            await _unitOfWork.Save();
            return Ok("The password was changed successfully");
        }

        [Authorize]
        [HttpPut("UserChangePassword")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UserChangePassword([FromBody] UserChangePasswordDTO userChangePasswordDTO)
        {
            _logger.LogInformation($"PUT Attempt for {nameof(UserChangePassword)}");
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid PUT Attempt for {nameof(UserChangePassword)}");
                return BadRequest(ModelState);
            }
            var user = await _authManager.ValidateClaims(HttpContext, ClaimValidationType.NormalUser);
            if (user == null)
            {
                _logger.LogError($"Invalid PUT Attempt for {nameof(UserChangePassword)}");
                return Unauthorized();
            }
            if (await HashHelper.GetHash(userChangePasswordDTO.CurrentPassword) != user.PasswordHash)
            {
                _logger.LogError($"Invalid PUT Attempt for {nameof(UserChangePassword)}");
                return BadRequest("Current Password Is not correct");
            }
            user.PasswordHash = await HashHelper.GetHash(userChangePasswordDTO.NewPassword);
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.Save();
            return Ok("Pssword Changed Successfully");
        }

        [Authorize]
        [HttpPut("EditUserProfile")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> EditUserProfile([FromForm] UserUpdateDTO userUpdateDTO)
        {
            _logger.LogInformation($"PUT Attempt for {nameof(EditUserProfile)}");
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid PUT Attempt for {nameof(EditUserProfile)}");
                return BadRequest(ModelState);
            }
            var currentUser = await _authManager.ValidateClaims(HttpContext, ClaimValidationType.NormalUser);
            if (currentUser == null)
            {
                _logger.LogError($"Invalid PUT Attempt for {nameof(EditUserProfile)}");
                return Unauthorized();
            }
            var existingUser = await _unitOfWork.UserRepository.Get(expression: x => x.Username == userUpdateDTO.Username);
            if (existingUser != null)
            {
                return BadRequest("Another user with the same Username exists.");
            }
            _mapper.Map(userUpdateDTO, currentUser);

            var profileImageFile = userUpdateDTO.ProfileImageFile;
            string currentProfileImageName = currentUser.ProfileImageName;
            if (profileImageFile != null)
            {
                currentUser.ProfileImageName = await _fileService.UploadImage(profileImageFile, UploadImageType.UserProfile);
            }
            _unitOfWork.UserRepository.Update(currentUser);
            await _unitOfWork.Save();
            if (currentProfileImageName != "DefaultProfile.png")
            {
                await _fileService.DeleteImage(currentProfileImageName, UploadImageType.UserProfile);
            }
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("DeactivateUser/{userId:long}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeactivateUser(long userId)
        {
            _logger.LogInformation($"PUT Attempt for {nameof(DeactivateUser)}");
            if (await _authManager.ValidateClaims(HttpContext, ClaimValidationType.Admin) == null)
            {
                _logger.LogError($"Invalid PUT Attempt for {nameof(DeactivateUser)}");
                return Unauthorized();
            }
            var user = await _unitOfWork.UserRepository.Get(x => x.Id == userId);
            if (user == null)
            {
                _logger.LogError($"Invalid PUT Attempt for {nameof(DeactivateUser)}");
                return BadRequest("No User with this Id");
            }
            else if (!user.IsActive)
            {
                _logger.LogError($"Invalid PUT Attempt for {nameof(DeactivateUser)}");
                return BadRequest("This USER is deactive");
            }
            user.IsActive = false;
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.Save();
            return Ok("User Deactivated Successfully");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteUser/{userId:long}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteUser(long userId)
        {
            if (await _authManager.ValidateClaims(HttpContext, ClaimValidationType.Admin) == null)
            {
                _logger.LogError($"Invalid DELETE Attempt for {nameof(DeleteUser)}");
                return Unauthorized();
            }
            _logger.LogInformation($"DELETE Attempt for {nameof(DeleteUser)}");
            var user = await _unitOfWork.UserRepository.Get(expression: x => x.Id == userId && !x.IsDeleted);
            if (user == null)
            {
                _logger.LogError($"Invalid DELETE Attempt for {nameof(DeleteUser)}");
                return BadRequest("No User with this Id Exists");
            }
            user.IsDeleted = true;
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.Save();
            return Ok("User Deleted Successfully");
        }
    }
}
