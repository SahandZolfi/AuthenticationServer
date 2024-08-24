using AutoMapper;
using AuthenticationServer.Core.DTOs.UserDTOs;
using AuthenticationServer.Domain.Entities.UserEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationServer.Core.AutoMapperProfiles
{
    public class MapperInitializer : Profile
    {
        public MapperInitializer()
        {
            #region User Maps Creation
            CreateMap<User, UserPublicViewDTO>().ReverseMap();
            CreateMap<User, UserOwnView>().ReverseMap();
            CreateMap<User, UserRegisterDTO>().ReverseMap();
            CreateMap<User, UserLoginDTO>().ReverseMap();
            CreateMap<User, UserUpdateDTO>().ReverseMap().ForAllMembers(opts =>
            {
                opts.Condition((src, dest, srcMember) => srcMember != null);
            });
            #endregion
        }
    }
}
