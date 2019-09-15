using AutoMapper;
using Surging.Hero.Auth.Domain.Permissions.Menus;
using Surging.Hero.Auth.Domain.Permissions.Operations;
using Surging.Hero.Auth.Domain.Shared.Permissions;
using Surging.Hero.Auth.IApplication.Permission.Dtos;

namespace Surging.Hero.Auth.Application.Permission
{
    public class PermissionProfile : Profile
    {
        public PermissionProfile() {
            CreateMap<CreateMenuInput, Menu>().AfterMap((src,dest)=> {
                if (dest.ParentId == 0)
                {
                    dest.Mold = Domain.Shared.Menus.MenuMold.Top;
                }
                else {
                    dest.Mold = Domain.Shared.Menus.MenuMold.SubMenu;
                }
            });
            CreateMap<CreateMenuInput, Domain.Permissions.Permission>().AfterMap((src,dest)=> {
                dest.Mold = PermissionMold.Menu;
            });
            CreateMap<UpdateMenuInput, Menu>();
            CreateMap<UpdateMenuInput, Domain.Permissions.Permission>();
            CreateMap<Menu, GetMenuOutput>();

            CreateMap<CreateOperationInput, Operation>();
            CreateMap<CreateOperationInput, Domain.Permissions.Permission>().AfterMap((src, dest) => {
                dest.Mold = PermissionMold.Operation;
            });
        }
    }
}
