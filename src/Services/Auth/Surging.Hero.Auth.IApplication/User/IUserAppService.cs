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
        [Service(Director = Developers.Liuhll,EnableAuthorization = true)]
        [HttpPost(true)]
        Task<string> Create(CreateUserInput input);

        [HttpPut(true)]
        Task<string> Update(UpdateUserInput input);
        
        [HttpDelete(true)]
        [ServiceRoute("delete/{id}")]
        Task<string> Delete(long id);

        [HttpPost(true)]
        Task<IPagedResult<GetUserNormOutput>> Query(QueryUserInput query);

        [HttpPut(true)]
        [ServiceRoute("update/status")]
        Task<string> UpdateStatus(UpdateUserStatusInput input);

        [HttpPut(true)]
        [ServiceRoute("reset/password")]
        Task<string> ResetPassword(ResetPasswordInput input);

        [HttpGet(true)]
        [ServiceRoute("get/org/users")]
        Task<IEnumerable<GetUserBasicOutput>> GetOrgUser(long orgId,bool includeSubOrg);

        //[ServiceRoute("{corporationId}")]
        //[HttpGet(true)]
        //Task<IEnumerable<GetUserBasicOutput>> GetCorporationUser(long corporationId);

        [ServiceRoute("get/{id}")]
        [HttpGet(true)]
        Task<GetUserNormOutput> Get(long id);


        [HttpPost(true)]
        [ServiceRoute("reset/user/org/{id}")]
        Task<bool> ResetUserOrgInfo(long id);

        [Service(DisableNetwork = true)]
        Task<int> GetPositionUserCount(long positionId);
    }
}
