using Dapper;
using Surging.Core.AutoMapper;
using Surging.Core.CPlatform.Exceptions;
using Surging.Core.Dapper.Manager;
using Surging.Core.Dapper.Repositories;
using Surging.Hero.Auth.Domain.Permissions.Menus;
using Surging.Hero.Auth.Domain.Permissions.Operations;
using Surging.Hero.Auth.Domain.Roles;
using Surging.Hero.Auth.Domain.UserGroups;
using Surging.Hero.Auth.IApplication.Role.Dtos;
using Surging.Hero.Auth.IApplication.User.Dtos;
using Surging.Hero.Common;
using Surging.Hero.Organization.IApplication.Department;
using Surging.Hero.Organization.IApplication.Position;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Surging.Hero.Auth.Domain.Users
{
    public class UserDomainService : ManagerBase, IUserDomainService
    {
        private readonly IDapperRepository<UserInfo, long> _userRepository;
        private readonly IDapperRepository<Roles.Role, long> _roleRepository;
        private readonly IDapperRepository<UserRole, long> _userRoleRepository;
        private readonly IDapperRepository<UserUserGroupRelation,long> _userUserGroupRelationRoleRepository;
        private readonly IDapperRepository<Menu, long> _menuRepository;
        private readonly IRoleDomainService _roleDomainService;
        private readonly IUserGroupDomainService _userGroupDomainService;
        private readonly IPasswordHelper _passwordHelper;
        private readonly IMenuDomainService _menuDomainService;

        public UserDomainService(IDapperRepository<UserInfo, long> userRepository,
            IDapperRepository<Roles.Role, long> roleRepository,
            IDapperRepository<UserRole, long> userRoleRepository,
            IDapperRepository<UserUserGroupRelation, long> userUserGroupRelationRoleRepository,
            IDapperRepository<Menu, long> menuRepository,
            IRoleDomainService roleDomainService,
            IUserGroupDomainService userGroupDomainService,
            IPasswordHelper passwordHelper,
            IMenuDomainService menuDomainService)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _userUserGroupRelationRoleRepository = userUserGroupRelationRoleRepository;
            _menuRepository = menuRepository;
            _roleDomainService = roleDomainService;
            _userGroupDomainService = userGroupDomainService;
            _passwordHelper = passwordHelper;
            _menuDomainService = menuDomainService;
        }

        public async Task<bool> CheckPermission(long userId, string serviceId)
        {
            var userRoles = await GetUserRoles(userId);
            foreach (var userRole in userRoles)
            {
                if (await _roleDomainService.CheckPermission(userRole.Id, serviceId))
                {
                    return true;
                }
            }
            return false;
        }

        public async Task Create(CreateUserInput input)
        {
            var userInfo = input.MapTo<UserInfo>();
            var departAppServiceProxy = GetService<IDepartmentAppService>();
            if (userInfo.OrgId.HasValue) 
            {
                if (!await departAppServiceProxy.Check(userInfo.OrgId.Value))
                {
                    throw new BusinessException($"不存在Id为{userInfo.OrgId}的部门信息");
                }
            }
            var positionAppServiceProxy = GetService<IPositionAppService>();
            if (userInfo.PositionId.HasValue) {
                if (!await positionAppServiceProxy.Check(userInfo.PositionId.Value))
                {
                    throw new BusinessException($"不存在Id为{userInfo.PositionId}的职位信息");
                }
            }
        
            userInfo.Password = _passwordHelper.EncryptPassword(userInfo.UserName, userInfo.Password);
            await UnitOfWorkAsync(async (conn, trans) => {
                var userId =  await _userRepository.InsertAndGetIdAsync(userInfo, conn, trans);
                foreach (var roleId in input.RoleIds) {
                    var role = await _roleRepository.SingleOrDefaultAsync(p => p.Id == roleId);
                    if (role == null) {
                        throw new BusinessException($"系统中不存在Id为{roleId}的角色信息");
                    }
                    
                    await _userRoleRepository.InsertAsync(new UserRole() { UserId = userId,RoleId = roleId }, conn, trans);
                }
               
            }, Connection);
           
        }

        public async Task Delete(long id)
        {
            await UnitOfWorkAsync(async (conn, trans) => {
                await _userRepository.DeleteAsync(p=>p.Id == id, conn, trans);
                await _userRoleRepository.DeleteAsync(p => p.UserId == id, conn, trans);
                await _userUserGroupRelationRoleRepository.DeleteAsync(p => p.UserId == id, conn, trans);

                // todo: 删除其他关联表

            }, Connection);
        }

        public async Task<IEnumerable<Menu>> GetUserMenu(long userId)
        {
            var userRoleIds = await GetAllUserRoleIds(userId);
            var menuSql = @"SELECT DISTINCT m.* FROM RolePermission as rp
INNER JOIN Menu as m ON m.PermissionId = rp.PermissionId AND m.IsDeleted=0
WHERE rp.RoleId in @RoleId";
            var operationSql = @"SELECT DISTINCT m.* FROM RolePermission as rp
INNER JOIN Operation as o ON o.PermissionId = rp.PermissionId AND o.IsDeleted=0
INNER JOIN Menu as m ON m.Id = o.MenuId AND m.IsDeleted=0
WHERE rp.RoleId in @RoleId";
            var allMenus = new List<Menu>();
            using (Connection) {
                var menus = await Connection.QueryAsync<Menu>(menuSql, new { RoleId = userRoleIds });
                foreach (var menu in menus) 
                {
                    allMenus.AddRange(await _menuDomainService.GetParents(menu.Id));
                }
                var operationMenus = await Connection.QueryAsync<Menu>(operationSql, new { RoleId = userRoleIds });
                foreach (var menu in operationMenus)
                {
                    allMenus.AddRange(await _menuDomainService.GetParents(menu.Id));
                }
                return allMenus.Distinct();
            }

        }

        public async Task<IEnumerable<Operation>> GetUserOperation(long userId, long menuId)
        {
            var userRoleIds = await GetAllUserRoleIds(userId);

            var sql = @"SELECT DISTINCT o.* FROM RolePermission as rp
LEFT JOIN Operation as o ON o.PermissionId = rp.PermissionId AND o.IsDeleted=0
WHERE rp.RoleId in @RoleId AND o.Status=@Status AND o.MenuId=@MenuId";
            using (Connection)
            {
                return await Connection.QueryAsync<Operation>(sql, new { RoleId = userRoleIds, Status = Status.Valid, MenuId = menuId });
            }
        }

        private async Task<long[]> GetAllUserRoleIds(long userId)
        {
            var userRoles = await _userRoleRepository.GetAllAsync(p => p.UserId == userId);
            var userRoleIds = userRoles.Select(p => p.RoleId).ToList();
            var userGroups = await _userUserGroupRelationRoleRepository.GetAllAsync(p => p.UserId == userId);
            foreach (var userGroup in userGroups)
            {
                var userGroupRoles = await _userGroupDomainService.GetUserGroupRoles(userGroup.UserGroupId);
                userRoleIds.AddRange(userGroupRoles.Select(p => p.Id));
            }

            return userRoleIds.ToArray();
        }

        public async Task<GetUserNormOutput> GetUserNormInfoById(long id)
        {
            var userInfo = await _userRepository.GetAsync(id);
            var userInfoOutput = userInfo.MapTo<GetUserNormOutput>();
            if (userInfoOutput.OrgId.HasValue) 
            {
                userInfoOutput.DeptId = (await GetService<IDepartmentAppService>().GetByOrgId(userInfoOutput.OrgId.Value)).Id;
                userInfoOutput.DeptName = (await GetService<IDepartmentAppService>().GetByOrgId(userInfoOutput.OrgId.Value)).Name;
            }
            if (userInfoOutput.PositionId.HasValue) 
            {
                userInfoOutput.PositionName = (await GetService<IPositionAppService>().Get(userInfoOutput.PositionId.Value)).Name;
            }
            if (userInfoOutput.LastModifierUserId.HasValue) 
            {
                var modifyUserInfo = await _userRepository.SingleOrDefaultAsync(p => p.Id == userInfoOutput.LastModifierUserId.Value);
                if (modifyUserInfo != null) 
                {
                    userInfoOutput.LastModificationUserName = modifyUserInfo.ChineseName;
                }
            }
            userInfoOutput.Roles = (await GetUserRoles(id)).MapTo<IEnumerable<GetDisplayRoleOutput>>();
            return userInfoOutput;
        }

     

        public async Task<IEnumerable<Role>> GetUserRoles(long userId)
        {
            var sql = @"SELECT r.* FROM UserRole as ur 
                        LEFT JOIN Role as r on ur.RoleId = r.Id WHERE ur.UserId=@UserId";
            using (Connection)
            {
                return (await Connection.QueryAsync<Role>(sql, param: new { UserId = userId }));
            }
        }

        public async Task ResetPassword(UserInfo userInfo, string newPassword)
        {
            userInfo.Password = _passwordHelper.EncryptPassword(userInfo.UserName, newPassword);
            await _userRepository.UpdateAsync(userInfo);
        }

        public async Task Update(UpdateUserInput input)
        {
            var updateUser = await _userRepository.SingleOrDefaultAsync(p => p.Id == input.Id);
            if (updateUser == null)
            {
                throw new BusinessException($"不存在Id为{input.Id}的账号信息");
            }
            if (input.Phone != updateUser.Phone)
            {
                var existUser = await _userRepository.FirstOrDefaultAsync(p => p.Phone == input.Phone);
                if (existUser != null)
                {
                    throw new UserFriendlyException($"已经存在手机号码为{input.Phone}的用户");
                }
            }
            if (input.Email != updateUser.Email)
            {
                var existUser = await _userRepository.FirstOrDefaultAsync(p => p.Email == input.Email);
                if (existUser != null)
                {
                    throw new UserFriendlyException($"已经存在Email为{input.Email}的用户");
                }
            }

            var departAppServiceProxy = GetService<IDepartmentAppService>();
            if (input.OrgId.HasValue) {
                if (!await departAppServiceProxy.Check(input.OrgId.Value))
                {
                    throw new BusinessException($"不存在Id为{input.OrgId}的部门信息");
                }
            }

            var positionAppServiceProxy = GetService<IPositionAppService>();
            if (input.PositionId.HasValue) 
            {
                if (!await positionAppServiceProxy.Check(input.PositionId.Value))
                {
                    throw new BusinessException($"不存在Id为{input.PositionId}的职位信息");
                }
            }
             
            updateUser = input.MapTo(updateUser);
            await UnitOfWorkAsync(async (conn, trans) => {
                await _userRepository.UpdateAsync(updateUser, conn, trans);
                await _userRoleRepository.DeleteAsync(p => p.UserId == updateUser.Id, conn, trans);
                foreach (var roleId in input.RoleIds)
                {
                    var role = await _roleRepository.SingleOrDefaultAsync(p => p.Id == roleId);
                    if (role == null)
                    {
                        throw new BusinessException($"系统中不存在Id为{roleId}的角色信息");
                    }
                    
                    await _userRoleRepository.InsertAsync(new UserRole() { UserId = updateUser.Id, RoleId = roleId },conn,trans);
                }

            }, Connection);
        }
    }
}
