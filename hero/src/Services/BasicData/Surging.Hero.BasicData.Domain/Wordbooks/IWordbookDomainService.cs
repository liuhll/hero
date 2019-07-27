using System.Threading.Tasks;
using Surging.Core.CPlatform.Ioc;
using Surging.Hero.BasicData.IApplication.Wordbook.Dtos;

namespace Surging.Hero.BasicData.Domain.Wordbooks
{
    public interface IWordbookDomainService : ITransientDependency
    {
        Task CreateWordbook(CreateWordbookInput input);
    }
}
