using System.Threading.Tasks;
using Surging.Cloud.AutoMapper;
using Surging.Cloud.CPlatform.Exceptions;
using Surging.Cloud.Dapper.Manager;
using Surging.Cloud.Dapper.Repositories;
using Surging.Hero.Auth.IApplication.Tenant.Dtos;

namespace Surging.Hero.Auth.Domain.Tenants
{
    public class TenantDomainService : ManagerBase, ITenantDomainService
    {
        private readonly IDapperRepository<Tenant, long> _tenantRepository;

        public TenantDomainService(IDapperRepository<Tenant, long> tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }

        public async Task<string> Create(CreateTenantInput input)
        {
            var exsitTenant = await _tenantRepository.FirstOrDefaultAsync(p => p.Name == input.Name.Trim());
            if (exsitTenant != null)
            {
                throw new BusinessException($"已经存在{input.Name}的租户");
            }

            await _tenantRepository.InsertAsync(input.MapTo<Tenant>());
            return "新增租户成功";
        }
    }
}