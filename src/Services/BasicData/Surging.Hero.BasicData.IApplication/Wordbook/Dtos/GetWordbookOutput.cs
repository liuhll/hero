using Surging.Hero.BasicData.Domain.Shared.Wordbooks;
using Surging.Hero.Common.Extensions;
using System;

namespace Surging.Hero.BasicData.IApplication.Wordbook.Dtos
{
    public class GetWordbookOutput : WordbookDtoBase
    {
        public long Id { get; set; }

        public string Code { get; set; }

        public bool IsSysPreset { get; set; }

        public string TypeDesc
        {
            get
            {
                return Type.GetDescription();
            }
         }

    }
}
