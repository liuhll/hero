using Surging.Core.CPlatform.Exceptions;
using Surging.Core.Dapper.Manager;
using Surging.Core.Dapper.Repositories;
using System;
using System.Threading.Tasks;

namespace Surging.Hero.Auth.Domain.User
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
            var existUser = await _userRepository.FirstOrDefaultAsync(p => p.UserName == userInfo.UserName);
            if (existUser != null)
            {
                throw new UserFriendlyException($"已经存在用户名为{userInfo.UserName}的用户");
            }
            existUser = await _userRepository.FirstOrDefaultAsync(p => p.Phone == userInfo.Phone);
            if (existUser != null)
            {
                throw new UserFriendlyException($"已经存在手机号码为{userInfo.Phone}的用户");
            }
            existUser = await _userRepository.FirstOrDefaultAsync(p => p.Email == userInfo.Email);
            if (existUser != null)
            {
                throw new UserFriendlyException($"已经存在Email为{userInfo.Email}的用户");
            }
            userInfo.Password = _passwordHelper.EncryptPassword(userInfo.UserName, userInfo.Password);
            await _userRepository.InsertAsync(userInfo);
        }
    }
}
