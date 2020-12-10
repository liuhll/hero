using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
using Surging.Hero.Auth.Domain.Roles;
using Surging.Hero.Auth.Domain.Users;
using Surging.Hero.Auth.IApplication.Role.Dtos;
using Surging.Hero.Auth.IApplication.User.Dtos;
using Surging.Hero.Auth.IApplication.UserGroup.Dtos;
using Surging.Hero.Common;
using Surging.Hero.Organization.IApplication.Department;
using Surging.Hero.Organization.IApplication.Organization;
using Surging.Hero.Organization.IApplication.Position;

namespace Surging.Hero.Auth.Domain.UserGroups
{
    public class UserGroupDomainService : ManagerBase, IUserGroupDomainService
    {
        private readonly IDapperRepository<UserGroup, long> _userGroupRepository;
        private readonly IDapperRepository<UserGroupRole, long> _userGroupRoleRepository;
        private readonly IDapperRepository<UserUserGroupRelation, long> _userUserGroupRelationRepository;
        private readonly IDapperRepository<UserInfo, long> _userRepository;
        private readonly IDapperRepository<Roles.Role, long> _roleRepository;
        private readonly IRoleDomainService _roleDomainService;
        private readonly ILockerProvider _lockerProvider;

        public UserGroupDomainService(IDapperRepository<UserGroup, long> userGroupRepository,
            IDapperRepository<UserGroupRole, long> userGroupRoleRepository,
            IDapperRepository<UserUserGroupRelation, long> userUserGroupRelationRepository,
            IDapperRepository<UserInfo, long> userRepository,
            IDapperRepository<Roles.Role, long> roleRepository,
            IRoleDomainService roleDomainService,
            ILockerProvider lockerProvider)
        {
            _userGroupRepository = userGroupRepository;
            _userGroupRoleRepository = userGroupRoleRepository;
            _userUserGroupRelationRepository = userUserGroupRelationRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _roleDomainService = roleDomainService;
            _lockerProvider = lockerProvider;
        }

        public async Task Create(CreateUserGroupInput input)
        {
            var exsitUserGroup = await _userGroupRepository.FirstOrDefaultAsync(p => p.Name == input.Name.Trim());
            if (exsitUserGroup != null) 
            {
                throw new BusinessException($"系统中已经存在{input.Name}的用户组");
            }
            var userGroup = input.MapTo<UserGroup>();
            using (var locker = await _lockerProvider.CreateLockAsync("CreateUserGroup"))
            {
                await locker.Lock(async () =>
                {
                    await UnitOfWorkAsync(async (conn, trans) =>
                    {
                        var userGroupId = await _userGroupRepository.InsertAndGetIdAsync(userGroup, conn, trans);
                        foreach (var roleId in input.RoleIds)
                        {
                            var roleInfo = await _roleRepository.SingleOrDefaultAsync(p => p.Id == roleId, conn, trans);
                            if (roleInfo == null)
                            {
                                throw new BusinessException($"不存在用户Id为{roleId}的角色信息");
                            }
                            await _userGroupRoleRepository.InsertAsync(new UserGroupRole() { UserGroupId = userGroupId, RoleId = roleId }, conn, trans);
                        }
                    }, Connection);
                });
            }
           

        }

        public async Task Delete(long id)
        {
            var userGroup = await _userGroupRepository.SingleOrDefaultAsync(p => p.Id == id);
            if (userGroup == null)
            {
                throw new BusinessException($"不存在Id为{id}的用户组信息");
            }
            using (var locker = await _lockerProvider.CreateLockAsync("DeleteUserGroup"))
            {
                await locker.Lock(async () =>
                {
                    await UnitOfWorkAsync(async (conn, trans) =>
                    {
                        await _userGroupRepository.DeleteAsync(p => p.Id == id, conn, trans);
                        await _userGroupRoleRepository.DeleteAsync(p => p.UserGroupId == id, conn, trans);
                        await _userUserGroupRelationRepository.DeleteAsync(p => p.UserGroupId == id, conn, trans);

                    }, Connection);
                });
            }
    
        }

