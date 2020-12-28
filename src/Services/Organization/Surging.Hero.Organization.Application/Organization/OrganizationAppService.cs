using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Surging.Cloud.AutoMapper;
using Surging.Cloud.CPlatform.Runtime.Session;
using Surging.Cloud.Dapper.Repositories;
using Surging.Cloud.Domain.PagedAndSorted;
using Surging.Cloud.Domain.PagedAndSorted.Extensions;
using Surging.Cloud.Domain.Trees;
using Surging.Cloud.ProxyGenerator;
using Surging.Hero.Organization.Domain;
using Surging.Hero.Organization.IApplication.Organization;
using Surging.Hero.Organization.IApplication.Organization.Dtos;

namespace Surging.Hero.Organization.Application.Organization
{
    public class OrganizationAppService : ProxyServiceBase, IOrganizationAppService
    {
        private readonly IOrganizationDomainService _organizationDomainService;
        private readonly IDapperRepository<Domain.Organization, long> _organizationRepository;
        private readonly ISurgingSession _session;

        public OrganizationAppService(IOrganizationDomainService organizationDomainService,
            IDapperRepository<Domain.Organization, long> organizationRepository)
        {
            _organizationDomainService = organizationDomainService;
            _organizationRepository = organizationRepository;
            _session = NullSurgingSession.Instance;
        }

        public async Task<IEnumerable<long>> GetSubOrgIds(long orgId)
        {
            return (await _organizationDomainService.GetSubOrgs(orgId)).Select(p => p.Id);
        }

        public async Task<IEnumerable<GetOrganizationTreeOutput>> GetParentsOrgs(long orgId)
        {
            var parentsOrgs = await _organizationDomainService.GetParentsOrganizations(orgId);
            return parentsOrgs.MapTo<IEnumerable<GetOrganizationTreeOutput>>();
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

        public async Task<IEnumerable<ITree<GetOrganizationTreeOutput>>> GetOwnTree()
        {
            if (_session.IsAllOrg)
            {
                return await GetTree();
            }
            var permissionOrganizations = new List<GetOrganizationTreeOutput>();
            var organizationAppServiceProxy = GetService<IOrganizationAppService>();
            foreach (var orgId in _session.DataPermissionOrgIds)
            {
                var orgParents = await organizationAppServiceProxy.GetParentsOrgs(orgId);
                foreach (var org in orgParents)
                {
                    if (permissionOrganizations.Any(p => p.Id == org.Id))
                    {
                        continue;
                    }
                    permissionOrganizations.Add(org);
                }
                
            }
            return permissionOrganizations.BuildTree();
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