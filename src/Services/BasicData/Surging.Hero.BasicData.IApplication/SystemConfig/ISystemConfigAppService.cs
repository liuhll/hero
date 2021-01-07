using System.Threading.Tasks;
using Surging.Cloud.CPlatform.Ioc;
using Surging.Cloud.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Hero.BasicData.IApplication.SystemConfig.Dtos;
using Surging.Hero.Common;

namespace Surging.Hero.BasicData.IApplication.SystemConfig
{
    [ServiceBundle(HeroConstants.RouteTemplet)]
    public interface ISystemConfigAppService : IServiceKey
    {
        /// <summary>
        /// 获取系统配置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ServiceRoute("")]
        [Service(Name = "获取系统配置", Date = "2021-01-07", Director = Developers.Liuhll, AllowPermission = true)]
        Task<GetSystemConfigOutput> GetSystemConfig();
    }
}