        public async Task Update(UpdateUserGroupInput input)
        {
            var userGroup = await _userGroupRepository.SingleOrDefaultAsync(p => p.Id == input.Id);
            if (userGroup == null)
            {
                throw new BusinessException($"不存在Id为{input.Id}的用户组");
            }
            if (!userGroup.Name.Equals(input.Name)) 
            {
                var exsitUserGroup = await _userGroupRepository.FirstOrDefaultAsync(p => p.Name == input.Name.Trim());
                if (exsitUserGroup != null)
                {
                    throw new BusinessException($"系统中已经存在{input.Name}的用户组");
                }
            }
            userGroup = input.MapTo(userGroup);
            using (var locker = await _lockerProvider.CreateLockAsync("UpdateUserGroup"))
            {
                await locker.Lock(async () =>
                {
                    await UnitOfWorkAsync(async (conn, trans) =>
                    {
                        await _userGroupRepository.UpdateAsync(userGroup, conn, trans);
                        await _userGroupRoleRepository.DeleteAsync(p => p.UserGroupId == userGroup.Id, conn, trans);

                        foreach (var roleId in input.RoleIds)
                        {
                            var roleInfo = await _roleRepository.SingleOrDefaultAsync(p => p.Id == roleId, conn, trans);
                            if (roleInfo == null)
                            {
                                throw new BusinessException($"不存在用户Id为{roleId}的角色信息");
                            }
                            await _userGroupRoleRepository.InsertAsync(new UserGroupRole() { UserGroupId = userGroup.Id, RoleId = roleId }, conn, trans);
                        }
                    }, Connection);
                });
            }
          
        }

        public async Task<IEnumerable<GetDisplayRoleOutput>> GetUserGroupRoles(long userGroupId)
        {
            var sql = @"SELECT r.* FROM UserGroupRole as ugr 
                        LEFT JOIN Role as r on ugr.RoleId = r.Id WHERE ugr.UserGroupId=@UserGroupId";
            using (Connection)
            {
                return (await Connection.QueryAsync<Role>(sql, param: new { UserGroupId = userGroupId })).MapTo<IEnumerable<GetDisplayRoleOutput>>();
            }
        }

        public async Task<IEnumerable<GetUserBasicOutput>> GetUserGroupUsers(long userGroupId)
        {
            var sql = @"SELECT uugr.*,u.* FROM UserUserGroupRelation as uugr 
                        LEFT JOIN UserInfo as u on uugr.UserId = u.Id WHERE uugr.UserGroupId=@UserGroupId";
            using (Connection)
            {
                return await Connection.QueryAsync<UserUserGroupRelation,UserInfo, GetUserBasicOutput>(sql,(uugr,u) => {
                    var output = u.MapTo<GetUserBasicOutput>();
                    var positionAppServiceProxy = GetService<IPositionAppService>();
                    
                    var departmentAppServiceProxy = GetService<IDepartmentAppService>();
                    if (u.OrgId.HasValue) {
                        output.DeptName = departmentAppServiceProxy.GetByOrgId(u.OrgId.Value).Result.Name;
                       
                    }
                    if (u.PositionId.HasValue) {
                        output.DeptName = positionAppServiceProxy.Get(u.PositionId.Value).Result.Name;
                    }
                   
                    return output;
                }, param: new { UserGroupId = userGroupId },splitOn: "Id");
            }
        }

