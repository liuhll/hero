using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Surging.Cloud.AutoMapper;
using Surging.Cloud.CPlatform;
using Surging.Cloud.CPlatform.Exceptions;
using Surging.Cloud.CPlatform.Runtime.Session;
using Surging.Cloud.CPlatform.Serialization;
using Surging.Cloud.CPlatform.Transport.Implementation;
using Surging.Cloud.CPlatform.Utilities;
using Surging.Cloud.Dapper.Extensions;
using Surging.Cloud.Dapper.Manager;
using Surging.Cloud.Dapper.Repositories;
using Surging.Cloud.Domain.PagedAndSorted;
using Surging.Cloud.Domain.PagedAndSorted.Extensions;
using Surging.Cloud.Lock;
using Surging.Cloud.Lock.Provider;
using Surging.Hero.Auth.Domain.Permissions;
using Surging.Hero.Auth.Domain.Permissions.Menus;
using Surging.Hero.Auth.Domain.Permissions.Operations;
using Surging.Hero.Auth.Domain.Roles;
using Surging.Hero.Auth.Domain.Shared;
using Surging.Hero.Auth.Domain.UserGroups;
using Surging.Hero.Auth.IApplication.FullAuditDtos;
using Surging.Hero.Auth.IApplication.Role.Dtos;
using Surging.Hero.Auth.IApplication.User.Dtos;
using Surging.Hero.Auth.IApplication.UserGroup.Dtos;
using Surging.Hero.Common;
using Surging.Hero.Organization.IApplication.Department;
using Surging.Hero.Organization.IApplication.Organization;
using Surging.Hero.Organization.IApplication.Position;

namespace Surging.Hero.Auth.Domain.Users
{
    public class UserDomainService : ManagerBase, IUserDomainService
    {
        private readonly ILockerProvider _lockerProvider;
        private readonly IMenuDomainService _menuDomainService;
        private readonly IDapperRepository<Menu, long> _menuRepository;
        private readonly IPasswordHelper _passwordHelper;
        private readonly IRoleDomainService _roleDomainService;
        private readonly IDapperRepository<Role, long> _roleRepository;
        private readonly IUserGroupDomainService _userGroupDomainService;
        private readonly IDapperRepository<UserGroup, long> _userGroupRepository;
        private readonly IDapperRepository<UserInfo, long> _userRepository;
        private readonly IDapperRepository<UserRole, long> _userRoleRepository;
        private readonly IDapperRepository<UserUserGroupRelation, long> _userUserGroupRelationRepository;
        private readonly IDapperRepository<UserGroupPermission, long> _userGroupPermissionRepository;
        private readonly IDapperRepository<UserGroupOrganization, long> _userGroupOrganizationDapperRepository;
        private readonly ISurgingSession _session;

        public UserDomainService(IDapperRepository<UserInfo, long> userRepository,
            IDapperRepository<Role, long> roleRepository,
            IDapperRepository<UserRole, long> userRoleRepository,
            IDapperRepository<UserUserGroupRelation, long> userUserGroupRelationRoleRepository,
            IDapperRepository<Menu, long> menuRepository,
            IRoleDomainService roleDomainService,
            IUserGroupDomainService userGroupDomainService,
            IPasswordHelper passwordHelper,
            IMenuDomainService menuDomainService,
            ILockerProvider lockerProvider,
            IDapperRepository<UserGroup, long> userGroupRepository,
            IDapperRepository<UserGroupPermission, long> userGroupPermissionRepository,
            IDapperRepository<UserGroupOrganization, long> userGroupOrganizationDapperRepository)
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
            _userGroupRepository = userGroupRepository;
            _userGroupPermissionRepository = userGroupPermissionRepository;
            _userGroupOrganizationDapperRepository = userGroupOrganizationDapperRepository;
            _session = NullSurgingSession.Instance;
        }

        public async Task<bool> CheckPermission(long userId, string serviceId)
        {
            var userRoles = await GetUserRoles(userId, Status.Valid);
            foreach (var userRole in userRoles)
                if (await _roleDomainService.CheckPermission(userRole.Id, serviceId))
                    return true;
            return await _userGroupDomainService.CheckPermission(userId, serviceId);
        }

