using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Surging.Core.AutoMapper;
using Surging.Core.CPlatform;
using Surging.Core.CPlatform.Exceptions;
using Surging.Core.CPlatform.Routing;
using Surging.Core.CPlatform.Transport.Implementation;
using Surging.Core.CPlatform.Utilities;
using Surging.Hero.Auth.Domain.Permissions.Actions;
using Surging.Hero.Auth.Domain.Permissions.Menus;
using Surging.Hero.Auth.Domain.Permissions.Operations;
using Surging.Hero.Auth.Domain.Roles;
using Surging.Hero.Auth.Domain.Shared;
using Surging.Hero.Auth.Domain.Shared.Operations;
using Surging.Hero.Auth.Domain.UserGroups;
using Surging.Hero.Auth.Domain.Users;
using Surging.Hero.Auth.IApplication.Role.Dtos;
using Surging.Hero.Common;

namespace Surging.Hero.Auth.Domain.Permissions
{
    public class PermissionDomainService : IPermissionDomainService
    {
        private readonly IMenuDomainService _menuDomainService;
        private readonly IOperationDomainService _operationDomainService;
        private readonly IRoleDomainService _roleDomainService;
        private readonly IServiceRouteProvider _serviceRouteProvider;
        private readonly IUserDomainService _userDomainService;
        private readonly IUserGroupDomainService _userGroupDomainService;

        public PermissionDomainService(IMenuDomainService menuDomainService,
            IOperationDomainService operationDomainService,
            IRoleDomainService roleDomainService,
            IServiceRouteProvider serviceRouteProvider,
            IUserDomainService userDomainService,
            IUserGroupDomainService userGroupDomainService)
        {
            _menuDomainService = menuDomainService;
            _operationDomainService = operationDomainService;
            _roleDomainService = roleDomainService;
            _serviceRouteProvider = serviceRouteProvider;
            _userDomainService = userDomainService;
            _userGroupDomainService = userGroupDomainService;
        }

        public async Task<IDictionary<string,object>> Check(long userId, string serviceId)
        {
            var permissionResult = new Dictionary<string, object>();
            var servcieRoute = await _serviceRouteProvider.Locate(serviceId);
            var isPermission = false;
            if (servcieRoute.ServiceDescriptor.GetMetadata<bool>("AllowPermission")) isPermission = true;
            var actionName = servcieRoute.ServiceDescriptor.GroupName().IsNullOrEmpty()
                ? servcieRoute.ServiceDescriptor.RoutePath
                : servcieRoute.ServiceDescriptor.GroupName();
            if (!isPermission)
            {
                isPermission = await _userDomainService.CheckPermission(userId, serviceId);
                if (!isPermission)
                {
                
                    throw new AuthException($"您没有{actionName}的权限", StatusCode.UnAuthorized);
                }
            }
            permissionResult.Add("isPermission",isPermission);
            var operations = await _operationDomainService.GetOperationsByServiceId(serviceId);
            if (operations.Any(p => p.Mold == OperationMold.Query || p.Mold == OperationMold.Look))
            {
                if (operations.Count(p => p.Mold == OperationMold.Query || p.Mold == OperationMold.Look) >1)
                {
                    throw new BusinessException($"{actionName}的权限被分配到两个以上的操作,权限分配异常,请与开发者联系");
                }

                var dataPermission = await _userDomainService.GetDataPermissions(userId, operations.First(p=>  p.Mold == OperationMold.Query || p.Mold == OperationMold.Look).PermissionId);
                permissionResult.Add(ClaimTypes.DataPermission, dataPermission.DataPermissionType);
                permissionResult.Add(ClaimTypes.DataPermissionOrgIds, dataPermission.DataPermissionOrgIds);
                permissionResult.Add(ClaimTypes.IsAllOrg, dataPermission.DataPermissionType == DataPermissionType.AllOrg);
            }

            return permissionResult;
        }

        private IEnumerable<GetRolePermissionTreeOutput> BuildRolePermissionChildren(
            GetRolePermissionTreeOutput menuOutput, IEnumerable<Menu> menus, IEnumerable<Operation> operations,
            IEnumerable<RolePermission> rolePermissions)
        {
            var children = new List<GetRolePermissionTreeOutput>();
            var menuChildren = menus.Where(p => p.ParentId == menuOutput.Id);
            var menuChildrenOutputs = menuChildren.MapTo<IEnumerable<GetRolePermissionTreeOutput>>();
            children.AddRange(menuChildrenOutputs);
            var operationChildren = operations.Where(p => p.MenuId == menuOutput.Id);
            var operationChildrenOutputs = operationChildren.MapTo<IEnumerable<GetRolePermissionTreeOutput>>();
            foreach (var operationOutput in operationChildrenOutputs)
                operationOutput.CheckStatus = rolePermissions.Any(p => p.PermissionId == menuOutput.PermissionId)
                    ? CheckStatus.Checked
                    : CheckStatus.UnChecked;
            // operationOutput.PermissionMold = PermissionMold.Operation;
            children.AddRange(operationChildrenOutputs);
            if (menuChildrenOutputs.Any())
                foreach (var menuChildOutput in menuChildrenOutputs)
                    menuChildOutput.Children =
                        BuildRolePermissionChildren(menuChildOutput, menus, operations, rolePermissions);
            return children;
        }
    }
}