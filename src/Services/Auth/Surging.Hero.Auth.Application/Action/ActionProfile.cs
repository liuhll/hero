using AutoMapper;
using Surging.Hero.Auth.IApplication.Action.Dtos;
using Surging.Hero.Common;

namespace Surging.Hero.Auth.Application.Action
{
    public class ActionProfile : Profile
    {
        public ActionProfile()
        {
            CreateMap<InitActionActionInput, Domain.Permissions.Actions.Action>()
                .AfterMap((src,dest)=> {
                    dest.Status = Status.Valid;
                });

            CreateMap<Domain.Permissions.Actions.Action, GetActionOutput>();
        }
    }
}
