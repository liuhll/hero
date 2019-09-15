using System;
using System.Threading.Tasks;
using Surging.Core.AutoMapper;
using Surging.Core.CPlatform.Exceptions;
using Surging.Core.Dapper.Manager;
using Surging.Core.Dapper.Repositories;
using Surging.Hero.Auth.Domain.Shared.Menus;
using Surging.Hero.Auth.IApplication.Permission.Dtos;
using Surging.Hero.Common;

namespace Surging.Hero.Auth.Domain.Permissions.Menus
{
    public class MenuDomainService : ManagerBase, IMenuDomainService
    {
        private readonly IDapperRepository<Menu, long> _menuRepository;
        private readonly IDapperRepository<Permission, long> _permissionRepository;

        public MenuDomainService(IDapperRepository<Menu, long> menuRepository,
            IDapperRepository<Permission, long> permissionRepository) {
            _menuRepository = menuRepository;
            _permissionRepository = permissionRepository;

        }

        public async Task Create(CreateMenuInput input)
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
        }

        public async Task Update(UpdateMenuInput input)
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
        }
    }
}
