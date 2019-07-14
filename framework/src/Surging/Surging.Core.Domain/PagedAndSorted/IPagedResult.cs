namespace Surging.Core.Domain.PagedAndSorted
{
    public interface IPagedResult<T> : IListResult<T>, IHasTotalCount
    {

    }
}
