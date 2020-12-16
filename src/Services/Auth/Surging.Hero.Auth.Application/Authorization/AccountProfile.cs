using AutoMapper;
using Surging.Hero.Auth.Domain.Permissions.Menus;
using Surging.Hero.Auth.Domain.Permissions.Operations;
using Surging.Hero.Auth.IApplication.Authorization.Dtos;
using Surging.Hero.Auth.IApplication.User.Dtos;
using Surging.Hero.Common.Runtime.Session;
using GetDisplayRoleOutput = Surging.Hero.Auth.IApplication.Role.Dtos.GetDisplayRoleOutput;

namespace Surging.Hero.Auth.Application.Authorization
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<GetUserNormOutput, LoginUserInfo>();
            CreateMap<GetDisplayRoleOutput, Common.Runtime.Session.GetDisplayRoleOutput>();
            CreateMap<Menu, GetUserMenuTreeOutput>().ForMember(p => p.Children, opt => opt.Ignore())
                .ForMember(p => p.FullName, opt => opt.Ignore());
            CreateMap<Menu, GetUserMenuOutput>().ForMember(p => p.FullName, opt => opt.Ignore());
            CreateMap<Operation, GetUserOperationOutput>();
        }
    }
}