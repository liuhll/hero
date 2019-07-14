namespace Surging.Core.Domain.PagedAndSorted
{
    public abstract class PagedResultRequestDto : IPagedResultRequest
    {
        public int PageCount { get; set; }
        public int PageIndex { get; set; }
    }
}
