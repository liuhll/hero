using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Surging.Core.AutoMapper;
using Surging.Core.CPlatform.Utilities;
using Surging.Core.Domain;
using Surging.Core.ProxyGenerator;
using Surging.Hero.Organization.Domain;
using Surging.Hero.Organization.Domain.Shared.Organizations;
using Surging.Hero.Organization.IApplication.Organization;
using Surging.Hero.Organization.IApplication.Organization.Dtos;

namespace Surging.Hero.Organization.Application.Organization
{
    public class OrganizationAppService : ProxyServiceBase, IOrganizationAppService
    {
        private readonly IOrganizationDomainService _organizationDomainService;

        public OrganizationAppService(IOrganizationDomainService organizationDomainService) {
            _organizationDomainService = organizationDomainService;
        }

        public async Task<IEnumerable<GetOrganizationTreeOutput>> GetTree()
        {
            var organizations =  await _organizationDomainService.GetOrganizations();
            var organizationTree = BuildOrganizationTree(organizations);
            return organizationTree;
        }

        public async Task<IPagedResult<QueryOrganizationOutput>> Query(QueryOrganizationInput query)
        {
            var organizations = await _organizationDomainService.GetOrganizations(query.Id, query.OrganizationType);
            organizations = organizations.WhereIf(!query.SearchKey.IsNullOrEmpty(), p => p.Code.Contains(query.SearchKey) || p.Name.Contains(query.SearchKey));
            return organizations.MapTo<IEnumerable<QueryOrganizationOutput>>().PageBy(query);
        }


        private IEnumerable<GetOrganizationTreeOutput> BuildOrganizationTree(IEnumerable<Domain.Organization> organizations)
        {
            var topOrgs = organizations.Where(p => p.ParentId == 0 && p.OrganizationType == OrganizationType.Corporation);
            var topOrgoutputs = topOrgs.MapTo<IEnumerable<GetOrganizationTreeOutput>>();
            foreach (var topOrgoutput in topOrgoutputs) {
                topOrgoutput.Children = BuildOrganizationChildren(topOrgoutput, organizations);
            }
            return topOrgoutputs;
           
        }

        private IEnumerable<GetOrganizationTreeOutput> BuildOrganizationChildren(GetOrganizationTreeOutput topOrgoutput, IEnumerable<Domain.Organization> organizations)
        {
            var children = new List<GetOrganizationTreeOutput>();
            if (topOrgoutput.OrganizationType == OrganizationType.Corporation)
            {
                var corporationChildren = organizations.Where(p => p.ParentId == topOrgoutput.Id && p.OrganizationType == OrganizationType.Corporation);
                if (corporationChildren.Any())
                {
                    var corporationChildrenOutputs = corporationChildren.MapTo<IEnumerable<GetOrganizationTreeOutput>>();
                    children.AddRange(corporationChildrenOutputs);
                    foreach (var item in corporationChildrenOutputs)
                    {
                        item.Children = BuildOrganizationChildren(item, organizations);
                    }
                }

                var departmentChildren = organizations.Where(p => p.ParentId == topOrgoutput.Id && p.OrganizationType == OrganizationType.Department && p.IsTopOrg);
                if (departmentChildren.Any())
                {
                    var departmentChildrenOutputs = departmentChildren.MapTo<IEnumerable<GetOrganizationTreeOutput>>();
                    children.AddRange(departmentChildrenOutputs);
                    foreach (var item in departmentChildrenOutputs)
                    {
                        item.Children = BuildOrganizationChildren(item, organizations);
                    }
                }

            }
            else
            {
                
                var departmentChildren = organizations.Where(p => p.ParentId == topOrgoutput.Id && p.OrganizationType == OrganizationType.Department && !p.IsTopOrg);

                if (departmentChildren.Any())
                {
                    var departmentChildrenOutputs = departmentChildren.MapTo<IEnumerable<GetOrganizationTreeOutput>>();
                    children.AddRange(departmentChildrenOutputs);
                    foreach (var item in departmentChildrenOutputs)
                    {
                        item.Children = BuildOrganizationChildren(item, organizations);
                    }
                }
            }
            return children;
        }

    }
}
