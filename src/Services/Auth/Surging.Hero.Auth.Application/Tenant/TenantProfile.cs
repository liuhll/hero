using AutoMapper;
using Surging.Hero.Auth.IApplication.Tenant.Dtos;
using Surging.Hero.Common;

namespace Surging.Hero.Auth.Application.Tenant
{
    public class TenantProfile: Profile
    {
        public TenantProfile()
        {
            CreateMap<CreateTenantInput, Domain.Tenants.Tenant>().AfterMap((src, dest) =>
            {
                dest.Status = Status.Valid;
            });
            CreateMap<UpdateTenantInput, Domain.Tenants.Tenant>();
        }
    }
}