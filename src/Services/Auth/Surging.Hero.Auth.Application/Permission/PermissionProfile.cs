using AutoMapper;
using Surging.Hero.Auth.Domain.Permissions.Menus;
using Surging.Hero.Auth.Domain.Permissions.Operations;
using Surging.Hero.Auth.Domain.Shared.Menus;
using Surging.Hero.Auth.Domain.Shared.Permissions;
using Surging.Hero.Auth.IApplication.Permission.Dtos;
using Surging.Hero.Common;

namespace Surging.Hero.Auth.Application.Permission
{
    public class PermissionProfile : Profile
    {
        public PermissionProfile()
        {
            CreateMap<CreateMenuInput, Menu>().AfterMap((src, dest) =>
            {
                if (src.ParentPermissionId == 0)
                    dest.Mold = MenuMold.Top;
                else
                    dest.Mold = MenuMold.SubMenu;
            });
            CreateMap<CreateMenuInput, Domain.Permissions.Permission>().AfterMap((src, dest) =>
            {
                dest.Mold = PermissionMold.Menu;
                dest.Status = Status.Valid;
            });
            CreateMap<UpdateMenuInput, Menu>();
            CreateMap<UpdateMenuInput, Domain.Permissions.Permission>();
            CreateMap<Menu, GetMenuOutput>();

            CreateMap<CreateOperationInput, Operation>();
            CreateMap<CreateOperationInput, Domain.Permissions.Permission>().AfterMap((src, dest) =>
            {
                dest.Mold = PermissionMold.Operation;
                dest.Status = Status.Valid;
            });

            CreateMap<UpdateOperationInput, Operation>();
            CreateMap<UpdateOperationInput, Domain.Permissions.Permission>();

            CreateMap<Operation, GetOperationOutput>().ForMember(d => d.ActionIds, opt => opt.Ignore());

            CreateMap<Menu, GetPermissionTreeOutput>().ForMember(d => d.Children, opt => opt.Ignore())
                .ForMember(d => d.Mold, opt => opt.Ignore()).AfterMap((src, dest) =>
                {
                    dest.Mold = PermissionMold.Menu;
                });
            CreateMap<Operation, GetPermissionTreeOutput>().ForMember(d => d.Children, opt => opt.Ignore())
                .ForMember(d => d.Mold, opt => opt.Ignore()).AfterMap((src, dest) =>
                {
                    dest.Mold = PermissionMold.Operation;
                });
            //CreateMap<Menu, GetPermissionTreeOutput>();
            //CreateMap<Operation, GetPermissionTreeOutput>();
        }
    }
}