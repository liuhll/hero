using System.Collections.Generic;
using System.Threading.Tasks;
using Surging.Cloud.CPlatform.Utilities;
using Surging.Cloud.Dapper.Manager;
using Surging.Cloud.Dapper.Repositories;

namespace Surging.Hero.Organization.Domain
{
    public class OrganizationDomainService : ManagerBase, IOrganizationDomainService
    {
        private readonly IDapperRepository<Corporation, long> _corporationRepository;
        private readonly IDapperRepository<Department, long> _departmentRepository;
        private readonly IDapperRepository<Organization, long> _organizationRepository;

        public OrganizationDomainService(IDapperRepository<Corporation, long> corporationRepository,
            IDapperRepository<Department, long> departmentRepository,
            IDapperRepository<Organization, long> organizationRepository)
        {
            _corporationRepository = corporationRepository;
            _departmentRepository = departmentRepository;
            _organizationRepository = organizationRepository;
        }

        public async Task<IEnumerable<Organization>> GetOrganizations()
        {
            var organizations = await _organizationRepository.GetAllAsync();
            return organizations;
        }

        public async Task<IEnumerable<Organization>> GetSubOrgs(long orgId)
        {
            var orgInfo = await _organizationRepository.GetAsync(orgId);
            var organizations = await _organizationRepository.GetAllAsync(p => p.Code.Contains(orgInfo.Code));

            return organizations;
        }

        public async Task<IEnumerable<Organization>> GetParentsOrganizations(long orgId)
        {
            var orgs = new List<Organization>();
            var orgInfo = await _organizationRepository.GetAsync(orgId);
            orgs.Add(orgInfo);
            return await GetParentsOrganizations(orgs, orgInfo);


        }

        private async Task<IEnumerable<Organization>> GetParentsOrganizations(IList<Organization> orgs,
            Organization orgInfo)
        {
            while (orgInfo.ParentId != 0)
            {
                var parentOrgInfo = await _organizationRepository.GetAsync(orgInfo.ParentId);
                orgs.AddIfNotContains(parentOrgInfo);
                return await GetParentsOrganizations(orgs, parentOrgInfo);
            }
            return orgs;
        }
    }
}