using System.Collections.Generic;

namespace Surging.Core.Domain.PagedAndSorted
{
    public class PagedResultDto<T> : ListResultDto<T>, IPagedResult<T>
    {
        public int TotalCount { get; set; }


        private PagedResultDto()
        {

        }


        public PagedResultDto(int totalCount, IReadOnlyList<T> items)
            : base(items)
        {
            TotalCount = totalCount;
        }
    }
}
