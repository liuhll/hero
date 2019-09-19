using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Surging.Core.AutoMapper;
using Surging.Core.CPlatform.Exceptions;
using Surging.Core.Dapper.Manager;
using Surging.Core.Dapper.Repositories;
using Surging.Hero.Auth.IApplication.Permission.Dtos;
using Surging.Hero.Common;

namespace Surging.Hero.Auth.Domain.Permissions.Operations
{
    public class OperationDomainService : ManagerBase, IOperationDomainService
    {
        private readonly IDapperRepository<Permission, long> _permissionRepository;
        private readonly IDapperRepository<Operation, long> _operationRepository;
        private readonly IDapperRepository<Menus.Menu, long> _menuRepository;
        private readonly IDapperRepository<OperationActionRelation, long> _operationActionRepository;
        private readonly IDapperRepository<Actions.Action, long> _actionRepository;

        public OperationDomainService(IDapperRepository<Permission, long> permissionRepository,
            IDapperRepository<Operation, long> operationRepository,
            IDapperRepository<Menus.Menu, long> menuRepository,
            IDapperRepository<OperationActionRelation, long> operationActionRepository,
            IDapperRepository<Actions.Action, long> actionRepository)
        {
            _permissionRepository = permissionRepository;
            _operationRepository = operationRepository;
            _menuRepository = menuRepository;
            _operationActionRepository = operationActionRepository;
            _actionRepository = actionRepository;
        }

        public async Task Create(CreateOperationInput input)
        {
            var menu = await _menuRepository.SingleOrDefaultAsync(p => p.Id == input.MenuId);
            if (menu == null)
            {
                throw new BusinessException($"不存在Id为{input.MenuId}的菜单信息");
            }
            var operation = input.MapTo<Operation>();
            var permission = input.MapTo<Permission>();

            var operationCount = await _operationRepository.GetCountAsync(p => p.MenuId == input.MenuId);
            operation.Code = menu.Code + HeroConstants.CodeRuleRestrain.CodeSeparator + (operationCount + 1).ToString().PadLeft(HeroConstants.CodeRuleRestrain.CodeCoverBit, HeroConstants.CodeRuleRestrain.CodeCoverSymbol);
            operation.Level = menu.Level + 1;

            await UnitOfWorkAsync(async (conn, trans) =>
            {
                var permissionId = await _permissionRepository.InsertAndGetIdAsync(permission, conn, trans);
                operation.PermissionId = permissionId;
                var operationId = await _operationRepository.InsertAndGetIdAsync(operation, conn, trans);
                if (input.ActionIds != null && input.ActionIds.Any())
                {
                    foreach (var actionId in input.ActionIds)
                    {
                        var action = await _actionRepository.SingleOrDefaultAsync(p => p.Id == actionId);
                        if (action == null)
                        {
                            throw new BusinessException($"系统中不存在Id为{actionId}的方法");
                        }
                        var operationAction = new OperationActionRelation() { ActionId = actionId, OperationId = operationId, ServiceId = action.ServiceId };
                        await _operationActionRepository.InsertAsync(operationAction, conn, trans);
                    }
                }
            }, Connection);
        }

        public async Task Delete(long id)
        {
            var operation = await _operationRepository.SingleOrDefaultAsync(p => p.Id == id);
            if (operation == null)
            {
                throw new BusinessException($"不存在Id为{id}的操作信息");
            }
            await UnitOfWorkAsync(async (conn, trans) => {
                await _operationRepository.DeleteAsync(p => p.Id == id, conn, trans);
                await _operationActionRepository.DeleteAsync(p => p.OperationId == id, conn, trans);
                await _permissionRepository.DeleteAsync(p => p.Id == operation.Id, conn, trans);
            }, Connection);
        }

        public async Task<IEnumerable<Operation>> GetAll()
        {
            return await _operationRepository.GetAllAsync();
        }

        public async Task Update(UpdateOperationInput input)
        {
            var operation = await _operationRepository.SingleOrDefaultAsync(p => p.Id == input.Id);
            if (operation == null)
            {
                throw new BusinessException($"不存在Id为{input.Id}的操作信息");
            }
            var permission = await _permissionRepository.SingleOrDefaultAsync(p => p.Id == operation.PermissionId);
            if (permission == null)
            {
                throw new BusinessException($"不存在Id为{operation.PermissionId}的权限信息");
            }
            var menu = await _menuRepository.SingleOrDefaultAsync(p => p.Id == operation.MenuId);
            if (menu == null)
            {
                throw new BusinessException($"不存在Id为{operation.MenuId}的菜单信息");
            }
            operation = input.MapTo(operation);
            permission = input.MapTo(permission);

            await UnitOfWorkAsync(async (conn, trans) =>
            {
                await _permissionRepository.UpdateAsync(permission, conn, trans);
                await _operationRepository.UpdateAsync(operation, conn, trans);
                await _operationActionRepository.DeleteAsync(p => p.OperationId == permission.Id, conn, trans);
                if (input.ActionIds != null && input.ActionIds.Any())
                {
                    foreach (var actionId in input.ActionIds)
                    {
                        var action = await _actionRepository.SingleOrDefaultAsync(p => p.Id == actionId);
                        if (action == null)
                        {
                            throw new BusinessException($"系统中不存在Id为{actionId}的方法");
                        }
                        var operationAction = new OperationActionRelation() { ActionId = actionId, OperationId = operation.Id, ServiceId = action.ServiceId };
                        await _operationActionRepository.InsertAsync(operationAction, conn, trans);
                    }
                }
            }, Connection);

        }
    }
}
