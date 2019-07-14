namespace Surging.Core.Domain.PagedAndSorted
{
    public interface IPagedResultRequest
    {
        int PageCount { get; set; }

        int PageIndex { get; set; }
    }
}
