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

        public async Task<string> Update(UpdateTenantInput input)
        {
            var tenant = await _tenantRepository.SingleOrDefaultAsync(p => p.Id == input.Id);
            if (tenant == null)
            {
                throw new BusinessException($"不存在Id为{input.Id}的租户信息");
            }

            if (!input.Name.Equals(tenant.Name))
            {   
                var exsitTenant = await _tenantRepository.FirstOrDefaultAsync(p => p.Name == input.Name.Trim());
                if (exsitTenant != null)
                {
                    throw new BusinessException($"已经存在{input.Name}的租户");
                }
            }

            tenant = input.MapTo(tenant);
            await _tenantRepository.UpdateAsync(tenant);
            return "更新租户信息成功";
        }

        public async Task<string> Delete(long id)
        {
            var tenant = await _tenantRepository.SingleOrDefaultAsync(p => p.Id == id);
            if (tenant == null)
            {
                throw new BusinessException($"不存在Id为{id}的租户信息");
            }

            await _tenantRepository.DeleteAsync(p => p.Id == id);
            return "租户删除成功";

        }
    }
}