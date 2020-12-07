using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Surging.Core.AutoMapper;
using Surging.Core.CPlatform.Exceptions;
using Surging.Core.Dapper.Manager;
using Surging.Core.Dapper.Repositories;
using Surging.Core.Domain.PagedAndSorted;
using Surging.Core.Domain.PagedAndSorted.Extensions;
using Surging.Hero.Auth.Domain.Permissions;
using Surging.Hero.Auth.Domain.Permissions.Operations;
using Surging.Hero.Auth.Domain.UserGroups;
using Surging.Hero.Auth.Domain.Users;
using Surging.Hero.Auth.IApplication.Role.Dtos;
using Surging.Hero.Organization.IApplication.Department;

namespace Surging.Hero.Auth.Domain.Roles
{
    public class RoleDomainService : ManagerBase, IRoleDomainService
    {
        private readonly IDapperRepository<Role, long> _roleRepository;
        private readonly IDapperRepository<RolePermission, long> _rolePermissionRepository;
        private readonly IDapperRepository<Permission, long> _permissionRepository;
        private readonly IDapperRepository<UserRole, long> _userRoleRepository;
        private readonly IDapperRepository<UserGroupRole, long> _userGroupRoleRepository;
        private readonly IDapperRepository<UserInfo, long> _userInfoRepository;
        private readonly IDapperRepository<Operation, long> _operationRepository;

        public RoleDomainService(IDapperRepository<Role, long> roleRepository,
            IDapperRepository<RolePermission, long> rolePermissionRepository,
            IDapperRepository<Permission, long> permissionRepository,
            IDapperRepository<UserRole, long> userRoleRepository,
            IDapperRepository<UserGroupRole, long> userGroupRoleRepository,
            IDapperRepository<UserInfo, long> userInfoRepository,
            IDapperRepository<Operation, long> operationRepository)
        {
            _roleRepository = roleRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _permissionRepository = permissionRepository;
            _userRoleRepository = userRoleRepository;
            _userGroupRoleRepository = userGroupRoleRepository;
            _userInfoRepository = userInfoRepository;
            _operationRepository = operationRepository;
        }

        public async Task<bool> CheckPermission(long roleId, string serviceId)
        {
            var rolePermissions = await GetRolePermissions(roleId);
            var servicePemission = await GetservicePemission(serviceId);
            if (servicePemission == null)
            {
                throw new AuthException($"通过{serviceId}未查询到相关权限信息,请于管理员联系", StatusCode.UnAuthorized);
            }
            if (servicePemission.Status == Common.Status.Invalid)
            {
                throw new AuthException($"{servicePemission.Name}--{servicePemission.Memo}权限状态无效", StatusCode.UnAuthorized);
            }
            if (servicePemission.Mold == Shared.Permissions.PermissionMold.Operation)
            {
                var serviceOperation = await _operationRepository.SingleOrDefaultAsync(p => p.PermissionId == servicePemission.Id);
                if (serviceOperation == null)
                {
                    throw new AuthException($"通过{serviceId}未查询到相关操作,请于管理员联系", StatusCode.UnAuthorized);
                }
                if (serviceOperation.Mold == Shared.Operations.OperationMold.Look || serviceOperation.Mold == Shared.Operations.OperationMold.Query)
                {
                    return true;
                }
            }
            if (rolePermissions.Any(p => p.PermissionId == servicePemission.Id))
            {
                return true;
            }
            return false;

        }

