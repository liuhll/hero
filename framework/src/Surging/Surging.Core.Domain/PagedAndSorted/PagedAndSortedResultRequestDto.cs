namespace Surging.Core.Domain.PagedAndSorted
{
    public abstract class PagedAndSortedResultRequestDto : PagedResultRequestDto, IPagedAndSortedResultRequest
    {
        public string Sorting { get; set; }
    }
}
