using System;

namespace Surging.Core.Domain.Entities.Auditing
{
    public interface IHasModificationTime
    {
        DateTime? LastModificationTime { get; set; }
    }
}