        public async Task Create(CreateRoleInput input)
        {
            var exsitRole = await _roleRepository.FirstOrDefaultAsync(p => p.Name == input.Name);
            if (exsitRole != null)
            {
                throw new BusinessException($"系统中已经存在{input.Name}的角色");
            }
            var role = input.MapTo<Role>();
            await UnitOfWorkAsync(async (conn, trans) =>
            {
                var roleId = await _roleRepository.InsertAndGetIdAsync(role, conn, trans);
                //var queryOperationSql = "SELECT o.* FROM `Operation` as o LEFT JOIN Permission as p ON o.PermissionId = p.Id AND p.IsDeleted = 0 AND o.IsDeleted = 0 WHERE o.PermissionId IN @PermissionIds";
                //var operations = await conn.QueryAsync<Operation>(queryOperationSql, new { PermissionIds = input.PermissionIds }, transaction: trans);
                //if (!operations.Any(p => p.Mold == Shared.Operations.OperationMold.Query || p.Mold == Shared.Operations.OperationMold.Look))
                //{
                //    throw new BusinessException($"分配的权限至少要包含查询或是查看类型操作");
                //}
                await _rolePermissionRepository.DeleteAsync(p => p.RoleId == roleId, conn, trans);
                foreach (var permissionId in input.PermissionIds)
                {
                    var permission = await _permissionRepository.SingleOrDefaultAsync(p => p.Id == permissionId);
                    if (permission == null)
                    {
                        throw new BusinessException($"不存在Id为{permissionId}的权限信息");
                    }
                    await _rolePermissionRepository.InsertAsync(new RolePermission() { PermissionId = permissionId, RoleId = roleId }, conn, trans);
                }
            }, Connection);

        }

        public async Task Delete(long roleid)
        {
            var role = await _roleRepository.SingleOrDefaultAsync(p => p.Id == roleid);
            if (role == null)
            {
                throw new BusinessException($"不存在Id为{roleid}的角色信息");
            }
            var userRoleCount = await _userRoleRepository.GetCountAsync(p => p.RoleId == roleid);
            if (userRoleCount > 0)
            {
                throw new BusinessException($"{role.Name}被分配用户,请先删除相关授权的用户信息");
            }
            var userGroupRoleCount = await _userGroupRoleRepository.GetCountAsync(p => p.RoleId == roleid);
            if (userRoleCount > 0)
            {
                throw new BusinessException($"{role.Name}被分配用户组,请先删除相关授权的用户组信息");
            }
            await UnitOfWorkAsync(async (conn, trans) =>
            {
                await _roleRepository.DeleteAsync(p => p.Id == roleid, conn, trans);
                await _rolePermissionRepository.DeleteAsync(p => p.RoleId == roleid, conn, trans);
            }, Connection);

        }

        public async Task<GetRoleOutput> Get(long id)
        {
            var role = await _roleRepository.SingleOrDefaultAsync(p => p.Id == id);
            if (role == null)
            {
                throw new BusinessException($"不存在Id为{id}的角色信息");
            }
            var roleOutput = role.MapTo<GetRoleOutput>();
            if (roleOutput.LastModifierUserId.HasValue)
            {
                var modifyUserInfo = await _userInfoRepository.SingleOrDefaultAsync(p => p.Id == roleOutput.LastModifierUserId.Value);
                if (modifyUserInfo != null)
                {
                    roleOutput.LastModificationUserName = modifyUserInfo.ChineseName;
                }
            }
            roleOutput.PermissionIds = (await _rolePermissionRepository.GetAllAsync(p => p.RoleId == role.Id)).Select(p => p.PermissionId).ToArray();

            return roleOutput;
        }

        public async Task<IEnumerable<RolePermission>> GetRolePermissions(long roleId)
        {
            var role = await _roleRepository.SingleOrDefaultAsync(p => p.Id == roleId);
            if (role == null)
            {
                throw new BusinessException($"不存在Id为{roleId}的角色信息");
            }
            return await _rolePermissionRepository.GetAllAsync(p => p.RoleId == roleId);
        }

        public async Task<IPagedResult<GetRoleOutput>> Query(QueryRoleInput query)
        {
            var queryResult = await _roleRepository.GetPageAsync(p => p.Name.Contains(query.SearchKey) && p.Memo.Contains(query.SearchKey), query.PageIndex, query.PageCount);

            var outputs = queryResult.Item1.MapTo<IEnumerable<GetRoleOutput>>().GetPagedResult(queryResult.Item2);
            foreach (var output in outputs.Items)
            {
                if (output.LastModifierUserId.HasValue)
                {
                    var modifyUserInfo = await _userInfoRepository.SingleOrDefaultAsync(p => p.Id == output.LastModifierUserId.Value);
                    if (modifyUserInfo != null)
                    {
                        output.LastModificationUserName = modifyUserInfo.ChineseName;
                    }
                }
                output.PermissionIds = (await _rolePermissionRepository.GetAllAsync(p => p.RoleId == output.Id)).Select(p => p.PermissionId).ToArray();

            }
            return outputs;
        }

