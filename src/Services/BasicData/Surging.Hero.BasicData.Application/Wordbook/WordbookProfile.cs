using AutoMapper;
using Surging.Hero.BasicData.Domain.Wordbooks;
using Surging.Hero.BasicData.IApplication.Wordbook.Dtos;

namespace Surging.Hero.BasicData.Application.Wordbook
{
    public class WordbookProfile : Profile
    {
        public WordbookProfile()
        {
            CreateMap<CreateWordbookInput, Domain.Wordbooks.Wordbook>()
                .AfterMap((src, dest) => { dest.IsSysPreset = false; });

            CreateMap<UpdateWordbookInput, Domain.Wordbooks.Wordbook>();
            CreateMap<Domain.Wordbooks.Wordbook, GetWordbookOutput>();
            CreateMap<WordbookItem, GetWordbookItemOutput>()
                .ForMember(p => p.WordbookCode, opt => opt.Ignore());

            CreateMap<CreateWordbookItemInput, WordbookItem>();
            CreateMap<UpdateWordbookItemInput, WordbookItem>();
        }
    }
}