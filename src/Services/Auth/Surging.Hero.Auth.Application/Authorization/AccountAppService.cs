using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Surging.Core.AutoMapper;
using Surging.Core.CPlatform.Exceptions;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Session;
using Surging.Core.ProxyGenerator;
using Surging.Hero.Auth.Domain.Users;
using Surging.Hero.Auth.IApplication.Authorization;
using Surging.Hero.Auth.IApplication.Authorization.Dtos;
using Surging.Hero.Common.Runtime.Session;

namespace Surging.Hero.Auth.Application.Authorization
{
    [ModuleName(AuthConstant.V1.AccountMoudleName,Version = AuthConstant.V1.Version)]
    public class AccountAppService : ProxyServiceBase, IAccountAppService
    {
        private readonly ILoginManager _loginManager;
        private readonly IUserDomainService _userDomainService;
        private readonly ISurgingSession _surgingSession;
        public AccountAppService(ILoginManager loginManager,
            IUserDomainService userDomainService)
        {
            _loginManager = loginManager;
            _userDomainService = userDomainService;
            _surgingSession = NullSurgingSession.Instance;
        }

        public async Task<LoginUserInfo> GetLoginUser()
        {
            if (_surgingSession == null || !_surgingSession.UserId.HasValue) {
                throw new BusinessException("您当前没有登录系统");
            }
            var userInfo = await _userDomainService.GetUserNormInfoById(_surgingSession.UserId.Value);
            return userInfo.MapTo<LoginUserInfo>();

        }

        public async Task<IDictionary<string, object>> Login(LoginInput input)
        {
            return await _loginManager.Login(input.UserName, input.Password);
        }
    }
}
