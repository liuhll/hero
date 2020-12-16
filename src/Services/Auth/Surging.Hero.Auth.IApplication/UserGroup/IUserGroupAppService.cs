using System.Threading.Tasks;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Core.Domain.PagedAndSorted;
using Surging.Hero.Auth.IApplication.User.Dtos;
using Surging.Hero.Auth.IApplication.UserGroup.Dtos;
using Surging.Hero.Common;

namespace Surging.Hero.Auth.IApplication.UserGroup
{
    [ServiceBundle(HeroConstants.RouteTemplet)]
    public interface IUserGroupAppService : IServiceKey
    {
        /// <summary>
        ///     新增用户组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [ServiceRoute("")]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "新增用户组")]
        Task<string> Create(CreateUserGroupInput input);

        /// <summary>
        ///     更新用户组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        [ServiceRoute("")]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "更新用户组")]
        Task<string> Update(UpdateUserGroupInput input);


        /// <summary>
        ///     更新用户组状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        [ServiceRoute("status")]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "冻结/激活用户组状态")]
        Task<string> UpdateStatus(UpdateUserGroupStatusInput input);

        /// <summary>
        ///     获取用户组信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ServiceRoute("{id}")]
        [HttpGet]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "获取用户组信息", AllowPermission = true)]
        Task<GetUserEditGroupOutput> Get(long id);

        /// <summary>
        ///     删除用户组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ServiceRoute("{id}")]
        [HttpDelete]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "删除用户组")]
        Task<string> Delete(long id);

        /// <summary>
        ///     检索用户组
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ServiceRoute("search")]
        [Service(Director = Developers.Liuhll, Date = "2020-12-08", Name = "查询用户组", AllowPermission = true)]
        Task<IPagedResult<GetUserGroupOutput>> Search(QueryUserGroupInput query);

        /// <summary>
        ///     给用户组分配用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [ServiceRoute("users")]
        [Service(Director = Developers.Liuhll, Date = "2020-12-08", Name = "添加用户组用户")]
        Task<string> AllocationUsers(AllocationUserIdsInput input);

        /// <summary>
        ///     获取该用户组所辖用户
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        [ServiceRoute("users/search")]
        [Service(Director = Developers.Liuhll, Date = "2020-12-08", Name = "查询用户组用户")]
        Task<IPagedResult<GetUserNormOutput>> SearchUserGroupUser(QueryUserGroupUserInput query);

        /// <summary>
        ///     删除用户组用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpDelete]
        [ServiceRoute("users")]
        [Service(Director = Developers.Liuhll, Date = "2020-12-08", Name = "删除用户组用户")]
        Task<string> DeleteUserGroupUser(DeleteUserGroupUserInput input);
    }
}