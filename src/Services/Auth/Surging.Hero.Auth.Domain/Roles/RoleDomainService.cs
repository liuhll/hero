using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dapper;
using Surging.Cloud.AutoMapper;
using Surging.Cloud.Caching;
using Surging.Cloud.CPlatform.Cache;
using Surging.Cloud.CPlatform.Configurations;
using Surging.Cloud.CPlatform.Exceptions;
using Surging.Cloud.CPlatform.Runtime.Session;
using Surging.Cloud.CPlatform.Utilities;
using Surging.Cloud.Dapper.Manager;
using Surging.Cloud.Dapper.Repositories;
using Surging.Cloud.Domain.PagedAndSorted;
using Surging.Cloud.Domain.PagedAndSorted.Extensions;
using Surging.Cloud.Lock;
using Surging.Cloud.Lock.Provider;
using Surging.Hero.Auth.Domain.Permissions;
using Surging.Hero.Auth.Domain.Permissions.Operations;
using Surging.Hero.Auth.Domain.Shared;
using Surging.Hero.Auth.Domain.Shared.Operations;
using Surging.Hero.Auth.Domain.Shared.Permissions;
using Surging.Hero.Auth.Domain.UserGroups;
using Surging.Hero.Auth.Domain.Users;
using Surging.Hero.Auth.IApplication.FullAuditDtos;
using Surging.Hero.Auth.IApplication.Role.Dtos;
using Surging.Hero.Common;
using Surging.Hero.Common.Runtime.Session;
using Surging.Hero.Organization.IApplication.Organization;
using AppConfig = Surging.Cloud.CPlatform.AppConfig;

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
        private readonly IDapperRepository<UserRole, long> _userRoleRepository;
        private readonly IDapperRepository<RoleOrganization, long> _roleOrganizationRepository;
        private readonly IDapperRepository<Permissions.Actions.Action,long> _actionRepository;
        private readonly ICacheProvider _cacheProvider;

        private readonly IDapperRepository<RoleDataPermissionOrgRelation, long>
            _roleDataPermissionOrgRelationRepository;

        public RoleDomainService(IDapperRepository<Role, long> roleRepository,
            IDapperRepository<RolePermission, long> rolePermissionRepository,
            IDapperRepository<Permission, long> permissionRepository,
            IDapperRepository<UserRole, long> userRoleRepository,
            IDapperRepository<UserGroupRole, long> userGroupRoleRepository,
            IDapperRepository<Operation, long> operationRepository,
            ILockerProvider lockerProvider, 
            IDapperRepository<RoleDataPermissionOrgRelation, long> roleDataPermissionOrgRelationRepository, 
            IDapperRepository<RoleOrganization, long> roleOrganizationRepository, 
            IDapperRepository<Permissions.Actions.Action, long> actionRepository)
        {
            _roleRepository = roleRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _permissionRepository = permissionRepository;
            _userRoleRepository = userRoleRepository;
            _userGroupRoleRepository = userGroupRoleRepository;
            _operationRepository = operationRepository;
            _lockerProvider = lockerProvider;
            _roleDataPermissionOrgRelationRepository = roleDataPermissionOrgRelationRepository;
            _roleOrganizationRepository = roleOrganizationRepository;
            _actionRepository = actionRepository;
            _session = NullSurgingSession.Instance;
            _cacheProvider = CacheContainer.GetService<ICacheProvider>(HeroConstants.CacheProviderKey);
        }

        public async Task<bool> CheckPermission(long roleId, string serviceId)
        {
            var role = await _roleRepository.SingleOrDefaultAsync(p => p.Id == roleId,false);
            if (role == null) throw new BusinessException($"不存在Id为{roleId}的角色信息");
            if (role.Status == Status.Invalid) return false;
            var rolePermissions = await GetRolePermissions(roleId);
            var servicePemissions = await GetservicePemission(serviceId);
            var action = await _actionRepository.SingleOrDefaultAsync(p => p.ServiceId == serviceId);
            if (!servicePemissions.Any())
                throw new AuthException($"通过{serviceId}未查询到相关权限信息,请于管理员联系", StatusCode.UnAuthorized);
            if (action.Status == Status.Invalid)
                throw new AuthException($"{action.Application}--{action.Name}权限状态无效",
                    StatusCode.UnAuthorized);
            foreach (var servicePemission in servicePemissions)
            {   
                if (servicePemission.Mold == PermissionMold.Operation)
                {
                    var serviceOperation =
                        await _operationRepository.SingleOrDefaultAsync(p => p.PermissionId == servicePemission.Id);
                    if (serviceOperation == null)
                        throw new BusinessException($"不存在{servicePemission.Name}的操作信息");
                    if (serviceOperation.Mold == OperationMold.Look || serviceOperation.Mold == OperationMold.Query)
                        return true;
                    if (AppConfig.ServerOptions.Environment == RuntimeEnvironment.Test)
                    {
                        if (AuthDomainConstants.PermissionServiceIdWhiteList.Contains(serviceId))
                            return true;
                        throw new AuthException("项目演示环境，不允许操作！", StatusCode.UnAuthorized);
                    }
                }
                
            }
            if (rolePermissions.Any(p => servicePemissions.Any(q=> q.Id == p.PermissionId))) return true;
            return false;
        }

        public async Task Create(CreateRoleInput input)
        {
            using (var locker = await _lockerProvider.CreateLockAsync("CreateRole"))
            {
                await locker.Lock(async () =>
                {
                    
                    var exsitRole = await _roleRepository.FirstOrDefaultAsync(p => p.Identification == input.Identification,false);
                    if (exsitRole != null) throw new BusinessException($"系统中已经存在{input.Identification}的角色");
                    CheckUserDefinedDataPermission(input.DataPermissionType,input.DataPermissionOrgIds);
                    var role = input.MapTo<Role>();
                    
                    await UnitOfWorkAsync(async (conn, trans) =>
                    {
                        var roleId = await _roleRepository.InsertAndGetIdAsync(role, conn, trans);
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
                        if (!input.IsAllOrg)
                        {
                            foreach (var orgId in input.OrgIds)
                            {
                                var roleOrg = new RoleOrganization() { RoleId = roleId, OrgId = orgId };
                                await _roleOrganizationRepository.InsertAsync(roleOrg, conn, trans);
                            }  
                        }
                        if (input.DataPermissionType == DataPermissionType.UserDefined)
                        {
                            var insertDataPermissionOrgSql =
                                "INSERT INTO RoleDataPermissionOrgRelation(RoleId,OrgId,CreateTime,CreateBy) VALUES(@RoleId,@OrgId,@CreationTime,@CreatorUserId)";
                            var dataPermissionOrgDatas = new List<RoleDataPermissionOrgRelation>();
                            foreach (var orgId in input.DataPermissionOrgIds)
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
            _session.CheckLoginUserDataPermision(role.DataPermissionType,"您设置的角色的数据权限大于您拥有数据权限,系统不允许该操作");
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
                        await _roleOrganizationRepository.DeleteAsync(p => p.RoleId == roleid, conn, trans);
                        await RemoveRoleCheckPemissionCache(roleid);
                    }, Connection);
                });
            }
        }

        public async Task<GetRoleOutput> Get(long id)
        {
            var role = await _roleRepository.SingleOrDefaultAsync(p => p.Id == id);
            if (role == null) throw new BusinessException($"不存在Id为{id}的角色信息");
            var roleOutput = role.MapTo<GetRoleOutput>();

             await roleOutput.SetAuditInfo();

            roleOutput.PermissionIds = (await _rolePermissionRepository.GetAllAsync(p => p.RoleId == role.Id))
                .Select(p => p.PermissionId).ToArray();
            roleOutput.DataPermissionOrgIds = (await _roleDataPermissionOrgRelationRepository.GetAllAsync(p => p.RoleId == role.Id))
                .Select(p => p.OrgId).ToArray();
            roleOutput.Organizations = await GetRoleOrgInfo(role.Id);

            return roleOutput;
        }

        public async Task<IEnumerable<RolePermission>> GetRolePermissions(long roleId)
        {
            return await _rolePermissionRepository.GetAllAsync(p => p.RoleId == roleId);
        }

        public async Task<IPagedResult<GetRoleOutput>> Search(QueryRoleInput query)
        {
            // Expression<Func<Role, bool>> predicate = p => p.Name.Contains(query.SearchKey);
            // if (query.Status.HasValue) predicate = predicate.And(p => p.Status == query.Status);
            // var queryResult = await _roleRepository.GetPageAsync(predicate, query.PageIndex,
            //     query.PageCount);

            var sql = @"
SELECT DISTINCT r.* ,r.CreateBy as CreatorUserId, r.CreateTime as CreationTime, r.UpdateBy as LastModifierUserId, r.UpdateTime as LastModificationTime FROM Role as r {0}
WHERE r.IsDeleted=@IsDeleted
";
            var sqlParams = new Dictionary<string, object>();
            sqlParams.Add("IsDeleted",HeroConstants.UnDeletedFlag);
            if (!query.SearchKey.IsNullOrWhiteSpace())
            {
                sql += " AND r.`Name` LIKE @SearchKey OR r.Identification LIKE @SearchKey ";
                sqlParams.Add("SearchKey",$"%{query.SearchKey}%");
            }

            if (query.Status.HasValue)
            {
                sql += " AND r.Status=@Status ";
                sqlParams.Add("Status",query.Status);
            }

            if (query.OrgIds !=null && query.OrgIds.Length > 0)
            {
                sql = string.Format(sql, " LEFT JOIN RoleOrganization as ro On ro.RoleId=r.Id ");
                sql += "AND (ro.OrgId in @OrgId OR r.IsAllOrg=@IsAllOrg)";
                sqlParams.Add("OrgId",query.OrgIds);
                sqlParams.Add("IsAllOrg",true);
            }
            else
            {
                sql = string.Format(sql, " ");
            }

            if (!query.Sorting.IsNullOrEmpty())
            {
                sql += $" ORDER BY r.{query.Sorting} {query.SortType}";
            }
            else
            {
                sql += " ORDER BY r.Id DESC";
            }

            var countSql = "SELECT COUNT(DISTINCT r.Id) FROM " + sql.Substring(sql.ToLower().IndexOf("from") + "from".Length);
            sql += $" LIMIT {(query.PageIndex - 1) * query.PageCount} , {query.PageCount} ";
            await using (Connection)
            {
                var queryResult = await Connection.QueryAsync<Role>(sql, sqlParams);
                var queryResultTotalCount = await Connection.ExecuteScalarAsync<int>(countSql, sqlParams);
                var outputs = queryResult.MapTo<IEnumerable<GetRoleOutput>>().GetPagedResult(queryResultTotalCount);
                foreach (var output in outputs.Items)
                {
                    await output.SetAuditInfo();
                    output.PermissionIds = (await _rolePermissionRepository.GetAllAsync(p => p.RoleId == output.Id))
                        .Select(p => p.PermissionId).ToArray();
                    output.DataPermissionOrgIds = (await _roleDataPermissionOrgRelationRepository.GetAllAsync(p => p.RoleId == output.Id))
                        .Select(p => p.OrgId).ToArray();
                    output.Organizations = await GetRoleOrgInfo(output.Id);
                }

                return outputs;
            }


        }
        
        public async Task Update(UpdateRoleInput input)
        {
            CheckUserDefinedDataPermission(input.DataPermissionType,input.DataPermissionOrgIds);
            using (var locker = await _lockerProvider.CreateLockAsync("UpdateRole"))
            {
                await locker.Lock(async () =>
                {
                    var role = await _roleRepository.GetAsync(input.Id,false);
                    if (role.DataPermissionType == DataPermissionType.UserDefined && _session.UserId != role.CreatorUserId)
                    {
                        throw new BusinessException("自定义数据权限的角色只允许用户创建者自己修改");
                    }
                    if (input.Identification != role.Identification)
                    {
                        var exsitRole = await _roleRepository.FirstOrDefaultAsync(p => p.Identification == input.Identification,false);
                        if (exsitRole != null) throw new BusinessException($"系统中已经存在{input.Identification}的角色");
                    }

                    role = input.MapTo(role);
                    await UnitOfWorkAsync(async (conn, trans) =>
                    {
                        await _roleRepository.UpdateAsync(role, conn, trans);
                        var deleteSql = "DELETE FROM RolePermission WHERE RoleId=@RoleId";
                        await conn.ExecuteAsync(deleteSql, new {RoleId = role.Id}, trans);
                        await _rolePermissionRepository.DeleteAsync(p => p.RoleId == role.Id, conn, trans);
                        await _roleDataPermissionOrgRelationRepository.DeleteAsync(p => p.RoleId == role.Id, conn,
                            trans);
                        await _roleOrganizationRepository.DeleteAsync(p => p.RoleId == role.Id,conn,trans);
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
                        if (!input.IsAllOrg)
                        {
                            foreach (var orgId in input.OrgIds)
                            {
                                var roleOrg = new RoleOrganization() { RoleId = role.Id, OrgId = orgId };
                                await _roleOrganizationRepository.InsertAsync(roleOrg, conn, trans);
                            }  
                        }                           
                        if (input.DataPermissionType == DataPermissionType.UserDefined)
                        {
                            var insertDataPermissionOrgSql =
                                "INSERT INTO RoleDataPermissionOrgRelation(RoleId,OrgId,CreateTime,CreateBy) VALUES(@RoleId,@OrgId,@CreationTime,@CreatorUserId)";
                            var dataPermissionOrgDatas = new List<RoleDataPermissionOrgRelation>();
                            foreach (var orgId in input.DataPermissionOrgIds)
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
                        await RemoveRoleCheckPemissionCache(role.Id);
                    }, Connection);
                });
            }
        }

        public async Task UpdateStatus(UpdateRoleStatusInput input)
        {
            var role = await _roleRepository.GetAsync(input.Id);
            role.Status = input.Status;
            await _roleRepository.UpdateAsync(role);
            await RemoveRoleCheckPemissionCache(role.Id);
        }
        
        public async Task RemoveRoleCheckPemissionCache(long roleId)
        {
            var sql = @"
SELECT oar.ServiceId FROM OperationActionRelation as oar 
INNER JOIN Operation as o on oar.OperationId=o.Id AND o.IsDeleted=@IsDeleted
INNER JOIN RolePermission as rp on o.PermissionId=rp.PermissionId 
WHERE RoleId=@RoleId
";
            var sqlParams = new Dictionary<string, object>() { { "IsDeleted", HeroConstants.UnDeletedFlag },{ "RoleId", roleId } };
            await using (Connection)
            {
                var roleServiceIds = await Connection.QueryAsync<string>(sql, sqlParams);
                foreach (var serviceId in roleServiceIds)
                {
                    var cacheKey = string.Format(HeroConstants.CacheKey.PermissionCheck,serviceId,"*");
                    await _cacheProvider.RemoveAsync(cacheKey);
                }
            }
        }       

        private async Task<IEnumerable<Permission>> GetservicePemission(string serviceId)
        {
            var sql = @"SELECT p.* FROM OperationActionRelation as oar 
LEFT JOIN Operation as o on oar.OperationId = o.Id AND o.IsDeleted = @IsDeleted
LEFT JOIN Permission as p on p.Id = o.PermissionId AND p.Mold=@Mold AND  p.IsDeleted = @IsDeleted
WHERE oar.ServiceId=@ServiceId";

            await using (Connection)
            {
                var permissions = await Connection.QueryAsync<Permission>(sql,
                    new {ServiceId = serviceId, IsDeleted = HeroConstants.UnDeletedFlag, Mold=PermissionMold.Operation });
                return permissions;
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
            }
        }
        
        private async Task<GetDisplayOrganizationOutput[]> GetRoleOrgInfo(long roleId)
        {
            var organziationAppServiceProxy = GetService<IOrganizationAppService>();
            var orgIds =
                (await _roleOrganizationRepository.GetAllAsync(p => p.RoleId == roleId)).Select(p => p.OrgId).ToArray();
            var roleOrgs = new List<GetDisplayOrganizationOutput>();
            foreach (var orgId in orgIds)
            {
                var orginInfo = await organziationAppServiceProxy.GetOrg(orgId);
                roleOrgs.Add(new GetDisplayOrganizationOutput() {OrgId = orgId, Name = orginInfo.Name});
            }
            
            return roleOrgs.ToArray();
        }
        
    }
}