using System.Collections.Generic;
using System.Threading.Tasks;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Core.Domain.PagedAndSorted;
using Surging.Hero.Auth.IApplication.Role.Dtos;
using Surging.Hero.Common;

namespace Surging.Hero.Auth.IApplication.Role
{
    [ServiceBundle(HeroConstants.RouteTemplet)]
    public interface IRoleAppService : IServiceKey
    {
        /// <summary>
        ///     新增角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "新增角色")]
        [ServiceRoute("")]
        Task<string> Create(CreateRoleInput input);

        /// <summary>
        ///     更新角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "更新角色")]
        [ServiceRoute("")]
        Task<string> Update(UpdateRoleInput input);

        /// <summary>
        ///     激活/冻结角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "激活/冻结角色")]
        [ServiceRoute("status")]
        Task<string> Status(UpdateRoleStatusInput input);

        /// <summary>
        ///     获取角色信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ServiceRoute("{id}")]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "获取角色信息", AllowPermission = true)]
        Task<GetRoleOutput> Get(long id);

        /// <summary>
        ///     查询角色
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "查询角色")]
        [ServiceRoute("search")]
        Task<IPagedResult<GetRoleOutput>> Search(QueryRoleInput query);

        /// <summary>
        ///     获取角色列表
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        [HttpGet]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "获取角色列表", AllowPermission = true)]
        [ServiceRoute("list")]
        Task<IEnumerable<GetRoleOutput>> List(string searchKey);

        //[HttpGet]
        //[ServiceRoute("get/permissions/{id}")]
        //Task<IEnumerable<GetRolePermissionTreeOutput>> GetRolePermissions(long id);

        //[HttpPut]
        //[ServiceRoute("set/permissions")]
        //Task<string> SetPermissions(SetRolePermissionInput input);

        [HttpDelete]
        [ServiceRoute("{id}")]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "删除角色")]
        Task<string> Delete(long id);
    }
}