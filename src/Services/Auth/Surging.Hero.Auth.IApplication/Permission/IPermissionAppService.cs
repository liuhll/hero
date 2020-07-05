using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Core.Domain;
using Surging.Hero.Auth.IApplication.Permission.Dtos;
using Surging.Hero.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Surging.Hero.Auth.IApplication.Permission
{
    [ServiceBundle(HeroConstants.RouteTemplet)]
    public interface IPermissionAppService : IServiceKey
    {
        /// <summary>
        /// 新增菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost(true)]
        [ServiceRoute("menu/create")]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "新增菜单")]
        Task<CreateMenuOutput> CreateMenu(CreateMenuInput input);

        /// <summary>
        /// 编辑菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut(true)]
        [ServiceRoute("menu/update")]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "编辑菜单")]
        Task<UpdateMenuOutput> UpdateMenu(UpdateMenuInput input);

        /// <summary>
        /// 根据权限id获取菜单信息
        /// </summary>
        /// <param name="permissionId"></param>
        /// <returns></returns>
        [HttpGet(true)]
        [ServiceRoute("get/menu/{permissionId}")]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "根据权限id获取菜单信息", AllowPermission = true)]
        Task<GetMenuOutput> GetMenu(long permissionId);

        /// <summary>
        /// 新增操作
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost(true)]
        [ServiceRoute("operation/create")]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "新增操作")]
        Task<CreateOperationOutput> CreateOperation(CreateOperationInput input);

        /// <summary>
        /// 更新操作
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut(true)]
        [ServiceRoute("operation/update")]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "更新操作")]
        Task<UpdateOperationOutput> UpdateOperation(UpdateOperationInput input);

        /// <summary>
        /// 根据权限id获取操作
        /// </summary>
        /// <param name="permissionId"></param>
        /// <returns></returns>
        [HttpGet(true)]
        [ServiceRoute("get/operation/{permissionId}")]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "根据权限id获取操作", AllowPermission = true)]
        Task<GetOperationOutput> GetOperation(long permissionId);

        /// <summary>
        /// 获取权限树
        /// </summary>
        /// <returns></returns>
        [HttpGet(true)]
        [ServiceRoute("get/tree")]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "获取权限树", AllowPermission = true)]
        Task<IEnumerable<GetPermissionTreeOutput>> GetTree();

        /// <summary>
        /// 删除菜单/操作
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpDelete(true)]
        [ServiceRoute("delete")]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "删除菜单/操作")]
        Task<string> Delete(DeletePermissionInput input);

        /// <summary>
        /// 接口鉴权
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        [HttpPost(true)]
        [Service(DisableNetwork = true, Name = "接口鉴权",Director = Developers.Liuhll)]
        [ServiceRoute("check")]
        Task<bool> Check(string serviceId);
    }
}