        public async Task Create(CreateUserInput input)
        {
            var userInfo = input.MapTo<UserInfo>();
            var departAppServiceProxy = GetService<IDepartmentAppService>();
            if (userInfo.OrgId.HasValue)
                if (!await departAppServiceProxy.Check(userInfo.OrgId.Value))
                    throw new BusinessException($"不存在Id为{userInfo.OrgId}的部门信息");
            var positionAppServiceProxy = GetService<IPositionAppService>();
            if (userInfo.PositionId.HasValue)
                if (!await positionAppServiceProxy.CheckExsit(userInfo.PositionId.Value))
                    throw new BusinessException($"不存在Id为{userInfo.PositionId}的职位信息");

            userInfo.Password = _passwordHelper.EncryptPassword(userInfo.UserName, userInfo.Password);
            using (var locker = await _lockerProvider.CreateLockAsync("CreateUser"))
            {
                await locker.Lock(async () =>
                {
                    await UnitOfWorkAsync(async (conn, trans) =>
                    {
                        var userId = await _userRepository.InsertAndGetIdAsync(userInfo, conn, trans);
                        foreach (var roleId in input.RoleIds)
                        {
                            var role = await _roleRepository.SingleOrDefaultAsync(p => p.Id == roleId);
                            if (role == null) throw new BusinessException($"系统中不存在Id为{roleId}的角色信息");

                            await _userRoleRepository.InsertAsync(new UserRole {UserId = userId, RoleId = roleId}, conn,
                                trans);
                        }

                        foreach (var userGroupId in input.UserGroupIds)
                        {
                            var userGroup = await _userGroupRepository.SingleOrDefaultAsync(p => p.Id == userGroupId);
                            if (userGroup == null) throw new BusinessException($"系统中不存在Id为{userGroupId}的用户组信息");

                            await _userUserGroupRelationRepository.InsertAsync(
                                new UserUserGroupRelation {UserId = userId, UserGroupId = userGroupId}, conn, trans);
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
                    await UnitOfWorkAsync(async (conn, trans) =>
                    {
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
            var userRoleIdsAndGroupIds = await GetAllUserRoleIdsAndUserGroupIds(userId, Status.Valid);
            var userRoleIds = userRoleIdsAndGroupIds.Item1;
            var userGroupIds = userRoleIdsAndGroupIds.Item2;
            var menuSql = @"SELECT DISTINCT m.* FROM RolePermission as rp
INNER JOIN Menu as m ON m.PermissionId = rp.PermissionId AND m.IsDeleted=@IsDeleted
WHERE rp.RoleId in @RoleIds AND rp.TenantId=@TenantId
UNION
SELECT DISTINCT m.* FROM UserGroupPermission as ugp
INNER JOIN Operation as o ON o.PermissionId=ugp.PermissionId AND o.IsDeleted=@IsDeleted 
INNER JOIN Menu as m ON m.Id =o.MenuId AND m.IsDeleted=@IsDeleted
WHERE ugp.UserGroupId in @UserGroupIds AND ugp.TenantId=@TenantId
";
            var operationSql = @"SELECT DISTINCT m.* FROM RolePermission as rp
INNER JOIN Operation as o ON o.PermissionId = rp.PermissionId AND o.IsDeleted=@IsDeleted
INNER JOIN Menu as m ON m.Id = o.MenuId AND m.IsDeleted=@IsDeleted
WHERE rp.RoleId in @RoleIds AND rp.TenantId=@TenantId
UNION
SELECT DISTINCT m.* FROM UserGroupPermission as ugp
INNER JOIN Operation as o ON o.PermissionId = ugp.PermissionId AND o.IsDeleted=@IsDeleted
INNER JOIN Menu as m ON m.Id = o.MenuId AND m.IsDeleted=@IsDeleted
WHERE ugp.UserGroupId in @UserGroupIds AND ugp.TenantId=@TenantId
";
            var allMenus = new List<Menu>();
            await using (Connection)
            {
                var menus = await Connection.QueryAsync<Menu>(menuSql,
                    new {RoleIds = userRoleIds, UserGroupIds = userGroupIds, IsDeleted = HeroConstants.UnDeletedFlag, TenantId = _session.TenantId});
                foreach (var menu in menus) allMenus.AddRange(await _menuDomainService.GetParents(menu.Id));
                var operationMenus = await Connection.QueryAsync<Menu>(operationSql,
                    new {RoleIds = userRoleIds, UserGroupIds = userGroupIds, IsDeleted = HeroConstants.UnDeletedFlag, TenantId = _session.TenantId});
                foreach (var menu in operationMenus) allMenus.AddRange(await _menuDomainService.GetParents(menu.Id));
                return allMenus.Distinct();
            }
        }


        public async Task<IEnumerable<Operation>> GetUserOperation(long userId, long menuId)
        {
            var userRoleIdsAndGroupIds = await GetAllUserRoleIdsAndUserGroupIds(userId, Status.Valid);
            var userRoleIds = userRoleIdsAndGroupIds.Item1;
            var userGroupIds = userRoleIdsAndGroupIds.Item2;

            var sql = @"SELECT DISTINCT o.* FROM RolePermission as rp
LEFT JOIN Operation as o ON o.PermissionId = rp.PermissionId AND o.IsDeleted=@IsDeleted
WHERE rp.RoleId in @RoleIds AND o.MenuId=@MenuId AND rp.TenantId=@TenantId
UNION
SELECT DISTINCT o.* FROM UserGroupPermission as ugp
LEFT JOIN Operation as o ON o.PermissionId = ugp.PermissionId AND o.IsDeleted=@IsDeleted
WHERE ugp.UserGroupId in @UserGroupIds AND o.MenuId=@MenuId AND ugp.TenantId=@TenantId
";
            await using (Connection)
            {
                var operations = await Connection.QueryAsync<Operation>(sql,
                    new
                    {
                        RoleIds = userRoleIds, 
                        UserGroupIds = userGroupIds,
                        Status = Status.Valid,
                        MenuId = menuId,
                        IsDeleted = HeroConstants.UnDeletedFlag,
                        TenantId= _session.TenantId
                    });
                return operations.Distinct();
            }
        }

        public async Task<GetUserNormOutput> GetUserNormInfoById(long id)
        {
            var userInfo = await _userRepository.SingleOrDefaultAsync(p => p.Id == id);
            if (userInfo == null) throw new BusinessException($"系统中不存在Id为{id}的用户");
            var userInfoOutput = userInfo.MapTo<GetUserNormOutput>();
            if (userInfoOutput.OrgId.HasValue)
            {
                userInfoOutput.DeptId =
                    (await GetService<IDepartmentAppService>().GetByOrgId(userInfoOutput.OrgId.Value)).Id;
                userInfoOutput.DeptName =
                    (await GetService<IDepartmentAppService>().GetByOrgId(userInfoOutput.OrgId.Value)).Name;
            }

            await userInfoOutput.SetAuditInfo();

            if (userInfoOutput.CreatorUserId.HasValue)
            {
                var creatorUserInfo =
                    await _userRepository.SingleOrDefaultAsync(p => p.Id == userInfoOutput.CreatorUserId.Value);
                if (creatorUserInfo != null) userInfoOutput.CreatorUserName = creatorUserInfo.ChineseName;
            }

            userInfoOutput.Roles = (await GetUserRoles(id)).MapTo<IEnumerable<GetDisplayRoleOutput>>();
            userInfoOutput.UserGroups = (await GetUserGroups(id)).MapTo<IEnumerable<GetDisplayUserGroupOutput>>();
            return userInfoOutput;
        }

        public async Task<IEnumerable<Role>> GetUserRoles(long userId, Status? status = null)
        {
            var sql = @"SELECT r.* FROM UserRole as ur 
                        LEFT JOIN Role as r on ur.RoleId = r.Id AND r.IsDeleted=@IsDeleted AND r.TenantId=@TenantId
                        WHERE ur.UserId=@UserId AND ur.TenantId=@TenantId";
            var sqlParams = new Dictionary<string, object>();
            sqlParams.Add("UserId", userId);
            sqlParams.Add("IsDeleted", HeroConstants.UnDeletedFlag);
            sqlParams.Add("TenantId", _session.TenantId);
            if (status.HasValue)
            {
                sql += " AND r.Status=@Status";
                sqlParams.Add("Status", status);
            }

            await using (Connection)
            {
                return await Connection.QueryAsync<Role>(sql, sqlParams);
            }
        }

        public async Task ResetPassword(UserInfo userInfo, string newPassword)
        {
            userInfo.Password = _passwordHelper.EncryptPassword(userInfo.UserName, newPassword);
            await _userRepository.UpdateAsync(userInfo);
        }

        public async Task Update(UpdateUserInput input)
        {
            var updateUser = await _userRepository.SingleOrDefaultAsync(p => p.Id == input.Id, false);
            if (updateUser == null) throw new BusinessException($"不存在Id为{input.Id}的账号信息");
            if (input.Phone != updateUser.Phone)
            {
                var existUser = await _userRepository.FirstOrDefaultAsync(p => p.Phone == input.Phone, false);
                if (existUser != null) throw new UserFriendlyException($"已经存在手机号码为{input.Phone}的用户");
            }

            if (input.Email != updateUser.Email)
            {
                var existUser = await _userRepository.FirstOrDefaultAsync(p => p.Email == input.Email, false);
                if (existUser != null) throw new UserFriendlyException($"已经存在Email为{input.Email}的用户");
            }

            var departAppServiceProxy = GetService<IDepartmentAppService>();
            if (input.OrgId.HasValue)
                if (!await departAppServiceProxy.Check(input.OrgId.Value))
                    throw new BusinessException($"不存在Id为{input.OrgId}的部门信息");

            var positionAppServiceProxy = GetService<IPositionAppService>();
            if (input.PositionId.HasValue)
                if (!await positionAppServiceProxy.CheckExsit(input.PositionId.Value))
                    throw new BusinessException($"不存在Id为{input.PositionId}的职位信息");

            updateUser = input.MapTo(updateUser);
            using (var locker = await _lockerProvider.CreateLockAsync("UpdateUser"))
            {
                await locker.Lock(async () =>
                {
                    await UnitOfWorkAsync(async (conn, trans) =>
                    {
                        await _userRepository.UpdateAsync(updateUser, conn, trans);
                        await _userRoleRepository.DeleteAsync(p => p.UserId == updateUser.Id, conn, trans);
                        await _userUserGroupRelationRepository.DeleteAsync(p => p.UserId == updateUser.Id, conn, trans);
                        foreach (var roleId in input.RoleIds)
                        {
                            var role = await _roleRepository.SingleOrDefaultAsync(p => p.Id == roleId);
                            if (role == null) throw new BusinessException($"系统中不存在Id为{roleId}的角色信息");

                            await _userRoleRepository.InsertAsync(
                                new UserRole {UserId = updateUser.Id, RoleId = roleId}, conn, trans);
                        }

                        foreach (var userGroupId in input.UserGroupIds)
                        {
                            var userGroup = await _userGroupRepository.SingleOrDefaultAsync(p => p.Id == userGroupId);
                            if (userGroup == null) throw new BusinessException($"系统中不存在Id为{userGroupId}的用户组信息");

                            await _userUserGroupRelationRepository.InsertAsync(
                                new UserUserGroupRelation {UserId = updateUser.Id, UserGroupId = userGroupId}, conn,
                                trans);
                        }
                    }, Connection);
                });
            }
        }

        public async Task<IPagedResult<GetUserNormOutput>> Search(QueryUserInput query)
        {
            var querySql =
                @"SELECT u.*,u.CreateBy as CreatorUserId, u.CreateTime as CreationTime, u.UpdateBy as LastModifierUserId, u.UpdateTime as LastModificationTime FROM  UserInfo as u 
                WHERE 1=1 ";

            var sqlParams = new Dictionary<string, object>();
            if (query.OrgId.HasValue && query.OrgId.Value != 0)
            {
                var subOrgIds = await GetService<IOrganizationAppService>().GetSubOrgIds(query.OrgId.Value);
                querySql += " AND u.OrgId in @DataPermissionOrgIds";
                sqlParams.Add("DataPermissionOrgIds", subOrgIds);
            }

            if (query.UserGroupId.HasValue && query.UserGroupId.Value > 0)
            {
                var userGroup = await _userGroupRepository.SingleOrDefaultAsync(p => p.Id == query.UserGroupId);
                if (userGroup == null)
                {
                    throw new BusinessException($"不存在IUserGroupId为{query.UserGroupId}的用户组");
                }

                if (!userGroup.IsAllOrg)
                {
                    var userGroupOrgIds =
                        (await _userGroupOrganizationDapperRepository.GetAllAsync(p =>
                            p.UserGroupId == query.UserGroupId))
                        .Select(p => p.OrgId);

                    if (sqlParams.ContainsKey("DataPermissionOrgIds"))
                    {
                        sqlParams["DataPermissionOrgIds"] = userGroupOrgIds;
                    }
                    else
                    {
                        querySql += " AND u.OrgId in @DataPermissionOrgIds";
                        sqlParams.Add("DataPermissionOrgIds", userGroupOrgIds);
                    }
                }
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
                querySql +=
                    " AND (u.UserName like @UserName or u.ChineseName like @ChineseName or u.Phone like @Phone or u.Email like @Email)";
                sqlParams.Add("UserName", $"%{query.SearchKey}%");
                sqlParams.Add("ChineseName", $"%{query.SearchKey}%");
                sqlParams.Add("Phone", $"%{query.SearchKey}%");
                sqlParams.Add("Email", $"%{query.SearchKey}%");
            }

            if (query.UserIds != null && query.UserIds.Ids != null && query.UserIds.Ids.Any())
            {
                var includeKey = query.UserIds.Include ? " in " : "not in ";
                querySql += $" AND u.Id {includeKey} @UserId";
                sqlParams.Add("UserId", query.UserIds.Ids);
            }

            await using var conn = Connection;
            var sortTypes = new Dictionary<string, SortType>();
            if (!query.Sorting.IsNullOrEmpty())
            {
                sortTypes.Add($"u.{query.Sorting}", query.SortType);
            }
            else
            {
                sortTypes.Add($"u.Id", SortType.Desc);
            }

            var queryResult = await conn.QueryDataPermissionPageAsync<UserInfo>(querySql, sqlParams, query.PageIndex,
                query.PageCount, sortTypes: sortTypes, "u.OrgId", deleteField: "u.IsDeleted", tenantField: "u.TenantId");

            var queryResultOutput = queryResult.Item1.MapTo<IEnumerable<GetUserNormOutput>>()
                .GetPagedResult((int) queryResult.Item2);
            foreach (var userOutput in queryResultOutput.Items)
            {
                if (userOutput.OrgId.HasValue)
                {
                    var department = await GetService<IDepartmentAppService>().GetByOrgId(userOutput.OrgId.Value);
                    userOutput.DeptId = department.Id;
                    userOutput.DeptName = department.Name;
                }

                if (userOutput.PositionId.HasValue)
                    userOutput.PositionName =
                        (await GetService<IPositionAppService>().Get(userOutput.PositionId.Value)).Name;
                userOutput.Roles = (await GetUserRoles(userOutput.Id)).MapTo<IEnumerable<GetDisplayRoleOutput>>();
                userOutput.UserGroups =
                    (await GetUserGroups(userOutput.Id)).MapTo<IEnumerable<GetDisplayUserGroupOutput>>();
                await userOutput.SetAuditInfo();
            }

            return queryResultOutput;
        }

        public async Task<CheckPermissionResult> GetDataPermissions(long userId, long permissionId)
        {
            var roles = await GetUserRoles(userId, Status.Valid);
            DataPermissionType dataPermissionType = DataPermissionType.OnlySelfOrg;
            var userDefinedRoleIds = new List<long>();
            foreach (var role in roles)
            {
                var rolePermissions = await _roleDomainService.GetRolePermissions(role.Id);
                if (!rolePermissions.Any(p => p.PermissionId == permissionId))
                {
                    continue;
                }

                if (role.DataPermissionType > dataPermissionType)
                {
                    dataPermissionType = role.DataPermissionType;
                }

                if (dataPermissionType == DataPermissionType.UserDefined)
                {
                    userDefinedRoleIds.Add(role.Id);
                }
            }

            var userGroups = await GetUserGroups(userId);
            var userDefinedUserGroupIds = new List<long>();
            foreach (var userGroup in userGroups)
            {
                var userGroupPermissions = await
                    _userGroupPermissionRepository.GetAllAsync(p => p.UserGroupId == userGroup.Id);
                if (!userGroupPermissions.Any(p => p.PermissionId == permissionId))
                {
                    continue;
                }

                if (userGroup.DataPermissionType.HasValue && userGroup.DataPermissionType.Value > dataPermissionType)
                {
                    dataPermissionType = userGroup.DataPermissionType.Value;
                }

                if (dataPermissionType == DataPermissionType.UserDefined)
                {
                    userDefinedUserGroupIds.Add(userGroup.Id);
                }

                var userGroupRoles = await _userGroupDomainService.GetUserGroupRoles(userGroup.Id, Status.Valid);
                foreach (var userGroupRole in userGroupRoles)
                {
                    var rolePermissions = await _roleDomainService.GetRolePermissions(userGroupRole.Id);
                    if (!rolePermissions.Any(p => p.PermissionId == permissionId))
                    {
                        continue;
                    }

                    if (userGroupRole.DataPermissionType > dataPermissionType)
                    {
                        dataPermissionType = userGroupRole.DataPermissionType;
                    }

                    if (dataPermissionType == DataPermissionType.UserDefined)
                    {
                        userDefinedRoleIds.Add(userGroupRole.Id);
                    }
                }
            }

            var checkPermission = new CheckPermissionResult(dataPermissionType);
            switch (dataPermissionType)
            {
                case DataPermissionType.AllOrg:
                    checkPermission.DataPermissionOrgIds = new long[0];
                    break;
                case DataPermissionType.OnlySelfOrg:
                    DebugCheck.NotNull(_session.OrgId);
                    checkPermission.DataPermissionOrgIds = new[] {_session.OrgId.Value};
                    break;
                case DataPermissionType.SelfAndLowerOrg:
                    DebugCheck.NotNull(_session.OrgId);
                    var organizationAppServiceProxy = GetService<IOrganizationAppService>();
                    var subOrgIds = await organizationAppServiceProxy.GetSubOrgIds(_session.OrgId.Value);
                    checkPermission.DataPermissionOrgIds = subOrgIds.ToArray();
                    break;
                case DataPermissionType.UserDefined:
                    checkPermission.DataPermissionOrgIds =
                        await GetUserDefinedPermissionOrgIds(userDefinedRoleIds, userDefinedUserGroupIds);
                    break;
            }

            return checkPermission;
        }


        private async Task<long[]> GetUserDefinedPermissionOrgIds(List<long> userDefinedRoleIds,
            List<long> userDefinedUserGroupIds)
        {
            var sql = @"
SELECT up.OrgId FROM UserGroupDataPermissionOrgRelation up WHERE up.UserGroupId IN @UserGroupId AND up.TenantId=@TenantId
UNION
SELECT rp.OrgId FROM RoleDataPermissionOrgRelation as rp WHERE rp.RoleId IN @RoleId AND rp.TenantId=@TenantId
";
            await using (Connection)
            {
                return (await Connection.QueryAsync<long>(sql,
                    new {RoleId = userDefinedRoleIds, UserGroupId = userDefinedUserGroupIds, TenantId = _session.TenantId})).ToArray();
            }
        }


        private async Task<Tuple<long[], long[]>> GetAllUserRoleIdsAndUserGroupIds(long userId,
            Status? status = Status.Valid)
        {
            var allUserRoleIds = new List<long>();
            var allUserGroupIds = new List<long>();
            var userRoles = await GetUserRoles(userId, status);
            allUserRoleIds.AddRange(userRoles.Select(p => p.Id));
            var userGroupRelations = await _userUserGroupRelationRepository.GetAllAsync(p => p.UserId == userId);
            foreach (var userGroupRelation in userGroupRelations)
            {
                var userGroup =
                    await _userGroupRepository.SingleOrDefaultAsync(p => p.Id == userGroupRelation.UserGroupId);
                if (userGroup == null) continue;
                if (status.HasValue && userGroup.Status != status) continue;
                allUserGroupIds.Add(userGroup.Id);
                var userGroupRoles = await _userGroupDomainService.GetUserGroupRoleOutputs(userGroup.Id, status);
                allUserRoleIds.AddRange(userGroupRoles.Select(p => p.Id));
            }

            return new Tuple<long[], long[]>(allUserRoleIds.ToArray(), allUserGroupIds.ToArray());
        }

        private async Task<IEnumerable<UserGroup>> GetUserGroups(long userId, Status? status = Status.Valid)
        {
            var sql = @"SELECT ug.* FROM  UserGroup as ug 
                        LEFT JOIN UserUserGroupRelation as uugr on uugr.UserGroupId = ug.Id AND ug.TenantId=@TenantId
                        WHERE uugr.UserId=@UserId and ug.Status=@Status and ug.IsDeleted=@IsDeleted AND uugr.TenantId=@TenantId";
            var sqlParams = new Dictionary<string, object>();
            sqlParams.Add("UserId", userId);
            sqlParams.Add("IsDeleted", HeroConstants.UnDeletedFlag);
            sqlParams.Add("TenantId", _session.TenantId);
            if (status.HasValue)
            {
                sql += " AND ug.Status=@Status";
                sqlParams.Add("Status", status);
            }

            await using (Connection)
            {
                return await Connection.QueryAsync<UserGroup>(sql, sqlParams);
            }
        }
    }
}