using AutoMapper;
using Surging.Hero.BasicData.IApplication.SystemConfig.Dtos;

namespace Surging.Hero.BasicData.Application.SystemConfig
{
    public class SystemConfigProfile : Profile
    {
        public SystemConfigProfile()
        {
            CreateMap<Domain.SystemConfigs.SystemConfig, GetSystemConfigOutput>();
        }
    }
}