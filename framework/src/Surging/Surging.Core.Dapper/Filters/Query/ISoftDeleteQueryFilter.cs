using Surging.Core.CPlatform.Ioc;
using Surging.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Surging.Core.Dapper.Filters.Query
{
    public interface ISoftDeleteQueryFilter 
    {
        Expression<Func<TEntity, bool>> ExecuteFilter<TEntity, TPrimaryKey>(Expression<Func<TEntity, bool>> predicate = null) where TEntity : class, IEntity<TPrimaryKey>;
    }
}
