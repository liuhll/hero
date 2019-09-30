using System.Threading.Tasks;
using Surging.Core.CPlatform.Ioc;
using Surging.Hero.Organization.IApplication.Corporation.Dtos;

namespace Surging.Hero.Organization.Domain.Organizations
{
    public interface ICorporationDomainService : ITransientDependency
    {
        Task CreateCorporation(CreateCorporationInput input);
        Task UpdateCorporation(UpdateCorporationInput input);
        Task DeleteCorporation(long orgId);

        Task<GetCorporationOutput> GetCorporation(long orgId);
    }
}
