using Dapper;
using Surging.Core.AutoMapper;
using Surging.Core.Dapper.Manager;
using Surging.Core.Dapper.Repositories;
using Surging.Hero.Auth.Domain.Permissions.Operations;
using Surging.Hero.Auth.IApplication.Action.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Surging.Hero.Auth.Domain.Permissions.Actions
{
    public class ActionDomainService : ManagerBase, IActionDomainService
    {
        private readonly IDapperRepository<Action, long> _actionRepository;
        private readonly IDapperRepository<OperationActionRelation, long> _operationActionRelationRepository;
        private readonly AsyncLocal<string> _lock = new AsyncLocal<string>();

        public ActionDomainService(IDapperRepository<Action, long> actionRepository,
            IDapperRepository<OperationActionRelation, long> operationActionRelationRepository)
        {
            _actionRepository = actionRepository;
            _operationActionRelationRepository = operationActionRelationRepository;
        }

        public async Task<IEnumerable<Action>> GetOperationOutputActions(long id)
        {
            var sql = "SELECT a.* FROM OperationActionRelation as oar LEFT JOIN Action as a on oar.ActionId = a.Id  WHERE oar.OperationId = @OperationId";
            using (Connection) {
                return await Connection.QueryAsync<Action>(sql, new { OperationId = id });
            }
        }

        public async Task InitActions(ICollection<InitActionActionInput> actions)
        {
            await UnitOfWorkAsync(async (conn, trans) => {
                foreach (var action in actions)
                {
                    var actionEntity = await _actionRepository.SingleOrDefaultAsync(p => p.ServiceId == action.ServiceId);
                    if (actionEntity == null)
                    {
                        actionEntity = action.MapTo<Action>();
                        var actionId = await _actionRepository.InsertAndGetIdAsync(actionEntity, conn, trans);
                        var operationActionRelations = await _operationActionRelationRepository.GetAllAsync(p => p.ServiceId == action.ServiceId);
                        if (operationActionRelations.Any())
                        {
                            foreach (var operationActionRelation in operationActionRelations)
                            {
                                operationActionRelation.ActionId = actionId;
                                await _operationActionRelationRepository.UpdateAsync(operationActionRelation, conn, trans);
                            }
                        }
                    }
                    else
                    {
                        actionEntity = action.MapTo(actionEntity);
                        await _actionRepository.UpdateAsync(actionEntity, conn, trans);
                        var operationActionRelations = await _operationActionRelationRepository.GetAllAsync(p => p.ServiceId == action.ServiceId);
                        if (operationActionRelations.Any())
                        {
                            foreach (var operationActionRelation in operationActionRelations)
                            {
                                if (operationActionRelation.ActionId != actionEntity.Id)
                                {
                                    operationActionRelation.ActionId = actionEntity.Id;
                                    await _operationActionRelationRepository.UpdateAsync(operationActionRelation, conn, trans);
                                }

                            }
                        }
                    }
                }
            }, Connection);

        }
    }
}
