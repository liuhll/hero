using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Surging.Core.AutoMapper;
using Surging.Core.CPlatform.Exceptions;
using Surging.Core.Dapper.Manager;
using Surging.Core.Dapper.Repositories;
using Surging.Hero.Auth.Domain.Roles;
using Surging.Hero.Auth.Domain.Users;
using Surging.Hero.Auth.IApplication.Role.Dtos;
using Surging.Hero.Auth.IApplication.User.Dtos;
using Surging.Hero.Auth.IApplication.UserGroup.Dtos;
using Surging.Hero.Common;
using Surging.Hero.Organization.IApplication.Department;
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

        public UserGroupDomainService(IDapperRepository<UserGroup, long> userGroupRepository,
            IDapperRepository<UserGroupRole, long> userGroupRoleRepository,
            IDapperRepository<UserUserGroupRelation, long> userUserGroupRelationRepository,
            IDapperRepository<UserInfo, long> userRepository,
            IDapperRepository<Roles.Role, long> roleRepository,
            IRoleDomainService roleDomainService)
        {
            _userGroupRepository = userGroupRepository;
            _userGroupRoleRepository = userGroupRoleRepository;
            _userUserGroupRelationRepository = userUserGroupRelationRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _roleDomainService = roleDomainService;
        }

        public async Task Create(CreateUserGroupInput input)
        {
            var exsitUserGroup = await _userGroupRepository.FirstOrDefaultAsync(p => p.Name == input.Name.Trim());
            if (exsitUserGroup != null) 
            {
                throw new BusinessException($"系统中已经存在{input.Name}的用户组");
            }
            var userGroup = input.MapTo<UserGroup>();
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

        }

        public async Task Delete(long id)
        {
            var userGroup = await _userGroupRepository.SingleOrDefaultAsync(p => p.Id == id);
            if (userGroup == null)
            {
                throw new BusinessException($"不存在Id为{id}的用户组信息");
            }
            await UnitOfWorkAsync(async (conn, trans) =>
            {
                await _userGroupRepository.DeleteAsync(p => p.Id == id, conn, trans);
                await _userGroupRoleRepository.DeleteAsync(p => p.UserGroupId == id, conn, trans);
                await _userUserGroupRelationRepository.DeleteAsync(p => p.UserGroupId == id, conn, trans);

            }, Connection);
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
            var userGroups = await _userUserGroupRelationRepository.GetAllAsync(p => p.UserId == userId);
            foreach (var userGroup in userGroups)
            {
                var userGroupRoles = await GetUserGroupRoles(userGroup.UserGroupId);
                foreach (var userGroupRole in userGroupRoles)
                {
                    if (await _roleDomainService.CheckPermission(userGroupRole.Id,serviceId))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public async Task<string> AllocationUsers(AllocationUserIdsInput input)
        {
            var userGroup = await _userGroupRepository.SingleOrDefaultAsync(p => p.Id == input.UserGroupId);
            if (userGroup == null)
            {
                throw new BusinessException($"不存在Id为{input.UserGroupId}的用户组");
            }
            await UnitOfWorkAsync(async (conn, trans) =>
            {
                foreach (var userId in input.UserIds)
                {
                    var userInfo = await _roleRepository.SingleOrDefaultAsync(p => p.Id == userId, conn, trans);
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
        }

        public async Task DeleteUserGroupUser(DeleteUserGroupUserInput input)
        {
            await _userUserGroupRelationRepository.DeleteAsync(p => p.UserId == input.UserId && p.UserGroupId == input.UserGroupId);
        }
    }
}
