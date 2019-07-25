using System.Collections.Generic;
using System.Threading.Tasks;
using Surging.Core.CPlatform.Exceptions;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.Dapper.Repositories;
using Surging.Hero.Common.Enums;

namespace Surging.Hero.Auth.Domain.Users
{
    public class LoginManager : ILoginManager
    {
        private readonly IPasswordHelper _passwordHelper;
        private readonly IDapperRepository<UserInfo, long> _userRepository;
        public LoginManager(IPasswordHelper passwordHelper, IDapperRepository<UserInfo, long> userRepository)
        {
            _passwordHelper = passwordHelper;
            _userRepository = userRepository;
        }

        public async Task<IDictionary<string, object>> Login(string userName, string password)
        {
            var userInfo = await _userRepository.SingleOrDefaultAsync(p => p.UserName == userName || p.Phone == userName || p.Email == userName);
            if (userInfo == null)
            {
                throw new AuthException($"不存在账号为{userName}的用户");
            }
            if (userInfo.Status == Status.Invalid)
            {
                throw new AuthException($"账号为被激活,请先激活该账号");
            }
            if (!_passwordHelper.EncryptPassword(userInfo.UserName, password).Equals(userInfo.Password))
            {
                throw new AuthException($"密码不正确");
            }
            var payload = new Dictionary<string, object>() {
                { "userId",userInfo.Id },
                { "userName",userInfo.UserName},
            };
            return payload;
        }
    }
}
