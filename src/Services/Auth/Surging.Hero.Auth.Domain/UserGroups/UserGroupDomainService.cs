using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Surging.Core.AutoMapper;
using Surging.Core.Caching;
using Surging.Core.CPlatform.Cache;
using Surging.Core.CPlatform.Exceptions;
using Surging.Core.CPlatform.Runtime.Session;
using Surging.Core.CPlatform.Utilities;
using Surging.Core.Dapper.Manager;
using Surging.Core.Dapper.Repositories;
using Surging.Core.Domain.PagedAndSorted;
using Surging.Core.Domain.PagedAndSorted.Extensions;
using Surging.Core.Lock;
using Surging.Core.Lock.Provider;
using Surging.Hero.Auth.Domain.Permissions;
using Surging.Hero.Auth.Domain.Permissions.Operations;
using Surging.Hero.Auth.Domain.Roles;
using Surging.Hero.Auth.Domain.Shared;
using Surging.Hero.Auth.Domain.Users;
using Surging.Hero.Auth.IApplication.FullAuditDtos;
using Surging.Hero.Auth.IApplication.Role.Dtos;
using Surging.Hero.Auth.IApplication.User.Dtos;
using Surging.Hero.Auth.IApplication.UserGroup.Dtos;
using Surging.Hero.Common;
using Surging.Hero.Common.Runtime.Session;
using Surging.Hero.Organization.IApplication.Department;
using Surging.Hero.Organization.IApplication.Organization;
using Surging.Hero.Organization.IApplication.Position;

namespace Surging.Hero.Auth.Domain.UserGroups
{
    public class UserGroupDomainService : ManagerBase, IUserGroupDomainService
    {
        private readonly ILockerProvider _lockerProvider;
        private readonly IOperationDomainService _operationDomainService;
        private readonly IDapperRepository<Permission, long> _permissionRepository;
        private readonly IRoleDomainService _roleDomainService;
        private readonly IDapperRepository<Role, long> _roleRepository;
        private readonly IDapperRepository<UserGroupPermission, long> _userGroupPermissionRepository;
        private readonly IDapperRepository<UserGroup, long> _userGroupRepository;
        private readonly IDapperRepository<UserGroupRole, long> _userGroupRoleRepository;
        private readonly IDapperRepository<UserInfo, long> _userRepository;
        private readonly IDapperRepository<UserUserGroupRelation, long> _userUserGroupRelationRepository;
        private readonly IDapperRepository<UserGroupDataPermissionOrgRelation, long>
            _userGroupDataPermissionOrgRelationRepository;
        private readonly ISurgingSession _session;
        private readonly ICacheProvider _cacheProvider;

        public UserGroupDomainService(IDapperRepository<UserGroup, long> userGroupRepository,
            IDapperRepository<UserGroupRole, long> userGroupRoleRepository,
            IDapperRepository<UserUserGroupRelation, long> userUserGroupRelationRepository,
            IDapperRepository<UserInfo, long> userRepository,
            IDapperRepository<Role, long> roleRepository,
            IRoleDomainService roleDomainService,
            ILockerProvider lockerProvider,
            IDapperRepository<Permission, long> permissionRepository,
            IDapperRepository<UserGroupPermission, long> userGroupPermissionRepository,
            IOperationDomainService operationDomainService,
            IDapperRepository<UserGroupDataPermissionOrgRelation, long> userGroupDataPermissionOrgRelationRepository)
        {
            _userGroupRepository = userGroupRepository;
            _userGroupRoleRepository = userGroupRoleRepository;
            _userUserGroupRelationRepository = userUserGroupRelationRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _roleDomainService = roleDomainService;
            _lockerProvider = lockerProvider;
            _permissionRepository = permissionRepository;
            _userGroupPermissionRepository = userGroupPermissionRepository;
            _operationDomainService = operationDomainService;
            _userGroupDataPermissionOrgRelationRepository = userGroupDataPermissionOrgRelationRepository;
            _session = NullSurgingSession.Instance;
            _cacheProvider = CacheContainer.GetService<ICacheProvider>(HeroConstants.CacheProviderKey);
        }

