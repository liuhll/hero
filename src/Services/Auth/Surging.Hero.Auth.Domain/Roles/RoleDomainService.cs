using System.Collections.Generic;
using System.Threading.Tasks;
using Surging.Core.AutoMapper;
using Surging.Core.CPlatform.Exceptions;
using Surging.Core.Dapper.Manager;
using Surging.Core.Dapper.Repositories;
using Surging.Hero.Auth.Domain.Permissions;
using Surging.Hero.Auth.IApplication.Role.Dtos;
using Surging.Hero.Organization.IApplication.Department;

namespace Surging.Hero.Auth.Domain.Roles
{
    public class RoleDomainService : ManagerBase, IRoleDomainService
    {
        private readonly IDapperRepository<Role, long> _roleRepository;
        private readonly IDapperRepository<RolePermission, long> _rolePermissionRepository;
        private readonly IDapperRepository<Permission,long> _permissionRepository;

        public RoleDomainService(IDapperRepository<Role, long> roleRepository,
            IDapperRepository<RolePermission, long> rolePermissionRepository,
            IDapperRepository<Permission, long> permissionRepository) {
            _roleRepository = roleRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _permissionRepository = permissionRepository;
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
