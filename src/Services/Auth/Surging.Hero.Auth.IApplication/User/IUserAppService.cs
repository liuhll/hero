using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Core.Domain.PagedAndSorted;
using Surging.Hero.Auth.IApplication.User.Dtos;
using Surging.Hero.Common;
using Surging.Hero.Common.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Surging.Hero.Auth.IApplication.User
{
    [ServiceBundle(HeroConstants.RouteTemplet)]
    public interface IUserAppService : IServiceKey
    {
        [Service(Director = Developers.Liuhll)]
        Task<string> Create(CreateUserInput input);

        Task<string> Update(UpdateUserInput input);
        
        Task<string> Delete(DeleteByIdInput input);

        Task<IPagedResult<GetUserOutput>> Query(QueryUserInput query);

        Task<string> UpdateStatus(UpdateUserStatusInput input);

        Task<string> ResetPassword(ResetPasswordInput input);

        [ServiceRoute("{deptId}")]
        Task<IEnumerable<GetUserOutput>> GetDepartmentUser(long deptId);

        [ServiceRoute("{corporationId}")]
        Task<IEnumerable<GetUserOutput>> GetCorporationUser(long corporationId);

        [ServiceRoute("{id}")]
        Task<GetUserOutput> Get(long id);
    }
}
