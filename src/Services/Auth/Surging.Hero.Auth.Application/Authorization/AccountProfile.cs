using AutoMapper;
using Surging.Core.Domain;
using Surging.Hero.Auth.Domain.Permissions.Menus;
using Surging.Hero.Auth.IApplication.Authorization.Dtos;
using Surging.Hero.Auth.IApplication.User.Dtos;
using Surging.Hero.Common.Runtime.Session;

namespace Surging.Hero.Auth.Application.Authorization
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<GetUserNormOutput, LoginUserInfo>();
            CreateMap<Menu, GetUserMenuTreeOutput>().ForMember(p => p.Children, opt => opt.Ignore()).ForMember(p=>p.FullName, opt => opt.Ignore());
        }
    }
}