        public async Task<bool> CheckPermission(long userId, string serviceId)
        {
            var querySql = @"SELECT ug.* FROM UserGroup as ug INNER JOIN UserUserGroupRelation as uugr ON ug.Id = uugr.UserGroupId
                            WHERE  ug.IsDeleted=@IsDeleted AND ug.`Status`=@Status";
            var sqlParams = new Dictionary<string, object>() { { "IsDeleted", 1 }, { "Status", Status.Valid } };
            using (var conn = Connection) 
            {
                var userGroups = await conn.QueryAsync<UserGroup>(querySql, sqlParams);
                foreach (var userGroup in userGroups)
                {                  
                    var userGroupRoles = await GetUserGroupRoles(userGroup.Id);
                    foreach (var userGroupRole in userGroupRoles)
                    {
                        if (await _roleDomainService.CheckPermission(userGroupRole.Id, serviceId))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
          
        }

        public async Task<string> AllocationUsers(AllocationUserIdsInput input)
        {
            var userGroup = await _userGroupRepository.SingleOrDefaultAsync(p => p.Id == input.UserGroupId);
            if (userGroup == null)
            {
                throw new BusinessException($"不存在Id为{input.UserGroupId}的用户组");
            }
            using (var locker = await _lockerProvider.CreateLockAsync("AllocationUsers"))
            {
                return await locker.Lock(async () =>
                {
                    await UnitOfWorkAsync(async (conn, trans) =>
                    {
                        foreach (var userId in input.UserIds)
                        {
                            var userInfo = await _userRepository.SingleOrDefaultAsync(p => p.Id == userId, conn, trans);
                            if (userInfo == null)
                            {
                                throw new BusinessException($"不存在用户Id为{userId}的用户信息");
                            }
                            var userUserGroupRelation = await _userUserGroupRelationRepository.SingleOrDefaultAsync(p => p.UserId == userId && p.UserGroupId == input.UserGroupId, conn, trans);
                            if (userUserGroupRelation == null)
                            {
                                await _userUserGroupRelationRepository.InsertAsync(new UserUserGroupRelation() { UserGroupId = userGroup.Id, UserId = userId }, conn, trans);
                            }
                        }
                    }, Connection);
                    return $"为用户组{userGroup.Name}分配用户成功";
                });
            }
           
        }

        public async Task DeleteUserGroupUser(DeleteUserGroupUserInput input)
        {
            await _userUserGroupRelationRepository.DeleteAsync(p => p.UserId == input.UserId && p.UserGroupId == input.UserGroupId);
        }

        public async Task<IPagedResult<GetUserNormOutput>> SearchUserGroupUser(QueryUserGroupUserInput query)
        {
            var querySql = @"SELECT {0} 
FROM UserGroup as ug INNER JOIN UserUserGroupRelation as uugr on ug.Id=uugr.UserGroupId AND ug.IsDeleted=0
INNER JOIN UserInfo as u on uugr.UserId=u.Id AND u.IsDeleted=0
WHERE UserGroupId=@UserGroupId";
            var sqlParams = new Dictionary<string, object>();
            sqlParams.Add("UserGroupId", query.UserGroupId);
            if (query.OrgId.HasValue && query.OrgId.Value !=0) 
            {
                var subOrgIds = await GetService<IOrganizationAppService>().GetSubOrgIds(query.OrgId.Value);
                querySql += " AND u.OrgId in @OrgIds";
                sqlParams.Add("OrgIds", subOrgIds);
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
            var queryCountSql = string.Format(querySql,"COUNT(u.Id)");

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
                        userOutput.DeptName = (await GetService<IDepartmentAppService>().GetByOrgId(userOutput.OrgId.Value)).Name;
                    }
                    if (userOutput.PositionId.HasValue)
                    {
                        userOutput.PositionName = (await GetService<IPositionAppService>().Get(userOutput.PositionId.Value)).Name;
                    }
                    userOutput.Roles = (await GetUserRoles(userOutput.Id)).MapTo<IEnumerable<GetDisplayRoleOutput>>();
                    userOutput.UserGroups = (await GetUserGroups(userOutput.Id)).MapTo<IEnumerable<GetDisplayUserGroupOutput>>();
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

        private async Task<IEnumerable<UserGroup>> GetUserGroups(long userId)
        {
            var sql = @"SELECT ug.* FROM  UserGroup as ug 
                        LEFT JOIN UserUserGroupRelation as uugr on uugr.UserGroupId = ug.Id WHERE uugr.UserId=@UserId";
            using (Connection)
            {
                return (await Connection.QueryAsync<UserGroup>(sql, param: new { UserId = userId }));
            }
        }

        private async Task<IEnumerable<Role>> GetUserRoles(long userId)
        {
            var sql = @"SELECT r.* FROM UserRole as ur 
                        LEFT JOIN Role as r on ur.RoleId = r.Id WHERE ur.UserId=@UserId";
            using (Connection)
            {
                return (await Connection.QueryAsync<Role>(sql, param: new { UserId = userId }));
            }
        }

        public async Task UpdateStatus(UpdateUserGroupStatusInput input)
        {
            var userGroup = await _userGroupRepository.SingleOrDefaultAsync(p => p.Id == input.Id);
            if (userGroup == null) 
            {
                throw new BusinessException($"不存在id为{input.Id}的用户组");
            }
            userGroup.Status = input.Status;
            await _userGroupRepository.UpdateAsync(userGroup);
        }
    }
}