        public async Task SetPermissions(SetRolePermissionInput input)
        {
            var role = await _roleRepository.SingleOrDefaultAsync(p => p.Id == input.RoleId);
            if (role == null)
            {
                throw new BusinessException($"不存在Id为{input.RoleId}的角色信息");
            }
            await UnitOfWorkAsync(async (conn, trans) =>
            {
                //var queryOperationSql = "SELECT o.* FROM `Operation` as o LEFT JOIN Permission as p ON o.PermissionId = p.Id AND p.IsDeleted = 0 AND o.IsDeleted = 0 WHERE o.PermissionId IN @PermissionIds";

                //var operations = await conn.QueryAsync<Operation>(queryOperationSql, new { PermissionIds = input.PermissionIds }, transaction: trans);
                //if (!operations.Any(p => p.Mold == Shared.Operations.OperationMold.Query || p.Mold == Shared.Operations.OperationMold.Look))
                //{
                //    throw new BusinessException($"分配的权限至少要包含查询或是查看类型操作");
                //}
                await _rolePermissionRepository.DeleteAsync(p => p.RoleId == input.RoleId, conn, trans);
                foreach (var permissionId in input.PermissionIds)
                {
                    var permission = await _permissionRepository.SingleOrDefaultAsync(p => p.Id == permissionId);
                    if (permission == null)
                    {
                        throw new BusinessException($"不存在Id为{permissionId}的权限信息");
                    }
                    await _rolePermissionRepository.InsertAsync(new RolePermission() { PermissionId = permissionId, RoleId = input.RoleId }, conn, trans);
                }
            }, Connection);
        }

        public async Task Update(UpdateRoleInput input)
        {
            var role = await _roleRepository.GetAsync(input.Id);
            if (input.Name != role.Name)
            {
                var exsitRole = await _roleRepository.FirstOrDefaultAsync(p => p.Name == input.Name);
                if (exsitRole != null)
                {
                    throw new BusinessException($"系统中已经存在{input.Name}的角色");
                }

            }
            role = input.MapTo(role);
            await UnitOfWorkAsync(async (conn, trans) =>
            {
                await _roleRepository.UpdateAsync(role, conn, trans);
                //var queryOperationSql = "SELECT o.* FROM `Operation` as o LEFT JOIN Permission as p ON o.PermissionId = p.Id AND p.IsDeleted = 0 AND o.IsDeleted = 0 WHERE o.PermissionId IN @PermissionIds";

                //var operations = await conn.QueryAsync<Operation>(queryOperationSql, new { PermissionIds = input.PermissionIds }, transaction: trans);
                //if (!operations.Any(p => p.Mold == Shared.Operations.OperationMold.Query || p.Mold == Shared.Operations.OperationMold.Look))
                //{
                //    throw new BusinessException($"分配的权限至少要包含查询或是查看类型操作");
                //}
                await _rolePermissionRepository.DeleteAsync(p => p.RoleId == input.Id, conn, trans);
                foreach (var permissionId in input.PermissionIds)
                {
                    var permission = await _permissionRepository.SingleOrDefaultAsync(p => p.Id == permissionId);
                    if (permission == null)
                    {
                        throw new BusinessException($"不存在Id为{permissionId}的权限信息");
                    }
                    await _rolePermissionRepository.InsertAsync(new RolePermission() { PermissionId = permissionId, RoleId = input.Id }, conn, trans);
                }
            }, Connection);


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
LEFT JOIN Operation as o on oar.OperationId = o.Id AND o.IsDeleted = 0
LEFT JOIN Permission as p on p.Id = o.PermissionId AND p.Mold=1 AND  p.IsDeleted = 0
WHERE oar.ServiceId=@ServiceId";

            using (Connection)
            {
                var permission = await Connection.QueryAsync<Permission>(sql, new { ServiceId = serviceId });
                return permission.FirstOrDefault();
            }

        }
    }
}
