using AutoMapper;
using Surging.Hero.Auth.IApplication.User.Dtos;
using Surging.Hero.Common.Runtime.Session;

namespace Surging.Hero.Auth.Application.Authorization
{
    public class AccountProfile : Profile
    {
        public AccountProfile() {
            CreateMap<GetUserNormOutput, LoginUserInfo>();
        }
    }
}
