using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Surging.Core.AutoMapper;
using Surging.Hero.Auth.Domain.Permissions.Menus;
using Surging.Hero.Auth.Domain.Permissions.Operations;
using Surging.Hero.Auth.Domain.Roles;
using Surging.Hero.Auth.Domain.Shared.Menus;
using Surging.Hero.Auth.Domain.Shared.Permissions;
using Surging.Hero.Auth.IApplication.Role.Dtos;

namespace Surging.Hero.Auth.Domain.Permissions
{
    public class PermissionDomainService : IPermissionDomainService
    {
        private readonly IMenuDomainService _menuDomainService;
        private readonly IOperationDomainService _operationDomainService;
        private readonly IRoleDomainService _roleDomainService;

        public PermissionDomainService(IMenuDomainService menuDomainService, IOperationDomainService operationDomainService, IRoleDomainService roleDomainService) {
            _menuDomainService = menuDomainService;
            _operationDomainService = operationDomainService;
            _roleDomainService = roleDomainService;
        }

        public async Task<IEnumerable<GetRolePermissionTreeOutput>> GetRolePermissions(long roleId)
        {
            var menus = await _menuDomainService.GetAll();
            var operations = await _operationDomainService.GetAll();
            var rolePermissions = await _roleDomainService.GetRolePermissions(roleId);

            return BuildRolePermissionTree(menus,operations,rolePermissions);
        }

        private IEnumerable<GetRolePermissionTreeOutput> BuildRolePermissionTree(IEnumerable<Menu> menus, IEnumerable<Operation> operations, IEnumerable<RolePermission> rolePermissions)
        {
            var topMenus = menus.Where(p => p.Mold == MenuMold.Top);
            var topMenuOutputs = topMenus.MapTo<IEnumerable<GetRolePermissionTreeOutput>>();
            foreach (var topMenuOutput in topMenuOutputs) {
                topMenuOutput.IsChecked = rolePermissions.Any(p => p.PermissionId == topMenuOutput.PermissionId);
                //topMenuOutput.PermissionMold = PermissionMold.Menu;
                topMenuOutput.Children = BuildRolePermissionChildren(topMenuOutput, menus, operations, rolePermissions);
            }
            return topMenuOutputs;

        }

        private IEnumerable<GetRolePermissionTreeOutput> BuildRolePermissionChildren(GetRolePermissionTreeOutput menuOutput, IEnumerable<Menu> menus, IEnumerable<Operation> operations, IEnumerable<RolePermission> rolePermissions)
        {
            var children = new List<GetRolePermissionTreeOutput>();
            var menuChildren = menus.Where(p => p.ParentId == menuOutput.Id);
            var menuChildrenOutputs = menuChildren.MapTo<IEnumerable<GetRolePermissionTreeOutput>>();
            children.AddRange(menuChildrenOutputs);
            var operationChildren = operations.Where(p => p.PermissionId == menuOutput.PermissionId);
            var operationChildrenOutputs = operationChildren.MapTo<IEnumerable<GetRolePermissionTreeOutput>>();
            foreach (var operationOutput in operationChildrenOutputs)
            {
                operationOutput.IsChecked = rolePermissions.Any(p => p.PermissionId == menuOutput.PermissionId);
               // operationOutput.PermissionMold = PermissionMold.Operation;
            }
            children.AddRange(operationChildrenOutputs);
            if (menuChildrenOutputs.Any())
            {
                foreach (var menuChildOutput in menuChildrenOutputs) {
                    menuChildOutput.Children = BuildRolePermissionChildren(menuChildOutput,menus,operations,rolePermissions);
                }
            }
            return children;
        }
    }
}
