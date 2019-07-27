using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Hero.BasicData.IApplication.Wordbook.Dtos;
using System.Threading.Tasks;

namespace Surging.Hero.BasicData.IApplication.Wordbook
{
    [ServiceBundle("v1/api/wordbook")]
    public interface IWordbookAppService : IServiceKey
    {
        Task<string> Create(CreateWordbookInput input);

        Task<string> Update(UpdateWordbookInput input);
    }
}
