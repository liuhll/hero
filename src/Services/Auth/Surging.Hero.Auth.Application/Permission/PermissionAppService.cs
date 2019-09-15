using System.Threading.Tasks;
using Surging.Core.AutoMapper;
using Surging.Core.CPlatform.Exceptions;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.Dapper.Repositories;
using Surging.Core.ProxyGenerator;
using Surging.Core.Validation.DataAnnotationValidation;
using Surging.Hero.Auth.Domain.Permissions.Menus;
using Surging.Hero.Auth.Domain.Permissions.Operations;
using Surging.Hero.Auth.IApplication.Permission;
using Surging.Hero.Auth.IApplication.Permission.Dtos;

namespace Surging.Hero.Auth.Application.Permission
{
    [ModuleName(AuthConstant.V1.PermissionMoudleName, Version = AuthConstant.V1.Version)]
    public class PermissionAppService : ProxyServiceBase, IPermissionAppService
    {
        private readonly IMenuDomainService _menuDomainService;
        private readonly IOperationDomainService _operationDomainService;
        private readonly IDapperRepository<Menu, long> _menuRepository;

        public PermissionAppService(IMenuDomainService menuDomainService,
            IOperationDomainService operationDomainService,
            IDapperRepository<Menu, long> menuRepository)
        {
            _menuDomainService = menuDomainService;
            _operationDomainService = operationDomainService;
            _menuRepository = menuRepository;
        }

        public async Task<string> CreateMenu(CreateMenuInput input)
        {
            input.CheckDataAnnotations().CheckValidResult();
            await _menuDomainService.Create(input);
            return "新增菜单成功";
        }

        public async Task<string> CreateOperation(CreateOperationInput input)
        {
            input.CheckDataAnnotations().CheckValidResult();
            await _operationDomainService.Create(input);
            return "新增操作成功";
        }

        public async Task<GetMenuOutput> GetMenu(long id)
        {
            var menu = await _menuRepository.FirstOrDefaultAsync(p => p.Id == id);
            if (menu == null) {
                throw new BusinessException($"不存在Id为{id}的菜单信息");
            }
            return menu.MapTo<GetMenuOutput>();
        }

        public async Task<string> UpdateMenu(UpdateMenuInput input)
        {
            input.CheckDataAnnotations().CheckValidResult();
            await _menuDomainService.Update(input);
            return "更新菜单信息成功";
        }
    }
}
