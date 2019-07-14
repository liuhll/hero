using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Surging.Core.Domain.PagedAndSorted.Extensions
{
    public static class PagedExtensions
    {
        public static IPagedResult<T> PageBy<T>(this IQueryable<T> query, int skipCount, int maxResultCount)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            var pageItems = query.Skip(skipCount).Take(maxResultCount);
            return new PagedResultDto<T>(query.Count(), pageItems.ToList());
        }


        public static IPagedResult<T> PageBy<T>(this IQueryable<T> query, IPagedResultRequest pagedResultRequest)
        {
            return query.PageBy((pagedResultRequest.PageCount * (pagedResultRequest.PageIndex - 1)), pagedResultRequest.PageCount);
        }

        public static IPagedResult<T> PageBy<T>(this IEnumerable<T> query, int skipCount, int maxResultCount)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            var pageItems = query.Skip(skipCount).Take(maxResultCount);
            return new PagedResultDto<T>(query.Count(), pageItems.ToList());
        }

        public static IPagedResult<T> PageBy<T>(this IEnumerable<T> query, IPagedResultRequest pagedResultRequest)
        {
            return query.PageBy((pagedResultRequest.PageCount * (pagedResultRequest.PageIndex - 1)), pagedResultRequest.PageCount);
        }

        public static IPagedResult<T> GetPagedResult<T>(this IEnumerable<T> pageResult, int totalCount)
        {
            return new PagedResultDto<T>(totalCount, pageResult.ToList());
        }
    }
}
