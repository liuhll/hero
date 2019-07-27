using AutoMapper;
using Surging.Hero.Auth.IApplication.Action.Dtos;

namespace Surging.Hero.Auth.Application.Action
{
    public class ActionProfile : Profile
    {
        public ActionProfile()
        {
            CreateMap<InitActionActionInput, Domain.Permissions.Actions.Action>()
                .AfterMap((src,dest)=> {
                    dest.Status = Common.Enums.Status.Valid;
                });
        }
    }
}
