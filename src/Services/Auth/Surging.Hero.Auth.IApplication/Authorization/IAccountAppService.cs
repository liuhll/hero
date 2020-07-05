using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Core.Domain;
using Surging.Core.Domain.Trees;
using Surging.Hero.Auth.IApplication.Authorization.Dtos;
using Surging.Hero.Common;
using Surging.Hero.Common.Runtime.Session;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Surging.Hero.Auth.IApplication.Authorization
{
    [ServiceBundle(HeroConstants.RouteTemplet)]
    public interface IAccountAppService : IServiceKey
    {
        /// <summary>
        /// 用户登录接口
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Service(Director = Developers.Liuhll, Date = "2019-07-14", Name = "用户登录接口", EnableAuthorization = false)]
        [HttpPost(true)]
        Task<IDictionary<string, object>> Login(LoginInput input);

        /// <summary>
        /// 获取登录用户信息
        /// </summary>
        /// <returns></returns>
        [Service(Director = Developers.Liuhll, Date = "2019-09-21", Name = "获取登录用户信息", EnableAuthorization = true,AllowPermission =true)]
        [HttpGet(true)]
        [ServiceRoute("get/userinfo")]
        Task<LoginUserInfo> GetLoginUser();

        /// <summary>
        /// 获取登录用户菜单权限列表
        /// </summary>
        /// <returns></returns>
        [HttpGet(true)]
        [Service(Director = Developers.Liuhll, Date = "2019-09-23", Name = "获取登录用户菜单权限列表", EnableAuthorization = true, AllowPermission = true)]
        [ServiceRoute("get/menu")]
        Task<IEnumerable<ITree<GetUserMenuTreeOutput>>> GetUserMenu();

        /// <summary>
        /// 通过菜单Id获取用户的操作权限
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        [HttpGet(true)]
        [Service(Director = Developers.Liuhll, Date = "2019-09-23", Name = "通过菜单Id获取用户的操作权限", EnableAuthorization = true, AllowPermission = true)]
        [ServiceRoute("get/operation/{menuId}")]
        Task<IEnumerable<GetUserOperationOutput>> GetUserOperation(long menuId);
    }
}
