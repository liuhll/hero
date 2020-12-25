using System.Collections.Generic;
using System.Threading.Tasks;
using Surging.Cloud.CPlatform.Ioc;
using Surging.Cloud.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Hero.Auth.IApplication.Permission.Dtos;
using Surging.Hero.Common;
using Surging.Hero.Common.Extensions;

namespace Surging.Hero.Auth.IApplication.Permission
{
    [ServiceBundle(HeroConstants.RouteTemplet)]
    public interface IPermissionAppService : IServiceKey
    {
        /// <summary>
        ///     新增菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [ServiceRoute("menu")]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "新增菜单")]
        Task<CreateMenuOutput> CreateMenu(CreateMenuInput input);

        /// <summary>
        ///     编辑菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        [ServiceRoute("menu")]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "编辑菜单")]
        Task<UpdateMenuOutput> UpdateMenu(UpdateMenuInput input);

        /// <summary>
        ///     根据权限id获取菜单信息
        /// </summary>
        /// <param name="permissionId"></param>
        /// <returns></returns>
        [HttpGet]
        [ServiceRoute("menu/{permissionId}")]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "根据权限id获取菜单信息", AllowPermission = true)]
        Task<GetMenuOutput> GetMenu(long permissionId);

        /// <summary>
        ///     新增操作
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [ServiceRoute("operation")]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "新增操作")]
        Task<CreateOperationOutput> CreateOperation(CreateOperationInput input);

        /// <summary>
        ///     更新操作
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        [ServiceRoute("operation")]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "更新操作")]
        Task<UpdateOperationOutput> UpdateOperation(UpdateOperationInput input);

        /// <summary>
        ///     根据权限id获取操作
        /// </summary>
        /// <param name="permissionId"></param>
        /// <returns></returns>
        [HttpGet]
        [ServiceRoute("operation/{permissionId}")]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "根据权限id获取操作", AllowPermission = true)]
        Task<GetOperationOutput> GetOperation(long permissionId);

        /// <summary>
        ///     获取权限树
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ServiceRoute("tree")]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "获取权限树", AllowPermission = true)]
        Task<IEnumerable<GetPermissionTreeOutput>> GetTree();

        /// <summary>
        ///     删除菜单/操作
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpDelete]
        [ServiceRoute("")]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "删除菜单/操作")]
        Task<string> Delete(DeletePermissionInput input);

        /// <summary>
        ///     接口鉴权
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        [HttpPost]
        [Service(DisableNetwork = true, Name = "接口鉴权", Director = Developers.Liuhll)]
        [ServiceRoute("check")]
        Task<IDictionary<string,object>> Check(string serviceId);

        /// <summary>
        /// 获取数据权限类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Service(Date = "2020-12-31", Name = "获取数据权限类型", Director = Developers.Liuhll, AllowPermission = true)]
        [ServiceRoute("datapermissiontypes")]
        Task<IEnumerable<EnumDto>> GetDataPermissionTypes();
    }
}