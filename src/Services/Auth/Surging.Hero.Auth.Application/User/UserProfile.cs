using AutoMapper;
using Surging.Hero.Auth.Domain.Users;
using Surging.Hero.Auth.IApplication.User.Dtos;

namespace Surging.Hero.Auth.Application.User
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserInput, UserInfo>();
            CreateMap<UpdateUserInput, UserInfo>();
            CreateMap<UserInfo, GetUserBasicOutput>();
            CreateMap<UserInfo, GetUserNormOutput>().ForMember(p => p.Roles, opt => opt.Ignore());
        }
    }
}
