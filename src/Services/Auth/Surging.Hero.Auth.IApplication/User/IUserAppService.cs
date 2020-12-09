using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Core.Domain.PagedAndSorted;
using Surging.Hero.Auth.IApplication.User.Dtos;
using Surging.Hero.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Surging.Hero.Auth.IApplication.User
{
    [ServiceBundle(HeroConstants.RouteTemplet)]
    public interface IUserAppService : IServiceKey
    {
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "新增用户")]
        [ServiceRoute("")]
        Task<string> Create(CreateUserInput input);

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "更新用户")]
        [ServiceRoute("")]
        Task<string> Update(UpdateUserInput input);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [ServiceRoute("{id}")]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "删除用户")]
        Task<string> Delete(long id);

        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "查询用户", AllowPermission = true)]
        [ServiceRoute("search")]
        Task<IPagedResult<GetUserNormOutput>> Query(QueryUserInput query);

        /// <summary>
        /// 激活/冻结用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        [ServiceRoute("status")]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "激活/冻结用户")]
        Task<string> UpdateStatus(UpdateUserStatusInput input);

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        [ServiceRoute("password")]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "重置密码")]
        Task<string> ResetPassword(ResetPasswordInput input);

        /// <summary>
        /// 根据组织id获取部门的用户
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="includeSubOrg"></param>
        /// <returns></returns>
        [HttpGet]
        [ServiceRoute("org")]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "根据组织id获取部门的用户", DisableNetwork = true)]
        Task<IEnumerable<GetUserBasicOutput>> GetOrgUser(long orgId,bool includeSubOrg);

        //[ServiceRoute("{corporationId}")]
        //[HttpGet]
        //Task<IEnumerable<GetUserBasicOutput>> GetCorporationUser(long corporationId);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ServiceRoute("{id}")]
        [HttpGet]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "获取用户信息", AllowPermission = true)]
        Task<GetUserNormOutput> Get(long id);

        /// <summary>
        /// 修改某个用户的部门信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ServiceRoute("user/org/{id}")]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "修改某个用户的部门信息", DisableNetwork = true)]
        Task<bool> ResetUserOrgInfo(long id);

        /// <summary>
        /// 获取某个职位的用户数
        /// </summary>
        /// <param name="positionId"></param>
        /// <returns></returns>
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "获取某个职位的用户数")]
        [HttpGet]
        [ServiceRoute("position/count/{positionId}")]
        Task<int> GetPositionUserCount(long positionId);
    }
}
