using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Surging.Core.AutoMapper;
using Surging.Core.Dapper.Repositories;
using Surging.Core.Domain.PagedAndSorted;
using Surging.Core.Domain.PagedAndSorted.Extensions;
using Surging.Core.Domain.Trees;
using Surging.Core.ProxyGenerator;
using Surging.Hero.Organization.Domain;
using Surging.Hero.Organization.IApplication.Organization;
using Surging.Hero.Organization.IApplication.Organization.Dtos;

namespace Surging.Hero.Organization.Application.Organization
{
    public class OrganizationAppService : ProxyServiceBase, IOrganizationAppService
    {
        private readonly IOrganizationDomainService _organizationDomainService;
        private readonly IDapperRepository<Domain.Organization, long> _organizationRepository;

        public OrganizationAppService(IOrganizationDomainService organizationDomainService,
            IDapperRepository<Domain.Organization, long> organizationRepository)
        {
            _organizationDomainService = organizationDomainService;
            _organizationRepository = organizationRepository;
        }

        public async Task<IEnumerable<long>> GetSubOrgIds(long orgId)
        {
            return (await _organizationDomainService.GetSubOrgs(orgId)).Select(p => p.Id);
        }

        public async Task<GetOrganizationOutput> GetOrg(long orgId)
        {
            var organization = await _organizationRepository.SingleOrDefaultAsync(p =>
                p.Id == orgId);
            if (organization == null)
            {
                return null;
            }
            //  organizations = organizations.WhereIf(!query.Code.IsNullOrEmpty(), p => p.Code.Contains(query.SearchKey)).WhereIf(!query.SearchKey.IsNullOrEmpty(), p=> p.Name.Contains(query.SearchKey));
            return organization.MapTo<GetOrganizationOutput>();
        }

        public async Task<IEnumerable<ITree<GetOrganizationTreeOutput>>> GetTree()
        {
            var organizations = await _organizationRepository.GetAllAsync();
            var organizationOutputs = organizations.MapTo<IEnumerable<GetOrganizationTreeOutput>>();
            return organizationOutputs.BuildTree();
        }

        public async Task<IPagedResult<GetOrganizationOutput>> Search(QueryOrganizationInput query)
        {
            var organizations = await _organizationRepository.GetAllAsync(p =>
                p.Code.Contains(query.Code) && p.Name.Contains(query.SearchKey));
            //  organizations = organizations.WhereIf(!query.Code.IsNullOrEmpty(), p => p.Code.Contains(query.SearchKey)).WhereIf(!query.SearchKey.IsNullOrEmpty(), p=> p.Name.Contains(query.SearchKey));
            return organizations.MapTo<IEnumerable<GetOrganizationOutput>>().PageBy(query);
        }
    }
}