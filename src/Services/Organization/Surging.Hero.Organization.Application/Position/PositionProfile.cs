using AutoMapper;
using Surging.Hero.Organization.IApplication.Position.Dtos;

namespace Surging.Hero.Organization.Application.Position
{
    public class PositionProfile : Profile
    {
        public PositionProfile() {
            CreateMap<CreateOrUpdatePositionInput, Domain.Positions.Position>();
            CreateMap<Domain.Positions.Position, GetPositionOutput>();
        }
    }
}
