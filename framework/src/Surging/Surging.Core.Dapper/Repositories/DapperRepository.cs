using Dapper;
using DapperExtensions;
using Microsoft.Extensions.Logging;
using Surging.Core.CPlatform.Exceptions;
using Surging.Core.CPlatform.Utilities;
using Surging.Core.Dapper.Expressions;
using Surging.Core.Dapper.Filters.Action;
using Surging.Core.Dapper.Filters.Query;
using Surging.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Surging.Core.Dapper.Repositories
{
    public class DapperRepository<TEntity, TPrimaryKey> : DapperRepositoryBase, IDapperRepository<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
    {
        private readonly ISoftDeleteQueryFilter _softDeleteQueryFilter;
        private readonly IAuditActionFilter<TEntity, TPrimaryKey> _creationActionFilter;
        private readonly IAuditActionFilter<TEntity, TPrimaryKey> _modificationActionFilter;
        private readonly IAuditActionFilter<TEntity, TPrimaryKey> _deletionAuditDapperActionFilter;
        private readonly ILogger<DapperRepository<TEntity, TPrimaryKey>> _logger;
        public DapperRepository(ISoftDeleteQueryFilter softDeleteQueryFilter,
            ILogger<DapperRepository<TEntity, TPrimaryKey>> logger)
        {
            _softDeleteQueryFilter = softDeleteQueryFilter;
            _logger = logger;
            _creationActionFilter = ServiceLocator.GetService<IAuditActionFilter<TEntity, TPrimaryKey>>(typeof(CreationAuditDapperActionFilter<TEntity, TPrimaryKey>).Name);
            _modificationActionFilter = ServiceLocator.GetService<IAuditActionFilter<TEntity, TPrimaryKey>>(typeof(ModificationAuditDapperActionFilter<TEntity, TPrimaryKey>).Name);
            _deletionAuditDapperActionFilter = ServiceLocator.GetService<IAuditActionFilter<TEntity, TPrimaryKey>>(typeof(DeletionAuditDapperActionFilter<TEntity, TPrimaryKey>).Name);
        }

        public Task InsertAsync(TEntity entity)
        {
            try
            {
                using (var conn = GetDbConnection())
                {
                    _creationActionFilter.ExecuteFilter(entity);

                    conn.Insert<TEntity>(entity);
                    return Task.CompletedTask;
                }
            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError(ex.Message, ex);
                }

                throw new DataAccessException(ex.Message, ex);
            }
        }



        public Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity)
        {
            try
            {
                using (var conn = GetDbConnection())
                {

                    _creationActionFilter.ExecuteFilter(entity);
                    conn.Insert(entity);
                    return Task.FromResult(entity.Id);
                }
            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError(ex.Message, ex);
                }

                throw new DataAccessException(ex.Message, ex);
            }
        }

        public async Task InsertOrUpdateAsync(TEntity entity)
        {
            try
            {
                if (entity.Id == null)
                {
                    _creationActionFilter.ExecuteFilter(entity);
                    await InsertAsync(entity);
                }
                else
                {
                    var existEntity = await SingleOrDefaultAsync(p => p.Id.Equals(entity.Id));
                    if (existEntity == null)
                    {
                        _creationActionFilter.ExecuteFilter(entity);
                        await InsertAsync(entity);
                    }
                    else
                    {
                        _modificationActionFilter.ExecuteFilter(entity);
                        await UpdateAsync(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError(ex.Message, ex);
                }

                throw new DataAccessException(ex.Message, ex);
            }

        }

        public async Task<TPrimaryKey> InsertOrUpdateAndGetIdAsync(TEntity entity)
        {
            try
            {
                if (entity.Id == null)
                {
                    _creationActionFilter.ExecuteFilter(entity);
                    return await InsertAndGetIdAsync(entity);
                }
                else
                {
                    var existEntity = SingleAsync(CreateEqualityExpressionForId(entity.Id));
                    if (existEntity == null)
                    {
                        _creationActionFilter.ExecuteFilter(entity);
                        return await InsertAndGetIdAsync(entity);
                    }
                    else
                    {
                        _modificationActionFilter.ExecuteFilter(entity);
                        await UpdateAsync(entity);
                        return entity.Id;
                    }
                }
            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError(ex.Message, ex);
                }

                throw new DataAccessException(ex.Message, ex);
            }
        }

        public Task DeleteAsync(TEntity entity)
        {
            try
            {
                using (var conn = GetDbConnection())
                {
                    if (entity is ISoftDelete)
                    {
                        _deletionAuditDapperActionFilter.ExecuteFilter(entity);
                        UpdateAsync(entity);
                    }
                    else
                    {
                        conn.Delete(entity);

                    }
                    return Task.CompletedTask;
                }
            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError(ex.Message, ex);
                }

                throw new DataAccessException(ex.Message, ex);
            }

        }

        public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            IEnumerable<TEntity> items = await GetAllAsync(predicate);
            foreach (TEntity entity in items)
            {
                await DeleteAsync(entity);
            }

        }

        public Task UpdateAsync(TEntity entity)
        {
            try
            {
                using (var conn = GetDbConnection())
                {
                    _modificationActionFilter.ExecuteFilter(entity);
                    conn.Update(entity);
                    return Task.CompletedTask;
                }
            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError(ex.Message, ex);
                }

                throw new DataAccessException(ex.Message, ex);
            }
        }

        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                using (var conn = GetDbConnection())
                {
                    predicate = _softDeleteQueryFilter.ExecuteFilter<TEntity, TPrimaryKey>(predicate);
                    var pg = predicate.ToPredicateGroup<TEntity, TPrimaryKey>();
                    return Task.FromResult(conn.GetList<TEntity>(pg).FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError(ex.Message, ex);
                }

                throw new DataAccessException(ex.Message, ex);
            }
        }

        public Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                using (var conn = GetDbConnection())
                {
                    predicate = _softDeleteQueryFilter.ExecuteFilter<TEntity, TPrimaryKey>(predicate);
                    var pg = predicate.ToPredicateGroup<TEntity, TPrimaryKey>();
                    return Task.FromResult(conn.GetList<TEntity>(pg).First());
                }
            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError(ex.Message, ex);
                }

                throw new DataAccessException(ex.Message, ex);
            }
        }


        public Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                using (var conn = GetDbConnection())
                {
                    predicate = _softDeleteQueryFilter.ExecuteFilter<TEntity, TPrimaryKey>(predicate);
                    var pg = predicate.ToPredicateGroup<TEntity, TPrimaryKey>();
                    return Task.FromResult(conn.GetList<TEntity>(pg).Single());
                }
            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError(ex.Message, ex);
                }

                throw new DataAccessException(ex.Message, ex);
            }
        }


        public Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                using (var conn = GetDbConnection())
                {
                    predicate = _softDeleteQueryFilter.ExecuteFilter<TEntity, TPrimaryKey>(predicate);
                    var pg = predicate.ToPredicateGroup<TEntity, TPrimaryKey>();
                    return Task.FromResult(conn.GetList<TEntity>(pg).SingleOrDefault());
                }
            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError(ex.Message, ex);
                }

                throw new DataAccessException(ex.Message, ex);
            }
        }

        public Task<TEntity> GetAsync(TPrimaryKey id)
        {
            return SingleAsync(CreateEqualityExpressionForId(id));
        }


        public Task<IEnumerable<TEntity>> GetAllAsync()
        {
            try
            {
                using (var conn = GetDbConnection())
                {
                    var predicate = _softDeleteQueryFilter.ExecuteFilter<TEntity, TPrimaryKey>();
                    var pg = predicate.ToPredicateGroup<TEntity, TPrimaryKey>();
                    var list = conn.GetList<TEntity>(pg);
                    return Task.FromResult(list);
                }
            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError(ex.Message, ex);
                }

                throw new DataAccessException(ex.Message, ex);
            }
        }

        public Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                using (var conn = GetDbConnection())
                {
                    predicate = _softDeleteQueryFilter.ExecuteFilter<TEntity, TPrimaryKey>(predicate);
                    var pg = predicate.ToPredicateGroup<TEntity, TPrimaryKey>();
                    var list = conn.GetList<TEntity>(pg);
                    return Task.FromResult(list);
                }
            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError(ex.Message, ex);
                }

                throw new DataAccessException(ex.Message, ex);
            }
        }


        public Task<IEnumerable<TEntity>> QueryAsync(string query, object parameters = null)
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException("Sql语句不允许为空");
            }

            try
            {
                using (var conn = GetDbConnection())
                {
                    return conn.QueryAsync<TEntity>(query, parameters);
                }
            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError(ex.Message, ex);
                }

                throw new DataAccessException(ex.Message, ex);
            }

        }

        public Task<IEnumerable<TAny>> Query<TAny>(string query, object parameters = null) where TAny : class
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException("Sql语句不允许为空");
            }

            try
            {
                using (var conn = GetDbConnection())
                {
                    return conn.QueryAsync<TAny>(query, parameters);
                }
            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError(ex.Message, ex);
                }

                throw new DataAccessException(ex.Message, ex);
            }
        }


        public Task InsertAsync(TEntity entity, DbConnection conn, DbTransaction trans)
        {
            try
            {
                _creationActionFilter.ExecuteFilter(entity);
                conn.Insert<TEntity>(entity, trans);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError(ex.Message, ex);
                }

                throw new DataAccessException(ex.Message, ex);
            }

        }

        public Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity, DbConnection conn, DbTransaction trans)
        {
            try
            {
                _creationActionFilter.ExecuteFilter(entity);
                conn.Insert<TEntity>(entity, trans);
                return Task.FromResult(entity.Id);
            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError(ex.Message, ex);
                }

                throw new DataAccessException(ex.Message, ex);
            }

        }

        public async Task InsertOrUpdateAsync(TEntity entity, DbConnection conn, DbTransaction trans)
        {
            try
            {
                if (entity.Id == null)
                {
                    _creationActionFilter.ExecuteFilter(entity);
                    await InsertAsync(entity, conn, trans);
                }
                else
                {
                    var existEntity = await SingleOrDefaultAsync(p => p.Id.Equals(entity.Id));
                    if (existEntity == null)
                    {
                        _creationActionFilter.ExecuteFilter(entity);
                        await InsertAsync(entity, conn, trans);
                    }
                    else
                    {
                        _modificationActionFilter.ExecuteFilter(entity);
                        await UpdateAsync(entity, conn, trans);
                    }
                }
            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError(ex.Message, ex);
                }

                throw new DataAccessException(ex.Message, ex);
            }


        }

        public async Task<TPrimaryKey> InsertOrUpdateAndGetIdAsync(TEntity entity, DbConnection conn, DbTransaction trans)
        {

            try
            {
                if (entity.Id == null)
                {
                    _creationActionFilter.ExecuteFilter(entity);
                    return await InsertAndGetIdAsync(entity, conn, trans);
                }
                else
                {
                    var existEntity = SingleAsync(CreateEqualityExpressionForId(entity.Id));
                    if (existEntity == null)
                    {
                        _creationActionFilter.ExecuteFilter(entity);
                        return await InsertAndGetIdAsync(entity, conn, trans);
                    }
                    else
                    {
                        _modificationActionFilter.ExecuteFilter(entity);
                        await UpdateAsync(entity, conn, trans);
                        return entity.Id;
                    }
                }

            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError(ex.Message, ex);
                }

                throw new DataAccessException(ex.Message, ex);
            }
        }

        public Task UpdateAsync(TEntity entity, DbConnection conn, DbTransaction trans)
        {
            try
            {
                _modificationActionFilter.ExecuteFilter(entity);
                conn.Update(entity, trans);
                return Task.CompletedTask;

            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError(ex.Message, ex);
                }

                throw new DataAccessException(ex.Message, ex);
            }


        }

        public Task DeleteAsync(TEntity entity, DbConnection conn, DbTransaction trans)
        {
            try
            {

                if (entity is ISoftDelete)
                {
                    _deletionAuditDapperActionFilter.ExecuteFilter(entity);
                    UpdateAsync(entity, conn, trans);
                }
                else
                {
                    conn.Delete(entity, trans);

                }
                return Task.CompletedTask;

            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError(ex.Message, ex);
                }

                throw new DataAccessException(ex.Message, ex);
            }

        }

        public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, DbConnection conn, DbTransaction trans)
        {
            IEnumerable<TEntity> items = await GetAllAsync(predicate);
            foreach (TEntity entity in items)
            {
                await DeleteAsync(entity, conn, trans);
            }

        }

        protected static Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TPrimaryKey id)
        {
            ParameterExpression lambdaParam = Expression.Parameter(typeof(TEntity));

            BinaryExpression lambdaBody = Expression.Equal(
                Expression.PropertyOrField(lambdaParam, "Id"),
                Expression.Constant(id, typeof(TPrimaryKey))
            );

            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }

        public async Task<int> GetCountAsync()
        {
            try
            {
                using (var conn = GetDbConnection())
                {
                    var predicate = _softDeleteQueryFilter.ExecuteFilter<TEntity, TPrimaryKey>();
                    var pg = predicate.ToPredicateGroup<TEntity, TPrimaryKey>();
                    var count = conn.Count<TEntity>(pg);
                    return count;
                }
            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError(ex.Message, ex);
                }

                throw new DataAccessException(ex.Message, ex);
            }
        }

        public async Task<int> GetCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                using (var conn = GetDbConnection())
                {
                    predicate = _softDeleteQueryFilter.ExecuteFilter<TEntity, TPrimaryKey>(predicate);
                    var pg = predicate.ToPredicateGroup<TEntity, TPrimaryKey>();
                    var count = conn.Count<TEntity>(pg);
                    return count;
                }
            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError(ex.Message, ex);
                }

                throw new DataAccessException(ex.Message, ex);
            }
        }

        public Task<IEnumerable<TEntity>> GetPageAsync(Expression<Func<TEntity, bool>> predicate, int index, int count, IDictionary<string, SortType> sortProps)
        {
            try
            {
                using (var conn = GetDbConnection())
                {
                    IList<ISort> sorts = new List<ISort>();

                    if (sortProps != null && sortProps.Any())
                    {
                        foreach (var sortProp in sortProps)
                        {
                            var sort = new Sort()
                            {
                                PropertyName = sortProp.Key,
                                Ascending = sortProp.Value == SortType.Asc ? true : false
                            };
                            sorts.Add(sort);
                        };
                    }
                    predicate = _softDeleteQueryFilter.ExecuteFilter<TEntity, TPrimaryKey>(predicate);
                    var pg = predicate.ToPredicateGroup<TEntity, TPrimaryKey>();
                    var list = conn.GetPage<TEntity>(predicate, sorts, index, count);
                    return Task.FromResult(list);
                }
            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError(ex.Message, ex);
                }

                throw new DataAccessException(ex.Message, ex);
            }

        }

        public Task<IEnumerable<TEntity>> GetPageAsync(Expression<Func<TEntity, bool>> predicate, int index, int count)
        {
            return GetPageAsync(predicate, index, count, null);
        }

        public Task<IEnumerable<TEntity>> GetPageAsync(int index, int count, IDictionary<string, SortType> sortProps)
        {
            try
            {
                using (var conn = GetDbConnection())
                {
                    IList<ISort> sorts = new List<ISort>();

                    if (sortProps != null && sortProps.Any())
                    {
                        foreach (var sortProp in sortProps)
                        {
                            var sort = new Sort()
                            {
                                PropertyName = sortProp.Key,
                                Ascending = sortProp.Value == SortType.Asc ? true : false
                            };
                            sorts.Add(sort);
                        };
                    }
                    var predicate = _softDeleteQueryFilter.ExecuteFilter<TEntity, TPrimaryKey>();
                    var pg = predicate.ToPredicateGroup<TEntity, TPrimaryKey>();
                    var list = conn.GetPage<TEntity>(predicate, sorts, index, count);
                    return Task.FromResult(list);
                }
            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError(ex.Message, ex);
                }

                throw new DataAccessException(ex.Message, ex);
            }

        }

        public Task<IEnumerable<TEntity>> GetPageAsync(int index, int count)
        {
            return GetPageAsync(index, count, null);
        }

    }
}
