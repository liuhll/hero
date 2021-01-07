using System.Threading.Tasks;
using Surging.Cloud.CPlatform.Ioc;
using Surging.Hero.BasicData.IApplication.SystemConfig.Dtos;

namespace Surging.Hero.BasicData.Domain.SystemConfigs
{
    public interface ISystemConfigDomainService : ITransientDependency
    {
        Task<GetSystemConfigOutput> GetSystemConfig();
    }
}