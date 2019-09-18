using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Surging.Core.CPlatform.Exceptions;
using Surging.Core.ProxyGenerator;
using Surging.Hero.Auth.Domain.Users;
using Surging.Hero.Auth.IApplication.Authorization;
using Surging.Hero.Auth.IApplication.Authorization.Dtos;

namespace Surging.Hero.Auth.Application.Authorization
{
    public class AccountAppService : ProxyServiceBase, IAccountAppService
    {
        private readonly ILoginManager _loginManager;

        public AccountAppService(ILoginManager loginManager)
        {
            _loginManager = loginManager;
        }
        public async Task<IDictionary<string, object>> Login(LoginInput input)
        {
            return await _loginManager.Login(input.UserName, input.Password);
        }
    }
}
