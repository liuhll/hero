using AutoMapper;
using Surging.Hero.Organization.IApplication.Organization.Dtos;

namespace Surging.Hero.Organization.Application.Organization
{
    public class OrganizationProfile : Profile
    {
        public OrganizationProfile()
        {
            CreateMap<Domain.Organization, GetOrganizationTreeOutput>().ForMember(p => p.Children, opt => opt.Ignore());
            CreateMap<Domain.Organization, QueryOrganizationOutput>().ForMember(p => p.Id, opt => opt.Ignore());
        }
    }
}
