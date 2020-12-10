using Dapper;
using Surging.Core.AutoMapper;
using Surging.Core.CPlatform.Exceptions;
using Surging.Core.CPlatform.Utilities;
using Surging.Core.Dapper.Manager;
using Surging.Core.Dapper.Repositories;
using Surging.Core.Domain.PagedAndSorted;
using Surging.Core.Domain.PagedAndSorted.Extensions;
using Surging.Core.Lock;
using Surging.Core.Lock.Provider;
using Surging.Hero.Auth.Domain.Permissions.Menus;
using Surging.Hero.Auth.Domain.Permissions.Operations;
using Surging.Hero.Auth.Domain.Roles;
using Surging.Hero.Auth.Domain.UserGroups;
using Surging.Hero.Auth.IApplication.Role.Dtos;
using Surging.Hero.Auth.IApplication.User.Dtos;
using Surging.Hero.Common;
using Surging.Hero.Organization.IApplication.Department;
using Surging.Hero.Organization.IApplication.Organization;
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
        private readonly IDapperRepository<UserUserGroupRelation,long> _userUserGroupRelationRepository;
        private readonly IDapperRepository<Menu, long> _menuRepository;
        private readonly IRoleDomainService _roleDomainService;
        private readonly IUserGroupDomainService _userGroupDomainService;
        private readonly IPasswordHelper _passwordHelper;
        private readonly IMenuDomainService _menuDomainService;
        private readonly ILockerProvider _lockerProvider;

        public UserDomainService(IDapperRepository<UserInfo, long> userRepository,
            IDapperRepository<Roles.Role, long> roleRepository,
            IDapperRepository<UserRole, long> userRoleRepository,
            IDapperRepository<UserUserGroupRelation, long> userUserGroupRelationRoleRepository,
            IDapperRepository<Menu, long> menuRepository,
            IRoleDomainService roleDomainService,
            IUserGroupDomainService userGroupDomainService,
            IPasswordHelper passwordHelper,
            IMenuDomainService menuDomainService, 
            ILockerProvider lockerProvider)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _userUserGroupRelationRepository = userUserGroupRelationRoleRepository;
            _menuRepository = menuRepository;
            _roleDomainService = roleDomainService;
            _userGroupDomainService = userGroupDomainService;
            _passwordHelper = passwordHelper;
            _menuDomainService = menuDomainService;
            _lockerProvider = lockerProvider;
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
                if (!await positionAppServiceProxy.CheckExsit(userInfo.PositionId.Value))
                {
                    throw new BusinessException($"不存在Id为{userInfo.PositionId}的职位信息");
                }
            }
        
            userInfo.Password = _passwordHelper.EncryptPassword(userInfo.UserName, userInfo.Password);
            using (var locker = await _lockerProvider.CreateLockAsync("CreateUser"))
            {
                await locker.Lock(async () =>
                {
                    await UnitOfWorkAsync(async (conn, trans) => {
                        var userId = await _userRepository.InsertAndGetIdAsync(userInfo, conn, trans);
                        foreach (var roleId in input.RoleIds)
                        {
                            var role = await _roleRepository.SingleOrDefaultAsync(p => p.Id == roleId);
                            if (role == null)
                            {
                                throw new BusinessException($"系统中不存在Id为{roleId}的角色信息");
                            }

                            await _userRoleRepository.InsertAsync(new UserRole() { UserId = userId, RoleId = roleId }, conn, trans);
                        }

                    }, Connection);
                });
            }

           
        }

        public async Task Delete(long id)
        {
            using (var locker = await _lockerProvider.CreateLockAsync("DeleteUser"))
            {
                await locker.Lock(async () =>
                {
                    await UnitOfWorkAsync(async (conn, trans) => {
                        await _userRepository.DeleteAsync(p => p.Id == id, conn, trans);
                        await _userRoleRepository.DeleteAsync(p => p.UserId == id, conn, trans);
                        await _userUserGroupRelationRepository.DeleteAsync(p => p.UserId == id, conn, trans);

                        // todo: 删除其他关联表

                    }, Connection);
                });
            }

            
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
            var userGroups = await _userUserGroupRelationRepository.GetAllAsync(p => p.UserId == userId);
            foreach (var userGroup in userGroups)
            {
                var userGroupRoles = await _userGroupDomainService.GetUserGroupRoles(userGroup.UserGroupId);
                userRoleIds.AddRange(userGroupRoles.Select(p => p.Id));
            }

            return userRoleIds.ToArray();
        }

        public async Task<GetUserNormOutput> GetUserNormInfoById(long id)
        {
            var userInfo = await _userRepository.SingleOrDefaultAsync(p=> p.Id == id);
            if (userInfo == null) 
            {
                throw new BusinessException($"系统中不存在Id为{id}的用户");
            }
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
            if (userInfoOutput.CreatorUserId.HasValue)
            {
                var creatorUserInfo = (await _userRepository.SingleOrDefaultAsync(p => p.Id == userInfoOutput.CreatorUserId.Value));
                if (creatorUserInfo != null)
                {
                    userInfoOutput.CreatorUserName = creatorUserInfo.ChineseName;
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
                if (!await positionAppServiceProxy.CheckExsit(input.PositionId.Value))
                {
                    throw new BusinessException($"不存在Id为{input.PositionId}的职位信息");
                }
            }
             
            updateUser = input.MapTo(updateUser);
            using (var locker = await _lockerProvider.CreateLockAsync("UpdateUser"))
            {
                await locker.Lock(async () =>
                {
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

                            await _userRoleRepository.InsertAsync(new UserRole() { UserId = updateUser.Id, RoleId = roleId }, conn, trans);
                        }

                    }, Connection);
                });
            }


            
        }

        public async Task<IPagedResult<GetUserNormOutput>> Search(QueryUserInput query)
        {
            var querySql = @"SELECT {0} FROM  UserInfo as u 
                WHERE u.IsDeleted=0 ";

            var sqlParams = new Dictionary<string, object>();
            if (query.OrgId.HasValue && query.OrgId.Value != 0)
            {
                var subOrgIds = await GetService<IOrganizationAppService>().GetSubOrgIds(query.OrgId.Value);
                querySql += " AND u.OrgId in @OrgIds";
                sqlParams.Add("OrgIds", subOrgIds);
            }
            if (query.Status.HasValue) 
            {
                querySql += " AND u.Status=@Status";
                sqlParams.Add("Status", query.Status.Value);
            }
            if (query.PositionId.HasValue && query.PositionId.Value != 0)
            {
                querySql += " AND u.PositionId=@PositionId";
                sqlParams.Add("PositionId", query.PositionId.Value);
            }
            if (!query.SearchKey.IsNullOrEmpty())
            {
                querySql += " AND (u.UserName like @UserName or u.ChineseName like @ChineseName or u.Phone like @Phone or u.Email like @Email)";
                sqlParams.Add("UserName", $"%{query.SearchKey}%");
                sqlParams.Add("ChineseName", $"%{query.SearchKey}%");
                sqlParams.Add("Phone", $"%{query.SearchKey}%");
                sqlParams.Add("Email", $"%{query.SearchKey}%");
            }
            var queryCountSql = string.Format(querySql, "COUNT(u.Id)");

            if (!query.Sorting.IsNullOrEmpty())
            {
                querySql += $" ORDER BY u.{query.Sorting} {query.SortType}";
            }
            else
            {
                querySql += $" ORDER BY u.Id DESC";
            }
            querySql += $" LIMIT {(query.PageIndex - 1) * query.PageCount} , {query.PageCount} ";
            querySql = string.Format(querySql, "u.*,u.CreateBy as CreatorUserId, u.CreateTime as CreationTime, u.UpdateBy as LastModifierUserId, u.UpdateTime as LastModificationTime");
            using (var conn = Connection)
            {
                var queryResult = await conn.QueryAsync<UserInfo>(querySql, sqlParams);
                var queryCount = await conn.ExecuteScalarAsync<int>(queryCountSql, sqlParams);

                var queryResultOutput = queryResult.MapTo<IEnumerable<GetUserNormOutput>>().GetPagedResult(queryCount);
                foreach (var userOutput in queryResultOutput.Items)
                {
                    if (userOutput.OrgId.HasValue)
                    {
                        var department = await GetService<IDepartmentAppService>().GetByOrgId(userOutput.OrgId.Value);
                        userOutput.DeptId = department.Id;
                        userOutput.DeptName = department.Name;
                    }
                    if (userOutput.PositionId.HasValue)
                    {
                        userOutput.PositionName = (await GetService<IPositionAppService>().Get(userOutput.PositionId.Value)).Name;
                    }
                    userOutput.Roles = (await GetUserRoles(userOutput.Id)).MapTo<IEnumerable<GetDisplayRoleOutput>>();
                    if (userOutput.LastModifierUserId.HasValue)
                    {
                        var modifyUserInfo = (await _userRepository.SingleOrDefaultAsync(p => p.Id == userOutput.LastModifierUserId.Value));
                        if (modifyUserInfo != null)
                        {
                            userOutput.LastModificationUserName = modifyUserInfo.ChineseName;
                        }

                    }
                    if (userOutput.CreatorUserId.HasValue)
                    {
                        var creatorUserInfo = (await _userRepository.SingleOrDefaultAsync(p => p.Id == userOutput.CreatorUserId.Value));
                        if (creatorUserInfo != null)
                        {
                            userOutput.CreatorUserName = creatorUserInfo.ChineseName;
                        }
                    }

                }

                return queryResultOutput;
            }
        }
    }
}