        public async Task Create(CreateUserGroupInput input)
        {
            CheckUserDefinedDataPermission(input.DataPermissionType,input.DataPermissionOrgIds);
            using (var locker = await _lockerProvider.CreateLockAsync("CreateUserGroup"))
            {
                await locker.Lock(async () =>
                {

                    var exsitUserGroup = await _userGroupRepository.FirstOrDefaultAsync(p => p.Identification == input.Identification,false);
                    if (exsitUserGroup != null) throw new BusinessException($"系统中已经存在{input.Identification}的用户组");
                    var userGroup = input.MapTo<UserGroup>();
                    await UnitOfWorkAsync(async (conn, trans) =>
                    {
                        var userGroupId = await _userGroupRepository.InsertAndGetIdAsync(userGroup, conn, trans);
                        foreach (var roleId in input.RoleIds)
                        {
                            var roleInfo = await _roleRepository.SingleOrDefaultAsync(p => p.Id == roleId, conn, trans);
                            if (roleInfo == null) throw new BusinessException($"不存在角色Id为{roleId}的角色信息");
                            await _userGroupRoleRepository.InsertAsync(
                                new UserGroupRole {UserGroupId = userGroupId, RoleId = roleId}, conn, trans);
                        }

                        foreach (var permissionId in input.PermissionIds)
                        {
                            var permissionInfo =
                                await _permissionRepository.SingleOrDefaultAsync(p => p.Id == permissionId);
                            if (permissionInfo == null) throw new BusinessException($"不存在权限Id为{permissionId}的权限信息");

                            await _userGroupPermissionRepository.InsertAsync(
                                new UserGroupPermission {UserGroupId = userGroupId, PermissionId = permissionId}, conn,
                                trans);
                        }
                        if (input.DataPermissionType == DataPermissionType.UserDefined)
                        {
                            var insertDataPermissionOrgSql =
                                "INSERT INTO UserGroupDataPermissionOrgRelation(UserGroupId,OrgId,CreateTime,CreateBy) VALUES(@UserGroupId,@OrgId,@CreationTime,@CreatorUserId)";
                            var dataPermissionOrgDatas = new List<UserGroupDataPermissionOrgRelation>();
                            foreach (var orgId in input.DataPermissionOrgIds)
                            {
                                dataPermissionOrgDatas.Add(new UserGroupDataPermissionOrgRelation()
                                {
                                    UserGroupId = userGroupId,
                                    OrgId = orgId,
                                    CreationTime = DateTime.Now,
                                    CreatorUserId = _session.UserId
                                });
                            }
                            await conn.ExecuteAsync(insertDataPermissionOrgSql, dataPermissionOrgDatas, trans);
                        }                        
                    }, Connection);
                });
            }
        }

        public async Task Delete(long id)
        {
            var userGroup = await _userGroupRepository.SingleOrDefaultAsync(p => p.Id == id);
            if (userGroup == null) throw new BusinessException($"不存在Id为{id}的用户组信息");
            _session.CheckLoginUserDataPermision(userGroup.DataPermissionType,"您设置的用户组的数据权限大于您拥有数据权限,系统不允许该操作");
            using (var locker = await _lockerProvider.CreateLockAsync("DeleteUserGroup"))
            {
                await locker.Lock(async () =>
                {
                    await UnitOfWorkAsync(async (conn, trans) =>
                    {
                        await _userGroupRepository.DeleteAsync(p => p.Id == id, conn, trans);
                        await _userGroupRoleRepository.DeleteAsync(p => p.UserGroupId == id, conn, trans);
                        await _userUserGroupRelationRepository.DeleteAsync(p => p.UserGroupId == id, conn, trans);
                        await _userGroupDataPermissionOrgRelationRepository.DeleteAsync(p => p.UserGroupId == id, conn,
                            trans);
                        await RemoveUserGroupCheckPemissionCache(id);
                    }, Connection);
                });
            }
        }

