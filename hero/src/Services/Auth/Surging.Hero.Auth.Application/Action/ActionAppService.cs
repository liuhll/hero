using System.Collections.Generic;
using System.Threading.Tasks;
using Surging.Core.CPlatform.Runtime.Server;
using Surging.Core.ProxyGenerator;
using Surging.Hero.Auth.IApplication.Action;
using Surging.Hero.Auth.IApplication.Action.Dtos;

namespace Surging.Hero.Auth.Application.Action
{
    public class ActionAppService : ProxyServiceBase, IActionAppService
    {
        public async Task<string> UpdateAppActions(ICollection<CreateActionInput> actions)
        {
            return "根据主机服务条目更新服务功能列表成功";
        }
    }
}
