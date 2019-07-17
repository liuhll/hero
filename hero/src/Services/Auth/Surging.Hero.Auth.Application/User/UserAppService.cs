using Surging.Core.CPlatform.Ioc;
using Surging.Core.ProxyGenerator;
using Surging.Hero.Auth.Domain.Shared;
using Surging.Hero.Auth.IApplication.User;
using Surging.Hero.Auth.IApplication.User.Dtos;
using System.Threading.Tasks;
using Surging.Core.Validation.DataAnnotationValidation;
using Surging.Core.AutoMapper;
using Surging.Hero.Auth.Domain.User;

namespace Surging.Hero.Auth.Application.User
{
    [ModuleName(ModuleNameConstants.UserModule, Version = ModuleNameConstants.ModuleVersionV1)]
    public class UserAppService : ProxyServiceBase, IUserAppService
    {
        private readonly IUserDomainService _userDomainService;

        public UserAppService(IUserDomainService userDomainService)
        {
            _userDomainService = userDomainService;
        }


        public async Task<string> CreateUser(CreateUserInput input)
        {
            input.CheckDataAnnotations().CheckValidResult();
            var userInfo = input.MapTo<UserInfo>();            
            await _userDomainService.CreateUser(userInfo);
            return "新增员工成功";
        }
    }
}
