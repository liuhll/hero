using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Core.Domain.Trees;
using Surging.Hero.Auth.IApplication.UserGroup.Dtos;
using Surging.Hero.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Surging.Hero.Auth.IApplication.UserGroup
{
    [ServiceBundle(HeroConstants.RouteTemplet)]
    public interface IUserGroupAppService : IServiceKey
    {
        /// <summary>
        /// 新增用户组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost(true)]
        [ServiceRoute("create")]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "新增用户组")]
        Task<string> Create(CreateUserGroupInput input);

        /// <summary>
        /// 更新用户组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut(true)]
        [ServiceRoute("update")]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "更新用户组")]
        Task<string> Update(UpdateUserGroupInput input);

        /// <summary>
        /// 获取用户组信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ServiceRoute("get/{id}")]
        [HttpGet(true)]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "获取用户组信息", AllowPermission = true)]
        Task<GetUserGroupOutput> Get(long id);

        /// <summary>
        /// 删除用户组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ServiceRoute("delete/{id}")]
        [HttpDelete(true)]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "删除用户组")]
        Task<string> Delete(long id);

        /// <summary>
        /// 获取用户组树形结构
        /// </summary>
        /// <returns></returns>
        [HttpGet(true)]
        [ServiceRoute("get/tree")]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "获取用户组树形结构", AllowPermission = true)]
        Task<IEnumerable<ITree<GetUserGroupTreeOutput>>> GetTree();
    }
}
