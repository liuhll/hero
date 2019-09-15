using System.Collections.Generic;
using System.Threading.Tasks;
using Surging.Core.AutoMapper;
using Surging.Core.CPlatform.Exceptions;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.Dapper.Repositories;
using Surging.Core.ProxyGenerator;
using Surging.Core.Validation.DataAnnotationValidation;
using Surging.Hero.Auth.Domain.Permissions.Actions;
using Surging.Hero.Auth.Domain.Permissions.Menus;
using Surging.Hero.Auth.Domain.Permissions.Operations;
using Surging.Hero.Auth.IApplication.Action.Dtos;
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
        private readonly IDapperRepository<Operation, long> _operationRepository;
        private readonly IActionDomainService _actionDomainService;

        public PermissionAppService(IMenuDomainService menuDomainService,
            IOperationDomainService operationDomainService,
            IDapperRepository<Menu, long> menuRepository,
            IDapperRepository<Operation, long> operationRepository,
            IActionDomainService actionDomainService)
        {
            _menuDomainService = menuDomainService;
            _operationDomainService = operationDomainService;
            _menuRepository = menuRepository;
            _operationRepository = operationRepository;
            _actionDomainService = actionDomainService;
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

        public async Task<GetOperationOutput> GetOperation(long id)
        {
            var operation = await _operationRepository.SingleOrDefaultAsync(p => p.Id == id);
            if (operation == null) {
                throw new BusinessException($"不存在Id为{id}的操作信息");
            }
            var operationOutput = operation.MapTo<GetOperationOutput>();
            operationOutput.Actions = (await _actionDomainService.GetOperationOutputActions(operation.Id)).MapTo<IEnumerable<GetActionOutput>>();
            return operationOutput;
        }

        public async Task<string> UpdateMenu(UpdateMenuInput input)
        {
            input.CheckDataAnnotations().CheckValidResult();
            await _menuDomainService.Update(input);
            return "更新菜单信息成功";
        }

        public async Task<string> UpdateOperation(UpdateOperationInput input)
        {
            input.CheckDataAnnotations().CheckValidResult();
            await _operationDomainService.Update(input);
            return "更新操作成功";
        }
    }
}
