using AutoMapper;
using Surging.Hero.Organization.Domain.Shared.Organizations;
using Surging.Hero.Organization.IApplication.Department.Dtos;

namespace Surging.Hero.Organization.Application.Department
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<CreateDepartmentInput, Domain.Department>();

            CreateMap<CreateDepartmentInput, Domain.Organization>().AfterMap((src, dest) =>
            {
                dest.OrgType = OrganizationType.Department;
            }).ForMember(p => p.Id, opt => opt.Ignore());

            CreateMap<UpdateDepartmentInput, Domain.Department>();
            CreateMap<UpdateDepartmentInput, Domain.Organization>().ForMember(p => p.Id, opt => opt.Ignore());

            CreateMap<Domain.Department, GetDepartmentOutput>();
            CreateMap<Domain.Organization, GetDepartmentOutput>().ForMember(p => p.Id, opt => opt.Ignore());
        }
    }
}