using System.Threading.Tasks;
using Surging.Core.ProxyGenerator;
using Surging.Core.Validation.DataAnnotationValidation;
using Surging.Hero.Auth.Domain.UserGroups;
using Surging.Hero.Auth.IApplication.UserGroup;
using Surging.Hero.Auth.IApplication.UserGroup.Dtos;

namespace Surging.Hero.Auth.Application.UserGroup
{
    public class UserGroupAppService : ProxyServiceBase, IUserGroupAppService
    {
        private readonly IUserGroupDomainService _userGroupDomainService;

        public UserGroupAppService(IUserGroupDomainService userGroupDomainService) {
            _userGroupDomainService = userGroupDomainService;
        }

        public async Task<string> Create(CreateUserGroupInput input)
        {
            input.CheckDataAnnotations().CheckValidResult();
            await _userGroupDomainService.Create(input);
            return "新增用户组成功";
        }

        public async Task<string> Update(UpdateUserGroupInput input)
        {
            input.CheckDataAnnotations().CheckValidResult();
            await _userGroupDomainService.Update(input);
            return "更新用户组成功"; ;
        }
    }
}
