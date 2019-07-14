using Surging.Core.Dapper.Utils;
using Surging.Core.Domain.Entities;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Surging.Core.Dapper.Filters.Query
{
    public class SoftDeleteQueryFilter : ISoftDeleteQueryFilter
    {
        private int IsDeleted => 0;

        public Expression<Func<TEntity, bool>> ExecuteFilter<TEntity, TPrimaryKey>(Expression<Func<TEntity, bool>> predicate = null) where TEntity : class, IEntity<TPrimaryKey>
        {
            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
            {
                PropertyInfo propType = typeof(TEntity).GetProperty(nameof(ISoftDelete.IsDeleted));
                if (predicate == null)
                {
                    predicate = ExpressionUtils.MakePredicate<TEntity>(nameof(ISoftDelete.IsDeleted), IsDeleted, propType.PropertyType);
                }
                else
                {
                    ParameterExpression paramExpr = predicate.Parameters[0];
                    MemberExpression memberExpr = Expression.Property(paramExpr, nameof(ISoftDelete.IsDeleted));
                    BinaryExpression body = Expression.AndAlso(
                        predicate.Body,
                        Expression.Equal(memberExpr, Expression.Constant(IsDeleted, propType.PropertyType)));
                    predicate = Expression.Lambda<Func<TEntity, bool>>(body, paramExpr);
                }
            }
            return predicate;
        }

    }
}
