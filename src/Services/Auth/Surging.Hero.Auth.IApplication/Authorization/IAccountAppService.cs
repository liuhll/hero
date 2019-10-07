using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Core.Domain;
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
        [Service(Director = Developers.Liuhll, Date = "2019-07-14", Name = "用户登录接口", EnableAuthorization = false)]
        [HttpPost(true)]
        Task<IDictionary<string, object>> Login(LoginInput input);

        [Service(Director = Developers.Liuhll, Date = "2019-09-21", Name = "获取登录用户信息",EnableAuthorization = true,AllowPermission =true)]
        [HttpGet(true)]
        Task<LoginUserInfo> GetLoginUser();

        [HttpGet(true)]
        [Service(Director = Developers.Liuhll, Date = "2019-09-23", Name = "获取用户菜单列表", EnableAuthorization = true, AllowPermission = true)]
        Task<IEnumerable<ITree<GetUserMenuTreeOutput>>> GetUserMenu();



        [HttpGet(true)]
        [Service(Director = Developers.Liuhll, Date = "2019-09-23", Name = "通过菜单Id获取用户操作权限", EnableAuthorization = true, AllowPermission = true)]
        Task<IEnumerable<GetUserOperationOutput>> GetUserOperation(long menuId);
    }
}
