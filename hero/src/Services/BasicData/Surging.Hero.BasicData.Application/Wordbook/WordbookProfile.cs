using AutoMapper;
using Surging.Hero.BasicData.Domain.Shared.Wordbooks;
using Surging.Hero.BasicData.IApplication.Wordbook.Dtos;

namespace Surging.Hero.BasicData.Application.Wordbook
{
    public class WordbookProfile : Profile
    {
        public WordbookProfile()
        {
            CreateMap<CreateWordbookInput, Domain.Wordbooks.Wordbook>().AfterMap((src,dest)=> {
                dest.Type = WordbookType.Business;
            });
        }
    }
}
