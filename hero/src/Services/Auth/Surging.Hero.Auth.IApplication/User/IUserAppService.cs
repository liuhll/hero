using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Core.Domain.PagedAndSorted;
using Surging.Hero.Auth.IApplication.User.Dtos;
using Surging.Hero.Common.Dtos;
using System.Threading.Tasks;

namespace Surging.Hero.Auth.IApplication.User
{
    [ServiceBundle("v1/api/user")]
    public interface IUserAppService : IServiceKey
    {
        Task<string> Create(CreateUserInput input);

        Task<string> Update(UpdateUserInput input);
        
        Task<string> Delete(DeleteByIdInput input);

        Task<IPagedResult<GetUserOutput>> Query(QueryUserInput query);

        Task<string> UpdateStatus(UpdateUserStatusInput input);

        Task<string> ResetPassword(ResetPasswordInput input);
    }
}
