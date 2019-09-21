using Dapper;
using Surging.Core.AutoMapper;
using Surging.Core.CPlatform.Exceptions;
using Surging.Core.Dapper.Manager;
using Surging.Core.Dapper.Repositories;
using Surging.Hero.Auth.Domain.Roles;
using Surging.Hero.Auth.Domain.UserGroups;
using Surging.Hero.Auth.IApplication.Role.Dtos;
using Surging.Hero.Auth.IApplication.User.Dtos;
using Surging.Hero.Organization.IApplication.Department;
using Surging.Hero.Organization.IApplication.Position;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Surging.Hero.Auth.Domain.Users
{
    public class UserDomainService : ManagerBase, IUserDomainService
    {
        private readonly IDapperRepository<UserInfo, long> _userRepository;
        private readonly IDapperRepository<Roles.Role, long> _roleRepository;
        private readonly IDapperRepository<UserRole, long> _userRoleRepository;
        private readonly IDapperRepository<UserUserGroupRelation,long> _userUserGroupRelationRoleRepository;
        private readonly IPasswordHelper _passwordHelper;

        public UserDomainService(IDapperRepository<UserInfo, long> userRepository,
            IDapperRepository<Roles.Role, long> roleRepository,
            IDapperRepository<UserRole, long> userRoleRepository,
            IDapperRepository<UserUserGroupRelation, long> userUserGroupRelationRoleRepository,
        IPasswordHelper passwordHelper)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _userUserGroupRelationRoleRepository = userUserGroupRelationRoleRepository;
            _passwordHelper = passwordHelper;

        }


        public async Task Create(CreateUserInput input)
        {
            var userInfo = input.MapTo<UserInfo>();
            var departAppServiceProxy = GetService<IDepartmentAppService>();
            if (!await departAppServiceProxy.Check(userInfo.DeptId)) {
                throw new BusinessException($"不存在Id为{userInfo.DeptId}的部门信息");
            }

            var positionAppServiceProxy = GetService<IPositionAppService>();
            if (!await positionAppServiceProxy.Check(userInfo.PositionId)) {
                throw new BusinessException($"不存在Id为{userInfo.PositionId}的职位信息");
            }
        
            userInfo.Password = _passwordHelper.EncryptPassword(userInfo.UserName, userInfo.Password);
            await UnitOfWorkAsync(async (conn, trans) => {
                var userId =  await _userRepository.InsertAndGetIdAsync(userInfo, conn, trans);
                foreach (var roleId in input.RoleIds) {
                    var role = await _roleRepository.SingleOrDefaultAsync(p => p.Id == roleId);
                    if (role == null) {
                        throw new BusinessException($"系统中不存在Id为{roleId}的角色信息");
                    }
                    await _userRoleRepository.InsertAsync(new UserRole() { UserId = userId,RoleId = roleId });
                }
               
            }, Connection);
           
        }

        public async Task Delete(long id)
        {
            await UnitOfWorkAsync(async (conn, trans) => {
                await _userRepository.DeleteAsync(p=>p.Id == id, conn, trans);
                await _userRoleRepository.DeleteAsync(p => p.UserId == id, conn, trans);
                await _userUserGroupRelationRoleRepository.DeleteAsync(p => p.UserId == id, conn, trans);

                // todo: 删除其他关联表

            }, Connection);
        }

        public async Task<GetUserNormOutput> GetUserNormInfoById(long id)
        {
            var userInfo = await _userRepository.GetAsync(id);
            var userInfoOutput = userInfo.MapTo<GetUserNormOutput>();
            userInfoOutput.DeptName = (await GetService<IDepartmentAppService>().Get(userInfoOutput.DeptId)).Name;
            userInfoOutput.PositionName = (await GetService<IPositionAppService>().Get(userInfoOutput.PositionId)).Name;
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
            if (!await departAppServiceProxy.Check(input.DeptId))
            {
                throw new BusinessException($"不存在Id为{input.DeptId}的部门信息");
            }

            var positionAppServiceProxy = GetService<IPositionAppService>();
            if (!await positionAppServiceProxy.Check(input.PositionId))
            {
                throw new BusinessException($"不存在Id为{input.PositionId}的职位信息");
            }

            updateUser = input.MapTo(updateUser);
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
                    await _userRoleRepository.InsertAsync(new UserRole() { UserId = updateUser.Id, RoleId = roleId });
                }

            }, Connection);
        }
    }
}
