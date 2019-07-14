using Surging.Core.Domain.Entities;

namespace Surging.Core.Domain.Repositories
{
    public interface IRepository<TEntity> : IRepository<TEntity, int> where TEntity : class, IEntity<int>
    {
    }
}
