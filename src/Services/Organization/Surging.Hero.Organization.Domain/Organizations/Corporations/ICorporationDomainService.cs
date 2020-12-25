using System.Threading.Tasks;
using Surging.Cloud.CPlatform.Ioc;
using Surging.Hero.Organization.IApplication.Corporation.Dtos;

namespace Surging.Hero.Organization.Domain.Organizations
{
    public interface ICorporationDomainService : ITransientDependency
    {
        Task<CreateCorporationOutput> CreateCorporation(CreateCorporationInput input);
        Task<UpdateCorporationOutput> UpdateCorporation(UpdateCorporationInput input);
        Task DeleteCorporation(long orgId);

        Task<GetCorporationOutput> GetCorporation(long orgId);
    }
}