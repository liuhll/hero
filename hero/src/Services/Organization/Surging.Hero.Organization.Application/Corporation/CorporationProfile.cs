using AutoMapper;
using Surging.Hero.Organization.IApplication.Corporation.Dtos;

namespace Surging.Hero.Organization.Application.Corporation
{
    public class CorporationProfile : Profile
    {
        public CorporationProfile() {
            CreateMap<CreateCorporationInput, Domain.Corporation>();
        }
    }
}
