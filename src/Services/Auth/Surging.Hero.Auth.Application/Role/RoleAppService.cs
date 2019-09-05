using System.Threading.Tasks;
using Surging.Core.ProxyGenerator;
using Surging.Core.Validation.DataAnnotationValidation;
using Surging.Hero.Auth.Domain.Roles;
using Surging.Hero.Auth.IApplication.Role;
using Surging.Hero.Auth.IApplication.Role.Dtos;

namespace Surging.Hero.Auth.Application.Role
{
    public class RoleAppService : ProxyServiceBase, IRoleAppService
    {
        private readonly IRoleDomainService _roleDomainService;

        public RoleAppService(IRoleDomainService roleDomainService) {
            _roleDomainService = roleDomainService;
        }

        public async Task<string> Create(CreateRoleInput input)
        {
            input.CheckDataAnnotations().CheckValidResult();
            await _roleDomainService.Create(input);
            return "新增角色信息成功";
        }

        public async Task<string> Update(UpdateRoleInput input)
        {
            input.CheckDataAnnotations().CheckValidResult();
            await _roleDomainService.Update(input);
            return "更新角色信息成功";
        }
    }
}
