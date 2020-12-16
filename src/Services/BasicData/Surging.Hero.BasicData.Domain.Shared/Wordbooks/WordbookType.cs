using System.ComponentModel;

namespace Surging.Hero.BasicData.Domain.Shared.Wordbooks
{
    public enum WordbookType
    {
        [Description("系统类")] System = 1,

        [Description("业务类")] Business
    }
}