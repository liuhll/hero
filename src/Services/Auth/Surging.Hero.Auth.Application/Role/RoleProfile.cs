using AutoMapper;
using Surging.Hero.Auth.Domain.Permissions.Menus;
using Surging.Hero.Auth.Domain.Permissions.Operations;
using Surging.Hero.Auth.IApplication.Role.Dtos;

namespace Surging.Hero.Auth.Application.Role
{
    public class RoleProfile : Profile
    {
        public RoleProfile() {
            CreateMap<CreateRoleInput, Domain.Roles.Role>().AfterMap((src,dest)=> {
                dest.Status = Common.Status.Valid;
            });
            CreateMap<UpdateRoleInput, Domain.Roles.Role>();
            CreateMap<Domain.Roles.Role, GetRoleOutput>();

            CreateMap<Menu, GetRolePermissionTreeOutput>().ForMember(p => p.Children, opt => opt.Ignore()).ForMember(p => p.PermissionMold, opt => opt.Ignore()).AfterMap((src,dest)=> {
                dest.PermissionMold = Domain.Shared.Permissions.PermissionMold.Menu;
            });

            CreateMap<Operation, GetRolePermissionTreeOutput>().ForMember(p => p.Children, opt => opt.Ignore()).ForMember(p => p.PermissionMold, opt => opt.Ignore()).AfterMap((src, dest) => {
                dest.PermissionMold = Domain.Shared.Permissions.PermissionMold.Operation;
            });
        }
    }
}
