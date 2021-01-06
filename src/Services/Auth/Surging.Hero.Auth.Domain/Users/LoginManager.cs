using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Surging.Cloud.Caching;
using Surging.Cloud.CPlatform;
using Surging.Cloud.CPlatform.Cache;
using Surging.Cloud.CPlatform.Configurations;
using Surging.Cloud.CPlatform.Exceptions;
using Surging.Cloud.Dapper.Repositories;
using Surging.Hero.Auth.Domain.Tenants;
using Surging.Hero.Auth.IApplication.Authorization.Dtos;
using Surging.Hero.Common;
using AppConfig = Surging.Cloud.CPlatform.AppConfig;

namespace Surging.Hero.Auth.Domain.Users
{
    public class LoginManager : ILoginManager
    {
        private readonly IPasswordHelper _passwordHelper;
        private readonly IDapperRepository<UserInfo, long> _userRepository;
        private readonly IDapperRepository<Tenant, long> _tenantRepository;
        private readonly ICacheProvider _cacheProvider;

        public LoginManager(IPasswordHelper passwordHelper, 
            IDapperRepository<UserInfo, long> userRepository, 
            IDapperRepository<Tenant, long> tenantRepository)
        {
            _passwordHelper = passwordHelper;
            _userRepository = userRepository;
            _tenantRepository = tenantRepository;
            _cacheProvider = CacheContainer.GetService<ICacheProvider>(HeroConstants.CacheProviderKey);;
        }

        public async Task<IDictionary<string, object>> Login(LoginInput input)
        {
            if (AppConfig.ServerOptions.Environment != RuntimeEnvironment.Development)
            {
                await ValidCaptcha(input);
            }
           
            await ValidTenant(input.TenantId);
            var userInfo = await _userRepository.SingleOrDefaultAsync(p =>
               ( p.UserName == input.UserName || p.Phone == input.UserName || p.Email == input.UserName) && p.TenantId == input.TenantId);
            if (userInfo == null) throw new BusinessException($"不存在账号为{input.UserName}的用户");
            if (userInfo.Status == Status.Invalid) throw new BusinessException("账号为被激活,请先激活该账号");
            if (!_passwordHelper.EncryptPassword(userInfo.UserName, input.Password).Equals(userInfo.Password))
                throw new BusinessException("密码不正确");
            var payload = new Dictionary<string, object>
            {
                {ClaimTypes.UserId, userInfo.Id},
                {ClaimTypes.UserName, userInfo.UserName},
                {ClaimTypes.OrgId, userInfo.OrgId},
              
            };
            if (userInfo.TenantId.HasValue)
            {
                payload.Add(ClaimTypes.TenantId, userInfo.TenantId);
            }
            return payload;
        }

        private async Task ValidTenant(long tenantId)
        {
            var tenant = await _tenantRepository.SingleOrDefaultAsync(p => p.Id == tenantId);
            if (tenant == null)
            {
                throw new BusinessException($"不存在id为{tenantId}的租户");
            }

            if (tenant.Status == Status.Invalid)
            {
                throw new BusinessException("当前租户被冻结,您没有权限登录该系统");
            }
        }

        private async Task ValidCaptcha(LoginInput input)
        {
            var captchakey = string.Format(HeroConstants.CacheKey.Captcha, input.Uuid);
            var captcha = await _cacheProvider.GetAsync<string>(captchakey);
            if (captcha == null || !captcha.Equals(input.Captcha, StringComparison.OrdinalIgnoreCase))
            {
                throw new BusinessException("验证码不正确,请重新输入");
            }

            await _cacheProvider.RemoveAsync(captchakey);
        }
    }
}