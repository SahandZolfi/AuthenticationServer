using AuthenticationServer.Core.DTOs.UserDTOs;
using AuthenticationServer.Core.IRepository;
using AuthenticationServer.Core.IServices;
using AuthenticationServer.Core.Security;
using AuthenticationServer.Domain.Entities.UserEntities;
using AuthenticationServer.Shared.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AuthenticationServer.Core.Services
{
    public class AuthManager : IAuthManager
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private User _user;
        public AuthManager(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> CreateToken(User? user = null)
        {
            if (user != null)
            {
                _user = user;
            }
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var token = GenerateTokenOptions(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JWT");
            var expiration = DateTime.Now.AddMinutes(Convert.ToDouble(
                jwtSettings.GetSection("lifetime").Value));

            var token = new JwtSecurityToken(
                issuer: jwtSettings.GetSection("Issuer").Value,
                claims: claims,
                expires: expiration,
                signingCredentials: signingCredentials
                );

            return token;
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
             {
                 new Claim(ClaimTypes.Name, _user.Username),
                 new Claim(ClaimTypes.Role, _user.Role.Title)
             };
            return claims;
        }

        private SigningCredentials GetSigningCredentials()
        {
            //var key = Environment.GetEnvironmentVariable("KEY");
            var key = "MYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEY";
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        public async Task<User> ValidateClaims(HttpContext httpContext, ClaimValidationType claimValidationType)
        {
            var user = await _unitOfWork.UserRepository.Get(x => x.Username == httpContext.User.Identity.Name && !x.IsDeleted && !x.IsBanned && x.IsActive, includes: new()
            {
                x=>x.Include(x => x.Role)
            });
            if (user != null)
            {
                switch (claimValidationType)
                {
                    case ClaimValidationType.NormalUser:
                        return user;
                        break;
                    case ClaimValidationType.Admin:
                        if (user.Role.Title == "Admin")
                            return user;
                        else return null;
                        break;
                }
            }
            return null;
        }

        public async Task<User> ValidateUser(UserLoginDTO userLoginDTO)
        {
            _user = await _unitOfWork.UserRepository.Get(expression: x => x.Username == userLoginDTO.UsernameOrEmail | x.EmailAddress == userLoginDTO.UsernameOrEmail, includes: new()
            {
                X => X.Include(x => x.Role),
            });
            if (_user != null && _user.PasswordHash == await HashHelper.GetHash(userLoginDTO.Password) && !_user.IsBanned && !_user.IsDeleted)
            {
                return _user;
            }
            return null;
        }
    }
}
