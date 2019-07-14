using System.Collections.Generic;

namespace Surging.Core.Domain.PagedAndSorted
{
    public interface IListResult<T>
    {
        IReadOnlyList<T> Items { get; set; }
    }
}