        public async Task Update(UpdateUserGroupInput input)
        {
            CheckUserDefinedDataPermission(input.DataPermissionType,input.DataPermissionOrgIds);
            using (var locker = await _lockerProvider.CreateLockAsync("UpdateUserGroup"))
            {
                await locker.Lock(async () =>
                {
                    var userGroup = await _userGroupRepository.SingleOrDefaultAsync(p => p.Id == input.Id);
                    if (userGroup == null) throw new BusinessException($"不存在Id为{input.Id}的用户组");
                    if (!userGroup.Identification.Equals(input.Identification))
                    {
                        var exsitUserGroup = await _userGroupRepository.FirstOrDefaultAsync(p => p.Identification == input.Identification,false);
                        if (exsitUserGroup != null) throw new BusinessException($"系统中已经存在{input.Identification}的用户组");
                    }
                    userGroup = input.MapTo(userGroup);
                    await UnitOfWorkAsync(async (conn, trans) =>
                    {
                        await _userGroupRepository.UpdateAsync(userGroup, conn, trans);
                        await _userGroupRoleRepository.DeleteAsync(p => p.UserGroupId == userGroup.Id, conn, trans);
                        await _userGroupPermissionRepository.DeleteAsync(p => p.UserGroupId == userGroup.Id, conn,
                            trans);
                        await _userGroupDataPermissionOrgRelationRepository.DeleteAsync(
                            p => p.UserGroupId == userGroup.Id, conn, trans);
                        
                        foreach (var roleId in input.RoleIds)
                        {
                            var roleInfo = await _roleRepository.SingleOrDefaultAsync(p => p.Id == roleId, conn, trans);
                            if (roleInfo == null) throw new BusinessException($"不存在角色Id为{roleId}的角色信息");
                            await _userGroupRoleRepository.InsertAsync(
                                new UserGroupRole {UserGroupId = userGroup.Id, RoleId = roleId}, conn, trans);
                        }

                        foreach (var permissionId in input.PermissionIds)
                        {
                            var permissionInfo =
                                await _permissionRepository.SingleOrDefaultAsync(p => p.Id == permissionId);
                            if (permissionInfo == null) throw new BusinessException($"不存在权限Id为{permissionId}的权限信息");

                            await _userGroupPermissionRepository.InsertAsync(
                                new UserGroupPermission {UserGroupId = userGroup.Id, PermissionId = permissionId}, conn,
                                trans);
                        }
                        if (input.DataPermissionType == DataPermissionType.UserDefined)
                        {
                            var insertDataPermissionOrgSql =
                                "INSERT INTO UserGroupDataPermissionOrgRelation(UserGroupId,OrgId,CreateTime,CreateBy) VALUES(@UserGroupId,@OrgId,@CreationTime,@CreatorUserId)";
                            var dataPermissionOrgDatas = new List<UserGroupDataPermissionOrgRelation>();
                            foreach (var orgId in input.DataPermissionOrgIds)
                            {
                                dataPermissionOrgDatas.Add(new UserGroupDataPermissionOrgRelation()
                                {
                                    UserGroupId = userGroup.Id,
                                    OrgId = orgId,
                                    CreationTime = DateTime.Now,
                                    CreatorUserId = _session.UserId
                                });
                            }
                            await conn.ExecuteAsync(insertDataPermissionOrgSql, dataPermissionOrgDatas, trans);
                            await RemoveUserGroupCheckPemissionCache(userGroup.Id);
                        }                         
                    }, Connection);
                });
            }
        }

        public async Task<IEnumerable<GetDisplayRoleOutput>> GetUserGroupRoleOutputs(long userGroupId, Status? status = null)
        {
            return (await GetUserGroupRoles(userGroupId, status)).MapTo<IEnumerable<GetDisplayRoleOutput>>();
        }
        
        public async Task<IEnumerable<Role>> GetUserGroupRoles(long userGroupId, Status? status = null)
        {
            var sql = @"SELECT r.* FROM UserGroupRole as ugr 
                        LEFT JOIN Role as r on ugr.RoleId = r.Id AND r.IsDeleted=@IsDeleted WHERE ugr.UserGroupId=@UserGroupId ";
            var sqlParams = new Dictionary<string, object>();
            sqlParams.Add("UserGroupId", userGroupId);
            sqlParams.Add("IsDeleted", HeroConstants.UnDeletedFlag);
            if (status.HasValue)
            {
                sql += " AND r.Status=@Status";
                sqlParams.Add("Status", status.Value);
            }

            await using (Connection)
            {
                return (await Connection.QueryAsync<Role>(sql, sqlParams));
            }
        }

