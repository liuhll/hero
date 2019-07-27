using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Test.Server.Demo.Dtos;
using System.Threading.Tasks;

namespace Surging.Test.Server.Demo
{
    [ServiceBundle("v1/api/{Application}/{Method}")]
    public interface IDemoApplication : IServiceKey
    {
        Task<GetUserInfoOutput> GetUserInfo(string userId);

        Task<string> TestError();

        [ServiceRoute("{workbooId}")]
        Task<string> TestServiceRoute(long workbooId);
    }
}
