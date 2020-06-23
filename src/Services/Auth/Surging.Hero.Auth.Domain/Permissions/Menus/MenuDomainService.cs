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
            var thisLevelMenuCount = await _menuRepository.GetCountAsync(p => p.ParentId == input.ParentId);
            if (menu.Mold == MenuMold.Top)
            {
                menu.Code = (thisLevelMenuCount + 1).ToString().PadRight(HeroConstants.CodeRuleRestrain.CodeCoverBit, HeroConstants.CodeRuleRestrain.CodeCoverSymbol);
                menu.Level = 1;
            }
            else {
                var parentMenu = await _menuRepository.SingleOrDefaultAsync(p => p.Id == input.ParentId);
                if (parentMenu == null) {
                    throw new BusinessException($"不存在Id为{input.ParentId}的菜单信息");
                }
                menu.Code = parentMenu.Code + HeroConstants.CodeRuleRestrain.CodeSeparator + (thisLevelMenuCount + 1).ToString().PadLeft(HeroConstants.CodeRuleRestrain.CodeCoverBit, HeroConstants.CodeRuleRestrain.CodeCoverSymbol);
                menu.Level = parentMenu.Level + 1;
            }
            var permission = input.MapTo<Permission>();
            
            await UnitOfWorkAsync(async (conn,trans) => {
                var permissionId = await _permissionRepository.InsertAndGetIdAsync(permission, conn, trans);
                menu.PermissionId = permissionId;
                await _menuRepository.InsertAsync(menu, conn, trans);

            },Connection);
            return new CreateMenuOutput() { Id = menu.Id, PermissionId = menu.PermissionId, Tips = "新增菜单成功" };
        }

        public async Task Delete(long id)
        {
            var menu = await _menuRepository.SingleOrDefaultAsync(p => p.Id == id);
            if (menu == null) {
                throw new BusinessException($"不存在Id为{id}的菜单信息");
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
    }
}
