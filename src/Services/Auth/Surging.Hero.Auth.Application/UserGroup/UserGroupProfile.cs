using AutoMapper;
using Surging.Hero.Auth.IApplication.UserGroup.Dtos;

namespace Surging.Hero.Auth.Application.UserGroup
{
    public class UserGroupProfile : Profile
    {
        public UserGroupProfile() {
            CreateMap<CreateUserGroupInput, Domain.UserGroups.UserGroup>();
            CreateMap<UpdateUserGroupInput, Domain.UserGroups.UserGroup>();
            CreateMap<Domain.UserGroups.UserGroup, GetUserGroupOutput>();
            CreateMap<Domain.UserGroups.UserGroup,GetUserGroupTreeOutput>();
        }
    }
}
