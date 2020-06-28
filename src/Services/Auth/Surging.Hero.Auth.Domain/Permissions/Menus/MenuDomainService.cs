using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Surging.Core.AutoMapper;
using Surging.Core.CPlatform.Exceptions;
using Surging.Core.Dapper.Manager;
using Surging.Core.Dapper.Repositories;
using Surging.Hero.Auth.Domain.Permissions.Operations;
using Surging.Hero.Auth.Domain.Shared.Menus;
using Surging.Hero.Auth.IApplication.Permission.Dtos;
using Surging.Hero.Common;

namespace Surging.Hero.Auth.Domain.Permissions.Menus
{
    public class MenuDomainService : ManagerBase, IMenuDomainService
    {
        private readonly IDapperRepository<Menu, long> _menuRepository;
        private readonly IDapperRepository<Permission, long> _permissionRepository;
        private readonly IDapperRepository<Operation, long> _operationRepository;
        private readonly IDapperRepository<OperationActionRelation, long> _operationActionRepository;

        public MenuDomainService(IDapperRepository<Menu, long> menuRepository,
            IDapperRepository<Permission, long> permissionRepository,
            IDapperRepository<Operation, long> operationRepository,
            IDapperRepository<OperationActionRelation, long> operationActionRepository) {
            _menuRepository = menuRepository;
            _permissionRepository = permissionRepository;
            _operationRepository = operationRepository;
            _operationActionRepository = operationActionRepository;

        }

        public async Task<CreateMenuOutput> Create(CreateMenuInput input)
        {
            var menu = input.MapTo<Menu>();
            long menuParentId = 0;
            if (menu.Mold == MenuMold.Top)
            {
                var thisLevelMenuCount = await _menuRepository.GetCountAsync(p => p.Mold == MenuMold.Top);
                menu.Code = (thisLevelMenuCount + 1).ToString().PadRight(HeroConstants.CodeRuleRestrain.CodeCoverBit, HeroConstants.CodeRuleRestrain.CodeCoverSymbol);
                menu.Level = 1;
            }
            else {
                var parentMenu = await _menuRepository.SingleOrDefaultAsync(p => p.PermissionId == input.ParentPermissionId);
                if (parentMenu == null) {
                    throw new BusinessException($"不存在PermissionId为{input.ParentPermissionId}的菜单信息");
                }
                menuParentId = parentMenu.Id;
                var thisLevelMenuCount = await _menuRepository.GetCountAsync(p => p.Mold == MenuMold.SubMenu && p.ParentId == parentMenu.Id);
                menu.Code = parentMenu.Code + HeroConstants.CodeRuleRestrain.CodeSeparator + (thisLevelMenuCount + 1).ToString().PadLeft(HeroConstants.CodeRuleRestrain.CodeCoverBit, HeroConstants.CodeRuleRestrain.CodeCoverSymbol);
                menu.Level = parentMenu.Level + 1;
            }
            var permission = input.MapTo<Permission>();
            
            await UnitOfWorkAsync(async (conn,trans) => {
                var permissionId = await _permissionRepository.InsertAndGetIdAsync(permission, conn, trans);
                menu.PermissionId = permissionId;
                menu.ParentId = menuParentId;
                await _menuRepository.InsertAsync(menu, conn, trans);

            },Connection);
            return new CreateMenuOutput() { Id = menu.Id, PermissionId = menu.PermissionId, Tips = "新增菜单成功" };
        }

        public async Task Delete(long permissionId)
        {
            var menu = await _menuRepository.SingleOrDefaultAsync(p => p.PermissionId == permissionId);
            if (menu == null) {
                throw new BusinessException($"不存在PermissionId为{permissionId}的菜单信息");
            }
            var allNeedDeleteMenus = await _menuRepository.GetAllAsync(p => p.Code.Contains(menu.Code));
           
            await UnitOfWorkAsync(async (conn, trans) => {
                await _menuRepository.DeleteAsync(p => p.Code.Contains(menu.Code), conn, trans);
                foreach (var needDeleteMenu in allNeedDeleteMenus) {
                    await _permissionRepository.DeleteAsync(p => p.Id == needDeleteMenu.PermissionId, conn, trans);
                    var operations = await _operationRepository.GetAllAsync(p => p.MenuId == needDeleteMenu.Id);
                    await _operationRepository.DeleteAsync(p => p.MenuId == needDeleteMenu.Id, conn, trans);
                    if (operations.Any())
                    {
                        foreach (var operation in operations)
                        {
                          
                            await _operationActionRepository.DeleteAsync(p => p.OperationId == operation.Id, conn, trans);
                            await _permissionRepository.DeleteAsync(p=> p.Id == operation.PermissionId, conn, trans);
                        }
                    }
                }
                
            }, Connection);
        }

        public async Task<IEnumerable<Menu>> GetAll()
        {
            return await _menuRepository.GetAllAsync();
        }


        public async Task<UpdateMenuOutput> Update(UpdateMenuInput input)
        {
            var menu = await _menuRepository.SingleOrDefaultAsync(p => p.Id == input.Id);
            if (menu == null) {
                throw new BusinessException($"不存在Id为{input.Id}的菜单信息");
            }
            var permission = await _permissionRepository.GetAsync(menu.PermissionId);
            menu = input.MapTo(menu);
            permission = input.MapTo(permission);
            await UnitOfWorkAsync(async (conn, trans) => {
                await _permissionRepository.UpdateAsync(permission, conn, trans);               
                await _menuRepository.UpdateAsync(menu, conn, trans);

            }, Connection);
            return new UpdateMenuOutput() { Id = menu.Id, PermissionId = menu.PermissionId, Tips = "更新菜单成功" };
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
            foreach (var topPermission in topPermissionOutputs)
            {
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

            if (menuPermissionChildrenOutput.Any())
            {
                foreach (var menuChildOutput in menuPermissionChildrenOutput)
                {
                    menuChildOutput.FullName = permissionTreeOutput.FullName + HeroConstants.CodeRuleRestrain.CodeSeparator + menuChildOutput.Name;
                    menuChildOutput.Children = await BuildPermissionChildren(menuChildOutput, menus);
                }
            }


            var operationChildren = await _operationRepository.GetAllAsync(p => p.MenuId == permissionTreeOutput.Id);
            var operationChildrenOutput = operationChildren.MapTo<IEnumerable<GetPermissionTreeOutput>>();
            foreach (var operationChildOutput in operationChildrenOutput)
            {
                operationChildOutput.FullName = permissionTreeOutput.FullName + HeroConstants.CodeRuleRestrain.CodeSeparator + operationChildOutput.Name;
            }
            permissionChildren.AddRange(operationChildrenOutput);
            return permissionChildren;

        }
    }
}
