using Surging.Core.CPlatform.Runtime.Session;
using Surging.Core.Domain.Entities;
using Surging.Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surging.Core.Dapper.Filters.Action
{
    public class CreationAuditDapperActionFilter<TEntity, TPrimaryKey> : DapperActionFilterBase, IAuditActionFilter<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
    {

        public void ExecuteFilter(TEntity entity)
        {
            if (typeof(ISoftDelete).IsAssignableFrom(entity.GetType()))
            {
                ((ISoftDelete)entity).IsDeleted = Normal;
            }
            if (typeof(IHasCreationTime).IsAssignableFrom(entity.GetType()))
            {
                ((IHasCreationTime)entity).CreationTime = DateTime.Now;
            }
            CheckAndSetId(entity);
            var loginUser = NullSurgingSession.Instance;
            if (typeof(ICreationAudited).IsAssignableFrom(typeof(TEntity)) && loginUser != null)
            {

                var record = entity as ICreationAudited;
                if (record.CreatorUserId == null)
                {
                    record.CreatorUserId = loginUser.UserId;
                }

            }
        }
    }
}
