using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dapper;
using Surging.Core.AutoMapper;
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
using Surging.Hero.Auth.Domain.Shared;
using Surging.Hero.Auth.Domain.Shared.Operations;
using Surging.Hero.Auth.Domain.Shared.Permissions;
using Surging.Hero.Auth.Domain.UserGroups;
using Surging.Hero.Auth.Domain.Users;
using Surging.Hero.Auth.IApplication.Role.Dtos;
using Surging.Hero.Common;

namespace Surging.Hero.Auth.Domain.Roles
{
    public class RoleDomainService : ManagerBase, IRoleDomainService
    {
        private readonly ILockerProvider _lockerProvider;
        private readonly IDapperRepository<Operation, long> _operationRepository;
        private readonly IDapperRepository<Permission, long> _permissionRepository;
        private readonly IDapperRepository<RolePermission, long> _rolePermissionRepository;
        private readonly IDapperRepository<Role, long> _roleRepository;
        private readonly ISurgingSession _session;
        private readonly IDapperRepository<UserGroupRole, long> _userGroupRoleRepository;
        private readonly IDapperRepository<UserInfo, long> _userInfoRepository;
        private readonly IDapperRepository<UserRole, long> _userRoleRepository;

        private readonly IDapperRepository<RoleDataPermissionOrgRelation, long>
            _roleDataPermissionOrgRelationRepository;

        public RoleDomainService(IDapperRepository<Role, long> roleRepository,
            IDapperRepository<RolePermission, long> rolePermissionRepository,
            IDapperRepository<Permission, long> permissionRepository,
            IDapperRepository<UserRole, long> userRoleRepository,
            IDapperRepository<UserGroupRole, long> userGroupRoleRepository,
            IDapperRepository<UserInfo, long> userInfoRepository,
            IDapperRepository<Operation, long> operationRepository,
            ILockerProvider lockerProvider, 
            IDapperRepository<RoleDataPermissionOrgRelation, long> roleDataPermissionOrgRelationRepository)
        {
            _roleRepository = roleRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _permissionRepository = permissionRepository;
            _userRoleRepository = userRoleRepository;
            _userGroupRoleRepository = userGroupRoleRepository;
            _userInfoRepository = userInfoRepository;
            _operationRepository = operationRepository;
            _lockerProvider = lockerProvider;
            _roleDataPermissionOrgRelationRepository = roleDataPermissionOrgRelationRepository;
            _session = NullSurgingSession.Instance;
        }

        public async Task<bool> CheckPermission(long roleId, string serviceId)
        {
            var role = await _roleRepository.SingleOrDefaultAsync(p => p.Id == roleId,false);
            if (role == null) throw new BusinessException($"不存在Id为{roleId}的角色信息");
            if (role.Status == Status.Invalid) return false;
            var rolePermissions = await GetRolePermissions(roleId);
            var servicePemission = await GetservicePemission(serviceId);
            if (servicePemission == null)
                throw new AuthException($"通过{serviceId}未查询到相关权限信息,请于管理员联系", StatusCode.UnAuthorized);
            if (servicePemission.Status == Status.Invalid)
                throw new AuthException($"{servicePemission.Name}--{servicePemission.Memo}权限状态无效",
                    StatusCode.UnAuthorized);
            if (servicePemission.Mold == PermissionMold.Operation)
            {
                var serviceOperation =
                    await _operationRepository.SingleOrDefaultAsync(p => p.PermissionId == servicePemission.Id);
                if (serviceOperation == null)
                    throw new AuthException($"通过{serviceId}未查询到相关操作,请于管理员联系", StatusCode.UnAuthorized);
                if (serviceOperation.Mold == OperationMold.Look || serviceOperation.Mold == OperationMold.Query)
                    return true;
            }

            if (rolePermissions.Any(p => p.PermissionId == servicePemission.Id)) return true;
            return false;
        }

