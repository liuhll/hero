using AutoMapper;
using Surging.Hero.Organization.IApplication.Corporation.Dtos;

namespace Surging.Hero.Organization.Application.Corporation
{
    public class CorporationProfile : Profile
    {
        public CorporationProfile() {
            CreateMap<CreateCorporationInput, Domain.Corporation>();
            CreateMap<CreateCorporationInput, Domain.Organization>().AfterMap((src, dest) => {
                dest.OrgType = Domain.Shared.Organizations.OrganizationType.Corporation;
            }).ForMember(p => p.Id, opt => opt.Ignore());
            CreateMap<UpdateCorporationInput, Domain.Corporation>();
            CreateMap<UpdateCorporationInput, Domain.Organization>().ForMember(p => p.Id, opt => opt.Ignore());

            // Todo
            CreateMap<Domain.Corporation, GetCorporationOutput>();
            CreateMap<Domain.Organization, GetCorporationOutput>().ForMember(p => p.Id, opt => opt.Ignore());
        }
    }
}
