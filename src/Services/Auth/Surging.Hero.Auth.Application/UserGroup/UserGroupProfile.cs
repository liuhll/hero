using AutoMapper;
using Surging.Hero.Auth.Domain.UserGroups;
using Surging.Hero.Auth.IApplication.Role.Dtos;
using Surging.Hero.Auth.IApplication.UserGroup.Dtos;
using Surging.Hero.Common;

namespace Surging.Hero.Auth.Application.UserGroup
{
    public class UserGroupProfile : Profile
    {
        public UserGroupProfile()
        {
            CreateMap<CreateUserGroupInput, Domain.UserGroups.UserGroup>().AfterMap((src, dest) =>
            {
                dest.Status = Status.Valid;
            });
            CreateMap<UpdateUserGroupInput, Domain.UserGroups.UserGroup>();
            CreateMap<Domain.UserGroups.UserGroup, GetUserGroupOutput>();
            CreateMap<Domain.UserGroups.UserGroup, GetUserEditGroupOutput>();
            CreateMap<Domain.UserGroups.UserGroup, GetUserGroupTreeOutput>();
            CreateMap<Domain.Roles.Role, GetDisplayRoleOutput>();
            CreateMap<Domain.UserGroups.UserGroup, GetDisplayUserGroupOutput>();
            CreateMap<UserGroupPermissionModel, GetDisplayPermissionOutput>();
        }
    }
}