        public async Task Create(CreateRoleInput input)
        {
            var exsitRole = await _roleRepository.FirstOrDefaultAsync(p => p.Name == input.Name);
            if (exsitRole != null) throw new BusinessException($"系统中已经存在{input.Name}的角色");
            CheckUserDefinedDataPermission(input.DataPermissionType,input.OrgIds);
            var role = input.MapTo<Role>();

            
            using (var locker = await _lockerProvider.CreateLockAsync("CreateRole"))
            {
                await locker.Lock(async () =>
                {
                    await UnitOfWorkAsync(async (conn, trans) =>
                    {
                        var roleId = await _roleRepository.InsertAndGetIdAsync(role, conn, trans);
                        var deleteSql = "DELETE FROM RolePermission WHERE RoleId=@RoleId";
                        await conn.ExecuteAsync(deleteSql, new {RoleId = role.Id}, trans);
                        await _rolePermissionRepository.DeleteAsync(p => p.RoleId == role.Id, conn, trans);
                        var insertSql =
                            "INSERT INTO RolePermission(PermissionId,RoleId,CreateTime,CreateBy) VALUES(@PermissionId,@RoleId,@CreationTime,@CreatorUserId)";
                        var rolePermissions = new List<RolePermission>();
                        foreach (var permissionId in input.PermissionIds)
                            rolePermissions.Add(new RolePermission
                            {
                                PermissionId = permissionId, RoleId = roleId, CreationTime = DateTime.Now,
                                CreatorUserId = _session.UserId
                            });
                        await conn.ExecuteAsync(insertSql, rolePermissions, trans);
                        if (input.DataPermissionType == DataPermissionType.UserDefined)
                        {
                            var insertDataPermissionOrgSql =
                                "INSERT INTO RoleDataPermissionOrgRelation(RoleId,OrgId,CreateTime,CreateBy) VALUES(@RoleId,@OrgId,@CreationTime,@CreatorUserId)";
                            var dataPermissionOrgDatas = new List<RoleDataPermissionOrgRelation>();
                            foreach (var orgId in input.OrgIds)
                            {
                                dataPermissionOrgDatas.Add(new RoleDataPermissionOrgRelation()
                                {
                                    RoleId = roleId,
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
        

        public async Task Delete(long roleid)
        {
            var role = await _roleRepository.SingleOrDefaultAsync(p => p.Id == roleid);
            if (role == null) throw new BusinessException($"不存在Id为{roleid}的角色信息");
            var userRoleCount = await _userRoleRepository.GetCountAsync(p => p.RoleId == roleid);
            if (userRoleCount > 0) throw new BusinessException($"{role.Name}被分配用户,请先删除相关授权的用户信息");
            var userGroupRoleCount = await _userGroupRoleRepository.GetCountAsync(p => p.RoleId == roleid);
            if (userGroupRoleCount > 0) throw new BusinessException($"{role.Name}被分配用户组,请先删除相关授权的用户组信息");
            using (var locker = await _lockerProvider.CreateLockAsync("DeleteRole"))
            {
                await locker.Lock(async () =>
                {
                    await UnitOfWorkAsync(async (conn, trans) =>
                    {
                        await _roleRepository.DeleteAsync(p => p.Id == roleid, conn, trans);
                        await _rolePermissionRepository.DeleteAsync(p => p.RoleId == roleid, conn, trans);
                        await _roleDataPermissionOrgRelationRepository.DeleteAsync(p => p.RoleId == roleid, conn,
                            trans);
                    }, Connection);
                });
            }
        }

        public async Task<GetRoleOutput> Get(long id)
        {
            var role = await _roleRepository.SingleOrDefaultAsync(p => p.Id == id);
            if (role == null) throw new BusinessException($"不存在Id为{id}的角色信息");
            var roleOutput = role.MapTo<GetRoleOutput>();
            if (roleOutput.LastModifierUserId.HasValue)
            {
                var modifyUserInfo =
                    await _userInfoRepository.SingleOrDefaultAsync(p => p.Id == roleOutput.LastModifierUserId.Value);
                if (modifyUserInfo != null) roleOutput.LastModificationUserName = modifyUserInfo.ChineseName;
            }

            if (roleOutput.CreatorUserId.HasValue)
            {
                var creatorUserInfo =
                    await _userInfoRepository.SingleOrDefaultAsync(p => p.Id == roleOutput.CreatorUserId.Value);
                if (creatorUserInfo != null) roleOutput.CreatorUserName = creatorUserInfo.ChineseName;
            }

            roleOutput.PermissionIds = (await _rolePermissionRepository.GetAllAsync(p => p.RoleId == role.Id))
                .Select(p => p.PermissionId).ToArray();
            roleOutput.OrgIds = (await _roleDataPermissionOrgRelationRepository.GetAllAsync(p => p.RoleId == role.Id))
                .Select(p => p.OrgId).ToArray();

            return roleOutput;
        }

        public async Task<IEnumerable<RolePermission>> GetRolePermissions(long roleId)
        {
            return await _rolePermissionRepository.GetAllAsync(p => p.RoleId == roleId);
        }

        public async Task<IPagedResult<GetRoleOutput>> Search(QueryRoleInput query)
        {
            Expression<Func<Role, bool>> predicate = p => p.Name.Contains(query.SearchKey);
            if (query.Status.HasValue) predicate = predicate.And(p => p.Status == query.Status);
            var queryResult = await _roleRepository.GetPageAsync(p => p.Name.Contains(query.SearchKey), query.PageIndex,
                query.PageCount);

            var outputs = queryResult.Item1.MapTo<IEnumerable<GetRoleOutput>>().GetPagedResult(queryResult.Item2);
            foreach (var output in outputs.Items)
            {
                if (output.LastModifierUserId.HasValue)
                {
                    var modifyUserInfo =
                        await _userInfoRepository.SingleOrDefaultAsync(p => p.Id == output.LastModifierUserId.Value);
                    if (modifyUserInfo != null) output.LastModificationUserName = modifyUserInfo.ChineseName;
                }

                if (output.CreatorUserId.HasValue)
                {
                    var creatorUserInfo =
                        await _userInfoRepository.SingleOrDefaultAsync(p => p.Id == output.CreatorUserId.Value);
                    if (creatorUserInfo != null) output.CreatorUserName = creatorUserInfo.ChineseName;
                }

                output.PermissionIds = (await _rolePermissionRepository.GetAllAsync(p => p.RoleId == output.Id))
                    .Select(p => p.PermissionId).ToArray();
                output.OrgIds = (await _roleDataPermissionOrgRelationRepository.GetAllAsync(p => p.RoleId == output.Id))
                    .Select(p => p.OrgId).ToArray();
            }

            return outputs;
        }

        public async Task SetPermissions(SetRolePermissionInput input)
        {
            var role = await _roleRepository.SingleOrDefaultAsync(p => p.Id == input.RoleId);
            if (role == null) throw new BusinessException($"不存在Id为{input.RoleId}的角色信息");
            using (var locker = await _lockerProvider.CreateLockAsync("SetPermissions"))
            {
                await locker.Lock(async () =>
                {
                    await UnitOfWorkAsync(async (conn, trans) =>
                    {
                        await _rolePermissionRepository.DeleteAsync(p => p.RoleId == input.RoleId, conn, trans);
                        foreach (var permissionId in input.PermissionIds)
                        {
                            var permission =
                                await _permissionRepository.SingleOrDefaultAsync(p => p.Id == permissionId);
                            if (permission == null) throw new BusinessException($"不存在Id为{permissionId}的权限信息");
                            await _rolePermissionRepository.InsertAsync(
                                new RolePermission {PermissionId = permissionId, RoleId = input.RoleId}, conn, trans);
                        }
                    }, Connection);
                });
            }
        }

        public async Task Update(UpdateRoleInput input)
        {
            var role = await _roleRepository.GetAsync(input.Id);
            if (input.Name != role.Name)
            {
                var exsitRole = await _roleRepository.FirstOrDefaultAsync(p => p.Name == input.Name);
                if (exsitRole != null) throw new BusinessException($"系统中已经存在{input.Name}的角色");
            }
            CheckUserDefinedDataPermission(input.DataPermissionType,input.OrgIds);
            role = input.MapTo(role);
            using (var locker = await _lockerProvider.CreateLockAsync("UpdateRole"))
            {
                await locker.Lock(async () =>
                {
                    await UnitOfWorkAsync(async (conn, trans) =>
                    {
                        await _roleRepository.UpdateAsync(role, conn, trans);
                        var deleteSql = "DELETE FROM RolePermission WHERE RoleId=@RoleId";
                        await conn.ExecuteAsync(deleteSql, new {RoleId = role.Id}, trans);
                        await _rolePermissionRepository.DeleteAsync(p => p.RoleId == role.Id, conn, trans);
                        await _roleDataPermissionOrgRelationRepository.DeleteAsync(p => p.RoleId == role.Id, conn,
                            trans);
                        var insertSql =
                            "INSERT INTO RolePermission(PermissionId,RoleId,CreateTime,CreateBy) VALUES(@PermissionId,@RoleId,@CreationTime,@CreatorUserId)";
                        var rolePermissions = new List<RolePermission>();
                        foreach (var permissionId in input.PermissionIds)
                            rolePermissions.Add(new RolePermission
                            {
                                PermissionId = permissionId, RoleId = role.Id, CreationTime = DateTime.Now,
                                CreatorUserId = _session.UserId
                            });
                        await conn.ExecuteAsync(insertSql, rolePermissions, trans);
                        if (input.DataPermissionType == DataPermissionType.UserDefined)
                        {
                            var insertDataPermissionOrgSql =
                                "INSERT INTO RoleDataPermissionOrgRelation(RoleId,OrgId,CreateTime,CreateBy) VALUES(@RoleId,@OrgId,@CreationTime,@CreatorUserId)";
                            var dataPermissionOrgDatas = new List<RoleDataPermissionOrgRelation>();
                            foreach (var orgId in input.OrgIds)
                            {
                                dataPermissionOrgDatas.Add(new RoleDataPermissionOrgRelation()
                                {
                                    RoleId = role.Id,
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

        public async Task UpdateStatus(UpdateRoleStatusInput input)
        {
            var role = await _roleRepository.GetAsync(input.Id);
            role.Status = input.Status;
            await _roleRepository.UpdateAsync(role);
        }

        private async Task<Permission> GetservicePemission(string serviceId)
        {
            var sql = @"SELECT p.* FROM OperationActionRelation as oar 
LEFT JOIN Operation as o on oar.OperationId = o.Id AND o.IsDeleted = @IsDeleted
LEFT JOIN Permission as p on p.Id = o.PermissionId AND p.Mold=1 AND  p.IsDeleted = @IsDeleted
WHERE oar.ServiceId=@ServiceId";

            await using (Connection)
            {
                var permission = await Connection.QueryAsync<Permission>(sql,
                    new {ServiceId = serviceId, IsDeleted = HeroConstants.UnDeletedFlag});
                return permission.FirstOrDefault();
            }
        }
        
        private void CheckUserDefinedDataPermission(DataPermissionType dataPermissionType, long[] orgIds)
        {
            if (dataPermissionType == DataPermissionType.UserDefined)
            {
                if (orgIds == null || !orgIds.Any())
                {
                    throw new BusinessException("设置角色的数据权限为自定义数据权限,则指定的部门不允许为空");
                }

                DebugCheck.NotNull(_session.OrgId);
                if (!orgIds.Contains(_session.OrgId.Value))
                {
                    throw new BusinessException("用户自定义数据权限指定的部门必须包含您所在的部门");
                }
            }
        }
    }
}