        public async Task<IEnumerable<GetUserBasicOutput>> GetUserGroupUsers(long userGroupId)
        {
            var sql = @"SELECT uugr.*,u.* FROM UserUserGroupRelation as uugr 
                        LEFT JOIN UserInfo as u on uugr.UserId = u.Id WHERE uugr.UserGroupId=@UserGroupId";
            await using (Connection)
            {
                return await Connection.QueryAsync<UserUserGroupRelation, UserInfo, GetUserBasicOutput>(sql,
                    (uugr, u) =>
                    {
                        var output = u.MapTo<GetUserBasicOutput>();
                        var positionAppServiceProxy = GetService<IPositionAppService>();

                        var departmentAppServiceProxy = GetService<IDepartmentAppService>();
                        if (u.OrgId.HasValue)
                            output.DeptName = departmentAppServiceProxy.GetByOrgId(u.OrgId.Value).Result.Name;
                        if (u.PositionId.HasValue)
                            output.DeptName = positionAppServiceProxy.Get(u.PositionId.Value).Result.Name;

                        return output;
                    }, new {UserGroupId = userGroupId}, splitOn: "Id");
            }
        }

        public async Task<bool> CheckPermission(long userId, string serviceId)
        {
            var querySql =
                @"SELECT ug.* FROM UserGroup as ug INNER JOIN UserUserGroupRelation as uugr ON ug.Id = uugr.UserGroupId
                            WHERE  ug.IsDeleted=@IsDeleted AND ug.`Status`=@Status  AND uugr.UserId=@UserId ";
            var sqlParams = new Dictionary<string, object>
                {{"IsDeleted", HeroConstants.UnDeletedFlag}, {"Status", Status.Valid}, {"UserId", userId}};
            await using var conn = Connection;
            var userGroups = await conn.QueryAsync<UserGroup>(querySql, sqlParams);
            foreach (var userGroup in userGroups)
            {
                var userGroupPermissions = await GetUserGroupPermissions(userGroup.Id);
                foreach (var userGroupPermission in userGroupPermissions)
                    if (await _operationDomainService.CheckPermission(userGroupPermission.OperationId, serviceId))
                        return true;

                var userGroupRoles = await GetUserGroupRoleOutputs(userGroup.Id, Status.Valid);
                foreach (var userGroupRole in userGroupRoles)
                    if (await _roleDomainService.CheckPermission(userGroupRole.Id, serviceId))
                        return true;
            }

            return false;
        }

        public async Task<string> AllocationUsers(AllocationUserIdsInput input)
        {
            var userGroup = await _userGroupRepository.SingleOrDefaultAsync(p => p.Id == input.UserGroupId);
            if (userGroup == null) throw new BusinessException($"不存在Id为{input.UserGroupId}的用户组");
            using (var locker = await _lockerProvider.CreateLockAsync("AllocationUsers"))
            {
                return await locker.Lock(async () =>
                {
                    await UnitOfWorkAsync(async (conn, trans) =>
                    {
                        foreach (var userId in input.UserIds)
                        {
                            var userInfo = await _userRepository.SingleOrDefaultAsync(p => p.Id == userId, conn, trans);
                            if (userInfo == null) throw new BusinessException($"不存在用户Id为{userId}的用户信息");
                            var userUserGroupRelation =
                                await _userUserGroupRelationRepository.SingleOrDefaultAsync(
                                    p => p.UserId == userId && p.UserGroupId == input.UserGroupId, conn, trans);
                            if (userUserGroupRelation == null)
                                await _userUserGroupRelationRepository.InsertAsync(
                                    new UserUserGroupRelation {UserGroupId = userGroup.Id, UserId = userId}, conn,
                                    trans);
                        }
                        // todo 可只删除受影响的用户
                        await RemoveUserGroupCheckPemissionCache(userGroup.Id);
                    }, Connection);
                    return $"为用户组{userGroup.Name}分配用户成功";
                });
            }
        }

