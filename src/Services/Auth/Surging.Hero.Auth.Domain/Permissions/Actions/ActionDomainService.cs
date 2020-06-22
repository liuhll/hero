using Dapper;
using Surging.Hero.Common;
using Surging.Core.AutoMapper;
using Surging.Core.CPlatform.Utilities;
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
using System.Linq.Expressions;

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

        public async Task<IEnumerable<GetAppServiceOutput>> GetAppServices(QueryAppServiceInput query)
        {
            var sql = $"SELECT Application as AppService, '{query.ServiceHost}' as ServiceHost FROM `Action` WHERE ServiceHost='{query.ServiceHost}' GROUP BY Application";
            var sqlParams = new Dictionary<string, object>();
            if (!query.AppService.IsNullOrWhiteSpace())
            {
                sql += " HAVING Application LIKE @Application";
                sqlParams.Add("Application", $"%{query.AppService}%");
            }
            using (Connection)
            {
                var queryResult = await Connection.QueryAsync<GetAppServiceOutput>(sql, sqlParams);
                return queryResult;
            }
        }

        public async Task<IEnumerable<Action>> GetOperationOutputActions(long id)
        {
            var sql = "SELECT a.* FROM OperationActionRelation as oar LEFT JOIN Action as a on oar.ActionId = a.Id  WHERE oar.OperationId = @OperationId";
            using (Connection) {
                return await Connection.QueryAsync<Action>(sql, new { OperationId = id });
            }
        }

        public async Task<IEnumerable<GetServiceHostOutput>> GetServiceHosts(QueryServiceHostInput query)
        {
            var sql = "SELECT ServiceHost FROM `Action` GROUP BY ServiceHost";
            var sqlParams = new Dictionary<string, object>();
            if (!query.ServiceHost.IsNullOrWhiteSpace()) 
            {
                sql += " HAVING ServiceHost LIKE @ServiceHost";
                sqlParams.Add("ServiceHost", $"%{query.ServiceHost}%");
            }
            using (Connection) 
            {
                var queryResult = await Connection.QueryAsync<GetServiceHostOutput>(sql, sqlParams);
                return queryResult;
            }
        }

        public async Task<IEnumerable<GetActionOutput>> GetServices(QueryActionInput query)
        {
            Expression<Func<Action, bool>> queryExpression = p => (p.ServiceId.Contains(query.Service) || p.Name.Contains(query.Service)) && p.Status == Common.Status.Valid;
            if (!query.ServiceHost.IsNullOrWhiteSpace()) 
            {
                queryExpression = queryExpression.And(p => p.ServiceHost == query.ServiceHost);
            }
            if (!query.AppService.IsNullOrWhiteSpace())
            {
                queryExpression = queryExpression.And(p => p.Application == query.AppService);
            }
            return (await _actionRepository.GetAllAsync(queryExpression)).MapTo<IEnumerable<GetActionOutput>>();
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
