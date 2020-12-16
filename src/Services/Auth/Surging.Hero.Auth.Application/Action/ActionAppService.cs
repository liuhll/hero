using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Surging.Core.CPlatform.Runtime.Server;
using Surging.Core.CPlatform.Utilities;
using Surging.Core.ProxyGenerator;
using Surging.Core.Validation.DataAnnotationValidation;
using Surging.Hero.Auth.Domain.Permissions.Actions;
using Surging.Hero.Auth.IApplication.Action;
using Surging.Hero.Auth.IApplication.Action.Dtos;

namespace Surging.Hero.Auth.Application.Action
{
    public class ActionAppService : ProxyServiceBase, IActionAppService
    {
        private const int hostNameSegmentLength = 3;
        private readonly IActionDomainService _actionDomainService;
        private readonly ILogger<ActionAppService> _logger;
        private readonly IServiceEntryProvider _serviceEntryProvider;

        public ActionAppService(IActionDomainService actionDomainService,
            IServiceEntryProvider serviceEntryProvider,
            ILogger<ActionAppService> logger)
        {
            _actionDomainService = actionDomainService;
            _serviceEntryProvider = serviceEntryProvider;
            _logger = logger;
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

        public async Task<string> InitAllActions()
        {
            var entries = _serviceEntryProvider.GetALLEntries();
            var actions = entries.Select(p => new InitActionActionInput
            {
                ServiceId = p.Descriptor.Id,
                ServiceHost = GetServiceHost(p.Type.FullName),
                Application = GetApplication(p.Type.FullName),
                WebApi = p.RoutePath,
                Method = string.Join(",", p.Methods),
                Name = p.Descriptor.GetMetadata<string>("GroupName"),
                DisableNetwork = p.Descriptor.GetMetadata<bool>("DisableNetwork"),
                EnableAuthorization = p.Descriptor.GetMetadata<bool>("EnableAuthorization"),
                AllowPermission = p.Descriptor.GetMetadata<bool>("AllowPermission"),
                Developer = p.Descriptor.GetMetadata<string>("Director"),
                Date = GetDevDate(p.Descriptor.GetMetadata<string>("Date"))
            }).ToList();

            try
            {
                await _actionDomainService.InitActions(actions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }

            return $"根据主机服务条目更新服务功能列表成功,一共有{actions.Count}个服务条目";
        }


        private string GetApplication(string serviceFullName)
        {
            return serviceFullName.Split(".").Last();
        }

        private string GetServiceHost(string serviceFullName)
        {
            return string.Join('.', serviceFullName.Split(".").Take(hostNameSegmentLength));
        }

        private DateTime? GetDevDate(string dateStr)
        {
            if (dateStr.IsNullOrEmpty()) return null;
            return Convert.ToDateTime(dateStr);
        }
    }
}