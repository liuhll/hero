using System;

namespace Surging.Core.Domain.Entities.Auditing
{
    public interface IHasCreationTime
    {
        DateTime CreationTime { get; set; }
    }
}
