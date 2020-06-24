using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Surging.Core.CPlatform.Runtime.Server;
using Surging.Core.Dapper.Repositories;
using Surging.Core.ProxyGenerator;
using Surging.Core.Validation.DataAnnotationValidation;
using Surging.Hero.Auth.Domain.Permissions.Actions;
using Surging.Hero.Auth.IApplication.Action;
using Surging.Hero.Auth.IApplication.Action.Dtos;

namespace Surging.Hero.Auth.Application.Action
{
    public class ActionAppService : ProxyServiceBase, IActionAppService
    {
        private readonly IActionDomainService _actionDomainService;
        public ActionAppService(IActionDomainService actionDomainService)
        {
            _actionDomainService = actionDomainService;
        }

        public async Task<IEnumerable<GetAppServiceOutput>> GetAppServices(QueryAppServiceInput query)
        {
            query.CheckDataAnnotations().CheckValidResult();
            return await _actionDomainService.GetAppServices(query);
        }

        public async Task<IEnumerable<GetServiceHostOutput>> GetServiceHosts(QueryServiceHostInput query)
        {
            return await _actionDomainService.GetServiceHosts(query);
        }

        public async Task<IEnumerable<GetActionOutput>> GetServices(QueryActionInput query)
        {
            query.CheckDataAnnotations().CheckValidResult();
            return await _actionDomainService.GetActionServices(query);
        }

        public async Task<IEnumerable<GetTreeActionOutput>> GetServicesTree()
        {
            return await _actionDomainService.GetServicesTree();
        }

        public async Task<string> InitActions(ICollection<InitActionActionInput> actions)
        {
            await _actionDomainService.InitActions(actions);
            return "根据主机服务条目更新服务功能列表成功";
        }
    }
}
