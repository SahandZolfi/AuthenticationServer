using AuthenticationServer.Core.DTOs.UserDTOs;
using AuthenticationServer.Domain.Entities.UserEntities;
using AuthenticationServer.Shared.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationServer.Core.IServices
{
    public interface IAuthManager
    {
        Task<User> ValidateUser(UserLoginDTO userLoginDTO);
        Task<string> CreateToken(User? user=null);
        Task<User> ValidateClaims(HttpContext httpContext, ClaimValidationType claimValidationType);
    }
}
