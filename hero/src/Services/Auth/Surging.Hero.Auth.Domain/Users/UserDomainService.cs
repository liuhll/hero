using Surging.Core.CPlatform.Exceptions;
using Surging.Core.Dapper.Manager;
using Surging.Core.Dapper.Repositories;
using Surging.Hero.Organization.IApplication.Department;
using Surging.Hero.Organization.IApplication.Position;
using System;
using System.Threading.Tasks;

namespace Surging.Hero.Auth.Domain.Users
{
    public class UserDomainService : ManagerBase, IUserDomainService
    {
        private readonly IDapperRepository<UserInfo, long> _userRepository;
        private readonly IPasswordHelper _passwordHelper;

        public UserDomainService(IDapperRepository<UserInfo, long> userRepository,
            IPasswordHelper passwordHelper)
        {
            _userRepository = userRepository;
            _passwordHelper = passwordHelper;

        }


        public async Task CreateUser(UserInfo userInfo)
        {
            var departAppServiceProxy = GetService<IDepartmentAppService>();
            if (!await departAppServiceProxy.Check(userInfo.DeptId)) {
                throw new BusinessException($"不存在Id为{userInfo.DeptId}的部门信息");
            }

            var positionAppServiceProxy = GetService<IPositionAppService>();
            if (!await positionAppServiceProxy.Check(userInfo.PositionId)) {
                throw new BusinessException($"不存在Id为{userInfo.PositionId}的职位信息");
            }
        
            userInfo.Password = _passwordHelper.EncryptPassword(userInfo.UserName, userInfo.Password);          
            await _userRepository.InsertAsync(userInfo);
        }

        public async Task ResetPassword(UserInfo userInfo, string newPassword)
        {
            userInfo.Password = _passwordHelper.EncryptPassword(userInfo.UserName, newPassword);
            await _userRepository.UpdateAsync(userInfo);
        }
    }
}
