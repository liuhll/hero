using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Core.Domain;
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
        [ServiceRoute("{id}")]
        Task<string> Delete(long id);

        [HttpPost(true)]
        Task<IPagedResult<GetUserNormOutput>> Query(QueryUserInput query);

        [HttpPut(true)]
        Task<string> UpdateStatus(UpdateUserStatusInput input);

        [HttpPut(true)]
        Task<string> ResetPassword(ResetPasswordInput input);

        [ServiceRoute("{deptId}")]
        [HttpGet(true)]
        Task<IEnumerable<GetUserBasicOutput>> GetDepartmentUser(long deptId);

        [ServiceRoute("{corporationId}")]
        [HttpGet(true)]
        Task<IEnumerable<GetUserBasicOutput>> GetCorporationUser(long corporationId);

        [ServiceRoute("{id}")]
        [HttpGet(true)]
        Task<GetUserNormOutput> Get(long id);

        [HttpPost(true)]
        Task<IEnumerable<GetUserRoleOutput>> QueryUserRoles(QueryUserRoleInput query);

        [HttpPost(true)]
        Task<bool> ResetUserOrgInfo(long id);
    }
}
