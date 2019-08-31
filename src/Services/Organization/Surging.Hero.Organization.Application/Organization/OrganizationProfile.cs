using AutoMapper;
using Surging.Hero.Organization.Domain.Shared.Organizations;
using Surging.Hero.Organization.IApplication.Organization.Dtos;
using System;

namespace Surging.Hero.Organization.Application.Organization
{
    public class OrganizationProfile : Profile
    {
        public OrganizationProfile()
        {
            CreateMap<Domain.Corporation, Domain.Organization>().AfterMap((src,dest)=> { dest.OrganizationType = (OrganizationType)(int)src.Mold; });
            CreateMap<Domain.Department, Domain.Organization>().ForMember(p=>p.ParentId,opt=>opt.Ignore()).AfterMap((src, dest) => { dest.OrganizationType = Domain.Shared.Organizations.OrganizationType.Department; });
            CreateMap<Domain.Organization, GetOrganizationTreeOutput>().ForMember(p => p.Children, opt => opt.Ignore());
            CreateMap<Domain.Organization, QueryOrganizationOutput>();
        }
    }
}
