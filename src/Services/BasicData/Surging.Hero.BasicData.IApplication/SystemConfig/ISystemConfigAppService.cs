using System.Collections.Generic;
using System.Threading.Tasks;
using Surging.Cloud.CPlatform.Ioc;
using Surging.Cloud.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Hero.BasicData.IApplication.SystemConfig.Dtos;
using Surging.Hero.Common;
using Surging.Hero.Common.Extensions;

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

        /// <summary>
        /// 设置系统配置
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HttpPut]
        [ServiceRoute("")]
        [Service(Name = "设置系统配置", Date = "2021-01-07", Director = Developers.Liuhll)]
        Task<string> SetSystemConfig(SetSystemConfigInput input);

        /// <summary>
        /// 获取没有权限的操作按钮风格
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ServiceRoute("nonpermissionoperationstyles")]
        [Service(Name = "获取没有权限的操作按钮风格", Date = "2021-01-07", Director = Developers.Liuhll, AllowPermission = true)]
        Task<IEnumerable<EnumDto>> GetNonPermissionOperationStyles();
    }
}