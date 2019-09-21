using System.Collections.Generic;
using System.Threading.Tasks;
using Surging.Core.AutoMapper;
using Surging.Core.CPlatform.Exceptions;
using Surging.Core.Dapper.Manager;
using Surging.Core.Dapper.Repositories;
using Surging.Hero.Auth.Domain.Permissions;
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
        private readonly IDapperRepository<Permission,long> _permissionRepository;
        private readonly IDapperRepository<UserRole, long> _userRoleRepository;
        private readonly IDapperRepository<UserGroupRole, long> _userGroupRoleRepository;

        public RoleDomainService(IDapperRepository<Role, long> roleRepository,
            IDapperRepository<RolePermission, long> rolePermissionRepository,
            IDapperRepository<Permission, long> permissionRepository,
            IDapperRepository<UserRole, long> userRoleRepository,
            IDapperRepository<UserGroupRole, long> userGroupRoleRepository) {
            _roleRepository = roleRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _permissionRepository = permissionRepository;
            _userRoleRepository = userRoleRepository;
            _userGroupRoleRepository = userGroupRoleRepository;
        }

        public async Task Create(CreateRoleInput input)
        {
            var exsitRole = await _roleRepository.FirstOrDefaultAsync(p => p.Name == input.Name && p.DeptId == input.DeptId);
            if (exsitRole != null) {
                throw new BusinessException($"系统中已经存在{input.Name}的角色");
            }
            var deptAppServiceProxy = GetService<IDepartmentAppService>();
            if (input.DeptId.HasValue && input.DeptId != 0 && !await deptAppServiceProxy.Check(input.DeptId.Value)) {
                throw new BusinessException($"系统中不存在Id为{input.DeptId}的部门信息");
            }
            var role = input.MapTo<Role>();
            await _roleRepository.InsertAsync(role);
        }

        public async Task Delete(long roleid)
        {
            var role = await _roleRepository.SingleOrDefaultAsync(p => p.Id == roleid);
            if (role == null)
            {
                throw new BusinessException($"不存在Id为{roleid}的角色信息");
            }
            var userRoleCount = await _userRoleRepository.GetCountAsync(p => p.RoleId == roleid);
            if (userRoleCount > 0) {
                throw new BusinessException($"{role.Name}被分配用户,请先删除相关授权的用户信息");
            }
            var userGroupRoleCount = await _userGroupRoleRepository.GetCountAsync(p => p.RoleId == roleid);
            if (userRoleCount > 0)
            {
                throw new BusinessException($"{role.Name}被分配用户组,请先删除相关授权的用户组信息");
            }
            await UnitOfWorkAsync(async(conn,trans)=> {
                await _roleRepository.DeleteAsync(p => p.Id == roleid,conn,trans);
                await _rolePermissionRepository.DeleteAsync(p => p.RoleId == roleid, conn, trans);
            },Connection);

        }

        public async Task<IEnumerable<RolePermission>> GetRolePermissions(long roleId)
        {
            var role = await _roleRepository.SingleOrDefaultAsync(p => p.Id == roleId);
            if (role == null) {
                throw new BusinessException($"不存在Id为{roleId}的角色信息");
            }
            return await _rolePermissionRepository.GetAllAsync(p => p.RoleId == roleId);
        }

        public async Task SetPermissions(SetRolePermissionInput input)
        {
            var role = await _roleRepository.SingleOrDefaultAsync(p => p.Id == input.RoleId);
            if (role == null)
            {
                throw new BusinessException($"不存在Id为{input.RoleId}的角色信息");
            }
            await UnitOfWorkAsync(async (conn, trans) => {
                await _rolePermissionRepository.DeleteAsync(p => p.RoleId == input.RoleId, conn, trans);
                foreach (var permissionId in input.PermissionIds) {
                    var permission = await _permissionRepository.SingleOrDefaultAsync(p => p.Id == permissionId);
                    if (permission == null) {
                        throw new BusinessException($"不存在Id为{permissionId}的权限信息");
                    }
                    await _rolePermissionRepository.InsertAsync(new RolePermission() { PermissionId = permissionId, RoleId = input.RoleId }, conn, trans);
                }
            }, Connection);
        }

        public async Task Update(UpdateRoleInput input)
        {
            var role = await _roleRepository.GetAsync(input.Id);
            if (input.Name != role.Name) {
                var exsitRole = await _roleRepository.FirstOrDefaultAsync(p => p.Name == input.Name && p.DeptId == input.DeptId);
                if (exsitRole != null)
                {
                    throw new BusinessException($"系统中已经存在{input.Name}的角色");
                }
                var deptAppServiceProxy = GetService<IDepartmentAppService>();
                if (input.DeptId.HasValue && input.DeptId != 0 && !await deptAppServiceProxy.Check(input.DeptId.Value))
                {
                    throw new BusinessException($"系统中不存在Id为{input.DeptId}的部门信息");
                }
            }
            role = input.MapTo(role);
            await _roleRepository.UpdateAsync(role);
        
        }

        public async Task UpdateStatus(UpdateRoleStatusInput input)
        {
            var role = await _roleRepository.GetAsync(input.Id);
            role.Status = input.Status;
            await _roleRepository.UpdateAsync(role);
        }
    }
}
