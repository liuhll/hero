using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Surging.Cloud.AutoMapper;
using Surging.Cloud.CPlatform.Exceptions;
using Surging.Cloud.Dapper.Manager;
using Surging.Cloud.Dapper.Repositories;
using Surging.Cloud.Lock;
using Surging.Cloud.Lock.Provider;
using Surging.Hero.Auth.Domain.Permissions.Operations;
using Surging.Hero.Auth.Domain.Shared.Menus;
using Surging.Hero.Auth.Domain.Shared.Permissions;
using Surging.Hero.Auth.IApplication.Permission.Dtos;
using Surging.Hero.Common;

namespace Surging.Hero.Auth.Domain.Permissions.Menus
{
    public class MenuDomainService : ManagerBase, IMenuDomainService
    {
        private readonly ILockerProvider _lockerProvider;
        private readonly IDapperRepository<Menu, long> _menuRepository;
        private readonly IDapperRepository<OperationActionRelation, long> _operationActionRepository;
        private readonly IDapperRepository<Operation, long> _operationRepository;
        private readonly IDapperRepository<Permission, long> _permissionRepository;

        public MenuDomainService(IDapperRepository<Menu, long> menuRepository,
            IDapperRepository<Permission, long> permissionRepository,
            IDapperRepository<Operation, long> operationRepository,
            IDapperRepository<OperationActionRelation, long> operationActionRepository,
            ILockerProvider lockerProvider)
        {
            _menuRepository = menuRepository;
            _permissionRepository = permissionRepository;
            _operationRepository = operationRepository;
            _operationActionRepository = operationActionRepository;
            _lockerProvider = lockerProvider;
        }

        public async Task<CreateMenuOutput> Create(CreateMenuInput input)
        {
            var menu = input.MapTo<Menu>();
            long menuParentId = 0;
            var exsitMenu = await _menuRepository.FirstOrDefaultAsync(p => p.Name == input.Name);
            if (exsitMenu != null) throw new BusinessException($"系统中已经存在{input.Name}的菜单,请勿重复添加");
            var exsitPermission = await _permissionRepository.FirstOrDefaultAsync(p => p.Name == input.Name);
            if (exsitPermission != null) throw new BusinessException($"系统中已经存在{input.Name}的权限,请勿重复添加");
            if (menu.Mold == MenuMold.Top)
            {
                var thisLevelMenuCount = await _menuRepository.GetCountAsync(p => p.Mold == MenuMold.Top);
                menu.Code = (thisLevelMenuCount + 1).ToString().PadRight(HeroConstants.CodeRuleRestrain.CodeCoverBit,
                    HeroConstants.CodeRuleRestrain.CodeCoverSymbol);
                menu.Level = 1;
            }
            else
            {
                var parentMenu =
                    await _menuRepository.SingleOrDefaultAsync(p => p.PermissionId == input.ParentPermissionId);
                if (parentMenu == null) throw new BusinessException($"不存在PermissionId为{input.ParentPermissionId}的菜单信息");
                menuParentId = parentMenu.Id;
                var thisLevelMenuCount =
                    await _menuRepository.GetCountAsync(p => p.Mold == MenuMold.SubMenu && p.ParentId == parentMenu.Id);
                menu.Code = parentMenu.Code + HeroConstants.CodeRuleRestrain.CodeSeparator + (thisLevelMenuCount + 1)
                    .ToString().PadLeft(HeroConstants.CodeRuleRestrain.CodeCoverBit,
                        HeroConstants.CodeRuleRestrain.CodeCoverSymbol);
                menu.Level = parentMenu.Level + 1;
            }

            var permission = input.MapTo<Permission>();

            using (var locker = await _lockerProvider.CreateLockAsync("CreateMenu"))
            {
                return await locker.Lock(async () =>
                {
                    await UnitOfWorkAsync(async (conn, trans) =>
                    {
                        var permissionId = await _permissionRepository.InsertAndGetIdAsync(permission, conn, trans);
                        menu.PermissionId = permissionId;
                        menu.ParentId = menuParentId;
                        await _menuRepository.InsertAsync(menu, conn, trans);
                    }, Connection);
                    return new CreateMenuOutput {Id = menu.Id, PermissionId = menu.PermissionId, Tips = "新增菜单成功"};
                });
            }
        }

        public async Task Delete(long permissionId)
        {
            var menu = await _menuRepository.SingleOrDefaultAsync(p => p.PermissionId == permissionId);
            if (menu == null) throw new BusinessException($"不存在PermissionId为{permissionId}的菜单信息");
            var allNeedDeleteMenus = await _menuRepository.GetAllAsync(p => p.Code.Contains(menu.Code));

            using (var locker = await _lockerProvider.CreateLockAsync("DeleteMenu"))
            {
                await locker.Lock(async () =>
                {
                    await UnitOfWorkAsync(async (conn, trans) =>
                    {
                        await _menuRepository.DeleteAsync(p => p.Code.Contains(menu.Code), conn, trans);
                        foreach (var needDeleteMenu in allNeedDeleteMenus)
                        {
                            await _permissionRepository.DeleteAsync(p => p.Id == needDeleteMenu.PermissionId, conn,
                                trans);
                            var operations = await _operationRepository.GetAllAsync(p => p.MenuId == needDeleteMenu.Id);
                            await _operationRepository.DeleteAsync(p => p.MenuId == needDeleteMenu.Id, conn, trans);
                            if (operations.Any())
                                foreach (var operation in operations)
                                {
                                    await _operationActionRepository.DeleteAsync(p => p.OperationId == operation.Id,
                                        conn, trans);
                                    await _permissionRepository.DeleteAsync(p => p.Id == operation.PermissionId, conn,
                                        trans);
                                }
                        }
                    }, Connection);
                });
            }
        }

