using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Hero.Auth.IApplication.Action.Dtos;
using Surging.Hero.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Surging.Hero.Auth.IApplication.Action
{
    [ServiceBundle(HeroConstants.RouteTemplet)]
    public interface IActionAppService : IServiceKey
    {
        /// <summary>
        /// 初始化服务方法
        /// </summary>
        /// <param name="actions"></param>
        /// <returns></returns>
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "初始化服务方法", DisableNetwork = true)]
        [HttpPost]
        [ServiceRoute("init")]
        Task<string> InitActions(ICollection<InitActionActionInput> actions);

        /// <summary>
        /// 获取微服务主机列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [ServiceRoute("host/search")]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "获取微服务主机列表", AllowPermission = true)]
        Task<IEnumerable<GetServiceHostOutput>> GetServiceHosts(QueryServiceHostInput query);

        /// <summary>
        /// 获取应用服务
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [ServiceRoute("service/search")]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "获取应用服务", AllowPermission = true)]
        Task<IEnumerable<GetAppServiceOutput>> GetAppServices(QueryAppServiceInput query);

        /// <summary>
        /// 查询服务方法
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [ServiceRoute("action/search")]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "查询服务方法", AllowPermission = true)]
        Task<IEnumerable<GetActionOutput>> GetServices(QueryActionInput query);

        /// <summary>
        /// 获取服务方法树形结构
        /// </summary>
        /// <returns></returns>
        [ServiceRoute("service/tree")]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "获取服务方法树形结构", AllowPermission = true)]
        Task<IEnumerable<GetTreeActionOutput>> GetServicesTree();

    }
}
