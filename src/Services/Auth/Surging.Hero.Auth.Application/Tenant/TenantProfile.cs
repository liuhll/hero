using AutoMapper;
using Surging.Hero.Auth.IApplication.Tenant.Dtos;

namespace Surging.Hero.Auth.Application.Tenant
{
    public class TenantProfile: Profile
    {
        public TenantProfile()
        {
            CreateMap<CreateTenantInput, Domain.Tenants.Tenant>();
        }
    }
}