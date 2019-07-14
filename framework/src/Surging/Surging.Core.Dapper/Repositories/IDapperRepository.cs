using Surging.Core.Domain.Entities;
using Surging.Core.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Surging.Core.Dapper.Repositories
{
    public interface IDapperRepository<TEntity, TPrimaryKey> : IRepository where TEntity : class, IEntity<TPrimaryKey>
    {
        Task InsertAsync(TEntity entity);

        Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity);

        Task InsertOrUpdateAsync(TEntity entity);

        Task<TPrimaryKey> InsertOrUpdateAndGetIdAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);

        Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);

        Task InsertAsync(TEntity entity, DbConnection conn, DbTransaction trans);

        Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity, DbConnection conn, DbTransaction trans);

        Task InsertOrUpdateAsync(TEntity entity, DbConnection conn, DbTransaction trans);

        Task<TPrimaryKey> InsertOrUpdateAndGetIdAsync(TEntity entity, DbConnection conn, DbTransaction trans);

        Task UpdateAsync(TEntity entity, DbConnection conn, DbTransaction trans);

        Task DeleteAsync(TEntity entity, DbConnection conn, DbTransaction trans);

        Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, DbConnection conn, DbTransaction trans);

        Task<int> GetCountAsync(Expression<Func<TEntity, bool>> predicate);

        Task<int> GetCountAsync();

        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> GetAsync(TPrimaryKey id);

        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<IEnumerable<TEntity>> QueryAsync(string query, object parameters = null);

        Task<IEnumerable<TAny>> Query<TAny>(string query, object parameters = null) where TAny : class;


        Task<IEnumerable<TEntity>> GetPageAsync(Expression<Func<TEntity, bool>> predicate, int index, int count, IDictionary<string, SortType> sortProps);

        Task<IEnumerable<TEntity>> GetPageAsync(Expression<Func<TEntity, bool>> predicate, int index, int count);

        Task<IEnumerable<TEntity>> GetPageAsync(int index, int count, IDictionary<string, SortType> sortProps);

        Task<IEnumerable<TEntity>> GetPageAsync(int index, int count);
    }
}
