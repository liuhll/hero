using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Surging.Core.AutoMapper;
using Surging.Core.CPlatform.Exceptions;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Session;
using Surging.Core.Dapper.Repositories;
using Surging.Core.Domain;
using Surging.Core.ProxyGenerator;
using Surging.Core.Validation.DataAnnotationValidation;
using Surging.Hero.Auth.Domain.Permissions;
using Surging.Hero.Auth.Domain.Permissions.Actions;
using Surging.Hero.Auth.Domain.Permissions.Menus;
using Surging.Hero.Auth.Domain.Permissions.Operations;
using Surging.Hero.Auth.IApplication.Action.Dtos;
using Surging.Hero.Auth.IApplication.Permission;
using Surging.Hero.Auth.IApplication.Permission.Dtos;
using Surging.Hero.Common;
using Surging.Hero.Common.Extensions;

namespace Surging.Hero.Auth.Application.Permission
{
    [ModuleName(AuthConstant.V1.PermissionMoudleName, Version = AuthConstant.V1.Version)]
    public class PermissionAppService : ProxyServiceBase, IPermissionAppService
    {
        private readonly IMenuDomainService _menuDomainService;
        private readonly IOperationDomainService _operationDomainService;
        private readonly IDapperRepository<Menu, long> _menuRepository;
        private readonly IDapperRepository<Operation, long> _operationRepository;
        private readonly IActionDomainService _actionDomainService;
        private readonly IPermissionDomainService _permissionDomainService;
        private readonly ISurgingSession _session;
        public PermissionAppService(IMenuDomainService menuDomainService,
            IOperationDomainService operationDomainService,
            IDapperRepository<Menu, long> menuRepository,
            IDapperRepository<Operation, long> operationRepository,
            IActionDomainService actionDomainService,
            IPermissionDomainService permissionDomainService)
        {
            _menuDomainService = menuDomainService;
            _operationDomainService = operationDomainService;
            _menuRepository = menuRepository;
            _operationRepository = operationRepository;
            _actionDomainService = actionDomainService;
            _permissionDomainService = permissionDomainService;
            _session = NullSurgingSession.Instance;
        }

        public async Task<CreateMenuOutput> CreateMenu(CreateMenuInput input)
        {
            input.CheckDataAnnotations().CheckValidResult();
            return await _menuDomainService.Create(input);
        }

        public async Task<CreateOperationOutput> CreateOperation(CreateOperationInput input)
        {
            input.CheckDataAnnotations().CheckValidResult();
            return await _operationDomainService.Create(input);
            
        }

        public async Task<GetMenuOutput> GetMenu(long id)
        {
            var menu = await _menuRepository.FirstOrDefaultAsync(p => p.Id == id);
            if (menu == null) {
                throw new BusinessException($"不存在Id为{id}的菜单信息");
            }
            return menu.MapTo<GetMenuOutput>();
        }

        public async Task<GetOperationOutput> GetOperation(long id)
        {
            var operation = await _operationRepository.SingleOrDefaultAsync(p => p.Id == id);
            if (operation == null) {
                throw new BusinessException($"不存在Id为{id}的操作信息");
            }
            var operationOutput = operation.MapTo<GetOperationOutput>();
            operationOutput.ActionIds = (await _actionDomainService.GetOperationOutputActions(operation.Id)).Select(p=> p.Id);
            return operationOutput;
        }

        public async Task<IEnumerable<GetPermissionTreeOutput>> GetTree()
        {
            var menus = await _menuRepository.GetAllAsync();
            var permissionTree = await BuildPermissionTree(menus);
            return permissionTree;
        }

        private async Task<IEnumerable<GetPermissionTreeOutput>> BuildPermissionTree(IEnumerable<Menu> menus)
        {
            var topMenus = menus.Where(p => p.Mold == Domain.Shared.Menus.MenuMold.Top);
            var topPermissionOutputs = topMenus.MapTo<IEnumerable<GetPermissionTreeOutput>>();
            foreach (var topPermission in topPermissionOutputs) {
                topPermission.FullName = topPermission.Name;
                topPermission.Children = await BuildPermissionChildren(topPermission, menus);
            }
            return topPermissionOutputs;
        }

        private async Task<IEnumerable<GetPermissionTreeOutput>> BuildPermissionChildren(GetPermissionTreeOutput permissionTreeOutput, IEnumerable<Menu> menus)
        {
            var permissionChildren = new List<GetPermissionTreeOutput>();
            var menuChildren = menus.Where(p => p.ParentId == permissionTreeOutput.Id);
            
            var menuPermissionChildrenOutput = menuChildren.MapTo<IEnumerable<GetPermissionTreeOutput>>();
            permissionChildren.AddRange(menuPermissionChildrenOutput);

            if (menuPermissionChildrenOutput.Any()) {
                foreach (var menuChildOutput in menuPermissionChildrenOutput) {
                    menuChildOutput.FullName = permissionTreeOutput.FullName + HeroConstants.CodeRuleRestrain.CodeSeparator + menuChildOutput.Name;
                    menuChildOutput.Children = await BuildPermissionChildren(menuChildOutput, menus);
                }
            }
           

            var operationChildren = await _operationRepository.GetAllAsync(p => p.MenuId == permissionTreeOutput.Id);
            var operationChildrenOutput = operationChildren.MapTo<IEnumerable<GetPermissionTreeOutput>>();
            foreach (var operationChildOutput in operationChildrenOutput) {
                operationChildOutput.FullName = permissionTreeOutput.FullName + HeroConstants.CodeRuleRestrain.CodeSeparator + operationChildOutput.Name;
            } 
            permissionChildren.AddRange(operationChildrenOutput);
            return permissionChildren;

        }

        public async Task<UpdateMenuOutput> UpdateMenu(UpdateMenuInput input)
        {
            input.CheckDataAnnotations().CheckValidResult();
            return await _menuDomainService.Update(input);
            
        }

        public async Task<UpdateOperationOutput> UpdateOperation(UpdateOperationInput input)
        {
            input.CheckDataAnnotations().CheckValidResult();
            return await _operationDomainService.Update(input);            
        }

        public async Task<string> Delete(DeletePermissionInput input)
        {            
            switch (input.Mold) {
                case Domain.Shared.Permissions.PermissionMold.Menu:
                    await _menuDomainService.Delete(input.Id);
                    break;
                case Domain.Shared.Permissions.PermissionMold.Operation:
                    await _operationDomainService.Delete(input.Id);
                    break;
                default:
                    throw new BusinessException("PermissionType不正确");
            }
            return $"删除{input.Mold.GetDescription()}成功";
        }

        public async Task<bool> Check(string serviceId)
        {
            if (_session == null || !_session.UserId.HasValue) {
                throw new AuthException("您还没有登录系统");
            }

            return await _permissionDomainService.Check(_session.UserId.Value,serviceId);
           
        }
    }
}
