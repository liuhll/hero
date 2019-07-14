using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;
using Surging.Core.CPlatform.EventBus.Events;
using Surging.Core.CPlatform.EventBus.Implementation;
using Surging.Core.CPlatform.Utilities;
using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Surging.Core.Dapper.Manager
{
    public abstract class ManagerBase 
    {
        protected virtual DbConnection Connection
        {
            get
            {
                if (DbSetting.Instance == null)
                {
                    throw new Exception("未设置数据库连接");
                }
                DbConnection conn = null;
                switch (DbSetting.Instance.DbType)
                {
                    case DbType.MySql:
                        conn = new MySqlConnection(DbSetting.Instance.ConnectionString);
                        break;
                    case DbType.Oracle:
                        conn = new OracleConnection(DbSetting.Instance.ConnectionString);
                        break;
                    case DbType.SqlServer:
                        conn = new SqlConnection(DbSetting.Instance.ConnectionString);
                        break;
                }
                conn.Open();
                return conn;
            }
        }

        protected virtual void UnitOfWork(Action<DbConnection, DbTransaction> action, DbConnection conn)
        {
            var trans = conn.BeginTransaction();
            try
            {
                action(conn, trans);
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                conn.Close();
            }

        }

        protected virtual async Task UnitOfWorkAsync(Func<DbConnection, DbTransaction, Task> action, DbConnection conn)
        {
            var trans = conn.BeginTransaction();
            try
            {
                await action(conn, trans);
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                conn.Close();
            }

        }

        public virtual T GetService<T>() where T : class
        {
            return ServiceLocator.GetService<T>();
        }

        public virtual T GetService<T>(string key) where T : class
        {
            return ServiceLocator.GetService<T>(key);
        }

        public virtual object GetService(Type type)
        {
            return ServiceLocator.GetService(type);
        }

        public virtual object GetService(string key, Type type)
        {
            return ServiceLocator.GetService(key, type);
        }

        public void Publish(IntegrationEvent @event)
        {
            GetService<IEventBus>().Publish(@event);
        }
    }
}
