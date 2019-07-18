using AutoMapper;
using Surging.Hero.Auth.Domain.User;
using Surging.Hero.Auth.IApplication.User.Dtos;

namespace Surging.Hero.Auth.Application.User
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserInput, UserInfo>();
            CreateMap<UpdateUserInput, UserInfo>();
            CreateMap<UserInfo, GetUserOutput>();
        }
    }
}
