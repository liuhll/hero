using AutoMapper;
using Surging.Hero.Organization.IApplication.Organization.Dtos;

namespace Surging.Hero.Organization.Application.Organization
{
    public class OrganizationProfile : Profile
    {
        public OrganizationProfile()
        {
            CreateMap<Domain.Corporation, Domain.Organization>().AfterMap((src,dest)=> { dest.OrganizationType = Domain.Shared.Organizations.OrganizationType.Corporation; });
            CreateMap<Domain.Department, Domain.Organization>().ForMember(p=>p.ParentId,opt=>opt.Ignore()).AfterMap((src, dest) => { dest.OrganizationType = Domain.Shared.Organizations.OrganizationType.Department; });
            CreateMap<Domain.Organization, GetOrganizationTreeOutput>().ForMember(p => p.Children, opt => opt.Ignore());
        }
    }
}
