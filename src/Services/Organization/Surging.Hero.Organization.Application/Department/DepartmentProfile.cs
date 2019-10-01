using AutoMapper;
using Surging.Hero.Organization.IApplication.Department.Dtos;

namespace Surging.Hero.Organization.Application.Department
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile() {
            CreateMap<CreateDepartmentInput, Domain.Department>();

            CreateMap<CreateDepartmentInput, Domain.Organization>().AfterMap((src, dest) => {
                dest.OrgType = Domain.Shared.Organizations.OrganizationType.Department;
            });
            CreateMap<UpdateDepartmentInput, Domain.Department>();
            CreateMap<UpdateDepartmentInput, Domain.Organization>();

            CreateMap<Domain.Department, GetDepartmentOutput>();
            CreateMap<Domain.Organization, GetDepartmentOutput>();
        }
    }
}
