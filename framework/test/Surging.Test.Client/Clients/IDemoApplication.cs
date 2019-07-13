using Surging.Core.CPlatform.Ioc;
using Surging.Test.Client.Clients.Dtos;
using System.Threading.Tasks;

namespace Surging.Test.Server.Demo
{
    public interface IDemoApplication : IServiceKey
    {
        Task<GetUserInfoOutput> GetUserInfo(string userId);
    }
}
