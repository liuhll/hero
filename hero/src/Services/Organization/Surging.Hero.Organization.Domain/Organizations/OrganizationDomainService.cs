using Surging.Core.AutoMapper;
using Surging.Core.Dapper.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Surging.Hero.Organization.Domain
{
    public class OrganizationDomainService : IOrganizationDomainService
    {
        private readonly IDapperRepository<Corporation, long> _corporationRepository;
        private readonly IDapperRepository<Department, long> _departmentRepository;

        public OrganizationDomainService(IDapperRepository<Corporation, long> corporationRepository,
            IDapperRepository<Department, long> departmentRepository)
        {
            _corporationRepository = corporationRepository;
            _departmentRepository = departmentRepository;
        }

        public async Task<IEnumerable<Organization>> GetOrganizations()
        {
            var organizations = new List<Organization>();
            var corporations = await _corporationRepository.GetAllAsync();
            organizations.AddRange(corporations.MapTo<IEnumerable<Organization>>());
            foreach (var corporation in corporations)
            {
                var departments = await _departmentRepository.GetAllAsync(p => p.CorporationId == corporation.Id);
                var departOrgs = departments.MapTo<IEnumerable<Organization>>();
                foreach (var deptOrg in departOrgs)
                {
                    deptOrg.ParentId = corporation.Id;
                }
                organizations.AddRange(departOrgs);
            }
            return organizations;

        }
    }
}
