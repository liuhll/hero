
using Surging.Core.CPlatform.Ioc;
using Surging.Core.Domain.Entities;

namespace Surging.Core.Dapper.Filters.Action
{
    public interface IAuditActionFilter<TEntity, TPrimaryKey> : ITransientDependency where TEntity : class, IEntity<TPrimaryKey>
    {
        void ExecuteFilter(TEntity entity);

    }
}
