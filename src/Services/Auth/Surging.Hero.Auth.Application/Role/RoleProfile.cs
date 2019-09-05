using AutoMapper;
using Surging.Hero.Auth.IApplication.Role.Dtos;

namespace Surging.Hero.Auth.Application.Role
{
    public class RoleProfile : Profile
    {
        public RoleProfile() {
            CreateMap<CreateRoleInput, Domain.Roles.Role>().AfterMap((src,dest)=> {
                dest.Status = Common.Status.Valid;
            });
            CreateMap<UpdateRoleInput, Domain.Roles.Role>();
            CreateMap<Domain.Roles.Role, GetRoleOutput>();
        }
    }
}
