using Surging.Core.AutoMapper;
using Surging.Core.Dapper.Repositories;
using Surging.Hero.Organization.Domain.Shared.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
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
                    if (deptOrg.ParentId == 0) {
                        deptOrg.IsTopOrg = true;
                        deptOrg.ParentId = corporation.Id;
                    }
                }
                organizations.AddRange(departOrgs);
            }
            return organizations;

        }

        public async Task<IEnumerable<Organization>> GetOrganizations(long? id, OrganizationType organizationType)
        {
            var organizations = new List<Organization>();
            if (!id.HasValue  || id == 0)
            {
                return await GetOrganizations();
            }
            else
            {
                switch (organizationType) {
                    case OrganizationType.Corporation:
                        var corporation = await _corporationRepository.GetAsync(id.Value);
                        organizations.Add(corporation.MapTo<Organization>());
                        var departments = await _departmentRepository.GetAllAsync(p => p.CorporationId == corporation.Id);
                        var departOrgs = departments.MapTo<IEnumerable<Organization>>();
                        foreach (var deptOrg in departOrgs)
                        {
                            if (deptOrg.ParentId == 0)
                            {
                                deptOrg.IsTopOrg = true;
                                deptOrg.ParentId = corporation.Id;
                            }
                        }
                        organizations.AddRange(departOrgs);
                        break;
                    case OrganizationType.Department:
                        var department = await _departmentRepository.GetAsync(id.Value);
                        var departOrg = department.MapTo<Organization>();
                        organizations.Add(departOrg);
                        var departChildren = await GetDepartChildren(departOrg.Id);
                        organizations.AddRange(departChildren.MapTo<IEnumerable<Organization>>());
                        break;
                }
            }

            return organizations;
        }

        private async Task<IEnumerable<Department>> GetDepartChildren(long departId)
        {
            var children = new List<Department>();
            var departs = await _departmentRepository.GetAllAsync(p => p.ParentId == departId);
            if (departs.Any()) {
                foreach (var depart in departs) {
                    var thisDepartChildren = await GetDepartChildren(depart.Id);
                    children.AddRange(thisDepartChildren);
                }
                children.AddRange(departs);
            }
            return children;
        }
    }
}