        public async Task DeleteUserGroupUser(DeleteUserGroupUserInput input)
        {
            await _userUserGroupRelationRepository.DeleteAsync(p =>
                p.UserId == input.UserId && p.UserGroupId == input.UserGroupId);
            // todo 可以只删除受影响的用户
            await RemoveUserGroupCheckPemissionCache(input.UserGroupId);
        }

        public async Task<IPagedResult<GetUserNormOutput>> SearchUserGroupUser(QueryUserGroupUserInput query)
        {
            var querySql = @"SELECT u.*,u.CreateBy as CreatorUserId, u.CreateTime as CreationTime, u.UpdateBy as LastModifierUserId, u.UpdateTime as LastModificationTime
FROM UserGroup as ug INNER JOIN UserUserGroupRelation as uugr on ug.Id=uugr.UserGroupId AND ug.IsDeleted=@IsDeleted
INNER JOIN UserInfo as u on uugr.UserId=u.Id AND u.IsDeleted=@IsDeleted
WHERE UserGroupId=@UserGroupId";
            var sqlParams = new Dictionary<string, object>();
            sqlParams.Add("UserGroupId", query.UserGroupId);
            sqlParams.Add("IsDeleted", HeroConstants.UnDeletedFlag);
            if (query.OrgId.HasValue && query.OrgId.Value != 0)
            {
                var subOrgIds = await GetService<IOrganizationAppService>().GetSubOrgIds(query.OrgId.Value);
                querySql += " AND u.OrgId in @DataPermissionOrgIds";
                sqlParams.Add("DataPermissionOrgIds", subOrgIds);
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

            var queryCountSql = string.Format(querySql, "COUNT(u.Id)");

            if (!query.Sorting.IsNullOrEmpty())
                querySql += $" ORDER BY u.{query.Sorting} {query.SortType}";
            else
                querySql += " ORDER BY u.Id DESC";
            querySql += $" LIMIT {(query.PageIndex - 1) * query.PageCount} , {query.PageCount} ";
            querySql = string.Format(querySql,
                "u.*,u.CreateBy as CreatorUserId, u.CreateTime as CreationTime, u.UpdateBy as LastModifierUserId, u.UpdateTime as LastModificationTime");
            await using var conn = Connection;
            var queryResult = await conn.QueryAsync<UserInfo>(querySql, sqlParams);
            var queryCount = await conn.ExecuteScalarAsync<int>(queryCountSql, sqlParams);

            var queryResultOutput = queryResult.MapTo<IEnumerable<GetUserNormOutput>>().GetPagedResult(queryCount);
            foreach (var userOutput in queryResultOutput.Items)
            {
                if (userOutput.OrgId.HasValue)
                    userOutput.DeptName =
                        (await GetService<IDepartmentAppService>().GetByOrgId(userOutput.OrgId.Value)).Name;
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

        public async Task UpdateStatus(UpdateUserGroupStatusInput input)
        {
            var userGroup = await _userGroupRepository.SingleOrDefaultAsync(p => p.Id == input.Id);
            if (userGroup == null) throw new BusinessException($"不存在id为{input.Id}的用户组");
            userGroup.Status = input.Status;
            await _userGroupRepository.UpdateAsync(userGroup);
        }

        public async Task<IEnumerable<UserGroupPermissionModel>> GetUserGroupPermissions(long userGroupId,
            Status status = Status.Valid)
        {
            var sql = @"SELECT p.Id,o.`Name`,o.Title,o.Id as OperationId FROM UserGroupPermission as ugp 
                        LEFT JOIN Permission as p on ugp.PermissionId = p.Id AND p.IsDeleted=@IsDeleted 
												LEFT JOIN Operation as o on o.PermissionId = ugp.PermissionId AND o.IsDeleted=@IsDeleted
												WHERE ugp.UserGroupId=@UserGroupId AND p.Status=@Status";
            var sqlParams = new Dictionary<string, object>();
            sqlParams.Add("UserGroupId", userGroupId);
            sqlParams.Add("IsDeleted", HeroConstants.UnDeletedFlag);
            sqlParams.Add("Status", status);
            await using (Connection)
            {
                await RemoveUserGroupCheckPemissionCache(userGroupId);
                return await Connection.QueryAsync<UserGroupPermissionModel>(sql, sqlParams);
                
            }
        }

        public async Task<IEnumerable<GetDisplayDataPermissionOrgOutput>> GetUserGroupDataPermissionOrgs(long userGroupId)
        {
            var userGroupDataPermissionOrgs = new List<GetDisplayDataPermissionOrgOutput>();
            var userGroupDataPermissionOrgIds =
                (await _userGroupDataPermissionOrgRelationRepository.GetAllAsync(p => p.UserGroupId == userGroupId)).Select(p=> p.OrgId);
            if (userGroupDataPermissionOrgIds.Any())
            {
                var orgAppServiceProxy = GetService<IOrganizationAppService>();
                foreach (var orgId in userGroupDataPermissionOrgIds)
                {
                    var output = new GetDisplayDataPermissionOrgOutput()
                    {
                        Id = orgId,
                        Name = (await orgAppServiceProxy.GetOrg(orgId))?.Name
                    };
                    userGroupDataPermissionOrgs.Add(output);
                }
            }

            return userGroupDataPermissionOrgs;
        }

        private async Task<IEnumerable<UserGroup>> GetUserGroups(long userId)
        {
            var sql = @"SELECT ug.* FROM  UserGroup as ug 
                        LEFT JOIN UserUserGroupRelation as uugr on uugr.UserGroupId = ug.Id WHERE uugr.UserId=@UserId";
            await using (Connection)
            {
                return await Connection.QueryAsync<UserGroup>(sql, new {UserId = userId});
            }
        }

        private async Task<IEnumerable<Role>> GetUserRoles(long userId)
        {
            var sql = @"SELECT r.* FROM UserRole as ur 
                        LEFT JOIN Role as r on ur.RoleId = r.Id WHERE ur.UserId=@UserId";
            await using (Connection)
            {
                return await Connection.QueryAsync<Role>(sql, new {UserId = userId});
            }
        }
        
        private void CheckUserDefinedDataPermission(DataPermissionType? dataPermissionType, long[] orgIds)
        {
            if (dataPermissionType == DataPermissionType.UserDefined)
            {
                if (orgIds == null || !orgIds.Any())
                {
                    throw new BusinessException("设置角色的数据权限为自定义数据权限,则指定的部门不允许为空");
                }
                //
                // DebugCheck.NotNull(_session.OrgId);
                // if (!orgIds.Contains(_session.OrgId.Value))
                // {
                //     throw new BusinessException("用户自定义数据权限指定的部门必须包含您所在的部门");
                // }
            }
        }


        private async Task RemoveUserGroupCheckPemissionCache(long userGroupId)
        {
            var sql = @"SELECT oar.ServiceId FROM OperationActionRelation as oar 
INNER JOIN Operation as o on oar.OperationId=o.Id AND o.IsDeleted=@IsDeleted
INNER JOIN UserGroupPermission as ugp on o.PermissionId=ugp.PermissionId 
WHERE UserGroupId=@UserGroupId";
            var sqlParams = new Dictionary<string, object>() { { "IsDeleted", HeroConstants.UnDeletedFlag },{ "UserGroupId", userGroupId } };
            await using (Connection)
            {
                var userGroupServiceIds = await Connection.QueryAsync<string>(sql, sqlParams);
                foreach (var serviceId in userGroupServiceIds)
                {
                    var cacheKey = string.Format(HeroConstants.CacheKey.PermissionCheck,serviceId,"*");
                    await _cacheProvider.RemoveAsync(cacheKey);
                }
            }

            var userGroupRoleIds =
                (await _userGroupRoleRepository.GetAllAsync(p => p.UserGroupId == userGroupId)).Select(p => p.RoleId);
            foreach (var roleId in userGroupRoleIds)
            {
                await _roleDomainService.RemoveRoleCheckPemissionCache(roleId);
            }
        }
    }
}