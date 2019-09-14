using System.Threading.Tasks;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.ProxyGenerator;
using Surging.Core.Validation.DataAnnotationValidation;
using Surging.Hero.Auth.Domain.Permissions.Menus;
using Surging.Hero.Auth.IApplication.Permission;
using Surging.Hero.Auth.IApplication.Permission.Dtos;

namespace Surging.Hero.Auth.Application.Permission
{
    [ModuleName(AuthConstant.V1.PermissionMoudleName, Version = AuthConstant.V1.Version)]
    public class PermissionAppService : ProxyServiceBase, IPermissionAppService
    {
        private readonly IMenuDomainService _menuDomainService;

        public PermissionAppService(IMenuDomainService menuDomainService) {
            _menuDomainService = menuDomainService;
        }

        public async Task<string> CreateMenu(CreateMenuInput input)
        {
            input.CheckDataAnnotations().CheckValidResult();
            await _menuDomainService.Create(input);
            return "新增菜单成功";
        }
    }
}
