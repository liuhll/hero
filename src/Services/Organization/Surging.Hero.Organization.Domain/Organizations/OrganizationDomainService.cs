using Dapper;
using Surging.Core.Dapper.Manager;
using Surging.Core.Dapper.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