        public async Task<IEnumerable<Menu>> GetAll()
        {
            return await _menuRepository.GetAllAsync();
        }


        public async Task<UpdateMenuOutput> Update(UpdateMenuInput input)
        {
            var menu = await _menuRepository.SingleOrDefaultAsync(p => p.Id == input.Id);
            if (menu == null) throw new BusinessException($"不存在Id为{input.Id}的菜单信息");
            var permission = await _permissionRepository.GetAsync(menu.PermissionId);
            menu = input.MapTo(menu);
            permission = input.MapTo(permission);
            using (var locker = await _lockerProvider.CreateLockAsync("UpdateMenu"))
            {
                return await locker.Lock(async () =>
                {
                    await UnitOfWorkAsync(async (conn, trans) =>
                    {
                        await _permissionRepository.UpdateAsync(permission, conn, trans);
                        await _menuRepository.UpdateAsync(menu, conn, trans);
                    }, Connection);
                    return new UpdateMenuOutput {Id = menu.Id, PermissionId = menu.PermissionId, Tips = "更新菜单成功"};
                });
            }
        }

        public async Task<IEnumerable<GetPermissionTreeOutput>> GetTree()
        {
            var menus = await _menuRepository.GetAllAsync();
            var permissionTree = await BuildPermissionTree(menus);
            return permissionTree;
        }

        public async Task<IEnumerable<Menu>> GetParents(long menuId, bool isIncludeSelf = true)
        {
            var parentMenus = new List<Menu>();
            var menu = await _menuRepository.GetAsync(menuId);
            if (isIncludeSelf) parentMenus.Add(menu);
            return await GetParentMenus(menu.ParentId, parentMenus);
        }

        public async Task<IEnumerable<Menu>> GetParentsByPermissionId(long permissionId, PermissionMold mold,
            bool isIncludeSelf = true)
        {
            var parentMenus = new List<Menu>();
            if (mold == PermissionMold.Menu)
            {
                var menu = await _menuRepository.SingleAsync(p => p.PermissionId == permissionId);
                if (isIncludeSelf) parentMenus.Add(menu);
                return await GetParentMenus(menu.ParentId, parentMenus);
            }
            else
            {
                var operation = await _operationRepository.SingleAsync(p => p.PermissionId == permissionId);
                var menu = await _menuRepository.SingleAsync(p => p.Id == operation.MenuId);
                parentMenus.Add(menu);
                return await GetParentMenus(menu.ParentId, parentMenus);
            }
        }


        private async Task<IEnumerable<GetPermissionTreeOutput>> BuildPermissionTree(IEnumerable<Menu> menus)
        {
            var topMenus = menus.Where(p => p.Mold == MenuMold.Top);
            var topPermissionOutputs = topMenus.MapTo<IEnumerable<GetPermissionTreeOutput>>();
            foreach (var topPermission in topPermissionOutputs)
            {
                topPermission.FullName = topPermission.Name;
                topPermission.Children = await BuildPermissionChildren(topPermission, menus);
            }

            return topPermissionOutputs;
        }

        private async Task<IEnumerable<GetPermissionTreeOutput>> BuildPermissionChildren(
            GetPermissionTreeOutput permissionTreeOutput, IEnumerable<Menu> menus)
        {
            var permissionChildren = new List<GetPermissionTreeOutput>();
            var menuChildren = menus.Where(p => p.ParentId == permissionTreeOutput.Id);

            var menuPermissionChildrenOutput = menuChildren.MapTo<IEnumerable<GetPermissionTreeOutput>>();
            permissionChildren.AddRange(menuPermissionChildrenOutput);

            if (menuPermissionChildrenOutput.Any())
                foreach (var menuChildOutput in menuPermissionChildrenOutput)
                {
                    menuChildOutput.ParentPermissionId = permissionTreeOutput.PermissionId;
                    menuChildOutput.FullName = permissionTreeOutput.FullName +
                                               HeroConstants.CodeRuleRestrain.CodeSeparator + menuChildOutput.Name;
                    menuChildOutput.Children = await BuildPermissionChildren(menuChildOutput, menus);
                }


            var operationChildren = await _operationRepository.GetAllAsync(p => p.MenuId == permissionTreeOutput.Id);
            var operationChildrenOutput = operationChildren.MapTo<IEnumerable<GetPermissionTreeOutput>>();
            foreach (var operationChildOutput in operationChildrenOutput)
            {
                operationChildOutput.ParentPermissionId = permissionTreeOutput.PermissionId;
                operationChildOutput.FullName = permissionTreeOutput.FullName +
                                                HeroConstants.CodeRuleRestrain.CodeSeparator +
                                                operationChildOutput.Name;
            }

            permissionChildren.AddRange(operationChildrenOutput);
            return permissionChildren;
        }

        private async Task<IEnumerable<Menu>> GetParentMenus(long parentId, List<Menu> menus)
        {
            if (parentId != 0)
            {
                var parentMenu = await _menuRepository.GetAsync(parentId);
                menus.Add(parentMenu);
                if (parentMenu.ParentId != 0) await GetParentMenus(parentMenu.ParentId, menus);
            }

            return menus;
        }
    }
}